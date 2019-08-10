using UnityEngine;
using System.Collections;
using Assets.Scripts.System;
using com.shephertz.app42.gaming.multiplayer.client;

public class JoinRoomResultScreen : MonoBehaviour {
    private static Transform transformInstance;
    public static int JoinResult = -1;

    public GUISkin JoinRoomGuiSkin;
    private static float LOGO_SIZE = Screen.height / 4;
    public Texture2D background, logo;
    public static string RoomId;

    private static bool tryAgainControl;
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
        JoinResult = -1;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
            RoomSelectionMenu.Show();
        }

        if (tryAgainControl)
        {
            tryAgainControl = false;
            StartCoroutine(TryAgain());
        }
    }

    public static void TryAgain(string roomId)
    {
        RoomId = roomId;
        tryAgainControl = true;
    }


    void OnGUI()
    {
        if (JoinResult != -1)
        {
            GUI.depth = 10;
            GUI.skin = JoinRoomGuiSkin;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

            if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
            {
                Hide();
                RoomSelectionMenu.Show();
            }

            float offset = Screen.height / 10;
            GUI.DrawTexture(new Rect(Screen.width / 2 - LOGO_SIZE, offset, LOGO_SIZE * 2, LOGO_SIZE), logo);

            offset += LOGO_SIZE + offset;

            float button_size = Screen.width / 3;
            switch (JoinResult)
            {

                case -2:

                    GUI.Box(new Rect(Screen.width / 4, offset, Screen.width / 2, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("WAIT"));

                    break;

                default:

                    GUI.Box(new Rect(Screen.width / 20, offset, Screen.width - Screen.width / 10, Properties.TEXT_HEIGHT * 4), LanguageManager.getString("JOINERR"));
                    offset += Properties.TEXT_HEIGHT * 5;

                    if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("REGAGAIN")))
                    {
                        StartCoroutine(TryAgain());
                    }

                    break;
            }

            GUI.EndGroup();
        }

    }

    private IEnumerator TryAgain()
    {
        JoinRoomResultScreen.JoinResult = -2;
        yield return null;
        WarpClient.GetInstance().JoinRoom(RoomId);
    }
   
}
