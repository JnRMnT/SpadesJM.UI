using Game.Structure;
using System.Collections.Generic;
using System.Text;
using Game.Actors;

namespace Assets.Scripts.Game.Actors
{
    public class UnityRound : Round
    {
        public UnityRound(GameObject gameInstance, Player firstPlayer)
            : base(gameInstance, firstPlayer)
        {

        }

        public override void StartRound()
        {

            UIHand firstHand = new UIHand(this.bidWinningPlayer, this.trumpType, this);
            firstHand.StartHand();
        }

        public override void HandEnded(Player winner)
        {
            PlayedCardsController.ResetHand(winner.GetPlayersSeat(),this,winner);
        }

        public void EndHand()
        {
            var awaitingWinner = PlayedCardsController.awaitingWinner;

            roundScores[awaitingWinner.GetPlayersSeat()].IncrementGot();
            handCount++;
            if (handCount > 13)
            {
                PlayedCardsController.EndRound(roundScores, bidWinningPlayer.GetPlayersSeat());
            }
            else
            {
                UIHand nextHand = new UIHand(awaitingWinner, trumpType, this);
                nextHand.StartHand();
            }
        }

        public override void InitiateBidding()
        {
            //  HANDLE BIDDING
            this.activeBidding = new UnityInitialPhase(firstPlayer, this);
            this.activeBidding.StartAsking();
        }

        public UnityInitialPhase GetInitialPhase()
        {
            return this.activeBidding as UnityInitialPhase;
        }

        public void ResetTurnTimer()
        {
            ((GetGameObjectReference() as UnityGame).GetGameTableRefrence() as UnityTable).UITable.ResetTurnTimer();
        }
    }

}
