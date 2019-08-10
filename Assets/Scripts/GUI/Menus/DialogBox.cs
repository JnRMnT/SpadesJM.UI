using UnityEngine;
using System.Collections;
using Assets.Scripts.System;

public class DialogBox : MonoBehaviour {
    public static string DialogText;
    public static GameObject CallbackObject;
    public static DialogBoxButtons DialogBoxButtons;

    private static Transform transformInstance;
    public GUISkin DialogBoxSkin;

    private string ok, yes, no;

    private Rect okButtonPos, yesButtonPos, noButtonPos, dialogBoxPosition;

    public static void Show()
    {
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);

    }

	// Use this for initialization
	void Start () {
        transformInstance = transform;

        dialogBoxPosition = new Rect(Screen.width / 10, Screen.height / 2 - Screen.height / 8, Screen.width - Screen.width/5, Screen.height / 4);

        yesButtonPos = new Rect(dialogBoxPosition.width / 8, dialogBoxPosition.height - Properties.TEXT_HEIGHT * 2, dialogBoxPosition.width / 4, Properties.TEXT_HEIGHT);
        noButtonPos = new Rect(dialogBoxPosition.width/2 + dialogBoxPosition.width / 8, dialogBoxPosition.height - Properties.TEXT_HEIGHT * 2, dialogBoxPosition.width / 4, Properties.TEXT_HEIGHT);
        okButtonPos = new Rect(dialogBoxPosition.width / 2 - dialogBoxPosition.width / 10, dialogBoxPosition.height - Properties.TEXT_HEIGHT * 2, dialogBoxPosition.width / 4, Properties.TEXT_HEIGHT);

        StartCoroutine(GetResources());
	}

    IEnumerator GetResources()
    {
        yield return null;
        while (!LanguageManager.IsLoaded)
        {
            //  BUSY WAITING
        }
        ok = LanguageManager.getString("OK");
        yes = LanguageManager.getString("YS");
        no = LanguageManager.getString("NO");

        Hide();
    }

    void OnGUI()
    {
        if (LanguageManager.IsLoaded)
        {
            GUI.depth = 0;
            GUI.skin = DialogBoxSkin;

            GUI.BeginGroup(dialogBoxPosition);
            GUI.Box(new Rect(0,0,dialogBoxPosition.width,dialogBoxPosition.height), DialogText);

            if (CallbackObject!=null && !CallbackObject.gameObject.activeSelf)
            {
                Hide();
            }

            if (DialogBoxButtons == global::DialogBoxButtons.OK)
            {
                if (GUI.Button(okButtonPos, ok))
                {
                    if (CallbackObject != null)
                    {
                        CallbackObject.SendMessage("DialogBoxCallback");
                    }
                    Hide();
                }
            }
            else if (DialogBoxButtons == global::DialogBoxButtons.YES_NO)
            {
                if (GUI.Button(yesButtonPos, yes))
                {
                    CallbackObject.SendMessage("DialogBoxCallback", true);
                    Hide();
                }
                else if (GUI.Button(noButtonPos, no))
                {
                    CallbackObject.SendMessage("DialogBoxCallback", false);
                    Hide();
                }
            }
            else
            {

            }
            GUI.EndGroup();
        }
    }
}
