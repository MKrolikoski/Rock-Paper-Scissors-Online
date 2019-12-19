using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Linq;


public class GameManager : NetworkBehaviour
{
    // GameManager singleton 
    public static GameManager instance;

    public GameObject sceneCamera;
    
    private Dictionary<string, Player> players;

    private int maxPlayerCount = 2;

    private bool gameOver = false;

    private int outlineColorGreen = 1;
    private int outlineColorRed = 2;


    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        players = new Dictionary<string, Player>();
    }

    void Start()
    {
        if(isServer)
        {
            StartCoroutine("WaitForPlayers");
        }
    }

    IEnumerator WaitForPlayers()
    {
        if(players.Count != maxPlayerCount)
        {
            CmdShowInfo("Awaiting players...");
            yield return new WaitForSeconds(1.0f);
            StartCoroutine("WaitForPlayers");
        }
        else
        {
            StartCoroutine(ShowInfoWaitAndCallFunction(1, "Match is starting..", CmdNewRound));
        }
    }

    void Update()
    {

    }

    public void SetSceneCameraActive(bool enabled)
    {
        if(sceneCamera != null)
        {
            sceneCamera.SetActive(enabled);
        }
    }


    // Adds new player to list of players

    public void RegisterPlayer(string playerId, Player player)
    {
        players.Add(playerId, player);
    }

    public void UnregisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }


    private bool CheckIfAllPlayersEndedTurn()
    {
        foreach (Player player in players.Values)
        {
            if (!player.endedRound)
            {
                return false;
            }
        }
        return true;
    }


    [Command]
    public void CmdPlayerEndedTurn(string playerName)
    {
        if (CheckIfAllPlayersEndedTurn())
        {
            RpcChangePlayerCamera(1);
            StartCoroutine(ShowInfoWaitAndCallFunction(2, "Comparing coins..", CmdCompareSelectedItems));
        }
    }

    //0 -> player Camera
    //1 -> overview Camera
    [ClientRpc]
    void RpcChangePlayerCamera(int cameraNumber)
    {
        foreach (Player player in players.Values)
        {
            player.ChangeCamera(cameraNumber);
        }
    }

    IEnumerator ShowInfoWaitAndCallFunction(int timeToWait, string info, System.Action methodToCallAfter)
    {
        CmdShowInfo(info);
        yield return new WaitForSeconds(timeToWait);
        methodToCallAfter();
    }


    [Command]
    void CmdShowInfo(string info)
    {
        RpcShowInfo(info);
    }


    [ClientRpc]
    void RpcShowInfo(string info)
    {
        foreach (Player player in players.Values)
        {
            player.DisplayText(info);
        }
    }

    [Command]
    public void CmdPlayerDied(string playerId)
    {
        gameOver = true;
        StartCoroutine(ShowInfoWaitAndCallFunction(4, playerId + " died!", RpcRestartGame));
    }

    [ClientRpc]
    private void RpcRestartGame()
    {
        gameOver = false;
        foreach (Player player in players.Values)
        {
            player.RpcOnNewGame();
        }
        NewRound();
    }


    // Resets properties to base values and invokes OnNewRound event
    void NewRound()
    {
        if(!gameOver)
        {
            CmdNewRound();
        }
    }

    [Command]
    void CmdNewRound()
    {
        foreach (Player player in players.Values)
        {
            player.RpcOnNewRound();
        }
        RpcChangePlayerCamera(0);
    }

   

    // Compares items selected by players and returns array of players (first element is a winner, second - loser), or null if it's a draw
    [Command]
    void CmdCompareSelectedItems()
    {
        RpcShowSelectedCoinsSymbols(true);

        Player[] playersArray = players.Values.ToArray();
        Player winner = null;
        Player loser = null;
        Item item1 = playersArray[0].SelectedCoin.Item;
        Item item2 = playersArray[1].SelectedCoin.Item;
        int result = item1.CompareTo(item2);
        string info;
        if (result == 1)
        {
            winner = playersArray[0];
            loser = playersArray[1];
            RpcChangeCoinOutlineColor(winner.name, outlineColorGreen, true);
            RpcChangeCoinOutlineColor(loser.name, outlineColorRed, true);

            info = winner.name + " won this round!";
        }
        else if (result == -1)
        {
            winner = playersArray[1];
            loser = playersArray[0];
            RpcChangeCoinOutlineColor(winner.name, outlineColorGreen, true);
            RpcChangeCoinOutlineColor(loser.name, outlineColorRed, true);

            info = winner.name + " won this round!";
        }
        else
        {
            RpcChangeCoinOutlineColor(playersArray[0].name, outlineColorGreen, true);
            RpcChangeCoinOutlineColor(playersArray[1].name, outlineColorGreen, true);
            info = "It's a draw!";
        }
        StartCoroutine(ShowInfoWaitAndCallFunction(3, "Coins compared! " + info, NewRound));

        if(winner != null && loser != null)
        {
            RpcDamagePlayer(loser.name, winner.playerStats.power.CurrentValue);
        }
    }


    [ClientRpc]
    void RpcDamagePlayer(string playerId, int damage)
    {
        players[playerId].playerStats.TakeDamage(damage);
    }

    [ClientRpc]
    void RpcShowSelectedCoinsSymbols(bool showSymbol)
    {
        foreach (Player player in players.Values)
        {
            player.SelectedCoin.Hidden = !showSymbol;
        }
    }

    [ClientRpc]
    void RpcChangeCoinOutlineColor(string playerId, int outlineNumber, bool highlight)
    {
        Coin coin = players[playerId].SelectedCoin;
        coin.ChangeOutlineColor(outlineNumber);
        if(highlight)
        {
            coin.Highlight();
        }
    }
}
