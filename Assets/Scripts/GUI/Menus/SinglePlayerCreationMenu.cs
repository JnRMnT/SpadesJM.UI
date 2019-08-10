using UnityEngine;
using System.Collections;
using Assets.Scripts.System;

public class SinglePlayerCreationMenu : MonoBehaviour
{
    public GUISkin GameCreationMenuSkin;
    public Texture2D Background, Logo;

    public static Transform transformInstance;
    public UIGameTable GameTable;
    private static float LOGO_SIZE = Screen.height / 4;
    public Transform MultiplayerObjects;
    private int gameTypeSelection = 0;
    private string[] selStrings = { "", "" };
    private string gameGoal = "5";

    void Start()
    {
        transformInstance = transform;
        StartCoroutine(GetSelStrings());
    }


    public static void Show()
    {
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);

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
            MainMenu.Show();
            Hide();
        }
    }

    void OnGUI()
    {

        GUI.depth = 10;
        GUI.skin = GameCreationMenuSkin;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Background);

        if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
        {
            MainMenu.Show();
            Hide();
        }


        float offset = Screen.height / 20;
        GUI.DrawTexture(new Rect(Screen.width / 2 - LOGO_SIZE, offset, LOGO_SIZE * 2, LOGO_SIZE), Logo);

        offset += LOGO_SIZE + offset;

        float button_size = Screen.width / 9 * 4;

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
        if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("STRTGM")) && isValid)
        {
            LoadingScreen.Show();
            Properties.ActiveGameType = GameType.SinglePlayer;
            MultiplayerObjects.gameObject.SetActive(false);
            TrumpSelection.Hide();
            ScoreBoard.Hide();
            PlayerStats.Show();
            UIGameTable.CleanTable();
            StartCoroutine(StartSinglePlayerGame());
        }

        GUI.EndGroup();

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

    IEnumerator StartSinglePlayerGame()
    {
        yield return null;
        int intGameGoal;
        int.TryParse(gameGoal, out intGameGoal);
        if (gameTypeSelection == 0)
        {
            GameTable.InitializeSinglePlayerGame(Common.Enums.GameMode.RoundCount, intGameGoal);
        }
        else
        {
            GameTable.InitializeSinglePlayerGame(Common.Enums.GameMode.TargetScore, intGameGoal);
        }
        
        UserInteraction.InGame = true;
        LoadingScreen.Hide();
        Hide();
    }
}
