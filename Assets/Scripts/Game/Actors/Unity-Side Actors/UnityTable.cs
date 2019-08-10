using Assets.Scripts.System;
using Common;
using Common.Enums;
using Common.Structure;
using Game.Actors;
using Game.Logic;
using Game.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Game.Actors
{
    public class UnityTable : GameTable
    {
        public UIGameTable UITable;
        public UnityTable(UIGameTable unityTable) : base(){
            this.UITable = unityTable;
        }

        public override void Sit(Player player, int seat)
        {
            if (seat == -1)
            {
                seat = GetAvailableSeats()[0];
            }

            seats[seat] = player;
            player.UpdateName(LanguageManager.getString("PLYR"));
        }

        public override void InitializeGame()
        {
            if (Properties.ActiveGameType == GameType.SinglePlayer)
            {
                GameEndingCondition gameEndingCondition = null;
                switch (gameMode)
                {
                    case GameMode.TargetScore:
                        gameEndingCondition = new TargetScoreCondition(modeParams);
                        break;
                    case GameMode.RoundCount:
                        object[] roundCountConditionParams = new object[modeParams.Length + 1];
                        roundCountConditionParams[0] = this;
                        modeParams.CopyTo(roundCountConditionParams, 1);
                        gameEndingCondition = new RoundCountCondition(modeParams);
                        break;
                }

                DealCards();
                UpdatePlayerDeckValues();

                game = new UnityGame(this, gameEndingCondition);
                game.Commence();
                state = GameState.PLAYING;
            }
            else
            {
                if (LobbyPlayerStats.RoomData.getRoomOwner() == MultiplayerManager.StaticPlayer.GetInternalPlayer().PlayerName)
                {
                    MultiplayerManager.DealCallback = 0;
                    DealCards();
                    UpdatePlayerDeckValues();
                }
            }

        }

        public override void DealSplittedCards(List<Card>[] splittedCards)
        {
            if (Properties.ActiveGameType == GameType.MultiPlayer)
            {
                UITable.MultiplayerGamers.DealCards(splittedCards);
            }
            base.DealSplittedCards(splittedCards);
        }

        public void ReceiveCards(List<Common.Card>[] splittedCards)
        {
            for (int i = 0; i < 4; i++)
            {
                Deck newPlayerDeck = new Deck(splittedCards[i]);
                seats[i].SetPlayersDeck(newPlayerDeck);
            }

            UITable.SoundManager.PlayDealSound();
        }

        /// <summary>
        /// Room Owner will send a Callback to this method after receiving feedback from all the players
        /// </summary>
        public void PlayersReadyCallback()
        {
            if (LobbyPlayerStats.RoomData.getRoomOwner() == MultiplayerManager.StaticPlayer.GetInternalPlayer().PlayerName)
            {
                MultiplayerManager.SendBytes(ByteHelper.GetBytes("START"));
            }
            GameEndingCondition gameEndingCondition = null;
            switch (MultiplayerManager.CurrentEndCondition)
            {
                case GameMode.TargetScore:
                    gameEndingCondition = new TargetScoreCondition(new object[1]{MultiplayerManager.CurrentEndConditionGoal});
                    break;
                case GameMode.RoundCount:
                    object[] roundCountConditionParams = new object[2];
                    roundCountConditionParams[0] = this;
                    roundCountConditionParams[1] = MultiplayerManager.CurrentEndConditionGoal;
                    gameEndingCondition = new RoundCountCondition(roundCountConditionParams);
                    break;
            }

            game = new UnityGame(this, gameEndingCondition);
            game.Commence(UITable.MultiplayerGamers.GetPlayerByPlayerName(LobbyPlayerStats.RoomData.getRoomOwner()).GetPlayersSeat());
            state = GameState.PLAYING;
        }
    }

}
