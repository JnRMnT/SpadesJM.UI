using UnityEngine;
using System.Collections;

using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client.message;
using com.shephertz.app42.gaming.multiplayer.client.transformer;

using System;
using System.Collections.Generic;
using Common;
using System.Runtime.CompilerServices;
using Common.Helpers;

public class MultiplayerListener : MonoBehaviour, ConnectionRequestListener, LobbyRequestListener, ZoneRequestListener, RoomRequestListener, ChatRequestListener, UpdateRequestListener, NotifyListener
{
    int state = 0;
    string debug = "";

    private MultiplayerManager m_apppwarp;
    private MultiplayerGamersCommunicator multiplayerGamers;
    public RoomSelectionMenu RoomSelectionMenu;
    public MonoNetworkPlayer LocalPlayer;

    public static int ActivePage = -1;

    private void Log(string msg)
    {
        debug = msg + "\n" + debug;
        LogManager.Log(msg);
    }

    public string getDebug()
    {
        return debug;
    }

    //Mono Behaviour
    void Start()
    {
        m_apppwarp = transform.GetComponent<MultiplayerManager>();
        multiplayerGamers = transform.parent.FindChild("MultiplayerGamers").GetComponent<MultiplayerGamersCommunicator>();
    }

    //ConnectionRequestListener
    public void onConnectDone(ConnectEvent eventObj)
    {
        Log("onConnectDone : " + eventObj.getResult());

        if (MultiplayerManager.useUDP)
        {
            WarpClient.GetInstance().initUDP();
        }

        if (LoginResultScreen.login_result == 0 && eventObj.getResult() == 0)
        {
            LoginResultScreen.RedirectToRoomSelection();
        }
        else
        {
            LoginResultScreen.login_result = eventObj.getResult();
        }
    }

    public void onInitUDPDone(byte res)
    {
    }

    public void onLog(String message)
    {
        Log(message);
    }

    public void onDisconnectDone(ConnectEvent eventObj)
    {
        Log("onDisconnectDone : " + eventObj.getResult());
    }

    //LobbyRequestListener
    public void onJoinLobbyDone(LobbyEvent eventObj)
    {
        Log("onJoinLobbyDone : " + eventObj.getResult());
        if (eventObj.getResult() == 0)
        {
            state = 1;
        }
    }

    public void onLeaveLobbyDone(LobbyEvent eventObj)
    {
        Log("onLeaveLobbyDone : " + eventObj.getResult());
    }

    public void onSubscribeLobbyDone(LobbyEvent eventObj)
    {
        Log("onSubscribeLobbyDone : " + eventObj.getResult());
        if (eventObj.getResult() == 0)
        {
            WarpClient.GetInstance().JoinLobby();
        }
    }

    public void onUnSubscribeLobbyDone(LobbyEvent eventObj)
    {
        Log("onUnSubscribeLobbyDone : " + eventObj.getResult());
    }

    public void onGetLiveLobbyInfoDone(LiveRoomInfoEvent eventObj)
    {
        Log("onGetLiveLobbyInfoDone : " + eventObj.getResult());
    }

    //ZoneRequestListener
    public void onDeleteRoomDone(RoomEvent eventObj)
    {
        Log("onDeleteRoomDone : " + eventObj.getResult());
    }

    public void onGetAllRoomsDone(AllRoomsEvent eventObj)
    {
        if (eventObj.getResult() == 0)
        {
            string[] roomIds = eventObj.getRoomIds();

            if (roomIds == null)
            {
                RoomSelectionMenu.RoomCount = 0;
                RoomSelectionMenu.Activate();
            }
            else
            {
                RoomSelectionMenu.ActiveRooms = new List<LiveRoomInfoEvent>();
                foreach (string roomId in roomIds)
                {
                    WarpClient.GetInstance().GetLiveRoomInfo(roomId);
                }

                RoomSelectionMenu.RoomCount = roomIds.Length;
            }
        }
        else
        {
            RoomSelectionMenu.RoomCount = 0;
            RoomSelectionMenu.Activate();
        }
        Log("onGetAllRoomsDone : " + eventObj.getResult());
    }

    public void onCreateRoomDone(RoomEvent eventObj)
    {
        CreateRoomMenu.Hide();
        LoadingScreen.Hide();
        if (eventObj.getResult() == 0)
        {
            WarpClient.GetInstance().SubscribeRoom(eventObj.getData().getId());
            JoinRoomResultScreen.JoinResult = -2;
            JoinRoomResultScreen.Show();
            JoinRoomResultScreen.TryAgain(eventObj.getData().getId());
            CreateRoomResultMenu.Hide();
        }
        else
        {
            CreateRoomResultMenu.Show();
            CreateRoomResultMenu.CreateResult = 5;
        }
       
        Log("onCreateRoomDone : " + eventObj.getResult());
    }

    public void onGetOnlineUsersDone(AllUsersEvent eventObj)
    {
        Log("onGetOnlineUsersDone : " + eventObj.getResult());
    }

    public void onGetLiveUserInfoDone(LiveUserInfoEvent eventObj)
    {
        Log("onGetLiveUserInfoDone : " + eventObj.getResult());
    }

    public void onSetCustomUserDataDone(LiveUserInfoEvent eventObj)
    {
        Log("onSetCustomUserDataDone : " + eventObj.getResult());
    }

    public void onGetMatchedRoomsDone(MatchedRoomsEvent eventObj)
    {
        if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
        {
            Log("GetMatchedRooms event received with success status");
            foreach (var roomData in eventObj.getRoomsData())
            {
                Log("Room ID:" + roomData.getId());
            }
        }
    }
    //RoomRequestListener
    public void onSubscribeRoomDone(RoomEvent eventObj)
    {
        if (eventObj.getResult() == 0)
        {
            /*string json = "{\"start\":\""+id+"\"}";
                    WarpClient.GetInstance().SendChat(json);
                    state = 1;*/
            //WarpClient.GetInstance().JoinRoom(m_apppwarp.roomid);
        }

        Log("onSubscribeRoomDone : " + eventObj.getResult());
    }

    public void onUnSubscribeRoomDone(RoomEvent eventObj)
    {
        Log("onUnSubscribeRoomDone : " + eventObj.getResult());
    }

    public void onJoinRoomDone(RoomEvent eventObj)
    {
        if (eventObj.getResult() == 0)
        {
            state = 1;
            //  JOIN
            ActivePage = 2;
            WarpClient.GetInstance().SubscribeRoom(eventObj.getData().getId());
            WarpClient.GetInstance().GetLiveRoomInfo(eventObj.getData().getId());
        }
        else
        {
            JoinRoomResultScreen.JoinResult = eventObj.getResult();
        }
        Log("onJoinRoomDone : " + eventObj.getResult());

    }

    public void onLockPropertiesDone(byte result)
    {
        Log("onLockPropertiesDone : " + result);
    }

    public void onUnlockPropertiesDone(byte result)
    {
        Log("onUnlockPropertiesDone : " + result);
    }

    public void onLeaveRoomDone(RoomEvent eventObj)
    {
        if (WarpClient.GetInstance() != null && eventObj != null && eventObj.getData() != null)
        {
            WarpClient.GetInstance().UnsubscribeRoom(eventObj.getData().getId());
        }
        Log("onLeaveRoomDone : " + eventObj.getResult());
    }

    public void onGetLiveRoomInfoDone(LiveRoomInfoEvent eventObj)
    {
        if (ActivePage == 1)
        {
            //  ROOM SELECTION
            if (eventObj.getResult() == 0)
            {
                RoomSelectionMenu.AddRoom(eventObj);
            }
            else
            {
                RoomSelectionMenu.RoomCount--;
            }
        }
        else if (ActivePage == 2)
        {
            if (eventObj.getResult() == 0)
            {
                //  JOIN ROOM
                LobbyPlayerStats.BindRoomData(eventObj.getData(), eventObj.getProperties());
                JoinRoomResultScreen.Hide();
                LobbyPlayerStats.Show();
            }
        }
        Log("onGetLiveRoomInfoDone : " + eventObj.getResult());
    }

    public void onSetCustomRoomDataDone(LiveRoomInfoEvent eventObj)
    {
        Log("onSetCustomRoomDataDone : " + eventObj.getResult());
    }

    public void onUpdatePropertyDone(LiveRoomInfoEvent eventObj)
    {
        if (WarpResponseResultCode.SUCCESS == eventObj.getResult())
        {
            Log("UpdateProperty event received with success status");
        }
        else
        {
            Log("Update Propert event received with fail status. Status is :" + eventObj.getResult().ToString());
        }
    }

    //ChatRequestListener
    public void onSendChatDone(byte result)
    {
        Log("onSendChatDone result : " + result);

    }

    public void onSendPrivateChatDone(byte result)
    {
        Log("onSendPrivateChatDone : " + result);
    }

    //UpdateRequestListener
    public void onSendUpdateDone(byte result)
    {
        Log("onSendUpdateDone : "+result);
    }

    //NotifyListener
    public void onRoomCreated(RoomData eventObj)
    {
        Log("onRoomCreated");
    }

    public void onRoomDestroyed(RoomData eventObj)
    {
        Log("onRoomDestroyed");
    }

    public void onUserLeftRoom(RoomData eventObj, string username)
    {
        multiplayerGamers.UserLeft(username);
        Log("onUserLeftRoom : " + username);
        m_apppwarp.onUserLeft(username);
    }

    public void onUserJoinedRoom(RoomData eventObj, string username)
    {
        Log("onUserJoinedRoom : " + username);
    }

    public void onUserLeftLobby(LobbyData eventObj, string username)
    {
        Log("onUserLeftLobby : " + username);
    }

    public void onUserJoinedLobby(LobbyData eventObj, string username)
    {
        Log("onUserJoinedLobby : " + username);
    }

    public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable)
    {
        LobbyPlayerStats.BindRoomData(roomData, properties);
        Log("onUserChangeRoomProperty : " + sender);
    }

    public void onPrivateChatReceived(string sender, string message)
    {
        Log("onPrivateChatReceived : " + sender);
    }

    public void onMoveCompleted(MoveEvent move)
    {
        Log("onMoveCompleted by : " + move.getSender());
    }

    public void onChatReceived(ChatEvent eventObj)
    {
        Log(eventObj.getSender() + " sent Message");
        m_apppwarp.onMsg(eventObj.getSender(), eventObj.getMessage());
    }

    public void onUpdatePeersReceived(UpdateEvent eventObj)
    {
        Log ("onUpdatePeersReceived");
        m_apppwarp.onBytes(eventObj.getUpdate());
        //Log("isUDP " + eventObj.getIsUdp());
    }

    public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<String, System.Object> properties)
    {
        Log("Notification for User Changed Room Property received");
        Log(roomData.getId());
        Log(sender);
        foreach (KeyValuePair<String, System.Object> entry in properties)
        {
            Log("KEY:" + entry.Key);
            Log("VALUE:" + entry.Value.ToString());
        }
    }

    public void sendMsg(string msg)
    {
        if (state == 1)
        {
            WarpClient.GetInstance().SendChat(msg);
        }
    }

    public void sendBytes(byte[] msg, bool useUDP)
    {
        if (state == 1)
        {
            if (useUDP == true)
                WarpClient.GetInstance().SendUDPUpdatePeers(msg);
            else
                WarpClient.GetInstance().SendUpdatePeers(msg);
        }
    }

    public void onUserPaused(string a, bool b, string c)
    {
    }

    public void onUserResumed(string a, bool b, string c)
    {
    }

    public void onGameStarted(string a, string b, string c)
    {
    }

    public void onGameStopped(string a, string b)
    {
    }

    public void onSendPrivateUpdateDone(byte result)
    {
        LogManager.Log(result.ToString());
    }


    public void onPrivateUpdateReceived(string sender, byte[] update, bool fromUdp)
    {
		LogManager.Log(sender.ToString()+": "+ByteHelper.GetString(update));

        string networkMessage = ByteHelper.GetString(update);

        string[] splittedMessage = networkMessage.Split(':');

        switch (splittedMessage[0])
        {
            case "DEAL":

                string[] cardsString = splittedMessage[1].Split('|');
                List<Card>[] splittedCards= new List<Card>[4];
                
                for (int i = 0; i < 4; i++)
                {
                    splittedCards[i] = new List<Card>();
                    string[] cards = cardsString[i].Split(',');
                    for (int j = 0; j < 13; j++)
                    {
                        splittedCards[i].Add(CardHelper.StringToCard(cards[j]));
                    }
                }
                
                multiplayerGamers.GameTable.gameTable.ReceiveCards(splittedCards);
                MultiplayerManager.SendBytes(ByteHelper.GetBytes("DEALCALLBACK"));
                break;
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
        {
            LogManager.Log("Resume");
        }
    }
}
