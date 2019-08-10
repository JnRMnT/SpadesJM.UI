using UnityEngine;
using System.Collections;
using Common;
using Common.Helpers;
using Common.Structure;

using System.Runtime.CompilerServices;
using Assets.Scripts.System;

public class UICard : MonoBehaviour {
    private Card card;
    private Texture cardTexture;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private SpriteRenderer cardSprite;
    private int layerOrder;

    private bool released;

	// Use this for initialization
	public void Start () {
        initialPosition = Vector3.zero;
        released = false;
	}

    public void BindCard(Card card)
    {
        this.card = card;
        this.name = card.ToCardString();
        cardSprite = this.transform.GetComponent<SpriteRenderer>();
        cardSprite.sprite = Resources.Load("Textures/Actors/Cards/" + this.name, typeof(Sprite)) as Sprite;
        layerOrder = cardSprite.sortingOrder;
    }

    public void HandleTap(Vector2 position)
    {
        if (initialPosition == Vector3.zero)
        {
            //  TAPPING FOR THE FIRST TIME
            cardSprite.sortingOrder = 99;
            initialPosition = transform.localPosition;
            initialRotation = transform.localRotation;
        }
        transform.position = new Vector3(position.x,position.y,5f);
    }

	public void Update () {
	
	}

    public void Release()
    {
        GameObject scriptWrapper = GameObject.FindGameObjectWithTag("SCRIPTWRAPPER");
        if(scriptWrapper!=null && !released){
            released = true;
            MonoPlayer player = scriptWrapper.GetComponent<MonoPlayer>();
            MonoNetworkPlayer networkPlayer = scriptWrapper.GetComponent<MonoNetworkPlayer>();
            if (transform.position.y > player.splitter.transform.position.y)
            {
                //  CARD PLAYED
                this.transform.position = new Vector3(transform.position.x, transform.position.y,1f);
                player.playerDeck.playedCard = this;

                PlayedCardsController.PlaceCard(transform);
                UserInteraction.InputActive = false;

                // FINALLY ACTUALLY PLAY THE CARD
                if (Properties.ActiveGameType == GameType.SinglePlayer)
                {
                    player.getInternalPlayer().PlayCard(this.card);
                }
                else
                {
                    networkPlayer.GetInternalPlayer().PlayCard(this.card);
                }

                released = false;
            }
            else
            {
                ResetPosition(false);   
            }

        }
        
    }

    public void ResetPosition(bool recallCard)
    {
        
        cardSprite.sortingOrder = layerOrder;
        transform.parent = GameObject.FindGameObjectWithTag("DECK").transform;
        this.transform.localPosition = initialPosition;
        this.transform.localRotation = initialRotation;
        initialPosition = Vector3.zero;

        if (recallCard)
        {
            PlayedCardsController.RecallCard();
        }
        released = false;
    }

    public Card GetCard()
    {
        return card;
    }

    public void HideCard()
    {
        PlayedCardsController.RecallCard();
        gameObject.SetActive(false);
    }
}
