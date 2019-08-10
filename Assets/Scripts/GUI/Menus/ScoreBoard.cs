using UnityEngine;
using System.Collections;
using Assets.Scripts.Game.Actors;
using Game.Structure;

public class ScoreBoard : MonoBehaviour {
    private static Transform transformInstance;
    private static bool timed;

    private Rect scorePosition, scorePosition2, buttonPosition;
    public Rect scorePopupPos;
    public bool Initialized;

    public GUISkin scoreboardSkin;

    public EndGamePopup EndGamePopup;

	// Use this for initialization
	void Start () {
        transformInstance = this.transform;
        timed = false;

        scorePopupPos = new Rect(Screen.width / 20,Screen.height/4,Screen.width - Screen.width/10 , Screen.height/2);

        scorePosition = new Rect(0, 0, scorePopupPos.width / 2, scorePopupPos.height / 12);
        scorePosition2 = new Rect(scorePosition.width, 0, scorePopupPos.width / 2, scorePopupPos.height / 12);

        buttonPosition = new Rect(scorePopupPos.width - scorePopupPos.width / 8, scorePopupPos.height / 20, scorePopupPos.width / 15, scorePopupPos.width / 15);

        Initialized = true;

        Hide();
	}

    void OnGUI()
    {
        GUI.depth = 20;
        GUI.skin = scoreboardSkin;

        if ((Game.Actors.ScoreBoard.gameTable.GetGameInstance() as UnityGame) == null)
        {
            return;
        }

        GUI.BeginGroup(scorePopupPos);
        GUI.Box(new Rect(0, 0, scorePopupPos.width, scorePopupPos.height), "");

        UnityRound round = ((Game.Actors.ScoreBoard.gameTable.GetGameInstance() as UnityGame).GetCurrentRound() as UnityRound);
        UnityInitialPhase initialPhase = null;
        RoundScore[] biddings = null;
        if (round != null)
        {
            initialPhase = round.GetInitialPhase();
        }
        if (initialPhase != null)
        {
            biddings = initialPhase.GetRoundScores();
        }


        if (biddings != null)
        {

            float offsetY = scorePopupPos.height / 12;
            GUI.Label(new Rect(0, offsetY, scorePopupPos.width, scorePosition.height), LanguageManager.getString("SCRBRD"));
            offsetY += scorePopupPos.height / 6;

            for (int i = 0; i < 4; i++)
            {
                var score = Game.Actors.ScoreBoard.GetPlayerScore(i);
                var playerName = Game.Actors.ScoreBoard.gameTable.GetPlayerSeatedAt(i).PlayerName;

                var said = biddings[i].GetSaid();
                var bid = "";
                if (said == -2)
                {
                    bid = "-";
                }
                else if (said == -1)
                {
                    bid = LanguageManager.getString("PASS");
                }
                else
                {
                    bid = said.ToString();
                }

                GUI.Box(new Rect(scorePopupPos.width / 8, offsetY, scorePopupPos.width - scorePopupPos.width / 4, scorePosition.height * 2), "");
                GUI.Label(new Rect(scorePosition.x, offsetY, scorePosition.width, scorePosition.height), playerName);
                GUI.Label(new Rect(scorePosition2.x, offsetY, scorePosition2.width, scorePosition.height), bid);
                offsetY += scorePosition.height;
                GUI.Label(new Rect(scorePosition.x, offsetY, scorePopupPos.width, scorePosition.height), LanguageManager.getString("SCR") + ": " + score.ToString());
                offsetY += scorePosition.height;
            }
        }


        if (!timed)
        {
            if (GUI.Button(buttonPosition, "X"))
            {
                if (EndGamePopup.IsActive)
                {
                    EndGamePopup.DialogBoxCallback(true);
                }
                Hide();
            }
        }

        GUI.EndGroup();

    }

    public static void Show(bool timed)
    {
        transformInstance.gameObject.SetActive(true);
        ScoreBoard.timed = timed;
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);
    }
}
