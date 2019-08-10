using Assets.Scripts.System;
using Game.Actors;
using Game.Logic;
using Game.Structure;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Game.Actors
{
    public class UnityGame : GameObject
    {
        public UnityGame(GameTable gameTable,GameEndingCondition gameEndingCondition)
            : base(gameTable, gameEndingCondition)
        {

        }

        public override void EndGame()
        {
            AdsManager.DisplayIntersAd();
            EndGamePopup.Show();
            base.EndGame();
        }

        public override void Commence()
        {
            this.currentRound = new UnityRound(this, gameTable.GetPlayerSeatedAt(0));
            this.currentRound.InitiateBidding();

            AdsManager.DisplayIntersAd();
        }

        public override void Commence(int firstPlayer)
        {
            this.currentRound = new UnityRound(this, gameTable.GetPlayerSeatedAt(firstPlayer));
            this.currentRound.InitiateBidding();
        }

        public override void RoundEnded(Player firstPlayer)
        {
            playedRoundCount++;
            if (IsGameConditionMet())
            {
                EndGame();
            }
            else
            {
                currentRound = new UnityRound(this, gameTable.GetPlayerSeatedAt((gameTable.GetPlayersSeat(firstPlayer) + 1) % 4));
                if (Properties.ActiveGameType == GameType.SinglePlayer)
                {
                    currentRound.InitiateBidding();
                }
                else
                {
                    MultiplayerManager.DealCallback = 1;
                }
            }
        }
        /// <summary>
        /// Room Owner will send a Callback to this method after receiving feedback from all the players
        /// </summary>
        public void CommenceRoundCallBack()
        {
            currentRound.InitiateBidding();
        }
    }
}
