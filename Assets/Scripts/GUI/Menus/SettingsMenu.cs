using UnityEngine;
using System.Collections;
using Assets.Scripts.System;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public GUISkin CreditsMenuGUISkin;
    private static Transform transformInstance;

    public Texture2D Background;

    private float offsetHeight, offsetWidth;
    private Rect scrollViewPosition, scrollViewInnerPosition;
    private Vector2 scrollPosition = Vector2.zero;
    private Vector2 touchPosition;

    public Texture2D Sound, SoundDisabled;

    public static System.Type BackMenu;
    private float buttonSize;

    void Start()
    {
        transformInstance = this.transform;

        offsetWidth = Screen.width / 20;
        offsetHeight = Screen.height / 40;
        scrollViewPosition = new Rect(offsetWidth / 2, offsetHeight * 2 + Properties.BackButtonSize, Screen.width - offsetWidth * 2, Screen.height - Properties.TEXT_HEIGHT / 2 * 7 - Properties.BackButtonSize);
        scrollViewInnerPosition = new Rect(0, 0, Screen.width - offsetWidth * 3, Screen.height);

        buttonSize = Screen.height / 6;
        Hide();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackButton();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                scrollPosition.y += touch.position.y - touchPosition.y;
                touchPosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            scrollPosition.y += Input.mousePosition.y - touchPosition.y;
            touchPosition = Input.mousePosition;
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



    void OnGUI()
    {

        GUI.depth = 0;
        GUI.skin = CreditsMenuGUISkin;

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Background);

        if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
        {
            HandleBackButton();
        }

        scrollPosition = GUI.BeginScrollView(scrollViewPosition, scrollPosition, scrollViewInnerPosition);

        float offsetY = Properties.TEXT_HEIGHT * 2;
        GUI.Label(new Rect(0, offsetY, scrollViewInnerPosition.width, Properties.TEXT_HEIGHT), LanguageManager.getString("SND"));
        offsetY += Properties.TEXT_HEIGHT * 3 / 2;
        if (GUI.Button(new Rect(scrollViewInnerPosition.width / 2 - buttonSize / 2, offsetY, buttonSize, buttonSize), SoundManager.SoundActive ? Sound : SoundDisabled))
        {
            if (SoundManager.SoundActive)
            {
                SoundManager.SoundActive = false;
                PlayerPrefs.SetInt("Sound", 0);
            }
            else
            {
                SoundManager.SoundActive = true;
                PlayerPrefs.SetInt("Sound", 1);
            }
        }

        offsetY += buttonSize + Properties.TEXT_HEIGHT;
        GUI.Label(new Rect(0, offsetY, scrollViewInnerPosition.width, Properties.TEXT_HEIGHT), LanguageManager.getString("LNG"));
        offsetY += Properties.TEXT_HEIGHT * 3 / 2;
        if (GUI.Button(new Rect(scrollViewInnerPosition.width / 2 - buttonSize / 2, offsetY, buttonSize, buttonSize), LanguageManager.languageFlag))
        {
            LanguageManager.ChangeLanguage();
        }
        GUI.EndScrollView();


    }

    private void HandleBackButton()
    {
        System.Reflection.MethodInfo showMethod = BackMenu.GetMethod("Show");
        showMethod.Invoke(this, null);
        Hide();
    }

}
