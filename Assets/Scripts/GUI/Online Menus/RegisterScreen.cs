using UnityEngine;
using System.Collections;
using Assets.Scripts.System;

public class RegisterScreen : MonoBehaviour {
	public UserManagement user_manager;
	public GUISkin login_gui_skin;
	private string username,pw,email;
	public Texture2D background,logo;

	private bool mail_error,username_error,password_error;
	
	private static float LOGO_SIZE=Screen.height/4;
	
	void Start(){
		username="";
		pw		="";
		email	="";

		mail_error		=	false;
		username_error	=	false;
		password_error	=	false;

        gameObject.SetActive(false);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoginScreen.Show();
            gameObject.SetActive(false);
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
            LoginScreen.Show();
            gameObject.SetActive(false);
        }

        float offset = Screen.height / 20;
        GUI.DrawTexture(new Rect(Screen.width / 2 - LOGO_SIZE, offset, LOGO_SIZE * 2, LOGO_SIZE), logo);

        offset += LOGO_SIZE + offset;

        float button_size = Screen.width / 3;
        GUI.Box(new Rect(Screen.width / 3 - button_size - Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("EML"));
        email = GUI.TextField(new Rect(Screen.width / 3 + Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), email);

        if (email.Length == 0)
        {
            GUI.Label(new Rect(Screen.width - Screen.width / 4, offset, Screen.width / 4, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("EMPTYMAIL"));
            mail_error = true;
        }
        else if (!email.Contains(".") || !email.Contains("@"))
        {
            GUI.Label(new Rect(Screen.width - Screen.width / 4, offset, Screen.width / 4, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("MAILNVLD"));
            mail_error = true;
        }
        else
        {
            mail_error = false;
        }


        offset += Properties.TEXT_HEIGHT * 4;

        GUI.Box(new Rect(Screen.width / 3 - button_size - Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("USRNM"));
        username = GUI.TextField(new Rect(Screen.width / 3 + Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), username);

        if (username.Length == 0)
        {
            GUI.Label(new Rect(Screen.width - Screen.width / 4, offset, Screen.width / 4, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("EMPTYUSRNM"));
            username_error = true;
        }
        else if (username.Length < 4)
        {
            GUI.Label(new Rect(Screen.width - Screen.width / 4, offset, Screen.width / 4, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("SHRTUSRNM"));
            username_error = true;
        }
        else
        {
            username_error = false;
        }

        offset += Properties.TEXT_HEIGHT * 4;

        GUI.Box(new Rect(Screen.width / 3 - button_size - Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("PW"));
        pw = GUI.PasswordField(new Rect(Screen.width / 3 + Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), pw, '*');

        if (pw.Length == 0)
        {
            GUI.Label(new Rect(Screen.width - Screen.width / 4, offset, Screen.width / 4, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("EPMTYPW"));
            password_error = true;
        }
        else if (pw.Length < 4)
        {
            GUI.Label(new Rect(Screen.width - Screen.width / 4, offset, Screen.width / 4, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("SHRTPW"));
            password_error = true;
        }
        else
        {
            password_error = false;
        }


        offset += Properties.TEXT_HEIGHT * 4;

        if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("REG")))
        {
            if (!mail_error && !username_error && !password_error)
            {
                gameObject.SetActive(false);
                user_manager.doRegister(email, username, pw);
                RegisterResultScreen.Show();
                RegisterResultScreen.reg_result = -2;
            }
        }


        GUI.EndGroup();

    }

}
