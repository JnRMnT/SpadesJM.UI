using UnityEngine;
using System.Collections;
using Assets.Scripts.System;

public class CreateRoomResultMenu : MonoBehaviour {
    public static int CreateResult = -1;

    public Texture2D Background,Logo;
    public GUISkin CreateRoomResultSkin;
    public CreateRoomMenu CreateRoomMenu;

    private static float LOGO_SIZE = Screen.height / 4;
    private static Transform transformInstance;

	void Start () {
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
            CreateRoomMenu.Hide();
        }
    }

    void OnGUI()
    {
        if (CreateResult != -1)
        {
            GUI.depth = 10;
            GUI.skin = CreateRoomResultSkin;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Background);

            if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
            {
                gameObject.SetActive(false);
                CreateRoomMenu.Hide();
            }

            float offset = Screen.height / 10;
            GUI.DrawTexture(new Rect(Screen.width / 2 - LOGO_SIZE, offset, LOGO_SIZE * 2, LOGO_SIZE), Logo);

            offset += LOGO_SIZE + offset;

            float button_size = Screen.width / 3;
            switch (CreateResult)
            {

                case -2:

                    GUI.Box(new Rect(Screen.width / 4, offset, Screen.width / 2, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("WAIT"));

                    break;

                case 5:

                    GUI.Box(new Rect(Screen.width / 20, offset, Screen.width - Screen.width / 10, Properties.TEXT_HEIGHT * 4), LanguageManager.getString("ROOMCRTERR"));
                    offset += Properties.TEXT_HEIGHT * 5;

                    if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("REGAGAIN")))
                    {
                        CreateResult = -1;
                        Hide();
                        CreateRoomMenu.Show();
                    }

                    break;
            }

            GUI.EndGroup();
        }

    }
}
