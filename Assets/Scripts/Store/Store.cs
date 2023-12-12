using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public SO_Deck storeDeck;
    public List<Card> availableCards;
    public int copiesPerCard = 10;
    private Dictionary<Card, int> cardStock;

    public Card cardPrefab; // Reference to the card prefab

    private void Start()
    {
        cardStock = new Dictionary<Card, int>();
        availableCards = new List<Card>();

        foreach (var cardData in storeDeck.Cards)
        {
            Card newCard = Instantiate(cardPrefab); // Create a new Card object
            newCard.name = "Store Card " + (availableCards.Count + 1).ToString();
            newCard.cardData = cardData; // Assign the SO_Card to the Card object
            availableCards.Add(newCard);
            cardStock[newCard] = copiesPerCard;
        }
    }

    public bool PurchaseCard(Card card, Player player)
    {
        if (cardStock[card] > 0)
        {
            cardStock[card]--;
            BattleController.TriggerOnCardBought(card, player);
            return true;
        }
        return false;
    }
}
