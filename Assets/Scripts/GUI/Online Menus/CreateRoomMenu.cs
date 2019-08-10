using UnityEngine;
using System.Collections;
using Assets.Scripts.System;
using com.shephertz.app42.gaming.multiplayer.client;
using System.Collections.Generic;

public class CreateRoomMenu : MonoBehaviour {
    public CreateRoomResultMenu CreateRoomResultMenu;
    public RoomSelectionMenu RoomSelectionMenu;
    public GUISkin CreateRoomMenuSkin;
    private string roomName = "", pw = "", gameGoal="5";
    public Texture2D Background;

    private int gameTypeSelection = 0;
    private string[] selStrings = {"",""};

    private static Transform transformInstance;
    public MonoNetworkPlayer Player;

	// Use this for initialization
	void Start () {
        transformInstance = transform;
        StartCoroutine(GetSelStrings());
	}

    IEnumerator GetSelStrings()
    {
        yield return null;
        while (!LanguageManager.IsLoaded)
        {
            //  IDLE WAIT
        }
        selStrings = new string[] { LanguageManager.getString("RNDCNT"), LanguageManager.getString("TRGTSCR") };
        Hide();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
            RoomSelectionMenu.Show();
        }
    }

    public static void Show()
    {
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);
    }

    private IEnumerator createRoom(Dictionary<string,object> tableProperties)
    {
        LoadingScreen.Show();
        yield return null;
        WarpClient.GetInstance().CreateRoom(roomName, Player.GetInternalPlayer().PlayerName, 4, tableProperties);
    }

    void OnGUI()
    {
        if (LanguageManager.IsLoaded && selStrings!=null)
        {
            GUI.depth = 10;
            GUI.skin = CreateRoomMenuSkin;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Background);

            if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
            {
                Hide();
                RoomSelectionMenu.Show();
            }

            float offset = Screen.height / 20 + Properties.BackButtonSize;

            float button_size = Screen.width / 9 * 4;
            GUI.Box(new Rect(Screen.width / 2 - button_size - Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("ROOMNM"));
            roomName = GUI.TextField(new Rect(Screen.width / 2 + Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), roomName);

            offset += Properties.TEXT_HEIGHT * 3;
            GUI.Box(new Rect(Screen.width / 2 - button_size - Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("ROOMPW"));
            pw = GUI.PasswordField(new Rect(Screen.width / 2 + Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), pw, '*');

            offset += Properties.TEXT_HEIGHT * 3;

            GUI.Box(new Rect(Screen.width / 2 - button_size, offset, button_size * 2, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("GMENDCNDTN"));
            offset += Properties.TEXT_HEIGHT * 2 + Properties.TEXT_HEIGHT / 3;
            gameTypeSelection = GUI.SelectionGrid(new Rect(Screen.width / 2 - button_size, offset, button_size * 2, Properties.TEXT_HEIGHT * 2), gameTypeSelection, selStrings, 2);

            offset += Properties.TEXT_HEIGHT * 3;
            string condition = string.Empty;
            if (gameTypeSelection == 0)
            {
                condition = LanguageManager.getString("HNDCNT");
            }
            else
            {
                condition = LanguageManager.getString("TRGTSCR");
            }
            GUI.Box(new Rect(Screen.width / 2 - button_size - Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), condition);
            gameGoal = GUI.TextField(new Rect(Screen.width / 2 + Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), gameGoal);

            bool isValid = ValidateGoalInput();

            offset += Properties.TEXT_HEIGHT * 3;
            if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("CRTROOM")) && !string.IsNullOrEmpty(roomName) && isValid)
            {
                Dictionary<string, object> tableProperties = new Dictionary<string, object>();
                tableProperties.Add("TYPE", gameTypeSelection);
                tableProperties.Add("GOAL", gameGoal);
                tableProperties.Add("PW", pw);
                CreateRoomResultMenu.CreateResult = -2;
                StartCoroutine(createRoom(tableProperties));
            }

            GUI.EndGroup();
        }
    }

    private bool ValidateGoalInput()
    {
        int temp;
        if (int.TryParse(gameGoal, out temp))
        {
            if (temp > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
