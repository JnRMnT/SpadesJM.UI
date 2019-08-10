using UnityEngine;
using System.Collections;
using Assets.Scripts.System;
using com.shephertz.app42.gaming.multiplayer.client;

public class LoginResultScreen : MonoBehaviour
{
    public GUISkin login_gui_skin;
    private string username, pw;
    public Texture2D background, logo;
    public static int login_result = -1;

    private static float LOGO_SIZE = Screen.height / 4;
    private static bool redirect = false;

    private static Transform transformInstance;

    private static TimeoutTimer timer;
    private static GameObject timerObject = null;

    public static void setRegResult(int result)
    {
        login_result = result;
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
        ActivateTimer();
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);
        DestroyTimer();
    }

    private static void ActivateTimer()
    {
        if (timerObject == null)
        {
            timerObject = new GameObject();
            timerObject.transform.parent = transformInstance;
            timer = timerObject.AddComponent<TimeoutTimer>();
            timer.StartTimer(30, transformInstance.gameObject);
        }
    }

    private static void DestroyTimer()
    {
        if (timerObject != null)
        {
            Destroy(timerObject.gameObject);
        }
        timerObject = null;
        timer = null;
    }

    public void Timeout()
    {
        RoomSelectionMenu.RoomCount = 0;
        RoomSelectionMenu.Activate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
            LoginScreen.Show();
        }else if (redirect)
        {
            redirect = false;
            StartCoroutine(GetAllRooms());
        }
    }

    public static void RedirectToRoomSelection()
    {
        redirect = true;
    }

    private IEnumerator GetAllRooms()
    {
        LoadingScreen.Show();
        MultiplayerListener.ActivePage = 1;
        yield return null;
        try
        {
            WarpClient.GetInstance().GetAllRooms();
        }
        catch (System.Exception e)
        {
            LogManager.Log(e.ToString());
            RoomSelectionMenu.RoomCount = 0;
            RoomSelectionMenu.Activate();
        }
    }

    void OnGUI()
    {
        if (login_result != -1)
        {
            GUI.depth = 10;
            GUI.skin = login_gui_skin;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

            if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
            {
                Hide();
                LoginScreen.Show();
            }

            float offset = Screen.height / 10;
            GUI.DrawTexture(new Rect(Screen.width / 2 - LOGO_SIZE, offset, LOGO_SIZE *2, LOGO_SIZE), logo);

            offset += LOGO_SIZE + offset;

            float button_size = Screen.width / 3;
            switch (login_result)
            {

                case -2:

                    GUI.Box(new Rect(Screen.width / 4, offset, Screen.width / 2, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("WAIT"));

                    break;

                case 0:

                    GUI.Box(new Rect(Screen.width / 4, offset, Screen.width / 2, Properties.TEXT_HEIGHT * 3), LanguageManager.getString("LOGINSCS"));
                    break;

                default:

                    GUI.Box(new Rect(Screen.width / 20, offset, Screen.width - Screen.width / 10, Properties.TEXT_HEIGHT * 4), LanguageManager.getString("USRERR"+login_result.ToString()));
                    offset += Properties.TEXT_HEIGHT * 5;

                    if (GUI.Button(new Rect(Screen.width / 2 - button_size / 2, offset, button_size, Properties.TEXT_HEIGHT * 2), LanguageManager.getString("REGAGAIN")))
                    {
                        login_result = -1;
                        LoginScreen.Show();
                        this.gameObject.SetActive(false);
                    }

                    break;
            }

            GUI.EndGroup();
        }

    }
}
