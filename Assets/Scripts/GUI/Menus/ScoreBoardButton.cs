using UnityEngine;
using System.Collections;

public class ScoreBoardButton : MonoBehaviour {
    public GUISkin ScoreBoardButtonSkin;
    public Texture2D ButtonBackground;
    private Rect boxPosition, selectionSize;
    public UIGameTable gameTable;

	// Use this for initialization
	void Start () {
        boxPosition = new Rect(Screen.width - Screen.width / 9 * 2- Screen.width/ 40, Screen.height / 40, Screen.width / 9, Screen.width / 9);
	}


    void OnGUI()
    {
        GUI.depth = 21;
        if (gameTable.gameTable.GetGameState() != Common.Enums.GameState.PLAYING)
        {
            return;
        }
        GUI.skin = ScoreBoardButtonSkin;

        if (GUI.Button(boxPosition, ButtonBackground))
        {
            ScoreBoard.Show(false);
        }

    }

}
