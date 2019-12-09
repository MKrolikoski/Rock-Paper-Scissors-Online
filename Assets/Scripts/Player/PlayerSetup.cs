using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    public Behaviour[] componentsToDisable;
    public GameObject[] coinsPositions;

    private string remotePlayerLayerName = "RemotePlayer";
    private string dontDrawLayerName = "DontDraw";
    private string interactableLayerName = "Interactable";

    [SerializeField]
    private GameObject playerGFX;

    [SerializeField]
    public GameObject UIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;

    // Start is called before the first frame update
    void Start()
    {
        if(!isLocalPlayer)
        {
            DisableComponents();
            AssignLayerRecursively(GetComponentInChildren<EndTurnButton>().gameObject, LayerMask.NameToLayer(remotePlayerLayerName));
            DisableCoinsInteractability();
            HideCoinsSymbols();
            EnemyHealthUI enemyHealthUI = GetComponentInChildren<EnemyHealthUI>();
            enemyHealthUI.SetupUI();
            GetComponent<PlayerStats>().onDamageTaken += enemyHealthUI.healthDisplay.TakeDamage;
        }
        else
        {
            GetComponentInChildren<Camera>().enabled = false;

            playerUIInstance = Instantiate(UIPrefab);

            PlayerUI playerUI = playerUIInstance.GetComponent<PlayerUI>();

            GetComponent<PlayerStats>().onDamageTaken += playerUI.playerHealthDisplay.TakeDamage;

            if (playerGFX != null)
            {
                AssignLayerRecursively(playerGFX, LayerMask.NameToLayer(dontDrawLayerName));
            }
            foreach (Coin coin in GetComponent<Player>().coins)
            {
                AssignLayerRecursively(coin.gameObject, LayerMask.NameToLayer(interactableLayerName));
            }
            AssignCoinsSymbols();
        }
    }


    private void AssignLayerRecursively(GameObject obj, LayerMask layerName)
    {
        obj.layer = layerName;

        foreach (Transform child in obj.transform)
        {
            AssignLayerRecursively(child.gameObject, layerName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();


        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        player.name = "Player" + netID;

        GameManager.instance.RegisterPlayer(player.name, player);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        GetComponentInChildren<Camera>().name = "LocalPlayerCamera";
    }

    void OnDisable()
    {
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
        }
        GameManager.instance.UnregisterPlayer(transform.name);
    }

    private void DisableComponents()
    {
        foreach (Behaviour component in componentsToDisable)
        {
            component.enabled = false;
        }
    }

    private void DisableCoinsInteractability()
    {
        foreach (Coin coin in GetComponent<Player>().coins)
        {
            coin.DisableInteractability();
        }

    }

    private void HideCoinsSymbols()
    {
        foreach (Coin coin in GetComponent<Player>().coins)
        {
            coin.Hidden = true;
        }
    }

    public void AssignCoinsSymbols()
    {
        Coin[] coins = GetComponent<Player>().coins;
        for (int i = 0; i < coins.Length; i++)
        {
            Coin temp = coins[i];
            int randomIndex = UnityEngine.Random.Range(i, coins.Length);
            coins[i] = coins[randomIndex];
            coins[randomIndex] = temp;
            for (int j = 0; j < ItemsDB.instance.items.Length; j++)
            {
                coins[j].Item = ItemsDB.instance.items[j];
            }
        }
        Array.Sort(coins, (x, y) => x.name.CompareTo(y.name));
    }


}
