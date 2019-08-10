using Game.Actors;
using Common.Infos;
using Common.Enums;
using AI;
using System.Timers;
using UnityEngine;
using Game.Logic;
using Game;

public class UIComputer : Computer
{
    private MonoComputer unityComputer;
    private static ActionType awaitingAction;
    private static object staticCallBackObject;

    public UIComputer(GameTable gameTable,MonoComputer computer)
        : base(gameTable)
    {
        this.unityComputer = computer;
    }

    public override void AskForAction(ActionType actionType, object callbackObject, InfoDescription error)
    {
        unityComputer.TurnTimeoutHandler.StartTurnTimer(this,actionType,callbackObject);
        LogManager.Log(unityComputer.transform.name + " : " + actionType.ToString("G"));
        staticCallBackObject = callbackObject;

        if (error == InfoDescription.NoError)
        {
            //  NO ERROR
            TurnArrowController.SetActive(UIPlayer.GetRelativePlayerSeat(base.GetPlayersSeat()));
            awaitingAction = actionType;
            unityComputer.WaitForAction();
        }
        else
        {
            //  SHOW ERROR
            TurnArrowController.SetActive(UIPlayer.GetRelativePlayerSeat(GetPlayersSeat()));
            LogManager.Log(error.ToString());
        }


    }

    public override void SetTrumpType(CardType trumpType)
    {
        base.SetTrumpType(trumpType);
        CurrentTrump.SetTrumpType(trumpType);
    }

    public void DoAction()
    {
        LogManager.Log("Doing Action to :" + staticCallBackObject.ToString());
        base.AskForAction(awaitingAction, staticCallBackObject, InfoDescription.NoError);
    }

    public override void PlayCard(Common.Card cardToPlay)
    {
        PlayedCardsController.PlayCard(cardToPlay,UIPlayer.GetRelativePlayerSeat(GetPlayersSeat()),this);
    }

    public void EndPlayCard(Common.Card cardToPlay)
    {
        base.PlayCard(cardToPlay);
    }


}