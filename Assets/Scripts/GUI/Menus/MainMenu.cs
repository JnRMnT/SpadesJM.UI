using UnityEngine;
using System.Collections;
using Assets.Scripts.System;
using com.shephertz.app42.gaming.multiplayer.client;

public class MainMenu : MonoBehaviour {
    public UIGameTable GameTable;
    public LoginScreen LoginScreen;

    public GUISkin MainMenuSkin;
    public Texture2D Background;
    public Texture2D Logo;
    public Texture2D BackButton,MenuButton;

    public Transform MultiplayerObjects;

    private static Transform transformInstance;

    private static float LOGO_SIZE = Screen.height / 4;

	// Use this for initialization
	void Start () {
        transformInstance = this.transform;

        Properties.BackButtonSize = Screen.width / 9;
        Properties.BackButton = BackButton;
        Properties.MenuButton = MenuButton;
	}

    public static void Show()
    {
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DialogBox.DialogBoxButtons = DialogBoxButtons.YES_NO;
            DialogBox.DialogText = LanguageManager.getString("PROMPT_EXIT");
            DialogBox.CallbackObject = gameObject;
            DialogBox.Show();
        }
	}

    public void DialogBoxCallback(bool answer)
    {
        if (answer)
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {

        GUI.depth = 10;
        GUI.skin = MainMenuSkin;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Background);

        if (Application.platform != RuntimePlatform.IPhonePlayer && GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
        {
            DialogBox.DialogBoxButtons = DialogBoxButtons.YES_NO;
            DialogBox.DialogText = LanguageManager.getString("PROMPT_EXIT");
            DialogBox.CallbackObject = gameObject;
            DialogBox.Show();
        }

        float offset = Screen.height / 20;
        GUI.DrawTexture(new Rect(Screen.width / 2 - LOGO_SIZE, offset, LOGO_SIZE * 2, LOGO_SIZE), Logo);

        offset += LOGO_SIZE + offset;

        float button_size = Screen.width / 3 * 2;

        if (UserInteraction.InGame)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("CNTNE")))
            {
                PlayerStats.Show();
                Hide();
            }
            offset += Properties.TEXT_HEIGHT * 3;
        }

        if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("SNGPLYR")))
        {
            Hide();
            SinglePlayerCreationMenu.Show();
        }

        offset += Properties.TEXT_HEIGHT * 3;

        if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("MLTPLYR")))
        {
            TrumpSelection.Hide();
            ScoreBoard.Hide();
            Properties.ActiveGameType = GameType.MultiPlayer;
            MultiplayerObjects.gameObject.SetActive(true);
            LoginScreen.Show();
            UserInteraction.InGame = false;
            Hide();
        }

        offset += Properties.TEXT_HEIGHT * 3;
        if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("CRDTSBTN")))
        {
            Hide();
            CreditsMenu.Show();
        }

        offset += Properties.TEXT_HEIGHT * 3;
        if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("STNGS")))
        {
            Hide();
            SettingsMenu.BackMenu = this.GetType();
            SettingsMenu.Show();
        }

        GUI.EndGroup();

    }
}
