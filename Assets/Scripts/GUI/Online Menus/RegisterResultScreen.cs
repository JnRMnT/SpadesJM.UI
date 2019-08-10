using UnityEngine;
using System.Collections;
using Assets.Scripts.System;

public class RegisterResultScreen : MonoBehaviour {
	public GUISkin login_gui_skin;
	private string username,pw;
	public Texture2D background,logo;
	public RegisterScreen reg_screen;
	public static int reg_result=-1;

    private static Transform transformInstance;
	
	private static float LOGO_SIZE=Screen.height/4;
	
	public static void setRegResult(int result){
		reg_result	=	result;
        LogManager.Log("RESULT : " + result.ToString());
	}

    void Start()
    {
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
            gameObject.SetActive(false);
            reg_screen.gameObject.SetActive(true);
        }
    }

	void OnGUI(){
		if(reg_result!=-1){
            GUI.depth = 10;
			GUI.skin	=	login_gui_skin;
			GUI.BeginGroup(new Rect(0,0,Screen.width,Screen.height));
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);

            if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
            {
                gameObject.SetActive(false);
                reg_screen.gameObject.SetActive(true);
            }

			float offset	=	Screen.height/10;
            GUI.DrawTexture(new Rect(Screen.width / 2 - LOGO_SIZE, offset, LOGO_SIZE * 2, LOGO_SIZE), logo);
			
			offset+=	LOGO_SIZE+offset;
			
			float button_size	=	Screen.width/3;
            switch (reg_result)
            {

                case -2:

                    GUI.Box(new Rect(Screen.width / 4, offset, Screen.width / 2, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("WAIT"));

                    break;

                case 0:

                    GUI.Box(new Rect(Screen.width / 4, offset, Screen.width / 2, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("LOGINSCS"));
                    offset += Properties.TEXT_HEIGHT * 3;

                    if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("OK")))
                    {
                        this.gameObject.SetActive(false);
                        RoomSelectionMenu.Show();
                    }
                    break;

                case 1:

                    GUI.Box(new Rect(Screen.width / 20, offset, Screen.width - Screen.width / 10, Properties.TEXT_HEIGHT * 4), LanguageManager.getString("ALRDYEXSTS"));
                    offset += Properties.TEXT_HEIGHT * 5;

                    if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("REGAGAIN")))
                    {
                        reg_result = -1;
                        reg_screen.gameObject.SetActive(true);
                        Hide();
                    }

                    break;

                case 2:
                    GUI.Box(new Rect(Screen.width / 20, offset, Screen.width - Screen.width / 10, Properties.TEXT_HEIGHT * 4), LanguageManager.getString("EMLINVLD"));
                    offset += Properties.TEXT_HEIGHT * 5;

                    if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("REGAGAIN")))
                    {
                        reg_result = -1;
                        reg_screen.gameObject.SetActive(true);
                        Hide();
                    }
                    break;
            }

			GUI.EndGroup();
		}

	}
}
