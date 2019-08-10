using UnityEngine;
using System.Collections;
using Assets.Scripts.System;

public class EndGamePopup : MonoBehaviour
{
    private static Transform transformInstance;
    private Rect groupPosition1,groupPosition2;

    public GUISkin EndGamePopupSkin;
    public ScoreBoard ScoreBoardInstance;

    public MonoNetworkPlayer Player;
    public InGameMenu InGameMenu;

    public bool Initialized;
    public static bool IsActive;

    public static void Show()
    {
        ScoreBoard.Show(false);
        IsActive = true;
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        IsActive = false;
        transformInstance.gameObject.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DialogBox.DialogBoxButtons = DialogBoxButtons.YES_NO;
            DialogBox.DialogText = LanguageManager.getString("PROMPT_LEAVEGM");
            DialogBox.CallbackObject = gameObject;
            DialogBox.Show();
        }

        if (!Initialized)
        {
            if (ScoreBoardInstance.Initialized)
            {
                Initialized = true;
                groupPosition1 = new Rect(ScoreBoardInstance.scorePopupPos.x, ScoreBoardInstance.scorePopupPos.y - Screen.height / 10, ScoreBoardInstance.scorePopupPos.width, Screen.height / 10);
                groupPosition2 = new Rect(ScoreBoardInstance.scorePopupPos.x, ScoreBoardInstance.scorePopupPos.y + ScoreBoardInstance.scorePopupPos.height, ScoreBoardInstance.scorePopupPos.width, Screen.height / 10);

                Hide();
            }
        }
    }

    void Start()
    {
        transformInstance = transform; 
    }

    public void DialogBoxCallback(bool answer)
    {
        if (answer)
        {
            InGameMenu.DialogBoxCallback(true);
            Hide();
        }
    }

    void OnGUI()
    {
        GUI.depth = 19;
        GUI.skin = EndGamePopupSkin;

        GUI.Box(groupPosition1, LanguageManager.getString("GMENDN"));
        GUI.BeginGroup(groupPosition1);

        GUI.EndGroup();

        GUI.Box(groupPosition2, "");
        GUI.BeginGroup(groupPosition2);

        if (LobbyPlayerStats.RoomData!= null && (Properties.ActiveGameType != GameType.MultiPlayer || LobbyPlayerStats.RoomData.getRoomOwner() == Player.GetInternalPlayer().PlayerName) && GUI.Button(new Rect(groupPosition2.width / 20, groupPosition2.height / 10, groupPosition2.width / 100 * 40, groupPosition2.height - groupPosition2.height / 5), LanguageManager.getString("RPLY")))
        {
            Replay();
        }

        if (GUI.Button(new Rect(groupPosition2.width / 2, groupPosition2.height / 10, groupPosition2.width / 100 * 43, groupPosition2.height - groupPosition2.height / 5), LanguageManager.getString("LEAVEGM")))
        {
            DialogBox.DialogBoxButtons = DialogBoxButtons.YES_NO;
            DialogBox.DialogText = LanguageManager.getString("PROMPT_LEAVEGM");
            DialogBox.CallbackObject = gameObject;
            DialogBox.Show();
        }

        GUI.EndGroup();
    }

    private void Replay()
    {
        (Player.GetInternalPlayer() as LocalNetworkPlayer).ResetGame();
        Hide();
    }

}
