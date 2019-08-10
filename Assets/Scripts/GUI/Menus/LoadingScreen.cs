using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {
    public GUISkin LoadingScreenGUISkin;
    private static Transform transformInstance;

    void Start()
    {
        transformInstance = this.transform;
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

    void OnGUI()
    {
        GUI.depth = 0;
        GUI.skin = LoadingScreenGUISkin;
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), LanguageManager.getString("LDNG"));
    }
}
