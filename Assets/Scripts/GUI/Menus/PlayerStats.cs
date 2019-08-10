using UnityEngine;
using System.Collections;
using Assets.Scripts.System;

public class PlayerStats : MonoBehaviour {
    private Rect[] playerStatPositions;
    public GUISkin playerStatsGUISkin,buttonSkin;
    public UIGameTable gameTable;
    public MonoPlayer player;

    public static Transform transformInstance;

    private static bool isShown;
    public static bool IsShown { get { return isShown; } }

	// Use this for initialization
	void Start () {
        float statSizeX = Screen.width/2;
        float statSizeY = Screen.height/13;

        playerStatPositions = new Rect[4];
        playerStatPositions[0] = new Rect(Screen.width/2-statSizeX/2,Screen.height-statSizeY,statSizeX,statSizeY);
        playerStatPositions[1] = new Rect(Screen.width - statSizeY, Screen.height/2 - statSizeX, statSizeY, statSizeX);
        playerStatPositions[2] = new Rect(Screen.width / 2 - statSizeX / 2, 0 , statSizeX, statSizeY);
        playerStatPositions[3] = new Rect(0, Screen.height / 2 - statSizeX, statSizeY, statSizeX);

        transformInstance = this.transform;

        Hide();
	}

    public static void Show()
    {
        isShown = true;
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        isShown = false;
        transformInstance.gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InGameMenu.Show();
        }
	}

    void OnGUI()
    {
        if (gameTable.gameTable.GetGameState() != Common.Enums.GameState.PLAYING)
        {
            return;
        }

        GUI.depth = 100;

        GUI.skin = buttonSkin;
        if (GUI.Button(new Rect(Screen.width / 80, Screen.height / 40, Screen.width / 9, Screen.width / 9), Properties.MenuButton))
        {
            InGameMenu.Show();
        }

        var internalPlayer = player.getInternalPlayer();
        var player2 = internalPlayer.GetNextPlayer();
        var player3 = player2.GetNextPlayer();
        var player4 = player3.GetNextPlayer();

        var roundScores = gameTable.gameTable.GetGameInstance().GetCurrentRound().GetRoundScores();

        string player1Text = internalPlayer.PlayerName;
        string player2Text = player2.PlayerName;
        string player3Text = player3.PlayerName;
        string player4Text = player4.PlayerName;

        if (roundScores != null && roundScores.Length == 4)
        {
            var player1Said = roundScores[internalPlayer.GetPlayersSeat()].GetSaid().ToString();
            var player2Said = roundScores[player2.GetPlayersSeat()].GetSaid().ToString();
            var player3Said = roundScores[player3.GetPlayersSeat()].GetSaid().ToString();
            var player4Said = roundScores[player4.GetPlayersSeat()].GetSaid().ToString();

            if(player1Said == "-2")player1Said = "-";
            else if(player1Said == "-1")player1Said = LanguageManager.getString("PASS");

            if (player2Said == "-2") player2Said = "-";
            else if (player2Said == "-1") player2Said = LanguageManager.getString("PASS");

            if (player3Said == "-2") player3Said = "-";
            else if (player3Said == "-1") player3Said = LanguageManager.getString("PASS");

            if (player4Said == "-2") player4Said = "-";
            else if (player4Said == "-1") player4Said = LanguageManager.getString("PASS");

            player1Text += "\n" + roundScores[internalPlayer.GetPlayersSeat()].GetGot() + " \\ " + player1Said;
            player2Text += "\n" + roundScores[player2.GetPlayersSeat()].GetGot() + " \\ " + player2Said;
            player3Text += "\n" + roundScores[player3.GetPlayersSeat()].GetGot() + " \\ " + player3Said;
            player4Text += "\n" + roundScores[player4.GetPlayersSeat()].GetGot() + " \\ " + player4Said;
        }
        GUI.skin = playerStatsGUISkin;
        //  PLAYER 1
        GUI.BeginGroup(playerStatPositions[0]);
        GUI.Box(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), player1Text);
        GUI.EndGroup();
        GUI.matrix = Matrix4x4.identity;

        //  PLAYER 2
        Matrix4x4 m = Matrix4x4.identity;
        m.SetTRS(new Vector3(playerStatPositions[1].x, playerStatPositions[1].y + playerStatPositions[1].height, 1), Quaternion.Euler(0, 0, -90), Vector3.one);
        GUI.matrix = m;
        GUI.Box(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), player2Text);
        GUI.matrix = Matrix4x4.identity;

        //  PLAYER 3
        m = Matrix4x4.identity;
        m.SetTRS(new Vector3(playerStatPositions[2].x + playerStatPositions[2].width, playerStatPositions[2].y + playerStatPositions[2].height, 1), Quaternion.Euler(0, 0, -180), Vector3.one);
        GUI.matrix = m;
        GUI.Box(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), player3Text);
        GUI.matrix = Matrix4x4.identity;

        //  PLAYER 4
        m = Matrix4x4.identity;
        m.SetTRS(new Vector3(playerStatPositions[3].x + playerStatPositions[3].width, playerStatPositions[3].y, 1), Quaternion.Euler(0, 0, 90), Vector3.one);
        GUI.matrix = m;
        GUI.Box(new Rect(0, 0, playerStatPositions[0].width, playerStatPositions[0].height), player4Text);
        GUI.matrix = Matrix4x4.identity;

    }
}
