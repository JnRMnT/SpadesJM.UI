using UnityEngine;
using System.Collections;
using Assets.Scripts.System;

public class PasswordEnterMenu : MonoBehaviour
{
    private static string password;
    private static string selectedRoomId;
    private static Transform transformInstance;

    public RoomSelectionMenu RoomSelectionMenu;

    public GUISkin PasswordEnterMenuSkin;
    public Texture2D Background, Logo;

    private static float LOGO_SIZE = Screen.height / 4;

    private string pw = string.Empty;

    void Start()
    {
        transformInstance = transform;

        Hide();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RoomSelectionMenu.Show();
            Hide();
        }
    }

    public static void Show(string roomPassword, string selectedRoomId)
    {
        password = roomPassword;
        PasswordEnterMenu.selectedRoomId = selectedRoomId;
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);

    }

    void OnGUI()
    {
        GUI.depth = 10;
        GUI.skin = PasswordEnterMenuSkin;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Background);

        if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
        {
            MainMenu.Show();
            Hide();
        }


        float offset = Screen.height / 20;
        GUI.DrawTexture(new Rect(Screen.width / 2 - LOGO_SIZE, offset, LOGO_SIZE * 2, LOGO_SIZE), Logo);

        offset += LOGO_SIZE + offset * 2;

        float button_size = Screen.width / 9 * 4;
        GUI.Box(new Rect(Screen.width / 2 - button_size - Screen.width / 40, offset, Screen.width - Screen.width / 20, Properties.TEXT_HEIGHT * 4), LanguageManager.getString("ENTRPW"));

        offset += Screen.height / 20 + Properties.TEXT_HEIGHT * 4;
        GUI.Box(new Rect(Screen.width / 2 - button_size - Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("PW"));
        pw = GUI.PasswordField(new Rect(Screen.width / 2 + Screen.width / 40, offset, button_size, Properties.TEXT_HEIGHT * 2), pw, '*');

        offset += Properties.TEXT_HEIGHT / 2 * 5;

        if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("JOINROOM")))
        {
            AuthenticatePassword();
        }
        GUI.EndGroup();
    }

    private void AuthenticatePassword()
    {
        if (password.Equals(pw))
        {
            RoomSelectionMenu.Show();
            RoomSelectionMenu.DoJoin(selectedRoomId);
            Hide();
        }
        else
        {
            DialogBox.DialogBoxButtons = DialogBoxButtons.OK;
            DialogBox.DialogText = LanguageManager.getString("WRNGPW");
            DialogBox.Show();
        }
    }
}
