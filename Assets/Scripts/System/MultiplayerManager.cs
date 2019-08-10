using UnityEngine;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.gaming.multiplayer.client;
using System.Collections.Generic;
using Common.Helpers;
using Common;
using Assets.Scripts.Game.Actors;

public class MultiplayerManager : MonoBehaviour {
    public MonoNetworkPlayer NonStaticPlayer;
	public static MonoNetworkPlayer StaticPlayer;
    private MultiplayerListener listener;
    public MultiplayerGamersCommunicator MultiplayerGamers;
    public ServiceAPI ServiceApi;
    public UserManagement UserManagement;
    public static bool useUDP = false;

    public static Common.Enums.GameMode CurrentEndCondition;
    public static object CurrentEndConditionGoal;

    public static int DealCallbackCount = 0;
    public static int DealCallback;

	// Use this for initialization
	void Start () {
        WarpClient.initialize("ad7a9695fd69c0d124977e2753fa6d3ae73a25d006e16d2192ed3548a220d11f", "b7b5f6797bc48d54628c14b98089ef64a3592deec4df49df7e19a5ac7a47c8de");
        ServiceApi = new ServiceAPI("ad7a9695fd69c0d124977e2753fa6d3ae73a25d006e16d2192ed3548a220d11f", "b7b5f6797bc48d54628c14b98089ef64a3592deec4df49df7e19a5ac7a47c8de");
 
        listener = GetComponent<MultiplayerListener>();
        WarpClient.GetInstance().AddConnectionRequestListener(listener);
        WarpClient.GetInstance().AddChatRequestListener(listener);
        WarpClient.GetInstance().AddLobbyRequestListener(listener);
        WarpClient.GetInstance().AddNotificationListener(listener);
        WarpClient.GetInstance().AddRoomRequestListener(listener);
        WarpClient.GetInstance().AddUpdateRequestListener(listener);
        WarpClient.GetInstance().AddZoneRequestListener(listener);

        UserManagement.InitializeUserManagement();

		StaticPlayer = NonStaticPlayer;
	}

    public static void connectToWarp(string username)
    {
		StaticPlayer.GetInternalPlayer().PlayerName=username;
        WarpClient.GetInstance().Connect(username);
    }

    void OnApplicationQuit()
    {
        if (LobbyPlayerStats.RoomData != null)
        {
            WarpClient.GetInstance().LeaveRoom(LobbyPlayerStats.RoomData.getId());
        }
        WarpClient.GetInstance().Disconnect();
    }

    public void onUserLeft(string user)
    {
        if (LobbyPlayerStats.RoomData!=null && LobbyPlayerStats.RoomData.getRoomOwner() == user && user != StaticPlayer.GetInternalPlayer().PlayerName)
        {
            WarpClient.GetInstance().LeaveRoom(LobbyPlayerStats.RoomData.getId());
            LobbyPlayerStats.Hide();
            string roomDeleteCommand = "ROOMDLT";
            RoomSelectionMenu.Show(roomDeleteCommand);
            RoomSelectionMenu.Activate();
        }
		else if (LobbyPlayerStats.RoomData != null && StaticPlayer.GetInternalPlayer().PlayerName == LobbyPlayerStats.RoomData.getRoomOwner())
        {
            int i = 0;
            int userSeat = -1;
            while (userSeat != -1 && i < 4)
            {
                if (LobbyPlayerStats.RoomProperties["SEAT" + i.ToString()].ToString() == user)
                {
                    userSeat = i;
                }
                i++;
            }
            if (userSeat != -1)
            {
                List<string> removeProperties = new List<string>();
                removeProperties.Add("SEAT" + userSeat.ToString());
                WarpClient.GetInstance().UpdateRoomProperties(LobbyPlayerStats.RoomData.getId(), null, removeProperties);
            }
        }
    }

    public void onMsg(string sender, string msg)
    {
        /*
        if(sender != username)
        {
			
        }
        */
    }

    public void onBytes(byte[] msg)
    {
        
        string networkMessage = ByteHelper.GetString(msg);

        LogManager.Log("Received Update: "+networkMessage);

        string[] messageArgs = networkMessage.Split('-');

        string messageCommand;
        if (messageArgs.Length == 0)
        {
            messageCommand = networkMessage;
        }
        else
        {
            messageCommand = messageArgs[0];
        }

        switch (messageCommand)
        {
            case "SIT":
                if (LobbyPlayerStats.RoomData.getRoomOwner() == NonStaticPlayer.GetInternalPlayer().PlayerName)
                {
                    int seat = -1;
                    if (int.TryParse(messageArgs[1], out seat))
                    {
                        string playerName = messageArgs[2];

                        //  SET ROOM PROPERTY
                        Dictionary<string, object> tableProperties = new Dictionary<string, object>();
                        tableProperties.Add("SEAT" + seat.ToString(), playerName);
                        List<string> removeProperties = new List<string>();

                        object seat0, seat1, seat2, seat3;
                        if (LobbyPlayerStats.RoomProperties.TryGetValue("SEAT0", out seat0) && seat0.ToString() == playerName)
                        {
                            removeProperties.Add("SEAT0");
                            NonStaticPlayer.gameTable.gameTable.LeaveTable(0);
                        }

                        if (LobbyPlayerStats.RoomProperties.TryGetValue("SEAT1", out seat1) && seat1.ToString() == playerName)
                        {
                            removeProperties.Add("SEAT1");
                            NonStaticPlayer.gameTable.gameTable.LeaveTable(1);
                        }

                        if (LobbyPlayerStats.RoomProperties.TryGetValue("SEAT2", out seat2) && seat2.ToString() == playerName)
                        {
                            removeProperties.Add("SEAT2");
                            NonStaticPlayer.gameTable.gameTable.LeaveTable(2);
                        }

                        if (LobbyPlayerStats.RoomProperties.TryGetValue("SEAT3", out seat3) && seat3.ToString() == playerName)
                        {
                            removeProperties.Add("SEAT3");
                            NonStaticPlayer.gameTable.gameTable.LeaveTable(3);
                        }

                        if (removeProperties.Count == 0)
                        {
                            removeProperties = null;
                        }

                        WarpClient.GetInstance().UpdateRoomProperties(LobbyPlayerStats.RoomData.getId(), tableProperties, removeProperties);

                    }
                }

                break;

            case "ROOMDLT":

                WarpClient.GetInstance().LeaveRoom(LobbyPlayerStats.RoomData.getId());
                LobbyPlayerStats.Hide();
                RoomSelectionMenu.Show("ROOMDLT");
                break;

            case "PLAY":
                int playerSeat,cardType,cardValue;
                if (int.TryParse(messageArgs[1], out playerSeat))
                {
                    if (playerSeat != NonStaticPlayer.GetInternalPlayer().GetPlayersSeat())
                    {
                        if (int.TryParse(messageArgs[2], out cardType))
                        {
                            if (int.TryParse(messageArgs[3], out cardValue))
                            {
                                RemoteNetworkPlayer networkPlayer = MultiplayerGamers.GetPlayerBySeat(playerSeat) as RemoteNetworkPlayer;
                                Card cardToPlay = new Card(cardValue, cardType);
                                networkPlayer.PlayCard(cardToPlay);
                            }
                        }
                    }
                }

                break;

            case "BID":
                int bidPlayerSeat,bid;
                if (int.TryParse(messageArgs[1], out bidPlayerSeat))
                {
                    if (bidPlayerSeat != NonStaticPlayer.GetInternalPlayer().GetPlayersSeat())
                    {
                        if (int.TryParse(messageArgs[2], out bid))
                        {
                            NetworkPlayerBase networkPlayer = MultiplayerGamers.GetPlayerBySeat(bidPlayerSeat);
                            if (bid == 999)
                            {
                                networkPlayer.Bid(-1);
                            }
                            else
                            {
                                networkPlayer.Bid(bid);
                            }
                        }
                    }
                }
                break;

            case "SETTRMP":
                int trumpPlayerSeat,trumpType;
                if (int.TryParse(messageArgs[1], out trumpPlayerSeat))
                {
                    if (trumpPlayerSeat != NonStaticPlayer.GetInternalPlayer().GetPlayersSeat())
                    {
                        if (int.TryParse(messageArgs[2], out trumpType))
                        {
                            NetworkPlayerBase networkPlayer = MultiplayerGamers.GetPlayerBySeat(trumpPlayerSeat);
                            networkPlayer.SetTrumpType(CardHelper.GetCardTypeFromValue(trumpType));
                        }
                    }
                }
                break;

            case "DEALCALLBACK":
                IncreaseDealCallback();
                break;

            case "START":
                if (LobbyPlayerStats.RoomData.getRoomOwner() != NonStaticPlayer.GetInternalPlayer().PlayerName)
                {
                    MultiplayerGamers.GameTable.gameTable.PlayersReadyCallback();
                }
                break;
        }

        LogManager.Log(networkMessage);
      
    }

    protected void IncreaseDealCallback()
    {
        if (LobbyPlayerStats.RoomData.getRoomOwner() == NonStaticPlayer.GetInternalPlayer().PlayerName)
        {
            DealCallbackCount++;
            if (DealCallbackCount == 3)
            {
                if (DealCallback == 0)
                {
                    MultiplayerGamers.GameTable.gameTable.PlayersReadyCallback();
                }
                else
                {
                    ((UnityGame)MultiplayerGamers.GameTable.gameTable.GetGameInstance()).CommenceRoundCallBack();
                }
            }
        }
    }

    public static void SendBytes(byte[] message)
    {
        if (useUDP)
        {
            WarpClient.GetInstance().SendUDPUpdatePeers(message);
        }
        else
        {
            WarpClient.GetInstance().SendUpdatePeers(message);
        }
        
    }
}
