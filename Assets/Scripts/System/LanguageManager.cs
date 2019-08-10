using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Assets.Scripts.System;

public class LanguageManager : MonoBehaviour
{
    public static bool IsLoaded = false;
    public static string playerLanguage;
    protected static Dictionary<string, string> dictionary;
    protected static string[] languages = { "en", "tr" };
    protected static int currentLanguageIndex = 1;
    public static Texture2D languageFlag;

    public static Texture2D EnglishFlag, TurkishFlag;

    protected void Start()
    {
        GetFlags();
        UpdateLanguage(PlayerPrefs.GetString("Language"));
        caculateFontSizes();
        IsLoaded = true;
    }

    public static void ChangeLanguage()
    {
        currentLanguageIndex = (currentLanguageIndex + 1) % languages.Length;
        UpdateLanguage(languages[currentLanguageIndex]);
        PlayerPrefs.SetString("Language", languages[currentLanguageIndex]);
    }

    private void GetFlags()
    {
        foreach (string language in languages)
        {
            Texture2D flag = Resources.Load("Textures/Flags/" + language) as Texture2D;

            if (language == "en")
            {
                EnglishFlag = flag;
            }
            else
            {
                TurkishFlag = flag;
            }
        }
    }

    private void caculateFontSizes()
    {

        //			FONT CALCULCATIONS
        int screenDPI = (int)Screen.dpi;
        int newSize = -1;

        if (screenDPI < 200)
        {
            Properties.TEXT_HEIGHT = 28;
            newSize = 18;
        }
        else if (screenDPI >= 200 && screenDPI < 225)
        {
            Properties.TEXT_HEIGHT = 31;
            newSize = 20;
        }
        else if (screenDPI >= 225 && screenDPI < 250)
        {
            Properties.TEXT_HEIGHT = 35;
            newSize = 22;
        }
        else if (screenDPI >= 250 && screenDPI < 275)
        {
            Properties.TEXT_HEIGHT = 38;
            newSize = 24;
        }
        else if (screenDPI >= 275 && screenDPI < 300)
        {
            Properties.TEXT_HEIGHT = 41;
            newSize = 26;
        }
        else if (screenDPI >= 300 && screenDPI < 325)
        {
            Properties.TEXT_HEIGHT = 44;
            newSize = 28;
        }
        else if (screenDPI >= 325 && screenDPI < 350)
        {
            Properties.TEXT_HEIGHT = 47;
            newSize = 30;
        }
        else if (screenDPI >= 350 && screenDPI < 375)
        {
            Properties.TEXT_HEIGHT = 50;
            newSize = 32;
        }
        else if (screenDPI >= 375 && screenDPI < 400)
        {
            Properties.TEXT_HEIGHT = 53;
            newSize = 34;
        }
        else if (screenDPI >= 400)
        {
            Properties.TEXT_HEIGHT = 56;
            newSize = 36;
        }

        GUISkin[] guiSkins = Resources.LoadAll<GUISkin>("GUISkins");

        foreach (GUISkin guiSkin in guiSkins)
        {
            if (guiSkin.name == "LoginScreen" || guiSkin.name == "RoomSelection" ||guiSkin.name == "CurrentTrump")
            {
                guiSkin.label.fontSize = newSize/5*4;
            }
            else
            {
                guiSkin.label.fontSize = newSize;
            }
            guiSkin.box.fontSize = newSize;
            guiSkin.button.fontSize = newSize;
            guiSkin.textField.fontSize = newSize;
        }


    }

    public static void UpdateLanguage(string language)
    {
        dictionary = new Dictionary<string, string>();
        playerLanguage = language;
        if (playerLanguage == null || playerLanguage == "")
        {

            if (Application.systemLanguage == SystemLanguage.Turkish)
            {
                playerLanguage = "tr";
                currentLanguageIndex = 1;
                languageFlag = TurkishFlag;
            }
            else
            {
                playerLanguage = "en";
                currentLanguageIndex = 0;
                languageFlag = EnglishFlag;
            }

        }

        if (playerLanguage == "en")
        {
            languageFlag = EnglishFlag;
        }
        else
        {
            languageFlag = TurkishFlag;
        }

        readFromLanguageFile();
    }

    private static void readFromLanguageFile()
    {
        FileInfo theSourceFile = null;

        TextReader reader = null;  // NOTE: TextReader, superclass of StreamReader and StringReader

        // Read from plain text file if it exists

        theSourceFile = new FileInfo(Application.dataPath + "../Resources/Languages/" + playerLanguage + ".txt");
        if (theSourceFile != null && theSourceFile.Exists)
        {
            reader = theSourceFile.OpenText();  // returns StreamReader
        }
        else
        {
            // try to read from Resources instead
            TextAsset puzdata = (TextAsset)Resources.Load("Languages/" + playerLanguage, typeof(TextAsset));
            reader = new StringReader(puzdata.text);  // returns StringReader
        }

        if (reader == null)
        {
            Debug.Log("'" + Application.dataPath + "/Languages/" + playerLanguage + ".txt' Language file not found or not readable");
        }
        else
        {
            // Read each line from the file/resource
            string txt;
            while ((txt = reader.ReadLine()) != null)
            {
                string[] key_value = txt.Split('=');
                if (dictionary.ContainsKey(key_value[0])) dictionary[key_value[0]] = key_value[1];
                else dictionary.Add(key_value[0], key_value[1]);
            }
        }
    }

    public static string getString(string key)
    {
        string value;
        dictionary.TryGetValue(key, out value);
        if (string.IsNullOrEmpty(value))
        {
            return key;
        }
        else
        {
            return value;
        }
    }

}