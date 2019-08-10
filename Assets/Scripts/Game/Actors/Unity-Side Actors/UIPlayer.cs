using Game.Actors;
using Common.Infos;
using Common.Enums;
using UnityEngine;
using Common;

public class UIPlayer : Game.Actors.Player
{
    private static MonoPlayer unityPlayer;
    private BiddingPopup biddingPopup;
    private UIDeck uiDeck;

    public UIPlayer(GameTable gameTable, MonoPlayer player)
        : base(gameTable)
    {
        unityPlayer = player;
        biddingPopup = GameObject.FindGameObjectWithTag("MENUS").transform.FindChild("BiddingPopup").GetComponent<BiddingPopup>();
        uiDeck = GameObject.FindGameObjectWithTag("DECK").GetComponent<UIDeck>();
    }

    public override void AskForAction(ActionType actionType, object callbackObject,InfoDescription error)
    {
        this.callbackObject = callbackObject;
        unityPlayer.TurnTimeoutHandler.StartTurnTimer(this, actionType, callbackObject);
        if (error == InfoDescription.NoError)
        {
            //  NO ERROR
            HandleActionRequest(actionType);
            TurnArrowController.SetActive(GetPlayersSeat());
        }
        else
        {
            //  SHOW ERROR
            HandleError(error);
            HandleActionRequest(actionType);
            TurnArrowController.SetActive(GetPlayersSeat());
            LogManager.Log(error.ToString("G"));
        }

       
    }

    private void HandleError(InfoDescription error)
    {
        switch (error)
        {
            case InfoDescription.CannotPlayThatCard:
                uiDeck.playedCard.ResetPosition(true);
                break;
        }
    }

    private void HandleActionRequest(ActionType actionRequest)
    {
        unityPlayer.SoundManager.PlayTurnReadySound();
        switch (actionRequest)
        {
            case ActionType.DO_BIDDING:
                UserInteraction.InputActive = false;
                biddingPopup.SetCurrentRound(gameTable.GetGameInstance().GetCurrentRound() as Assets.Scripts.Game.Actors.UnityRound);
                biddingPopup.SetActive(true);
                break;

            case ActionType.PLAY_CARD:
                UserInteraction.InputActive = true;
                unityPlayer.Test("Play Card");
                break;

            case ActionType.SET_TRUMP_TYPE:
                UserInteraction.InputActive = false;
                TrumpSelection.Show();
                break;
        }
    }

    public override void PlayCard(Common.Card cardToPlay)
    {
        Card playedCard = null;
        if (uiDeck.playedCard != null)
        {
            playedCard = uiDeck.playedCard.GetCard();
        }
        
        if (playedCard == null || playedCard.GetCardType() != cardToPlay.GetCardType() || playedCard.GetCardValue() != cardToPlay.GetCardValue())
        {
            CardSlot[] cardSlots = uiDeck.GetCardSlots();
            int i = 0;
            bool found = false;
            while (!found)
            {
                if (cardSlots[i].Card.GetCardType() == cardToPlay.GetCardType() && cardSlots[i].Card.GetCardValue() == cardToPlay.GetCardValue())
                {
                    uiDeck.playedCard = cardSlots[i].CardObject.transform.GetComponent<UICard>();
                    PlayedCardsController.PlaceCard(cardSlots[i].CardObject.transform);
                    UserInteraction.InputActive = false;

                    // FINALLY ACTUALLY PLAY THE CARD
                    base.PlayCard(cardToPlay);
                    found = true;
                }
                i++;
            }
        }
        else
        {
            base.PlayCard(cardToPlay);
        }

    }

    public override void Bid(int bid)
    {
        base.Bid(bid);
        biddingPopup.SetActive(false);
    }

    public override void SetTrumpType(CardType trumpType)
    {
        base.SetTrumpType(trumpType);
        CurrentTrump.SetTrumpType(trumpType);
    }

    public static int GetRelativePlayerSeat(int player)
    {
        var seat = player - unityPlayer.getInternalPlayer().GetPlayersSeat();
        if (seat < 0) seat += 4;

        return seat;
    }

    public override void SetPlayersDeck(Common.Structure.Deck newDeck)
    {
        base.SetPlayersDeck(newDeck);
        uiDeck.BindCards(newDeck);
    }

    public static void Test(string test)
    {
        Debug.Log(test);
    }
}
