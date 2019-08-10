using UnityEngine;
using System.Collections;
using Assets.Scripts.System;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System.Collections.Generic;

public class CreditsMenu : MonoBehaviour
{
    public GUISkin CreditsMenuGUISkin;
    private static Transform transformInstance;

    public Texture2D Background;

    private float offsetHeight, offsetWidth;
    private Rect scrollViewPosition, scrollViewInnerPosition;
    private Vector2 scrollPosition = Vector2.zero;
    private Vector2 touchPosition;

    private static bool isActive = false;

    private string credits = string.Empty;

    void Start()
    {
        transformInstance = this.transform;

        offsetWidth = Screen.width / 20;
        offsetHeight = Screen.height / 40;
        scrollViewPosition = new Rect(offsetWidth / 2, offsetHeight * 2 + Properties.BackButtonSize, Screen.width - offsetWidth * 2, Screen.height - Properties.TEXT_HEIGHT / 2 * 7 - Properties.BackButtonSize);
        scrollViewInnerPosition = new Rect(0, 0, Screen.width - offsetWidth * 3, Screen.height * 2);

        StartCoroutine(PrepareCreditsText());
    }

    IEnumerator PrepareCreditsText()
    {
        yield return null;
        while(!LanguageManager.IsLoaded)
        {
            //  BUSY WAIT
        }

        credits = System.Text.RegularExpressions.Regex.Replace(LanguageManager.getString("CRDTS"), "--n", System.Environment.NewLine);
        string [] temp = System.Text.RegularExpressions.Regex.Split(LanguageManager.getString("CRDTS"), "--n");
        scrollViewInnerPosition.height = Properties.TEXT_HEIGHT * temp.Length*2;

        Hide();
    }

    void Update()
    {
        scrollPosition.y += Time.deltaTime * 20;
        if (scrollPosition.y > scrollViewPosition.height)
        {
            scrollPosition.y = scrollViewPosition.height;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
            MainMenu.Show();
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
        isActive = true;
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);
    }



    void OnGUI()
    {
        if (isActive && !string.IsNullOrEmpty(credits))
        {
            GUI.depth = 0;
            GUI.skin = CreditsMenuGUISkin;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Background);

            if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
            {
                Hide();
                MainMenu.Show();
            }

            scrollPosition = GUI.BeginScrollView(scrollViewPosition, scrollPosition, scrollViewInnerPosition);

            GUI.Label(scrollViewInnerPosition, credits);

            GUI.EndScrollView();

        }
    }


}
