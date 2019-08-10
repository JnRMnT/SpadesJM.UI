using UnityEngine;
using System.Collections;
using Game.Actors;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System.Collections.Generic;
using com.shephertz.app42.gaming.multiplayer.client;
using Assets.Scripts.System;

public class LobbyPlayerStats : MonoBehaviour
{
    private Rect[] playerStatPositions;
    public GUISkin playerStatsGUISkin, buttonSkin;
    public UIGameTable gameTable;
    public MonoNetworkPlayer player;
    public MultiplayerGamersCommunicator MultiplayerGamers;

    private static Transform transformInstance;

    private static bool dataNewlyBound;
    public static RoomData RoomData;
    public static Dictionary<string,object> RoomProperties;

    public static bool IsShown;

    // Use this for initialization
    void Start()
    {
        float statSizeX = Screen.width / 2;
        float statSizeY = Screen.height / 13;

        playerStatPositions = new Rect[4];
        playerStatPositions[0] = new Rect(Screen.width / 2 - statSizeX / 2, Screen.height - statSizeY, statSizeX, statSizeY);
        playerStatPositions[1] = new Rect(Screen.width - statSizeY, Screen.height / 2 - statSizeX, statSizeY, statSizeX);
        playerStatPositions[2] = new Rect(Screen.width / 2 - statSizeX / 2, 0, statSizeX, statSizeY);
        playerStatPositions[3] = new Rect(0, Screen.height / 2 - statSizeX, statSizeY, statSizeX);
        transformInstance = transform;

        Hide();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InGameMenu.Show();
        }

        if (dataNewlyBound)
        {
            dataNewlyBound = false;

            MultiplayerGamers.ResetPlayers();
            UIGameTable.CleanTable();

            //  GET CURRENT PLAYER SEATS
            object seat0,seat1,seat2,seat3;
            if (RoomProperties.TryGetValue("SEAT0", out seat0))
            {
                MultiplayerGamers.UserJoined(seat0.ToString(), 0, seat0.ToString().Equals(player.GetInternalPlayer().PlayerName));
            }

            if (RoomProperties.TryGetValue("SEAT1", out seat1))
            {
                MultiplayerGamers.UserJoined(seat1.ToString(), 1, seat1.ToString().Equals(player.GetInternalPlayer().PlayerName));
            }

            if (RoomProperties.TryGetValue("SEAT2", out seat2))
            {
                MultiplayerGamers.UserJoined(seat2.ToString(), 2, seat2.ToString().Equals(player.GetInternalPlayer().PlayerName));
            }

            if (RoomProperties.TryGetValue("SEAT3", out seat3))
            {
                MultiplayerGamers.UserJoined(seat3.ToString(), 3, seat3.ToString().Equals(player.GetInternalPlayer().PlayerName));
            }

            object gameType, gameGoal;

            if (RoomProperties.TryGetValue("TYPE", out gameType))
            {
                if (gameType.ToString() == "0")
                {
                    MultiplayerManager.CurrentEndCondition = Common.Enums.GameMode.RoundCount;
                }
                else
                {
                    MultiplayerManager.CurrentEndCondition = Common.Enums.GameMode.TargetScore;
                }
                
            }
            if (RoomProperties.TryGetValue("GOAL", out gameGoal))
            {
                MultiplayerManager.CurrentEndConditionGoal = gameGoal;
            }

        }
    }

    public static void BindRoomData(RoomData data,Dictionary<string,object> properties)
    {
        dataNewlyBound = true;
        LobbyPlayerStats.RoomData = data;
        LobbyPlayerStats.RoomProperties = properties;
    }

    public static void Show()
    {
        IsShown = true;
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        IsShown = false;
        transformInstance.gameObject.SetActive(false);

    }

    private int NextSeat(int seat)
    {
        return (seat + 1) % 4;
    }

    void OnGUI()
    {
        GUI.depth = 100;

        GUI.skin = buttonSkin;
        if (GUI.Button(new Rect(Screen.width / 80, Screen.height / 40, Screen.width / 9, Screen.width / 9), Properties.MenuButton))
        {
            InGameMenu.Show();
        }

        NetworkPlayerBase mainPlayer = player.GetInternalPlayer() as NetworkPlayerBase;

        Player[] multiplayerPlayers = new Player[4];

        int mainPlayersSeat = mainPlayer.GetPlayersSeat();
        if (mainPlayersSeat != -1)
        {
            //  PLAYER IS SEATED
            multiplayerPlayers[0] = (NetworkPlayerBase) mainPlayer;
            int nextSeat = NextSeat(mainPlayersSeat);
            multiplayerPlayers[1] = MultiplayerGamers.GetPlayerBySeat(nextSeat);
            nextSeat = NextSeat(nextSeat);
            multiplayerPlayers[2] = MultiplayerGamers.GetPlayerBySeat(nextSeat);
            nextSeat = NextSeat(nextSeat);
            multiplayerPlayers[3] = MultiplayerGamers.GetPlayerBySeat(nextSeat);
        }
        else
        {
            //  PLAYER HAS NOT SIT DOWN
            for (int i = 0; i < 4; i++)
            {
                multiplayerPlayers[i] = MultiplayerGamers.GetPlayerBySeat(i);
            }
        }

        GUI.skin = playerStatsGUISkin;
        //  PLAYER 1
        GUI.BeginGroup(playerStatPositions[0]);
        if (multiplayerPlayers[0] != null)
        {
            GUI.Box(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), multiplayerPlayers[0].PlayerName);
        }
        else if(GUI.Button(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height),LanguageManager.getString("SIT")))
        {
            //  SIT TO FIRST POSITION
            ((LocalNetworkPlayer)mainPlayer).SitToTable(0,MultiplayerGamers);
        }
        GUI.EndGroup();
        GUI.matrix = Matrix4x4.identity;

        //  PLAYER 2
        Matrix4x4 m = Matrix4x4.identity;
        m.SetTRS(new Vector3(playerStatPositions[1].x, playerStatPositions[1].y + playerStatPositions[1].height, 1), Quaternion.Euler(0, 0, -90), Vector3.one);
        GUI.matrix = m;
        if (multiplayerPlayers[1] != null)
        {
            GUI.Box(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), multiplayerPlayers[1].PlayerName);
        }
        else if (GUI.Button(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), LanguageManager.getString("SIT"))){
            //  SIT TO SECOND POSITION
            ((LocalNetworkPlayer)mainPlayer).SitToTable(1, MultiplayerGamers);
        }
        GUI.matrix = Matrix4x4.identity;

        //  PLAYER 3
        m = Matrix4x4.identity;
        m.SetTRS(new Vector3(playerStatPositions[2].x + playerStatPositions[2].width, playerStatPositions[2].y + playerStatPositions[2].height, 1), Quaternion.Euler(0, 0, -180), Vector3.one);
        GUI.matrix = m;
        if (multiplayerPlayers[2] != null)
        {
            GUI.Box(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), multiplayerPlayers[2].PlayerName);
        }
        else if (GUI.Button(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), LanguageManager.getString("SIT")))
        {
            //  SIT TO THIRD POSITION
            ((LocalNetworkPlayer)mainPlayer).SitToTable(2, MultiplayerGamers);
        }
        GUI.matrix = Matrix4x4.identity;

        //  PLAYER 4
        m = Matrix4x4.identity;
        m.SetTRS(new Vector3(playerStatPositions[3].x + playerStatPositions[3].width, playerStatPositions[3].y, 1), Quaternion.Euler(0, 0, 90), Vector3.one);
        GUI.matrix = m;
        if (multiplayerPlayers[3] != null)
        {
            GUI.Box(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), multiplayerPlayers[3].PlayerName);
        }
        else if (GUI.Button(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), LanguageManager.getString("SIT")))
        {
            //  SIT TO FOURTH POSITION
            ((LocalNetworkPlayer)mainPlayer).SitToTable(3, MultiplayerGamers);
        }
        GUI.matrix = Matrix4x4.identity;

    }
}
