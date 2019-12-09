using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerStats))]
public class Player : NetworkBehaviour
{
    // Player statistics
    [HideInInspector]
    public PlayerStats playerStats;

    public Camera playerCamera;


    // Player input
    private PlayerInput playerInput;

    // Coin selected by player
    private Coin _selectedCoin;

    public Coin[] coins;

    public bool endedRound = false;

    // Enables or disables player input
    private bool _inputEnabled;

    public delegate void OnCoinSelected();
    public OnCoinSelected onCoinSelected;

    public delegate void OnNewRound();
    public OnNewRound onNewRound;

    public bool InputEnabled
    {
        get { return _inputEnabled; }
        set
        {
            _inputEnabled = value;
            playerInput.enabled = _inputEnabled; 
        }
    }

    public Coin SelectedCoin
    {
        get
        {
            return _selectedCoin;
        }
        set
        {
            if (_selectedCoin != null)
            {
                _selectedCoin.Selected = false;
                _selectedCoin.ResetToDefaultValues();
            }
            _selectedCoin = value;
            if(value != null)
            {
                value.Selected = true;
            }
        }
    }

    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Start()
    {
        playerStats.hp.OnDeath += OnDeath;
    }

    void Update()
    {

    }

    public void SelectCoin(Coin coin)
    {
        int coinIndex = GetCoinIndex(coin);
        if(coinIndex == -1)
        {
            Debug.LogError("[Player] Coin index not found");
            return;
        }
        int itemIndex = ItemsDB.instance.GetItemIndex(coin.Item);
        if(itemIndex == -1)
        {
            Debug.LogError("[Player] Item index not found");
            return;
        }
        onCoinSelected?.Invoke();
        CmdCoinSelected(coinIndex, itemIndex);
    }

    [Command]
    void CmdCoinSelected(int coinIndex, int itemIndex)
    {
        RpcCoinSelected(coinIndex, itemIndex);
    }

    [ClientRpc]
    void RpcCoinSelected(int coinIndex, int itemIndex)
    {
        SelectedCoin = coins[coinIndex];
        Item item = ItemsDB.instance.items[itemIndex];
        if (SelectedCoin.Item != item)
        {
            SelectedCoin.Item = item;
        }
    }

    private int GetCoinIndex(Coin coin)
    {
        for (int i = 0; i < coins.Length; i++)
        {
            if (coin == coins[i])
                return i;
        }
        return -1;
    }

    [ClientRpc]
    public void RpcOnNewRound()
    {
        endedRound = false;
        if(!isLocalPlayer)
        {
            if(SelectedCoin != null)
            {
                SelectedCoin.Hidden = true;
            }
        }
        else
        {
            GetComponentInChildren<EndTurnButton>().OnNewRound();
            
        }
        foreach (Coin coin in coins)
        {
            coin.OnNewRound();
        }
        SelectedCoin = null;
        GetComponent<PlayerSetup>().AssignCoinsSymbols();
        onNewRound?.Invoke();
        DisplayText("Choose a coin");
    }

    private void OnDeath()
    {
        GameManager.instance.CmdPlayerDied(name);
    }

    [Command]
    public void CmdEndTurn()
    {
        RpcEndTurn();
    }

    [ClientRpc]
    public void RpcEndTurn()
    {
        endedRound = true;
        if(isLocalPlayer)
        {
            EnableCoinInteraction(false);
        }
        GameManager.instance.CmdPlayerEndedTurn(name);
    }

    private void EnableCoinInteraction(bool enableInteraction)
    {
        foreach (Coin coin in coins)
        {
            if(enableInteraction)
            {
                coin.EnableInteractability();
            }
            else
            {
                coin.DisableInteractability();
            }
        }
    }

    //0 -> player Camera
    //1 -> overview Camera
    public void ChangeCamera(int cameraNumber)
    {
        if(isLocalPlayer)
        {
            if(cameraNumber == 0)
            {
                GameManager.instance.SetSceneCameraActive(false);
                playerCamera.enabled = true;
            }
            else if(cameraNumber == 1)
            {
                GameManager.instance.SetSceneCameraActive(true);
                playerCamera.enabled = false;
            }
            else
            {
                Debug.LogError("Wrong camera number");
            }
        }
    }

    public void DisplayText(string text)
    {
        if(isLocalPlayer)
        {
            PlayerUI playerUI = GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>();
            playerUI.DisplayText(text);
        }
    }
}
