using UnityEngine;
using System.Collections;
using Game.Actors;
using AI;
using Assets.Scripts.Game.Actors;

public class UIGameTable : MonoBehaviour {

    public MonoComputer computer1, computer2, computer3;
    public MonoPlayer player;
    public UnityTable gameTable;
    private static UnityTable staticGameTable;
    public MultiplayerGamersCommunicator MultiplayerGamers;
    public TurnTimeoutHandler TurnTimeoutHandler;
    public SoundManager SoundManager;

	// Use this for initialization
	void Start () {
        gameTable = new UnityTable(this);
        player.InitializePlayer(gameTable);
        staticGameTable = gameTable;
	}

    public void InitializeMultiPlayerGame(Common.Enums.GameMode gameMode,object endingCondition)
    {
        object[] endingConditionParams = new object[2];
        endingConditionParams[0] = gameTable;
        endingConditionParams[1] = endingCondition;
        gameTable.ChangeGameMode(gameMode, endingConditionParams);

        UIDeck.Show();

        gameTable.InitializeGame();
       
    }


    public void InitializeSinglePlayerGame()
    {
        ResetPlayers();

        //  DUMMY CONDITIONS
        object[] endingConditionParams = new object[2];
        endingConditionParams[0] = gameTable;
        endingConditionParams[1] = (object)5;
        gameTable.ChangeGameMode(Common.Enums.GameMode.RoundCount, endingConditionParams);

        UIDeck.Show();

        player.InitializePlayer(gameTable);

        UIComputer player2 = computer1.InitializeComputer();
        UIComputer player3 = computer2.InitializeComputer();
        UIComputer player4 = computer3.InitializeComputer();

        AddPlayer(player.getInternalPlayer());
        AddPlayer(player2);
        AddPlayer(player3);
        AddPlayer(player4);

        gameTable.InitializeGame();
    }

    public void InitializeSinglePlayerGame(Common.Enums.GameMode gameMode,object endingCondition)
    {
        ResetPlayers();

        object[] endingConditionParams = new object[2];
        endingConditionParams[0] = gameTable;
        endingConditionParams[1] = endingCondition;
        gameTable.ChangeGameMode(gameMode, endingConditionParams);

        UIDeck.Show();

        player.InitializePlayer(gameTable);

        UIComputer player2 = computer1.InitializeComputer();
        UIComputer player3 = computer2.InitializeComputer();
        UIComputer player4 = computer3.InitializeComputer();

        AddPlayer(player.getInternalPlayer());
        AddPlayer(player2);
        AddPlayer(player3);
        AddPlayer(player4);

        gameTable.InitializeGame();
    }

    public void ResetPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            Player currentPlayer = gameTable.GetPlayerSeatedAt(i);
            if (currentPlayer != null)
            {
                gameTable.LeaveTable(currentPlayer);
            }
        }
    }

    public void AddPlayer(Game.Actors.Player player)
    {
        gameTable.Sit(player);
    }
    public void AddPlayer(Game.Actors.Player player,int seat)
    {
        gameTable.Sit(player, seat);
    }

    public void ResetTurnTimer()
    {
        TurnTimeoutHandler.ResetTurnTimer();
    }

    public static void CleanTable()
    {
        TurnArrowController.TurnArrowOff();
        TrumpSelection.Hide();
        ScoreBoard.Hide();
        staticGameTable.SetGameState(Common.Enums.GameState.INLOBBY);
        Game.Actors.ScoreBoard.ResetScores();
        BiddingPopup.Hide();
        CurrentTrump.currentTrumpActive = false;
        UserInteraction.InputActive = false;

        foreach (Transform card in GameObject.FindGameObjectWithTag("DECK").transform)
        {
            if (!card.name.Contains("Card"))
            {
                Destroy(card.gameObject);
            }
        }

        PlayedCardsController.Cards[0].gameObject.SetActive(false);
        PlayedCardsController.Cards[1].gameObject.SetActive(false);
        PlayedCardsController.Cards[2].gameObject.SetActive(false);
        PlayedCardsController.Cards[3].gameObject.SetActive(false);

        PlayedCardsController.playedCardCount = 0;
    }
}
