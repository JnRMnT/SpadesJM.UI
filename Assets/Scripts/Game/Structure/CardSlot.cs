using Common;
using UnityEngine;

public class CardSlot
{
    public Card Card { get; set; }
    public GameObject CardObject { get; set; }

    public CardSlot(GameObject card)
    {
        this.CardObject = card;
    }
}
