using UnityEngine;
using System.Collections;
using Common.Enums;

public class CurrentTrump : MonoBehaviour {

    public GUISkin GuiSkin;
    public Texture2D diamond, spade, club, heart;

    private Rect boxPosition, trumpPosition;

    private static CardType currentTrump;
    public static bool currentTrumpActive;

	// Use this for initialization
	void Start () {
        boxPosition = new Rect(Screen.width - Screen.width / 9 - Screen.width / 80, Screen.height / 40, Screen.width / 9, Screen.width / 9);
        currentTrumpActive = false;
	}

    void OnGUI()
    {
        if (currentTrumpActive)
        {
            GUI.depth = 20;
            GUI.skin = GuiSkin;

            if (currentTrump == CardType.Club)
            {
                GUI.Box(boxPosition, club);
            }
            else if (currentTrump == CardType.Diamond)
            {
                GUI.Box(boxPosition, diamond);
            }
            else if (currentTrump == CardType.Heart)
            {
                GUI.Box(boxPosition, heart);
            }
            else if (currentTrump == CardType.Spade)
            {
                GUI.Box(boxPosition, spade);
            }
        }   
    }

    public static void SetTrumpType(CardType trumpType)
    {
        currentTrump = trumpType;
        currentTrumpActive = true;
    }
}
