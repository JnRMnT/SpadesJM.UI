using Common.Enums;
using Common.Infos;
using Game.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class NetworkPlayerBase : Player
{
    public MonoNetworkPlayerBase unityPlayer;
    protected UIDeck uiDeck;
    protected BiddingPopup biddingPopup;

    public NetworkPlayerBase(GameTable gameTable, MonoNetworkPlayerBase player)
        : base(gameTable)
    {
        unityPlayer = player;
        uiDeck = GameObject.FindGameObjectWithTag("DECK").GetComponent<UIDeck>();
        biddingPopup = GameObject.FindGameObjectWithTag("MENUS").transform.FindChild("BiddingPopup").GetComponent<BiddingPopup>();
    }

    public Transform GetTransform()
    {
        return unityPlayer.transform;
    }

    public override void AskForAction(ActionType actionType, object callbackObject, InfoDescription error)
    {
        this.callbackObject = callbackObject;
        unityPlayer.TurnTimeoutHandler.StartTurnTimer(this, actionType, callbackObject);
        if (error == InfoDescription.NoError)
        {
            //  NO ERROR
            HandleActionRequest(actionType);
            TurnArrowController.SetActive(LocalNetworkPlayer.GetRelativePlayerSeat(GetPlayersSeat()));
        }
        else
        {
            //  SHOW ERROR
            HandleError(error);
            HandleActionRequest(actionType);
            TurnArrowController.SetActive(LocalNetworkPlayer.GetRelativePlayerSeat(GetPlayersSeat()));
            LogManager.Log(error.ToString("G"));
        }


    }

    protected virtual void HandleError(InfoDescription error)
    {
        switch (error)
        {
            case InfoDescription.CannotPlayThatCard:
                uiDeck.playedCard.ResetPosition(true);
                break;
        }
    }

    protected virtual void HandleActionRequest(ActionType actionRequest)
    {
        //  DUMMY
    }

    public virtual void EndPlayCard(Common.Card cardToPlay)
    {
        base.PlayCard(cardToPlay);
    }
}
