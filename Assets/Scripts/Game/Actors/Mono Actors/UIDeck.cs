using UnityEngine;
using System.Collections;
using Common.Structure;
using System.Collections.Generic;
using Common;

public class UIDeck : MonoBehaviour {
    private CardSlot[] cardSlots, cardSlotsInitial;
    public GameObject cardPrefab;
    public UICard playedCard;

    private static Transform transformInstance;

	// Use this for initialization
	void Start () {
        transformInstance = transform;
        cardSlots = new CardSlot[13];
        cardSlotsInitial = new CardSlot[13];
        for (int i = 0; i < 13; i++)
        {
            var thisCard = transform.FindChild("Card" + (i+1).ToString());
            cardSlotsInitial[i] = new CardSlot(thisCard.gameObject);
            thisCard.gameObject.SetActive(false);
        }
	}

    public static void Show()
    {
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);

    }

    public void BindCards(List<Card> cards)
    {
        for(int i=0;i<cards.Count;i++)
        {
            GameObject newCardObject = Instantiate(cardPrefab, cardSlotsInitial[i].CardObject.transform.position, transform.rotation) as GameObject;
            if (cardSlots[i] != null)
            {
                Destroy(cardSlots[i].CardObject);
            }

            cardSlots[i] = new CardSlot(newCardObject);
            cardSlots[i].Card = cards[i];

            newCardObject.transform.parent = this.transform;
            newCardObject.transform.position = cardSlotsInitial[i].CardObject.transform.position;
            SpriteRenderer newSpriteRenderer = newCardObject.GetComponent<SpriteRenderer>();
            if (i < 7)
            {
                newSpriteRenderer.sortingOrder = i + 8;
            }
            else
            {
                newSpriteRenderer.sortingOrder = i - 5;
            }
            UICard newCard = newCardObject.GetComponent<UICard>();
            newCard.BindCard(cards[i]);
        }

        Hide();
    }

    public void BindCards(Deck deck)
    {
        var cards = deck.GetCards();
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject newCardObject = Instantiate(cardPrefab, cardSlotsInitial[i].CardObject.transform.position, transform.rotation) as GameObject;
            if (cardSlots[i] != null)
            {
                Destroy(cardSlots[i].CardObject);
            }

            cardSlots[i] = new CardSlot(newCardObject);
            cardSlots[i].Card = cards[i];

            newCardObject.transform.parent = this.transform;
            newCardObject.transform.position = cardSlotsInitial[i].CardObject.transform.position;
            SpriteRenderer newSpriteRenderer = newCardObject.GetComponent<SpriteRenderer>();
            if (i < 7)
            {
                newSpriteRenderer.sortingOrder = i + 8;
            }
            else
            {
                newSpriteRenderer.sortingOrder = i - 5;
            }
            UICard newCard = newCardObject.GetComponent<UICard>();
            newCard.BindCard(cards[i]);
        }
    }

    public CardSlot[] GetCardSlots()
    {
        return this.cardSlots;
    }
}
