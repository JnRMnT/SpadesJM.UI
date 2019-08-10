using UnityEngine;
using System.Collections;
using Common;
using Assets.Scripts.Game.Actors;
using Game.Actors;
using Assets.Scripts.System;

public class PlayedCardsController : MonoBehaviour {
    public SoundManager SoundManager;
    private static SoundManager SoundManagerInstance;

    private static Transform playedCardsTransform;

    public static Transform[] Cards;
    public static int playedCardCount;
    private static GameObject cardPrefab;
    public GameObject cardPrefabForEditor;

    private static Card cardToPlay;
    private static Vector3 destinationPosition;
    private static Quaternion destinationRotation;
    private static Transform[] playerPositions;
    private static bool moveCard, moveCard2, moveCards;
    private static UIComputer cardPlayer;
    private static NetworkPlayerBase cardPlayer2;

    public static Player awaitingWinner;
    private static UnityRound unityRound;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private float rotationSpeed = 10f;
    private float moveSpeed = 40f, moveSpeed2 = 10f;


    private static bool endRound;
    private float roundEndCounter,handEndCounter;

	// Use this for initialization
	void Start () {
        Cards = new Transform[4];

        Cards[0] = transform.FindChild("Card1");
        Cards[1] = transform.FindChild("Card2");
        Cards[2] = transform.FindChild("Card3");
        Cards[3] = transform.FindChild("Card4");

        Cards[0].gameObject.SetActive(false);
        Cards[1].gameObject.SetActive(false);
        Cards[2].gameObject.SetActive(false);
        Cards[3].gameObject.SetActive(false);

        playedCardCount = 0;

        cardPrefab = cardPrefabForEditor;

        playerPositions = new Transform[4];
        playerPositions[0] = GameObject.FindGameObjectWithTag("PLAYER1").transform;
        playerPositions[1] = GameObject.FindGameObjectWithTag("PLAYER2").transform;
        playerPositions[2] = GameObject.FindGameObjectWithTag("PLAYER3").transform;
        playerPositions[3] = GameObject.FindGameObjectWithTag("PLAYER4").transform;

        moveCard = false;

        playedCardsTransform = transform;
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        endRound = false;
        roundEndCounter = 6f;
        handEndCounter = 1.5f;

        SoundManagerInstance = SoundManager;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (endRound)
        {
            roundEndCounter -= Time.fixedDeltaTime;

            if (roundEndCounter < 0)
            {
                playedCardCount = 0;
                endRound = false;
                ScoreBoard.Hide();
                roundEndCounter = 6f;
                unityRound.EndRound();
            }
        }


        if (moveCard)
        {
            if (MoveCard(Cards[playedCardCount - 1].transform,moveSpeed))
            {
                moveCard = false;
                cardPlayer.EndPlayCard(cardToPlay);
            }
        }

        if (moveCard2)
        {
            if (MoveCard(Cards[playedCardCount - 1].transform, moveSpeed))
            {
                moveCard2 = false;
                cardPlayer2.EndPlayCard(cardToPlay);
            }
        }

        if (moveCards)
        {
            if (handEndCounter <= 0)
            {
                if (MoveCard(playedCardsTransform,moveSpeed2))
                {
                    var playedCards = GetComponentsInChildren<Transform>();

                    foreach (Transform card in playedCards)
                    {
                        if (card != transform)
                        {
                            card.gameObject.SetActive(false);
                        }
                    }
                    playedCardCount = 0;
                    transform.rotation = initialRotation;
                    transform.position = initialPosition;

                    handEndCounter = 1.5f;
                    unityRound.EndHand();
                }
            }
            else
            {
                handEndCounter -= Time.fixedDeltaTime;
            }
        }
	}

    private bool MoveCard(Transform cardToMove,float moveSpeed)
    {
        cardToMove.position = Vector3.MoveTowards(cardToMove.position, destinationPosition, Time.deltaTime * moveSpeed);

        cardToMove.rotation = Quaternion.Lerp(cardToMove.rotation, destinationRotation, rotationSpeed * Time.deltaTime);

        Quaternion inverseTargetRotation = new Quaternion(-destinationRotation.x, transform.rotation.y, -destinationRotation.z, -destinationRotation.w);
        if ((cardToMove.rotation == destinationRotation || cardToMove.rotation == inverseTargetRotation) && (cardToMove.position == destinationPosition))
        {
            moveCards = false;
            return true;
        }else return false;
    }

    public static void PlayCard(Card card,int playedBy,UIComputer playerComputer)
    {
        destinationPosition = Cards[playedCardCount].position;
        destinationRotation = Cards[playedCardCount].rotation;

        GameObject newCardObject = Instantiate(cardPrefab, playerPositions[playedBy].position,cardPrefab.transform.rotation) as GameObject;
        newCardObject.transform.parent = playedCardsTransform;
        UICard cardScript = newCardObject.transform.GetComponent<UICard>();
        newCardObject.transform.GetComponent<SpriteRenderer>().sortingOrder = 8;

        cardScript.BindCard(card);
        Cards[playedCardCount] = newCardObject.transform;
        playedCardCount++;
        cardToPlay = card;
        cardPlayer = playerComputer;
        moveCard = true;

        SoundManagerInstance.PlayCardPlaySound();
        
    }

    public static void PlayCard(Card card, int playedBy, RemoteNetworkPlayer networkPlayer)
    {
        destinationPosition = Cards[playedCardCount].position;
        destinationRotation = Cards[playedCardCount].rotation;

        GameObject newCardObject = Instantiate(cardPrefab, playerPositions[playedBy].position, cardPrefab.transform.rotation) as GameObject;
        newCardObject.transform.parent = playedCardsTransform;
        UICard cardScript = newCardObject.transform.GetComponent<UICard>();
        newCardObject.transform.GetComponent<SpriteRenderer>().sortingOrder = 8;

        cardScript.BindCard(card);
        Cards[playedCardCount] = newCardObject.transform;
        playedCardCount++;
        cardToPlay = card;
        cardPlayer2 = networkPlayer;
        moveCard2 = true;

        UIDeck deck = GameObject.FindGameObjectWithTag("DECK").GetComponent<UIDeck>();
        deck.playedCard = cardScript;

        SoundManagerInstance.PlayCardPlaySound();
    }

    public static void PlaceCard(Transform card)
    {
        SoundManagerInstance.PlayCardPlaySound();

        LogManager.Log("Played Card Count : "+playedCardCount.ToString());
        card.GetComponent<SpriteRenderer>().sortingOrder = 8;
        card.position = Cards[playedCardCount].position;
        card.rotation = Cards[playedCardCount].rotation;
        card.parent = playedCardsTransform;
        playedCardCount++;
    }



    public static void ResetHand(int winner,UnityRound currentRound,Player winnerPlayer)
    {
        if (Properties.ActiveGameType == GameType.SinglePlayer)
        {
            destinationPosition = playerPositions[winner].position;
            destinationRotation = playerPositions[winner].rotation;
        }
        else
        {
            int networkWinnerSeat = LocalNetworkPlayer.GetRelativePlayerSeat(winner);
            destinationPosition = playerPositions[networkWinnerSeat].position;
            destinationRotation = playerPositions[networkWinnerSeat].rotation;
        }

        unityRound = currentRound;

        playedCardCount = 0;
        PlayedCardsController.awaitingWinner = winnerPlayer;

        moveCards = true;

        TurnArrowController.TurnArrowOff();
    }

    public static void RecallCard()
    {
        playedCardCount--;
        if (playedCardCount < 0) playedCardCount = 0;
    }


    public static void EndRound(Game.Structure.RoundScore[] roundScores, int initiater)
    {
        endRound = true;
        TurnArrowController.TurnArrowOff();
        Game.Actors.ScoreBoard.UpdateScores(roundScores, initiater);
        ScoreBoard.Show(true);
    }

}
