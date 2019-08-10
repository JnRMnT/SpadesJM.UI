

using com.shephertz.app42.gaming.multiplayer.client;
using Common;
using Common.Enums;
using Common.Helpers;
using Game.Actors;
using System.Collections.Generic;
public class LocalNetworkPlayer : NetworkPlayerBase
{
    private static MonoNetworkPlayer unityPlayer;

    public LocalNetworkPlayer(GameTable gameTable, MonoNetworkPlayer player)
        : base(gameTable, player)
    {
        unityPlayer = player;
    }

    protected override void HandleActionRequest(ActionType actionRequest)
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
                LogManager.Log("Play Card");
                break;

            case ActionType.SET_TRUMP_TYPE:
                UserInteraction.InputActive = false;
                TrumpSelection.Show();
                break;
        }
    }

    public override void SetTrumpType(CardType trumpType)
    {
        //  SEND DATA
        SendNetworkData(ActionType.SET_TRUMP_TYPE, CardHelper.GetNumericTypeFromEnum(trumpType));
        base.SetTrumpType(trumpType);
        CurrentTrump.SetTrumpType(trumpType);
    }

    public override void Bid(int bid)
    {
        //  SEND DATA
        SendNetworkData(ActionType.DO_BIDDING, bid);
        base.Bid(bid);
    }

    public override void SetPlayersDeck(Common.Structure.Deck newDeck)
    {
        base.SetPlayersDeck(newDeck);
        uiDeck.BindCards(newDeck);
    }

    public override void PlayCard(Common.Card cardToPlay)
    {
        if (uiDeck.playedCard == null || uiDeck.playedCard.GetCard().GetCardType() != cardToPlay.GetCardType() || uiDeck.playedCard.GetCard().GetCardValue() != cardToPlay.GetCardValue())
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

                    //  SEND DATA
                    SendNetworkData(ActionType.PLAY_CARD, cardToPlay);
                    // FINALLY ACTUALLY PLAY THE CARD
                    EndPlayCard(cardToPlay);
                    found = true;
                }
                i++;
            }
        }
        else
        {
            SendNetworkData(ActionType.PLAY_CARD, cardToPlay);
            // FINALLY ACTUALLY PLAY THE CARD
            EndPlayCard(cardToPlay);
        }

    }

    public void SitToTable(int seat,MultiplayerGamersCommunicator multiplayerGamersCommunicator)
    {
        int playersSeat = GetPlayersSeat();
        if (LobbyPlayerStats.RoomData.getRoomOwner() == PlayerName)
        {
            //  INFORM OTHERS
            Dictionary<string, object> tableProperties = new Dictionary<string, object>();
            tableProperties.Add("SEAT" + seat.ToString(), PlayerName);
            List<string> removeProperties = null;
            if (playersSeat != -1)
            {
                gameTable.LeaveTable(playersSeat);
                removeProperties = new List<string>();
                removeProperties.Add("SEAT" + playersSeat.ToString());
            }
            WarpClient.GetInstance().UpdateRoomProperties(LobbyPlayerStats.RoomData.getId(), tableProperties, removeProperties);
        }
        else
        {
            string peerSitUpdate = "SIT-" + seat.ToString() + "-" + PlayerName;
            if (playersSeat != -1)
            {
                gameTable.LeaveTable(playersSeat);
            }
            MultiplayerManager.SendBytes(ByteHelper.GetBytes(peerSitUpdate));
        }
    }

    protected void SendNetworkData(ActionType actionType, object actionValue)
    {
        string networkMessage = string.Empty;
        switch (actionType)
        {
            case ActionType.DO_BIDDING:
                string bid = actionValue.ToString();
                if (bid == "-1")
                {
                    bid = "999";
                }
                networkMessage = "BID-" + GetPlayersSeat() + "-" + bid;
                break;

            case ActionType.SET_TRUMP_TYPE:
                networkMessage = "SETTRMP-" + GetPlayersSeat() + "-" + actionValue.ToString();
                break;

            case ActionType.PLAY_CARD:
                Card cardToPlay = (Card) actionValue;
                networkMessage = "PLAY-" + GetPlayersSeat() + "-" + CardHelper.GetNumericTypeFromEnum(cardToPlay.GetCardType()) + "-" + cardToPlay.GetNumericValue();
                break;
        }

        MultiplayerManager.SendBytes(ByteHelper.GetBytes(networkMessage));
    }

    public static int GetRelativePlayerSeat(int player)
    {
        var seat = player - unityPlayer.GetInternalPlayer().GetPlayersSeat();
        if (seat < 0) seat += 4;

        return seat;
    }

    public void ResetGame()
    {
        List<string> removeList = new List<string>();
        removeList.Add("SEAT0");
        removeList.Add("SEAT1");
        removeList.Add("SEAT2");
        removeList.Add("SEAT3");
        WarpClient.GetInstance().UpdateRoomProperties(LobbyPlayerStats.RoomData.getId(), null, removeList);
    }
}

