using Common.Enums;
using Game;
using Game.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Game.Actors
{
    public class UIHand : Hand
    {
        public UIHand(Player initiater, CardType currentTrump, UnityRound currentRound)
            : base(initiater, currentTrump, currentRound)
        {
            currentRound.ResetTurnTimer();
        }
    }
}
