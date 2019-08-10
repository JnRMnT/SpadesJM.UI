using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Actors;
using System.Runtime.CompilerServices;
using com.shephertz.app42.gaming.multiplayer.client;

public class MultiplayerGamersCommunicator : MonoBehaviour {
    private NetworkPlayerBase[] networkPlayers;
    public UIGameTable GameTable;
	public MonoNetworkPlayer localPlayer;
    public TurnTimeoutHandler TurnTimeoutHandler;

	// Use this for initialization
	void Start () {
        networkPlayers = new NetworkPlayerBase[4];
	}

    public NetworkPlayerBase[] GetNetworkPlayers()
    {
        return this.networkPlayers;
    }

    public NetworkPlayerBase GetPlayerBySeat(int seat)
    {
        return networkPlayers[seat];
    }

    public NetworkPlayerBase GetPlayerByPlayerName(string playerName)
    {
        for (int i = 0; i < 4; i++)
        {
            if (networkPlayers[i].PlayerName == playerName)
            {
                return networkPlayers[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
	
	}

    public void ResetPlayers()
    {
        GameTable.gameTable.ResetPlayers();
        for (int i = 0; i < 4; i++)
        {
            if (networkPlayers[i] != null)
            {
                if (!(networkPlayers[i] is LocalNetworkPlayer))
                {
                    Destroy(networkPlayers[i].GetTransform().gameObject);
                }
                networkPlayers[i] = null;
            }
        }
    }

    protected bool IsRoomFilled()
    {
        bool emptySeat = false;
        int i=0;

        while (!emptySeat && i < 4)
        {
            if (networkPlayers[i] == null)
            {
                emptySeat = true;
            }
            i++;
        }

        return !emptySeat;
    }

    public void UserLeft(string userName)
    {
        for (int i = 0; i < 4; i++)
        {
            if (networkPlayers[i] != null && networkPlayers[i].PlayerName == userName)
            {
                if (networkPlayers[i].GetPlayersSeat() != -1)
                {
                    GameTable.gameTable.LeaveTable(networkPlayers[i]);
                }
                if (!(networkPlayers[i] is LocalNetworkPlayer))
                {
                    Destroy(networkPlayers[i].GetTransform().gameObject);
                }
                networkPlayers[i] = null;
            }
        }
    }

    public void UserJoined(string username, int seat,bool local)
    {
        if (local)
        {
            networkPlayers[seat] = localPlayer.GetInternalPlayer();
        }
        else
        {
            GameObject newPlayer = new GameObject();
            newPlayer.transform.parent = transform;
            newPlayer.name = username;
            MonoNetworkPlayerBase player = null;
            player = newPlayer.AddComponent<MonoRemoteNetworkPlayer>();
            player.TurnTimeoutHandler = TurnTimeoutHandler;
            player.gameTable = GameTable;
            networkPlayers[seat] = player.InitializeNetworkPlayer();
            networkPlayers[seat].PlayerName = username;
        }

        if (networkPlayers[seat] != null)
        {
            SeatPlayer(networkPlayers[seat], seat);
        }

        if (IsRoomFilled())
        {
            GameTable.InitializeMultiPlayerGame(MultiplayerManager.CurrentEndCondition,MultiplayerManager.CurrentEndConditionGoal);
        }
    }

    public void SeatPlayer(NetworkPlayerBase networkPlayer, int seat)
    {
        if (networkPlayer.GetPlayersSeat() != -1)
		{
			networkPlayers[networkPlayer.GetPlayersSeat()] = null;
            GameTable.gameTable.LeaveTable(networkPlayer);
        }
        if (networkPlayers[seat] != null)
        {
            GameTable.gameTable.LeaveTable(seat);
        }
         
        networkPlayers[seat] = networkPlayer;
        GameTable.AddPlayer(networkPlayers[seat] as Player, seat);
    }


    public void DealCards(List<Common.Card>[] splittedCards)
    {
        MultiplayerManager.DealCallbackCount = 0;
        for(int k=0;k<4;k++){
            string networkMessage = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                if (i != 0)
                {
                    networkMessage += "|";
                }
                else
                {
                    networkMessage = "DEAL:";
                }
                for (int j = 0; j < 13; j++)
                {
                    if (j == 0)
                    {
                        networkMessage += splittedCards[i][j].ToNetworkCardString();
                    }
                    else
                    {
                        networkMessage += "," + splittedCards[i][j].ToNetworkCardString();
                    }
                }
            }
            if (networkPlayers[k].PlayerName != localPlayer.GetInternalPlayer().PlayerName)
            {
                WarpClient.GetInstance().sendPrivateUpdate(networkPlayers[k].PlayerName, ByteHelper.GetBytes(networkMessage));
            }
        }
    }
}
