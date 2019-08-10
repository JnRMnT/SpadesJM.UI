using Common.Enums;
using Common.Infos;
using Game.Actors;


public class RemoteNetworkPlayer: NetworkPlayerBase
{
    private static ActionType awaitingAction;

    public RemoteNetworkPlayer(GameTable gameTable,MonoNetworkPlayerBase player)
        : base(gameTable,player)
    {

    }

    public override void AskForAction(ActionType actionType, object callbackObject, InfoDescription error)
    {
        base.AskForAction(actionType, callbackObject, error);

        unityPlayer.TurnTimeoutHandler.StartTurnTimer(this, actionType, callbackObject);
    }

    public override void SetTrumpType(CardType trumpType)
    {
        base.SetTrumpType(trumpType);
        CurrentTrump.SetTrumpType(trumpType);
    }

    protected override void HandleError(InfoDescription error)
    {
        switch (error)
        {
            case InfoDescription.CannotPlayThatCard:
                uiDeck.playedCard.HideCard();
                break;
        }
    }

    public override void PlayCard(Common.Card cardToPlay)
    {
        PlayedCardsController.PlayCard(cardToPlay, LocalNetworkPlayer.GetRelativePlayerSeat(GetPlayersSeat()), this);
    }

    public override void EndPlayCard(Common.Card cardToPlay)
    {
        base.EndPlayCard(cardToPlay);
    }

}

