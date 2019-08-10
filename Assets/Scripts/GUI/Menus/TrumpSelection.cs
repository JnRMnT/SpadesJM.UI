using UnityEngine;
using System.Collections;
using Assets.Scripts.System;

public class TrumpSelection : MonoBehaviour {

    public Texture2D diamond, spade, club, heart;
    public GUISkin trumpSelectionSkin;

    private Rect popupPosition;
    private static Transform transformInstance;
    private float selectionSize,marginX;

    public MonoPlayer player;
    public MonoNetworkPlayer networkPlayer;

    private GUIContent diamondContent, spadeContent, clubContent, heartContent;
	// Use this for initialization
	void Start () {
        transformInstance = this.transform;
        popupPosition = new Rect(Screen.width/6,Screen.height/4,Screen.width - Screen.width/3,Screen.height/4);
        selectionSize = popupPosition.width / 5;

        diamondContent = new GUIContent();
        diamondContent.image = diamond;

        spadeContent = new GUIContent();
        spadeContent.image = spade;

        heartContent = new GUIContent();
        heartContent.image = heart;

        clubContent = new GUIContent();
        clubContent.image = club;

        marginX = (popupPosition.width - selectionSize * 4) / 5;

        Hide();
	}

    void OnGUI()
    {
        GUI.depth = 20;
        GUI.skin = trumpSelectionSkin;

        GUI.Box(popupPosition, "");
        GUI.BeginGroup(popupPosition);
        float offsetX = marginX;
        float offsetY = 0;
        GUI.Label(new Rect(0,0,popupPosition.width,popupPosition.height/3),LanguageManager.getString("TRMPSLCTION"));
        offsetY += popupPosition.height / 2;

        if(GUI.Button(new Rect(offsetX, offsetY, selectionSize, selectionSize),diamondContent)){
            SetTrump(Common.Enums.CardType.Diamond);
        }
        offsetX += marginX + selectionSize;

        if (GUI.Button(new Rect(offsetX, offsetY, selectionSize, selectionSize), clubContent))
        {
            SetTrump(Common.Enums.CardType.Club);
        }
        offsetX += marginX + selectionSize;
        if (GUI.Button(new Rect(offsetX, offsetY, selectionSize, selectionSize), spadeContent))
        {
            SetTrump(Common.Enums.CardType.Spade);
        }
        offsetX += marginX + selectionSize;
        if (GUI.Button(new Rect(offsetX, offsetY, selectionSize, selectionSize), heartContent))
        {
            SetTrump(Common.Enums.CardType.Heart);
        }

        GUI.EndGroup();
    }

    private void SetTrump(Common.Enums.CardType cardType)
    {
        Hide();
        if (Properties.ActiveGameType == GameType.SinglePlayer)
        {
            player.getInternalPlayer().SetTrumpType(cardType);
        }
        else
        {
            networkPlayer.GetInternalPlayer().SetTrumpType(cardType);
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
}
