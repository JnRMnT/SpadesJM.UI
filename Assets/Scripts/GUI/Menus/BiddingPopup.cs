using UnityEngine;
using System.Collections;
using Assets.Scripts.Game.Actors;
using Assets.Scripts.System;

public class BiddingPopup : MonoBehaviour {

    public UIGameTable gameTable;
    private UnityRound currentRound;

    private static BiddingPopup activePopupScript;
    private static MonoPlayer player;
    public MonoNetworkPlayer NetworkPlayer;

    public Rect menuPosition;
    private Rect mainGroupPosition, innerGroupPosition, biddingsPos, biddingsPos2, dealTitlePosition;
    private float buttonWidth, marginWidth, marginWidth2, marginHeight;

    public Texture2D Border1, Border2, DealTitle,DealTitle_EN;

    public GUISkin popupSkin;

    private static Transform transformInstance;

    public void SetActive(bool active)
    {
       this.gameObject.SetActive(active);
    }
    
    public void SetCurrentRound(UnityRound unityRound)
    {
        this.currentRound = unityRound;
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("SCRIPTWRAPPER").GetComponent<MonoPlayer>();

        menuPosition = new Rect(Screen.width / 20, Screen.height / 20, Screen.width - Screen.width / 10, Screen.height / 2);
        mainGroupPosition = new Rect(0, 0, menuPosition.width, menuPosition.height);
        innerGroupPosition = new Rect(menuPosition.width / 10, menuPosition.height / 30, menuPosition.width - menuPosition.width / 5, (menuPosition.height - menuPosition.height / 10));
        biddingsPos = new Rect(0, 0, innerGroupPosition.width / 2, innerGroupPosition.height / 2);
        biddingsPos2 = new Rect(innerGroupPosition.width/2, 0, innerGroupPosition.width / 2, innerGroupPosition.height / 2);
        dealTitlePosition = new Rect(-innerGroupPosition.width / 11, -innerGroupPosition.height / 6, innerGroupPosition.width / 7 * 10, innerGroupPosition.height);

        buttonWidth = innerGroupPosition.width / 8;
        marginWidth = (innerGroupPosition.width - buttonWidth * 6) / 5;
        marginWidth2 = (innerGroupPosition.width - buttonWidth * 6) / 4;
        marginHeight = (innerGroupPosition.height/3*2 - buttonWidth * 4) / 3 + buttonWidth ;

        transformInstance = transform;
        SetActive(false);

    }

	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.depth = 20;
        GUI.skin = popupSkin;
        GUI.BeginGroup(menuPosition);
        //  MAIN BOX
        GUI.Box(mainGroupPosition, "");
        //  INNER GROUP
        GUI.DrawTexture(dealTitlePosition, LanguageManager.playerLanguage == "en" ? DealTitle_EN : DealTitle);
        GUI.BeginGroup(innerGroupPosition);

        GUI.DrawTexture(new Rect(0, 0, innerGroupPosition.width / 10, innerGroupPosition.width / 10), Border1);
        GUI.DrawTexture(new Rect(innerGroupPosition.width / 10 * 9, 0, innerGroupPosition.width / 10, innerGroupPosition.width / 10), Border2);
        

        //GUI.Label(new Rect(0, 0, innerGroupPosition.width, marginHeight), LanguageManager.getString("MKBDNG"));

        float offsetX = 0;
        float offsetY = 0;


        offsetY += innerGroupPosition.height / 4;
        //  FIRST ROW
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonWidth), "0"))
        {
            //  BID 0
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(0);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(0);
            }
        }
        offsetX += buttonWidth + marginWidth;
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonWidth), "5"))
        {
            //  BID 5
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(5);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(5);
            }
        }
        offsetX += buttonWidth + marginWidth;
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonWidth), "6"))
        {
            //  BID 6
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(6);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(6);
            }
        }
        offsetX += buttonWidth + marginWidth;
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonWidth), "7"))
        {
            //  BID 7
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(7);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(7);
            }
        }
        offsetX += buttonWidth + marginWidth;
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonWidth), "8"))
        {
            //  BID 8
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(8);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(8);
            }
        }
        offsetX += buttonWidth + marginWidth;
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonWidth), "9"))
        {
            //  BID 9
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(9);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(9);
            }
        }


        //  SECOND ROW
        offsetY += marginHeight;
        offsetX = 0;

        
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonWidth), "10"))
        {
            //  BID 10
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(10);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(10);
            }
        }
        offsetX += buttonWidth + marginWidth2;
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonWidth), "11"))
        {
            //  BID 11
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(11);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(11);
            }
        }
        offsetX += buttonWidth + marginWidth2;
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonWidth), "12"))
        {
            //  BID 12
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(12);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(12);
            }
        }
        offsetX += buttonWidth + marginWidth2;
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonWidth), "13"))
        {
            //  BID 13
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(13);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(13);
            }
        }
        offsetX += buttonWidth + marginWidth2;
        if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth*2, buttonWidth),LanguageManager.getString("PASS")))
        {
            //  PASS
            SetActive(false);
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                player.getInternalPlayer().Bid(-1);
            }
            else
            {
                NetworkPlayer.GetInternalPlayer().Bid(-1);
            }
        }

        if (currentRound != null)
        {
            var biddings = currentRound.GetInitialPhase().GetRoundScores();

            for (int i = 0; i < 4; i++)
            {
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


                GUI.Label(new Rect(biddingsPos.x, offsetY, biddingsPos.width, biddingsPos.height), gameTable.gameTable.GetPlayerSeatedAt(i).PlayerName);
                GUI.Label(new Rect(biddingsPos2.x, offsetY, biddingsPos2.width, biddingsPos2.height), bid);
                offsetY += marginHeight/2;
            }
        }

        GUI.EndGroup();
        GUI.EndGroup();
    }


}
