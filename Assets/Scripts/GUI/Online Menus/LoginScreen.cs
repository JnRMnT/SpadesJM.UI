using UnityEngine;
using System.Collections;
using Assets.Scripts.System;
using com.shephertz.app42.gaming.multiplayer.client;

public class LoginScreen : MonoBehaviour {
	public UserManagement user_manager;
    public LoginResultScreen LoginResultScreen;
	public GUISkin login_gui_skin;
	private string username,pw;
	public Texture2D background,logo;
	public RegisterScreen reg_screen;

    public static Transform transformInstance;

	private static float LOGO_SIZE=Screen.height/4;

	void Start(){
		pw	=	"";
		if(PlayerPrefs.HasKey("username"))username	=	PlayerPrefs.GetString("username");
		else username="";

        transformInstance = transform;

        Hide();
	}


    public static void Show()
    {
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);

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
        GUI.skin = login_gui_skin;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

        if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
        {
            MainMenu.Show();
            Hide();
        }


        float offset = Screen.height / 20;
        GUI.DrawTexture(new Rect(Screen.width / 2 - LOGO_SIZE, offset, LOGO_SIZE * 2, LOGO_SIZE), logo);

        offset += LOGO_SIZE + offset;

        float button_size = Screen.width / 9 * 4;
        GUI.Box(new Rect(Screen.width / 2 - button_size - Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("USRNM"));
        username = GUI.TextField(new Rect(Screen.width / 2 + Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), username);

        offset += Properties.TEXT_HEIGHT * 3;
        GUI.Box(new Rect(Screen.width / 2 - button_size - Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("PW"));
        pw = GUI.PasswordField(new Rect(Screen.width / 2 + Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), pw, '*');

        offset += Properties.TEXT_HEIGHT / 2 * 7;

        if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("LGN")) && !string.IsNullOrEmpty(pw) && !string.IsNullOrEmpty(username))
        {
            WarpClient.GetInstance().Disconnect();
            user_manager.doLogin(username, pw);
            gameObject.SetActive(false);
            LoginResultScreen.Show();
            LoginResultScreen.login_result = -2;
            Hide();
        }
        offset += Properties.TEXT_HEIGHT / 2 * 5;

        if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("REG")))
        {
            reg_screen.gameObject.SetActive(true);
            Hide();
        }
        GUI.EndGroup();

    }

}
