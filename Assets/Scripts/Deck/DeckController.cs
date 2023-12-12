using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public SO_Deck deckData;
    public List<Card> cards;
    public DiscardPileController discardPile;
    public DrawPileController drawPileController;
    public HandController handController;
    public Card cardPrefab; // Reference to the card prefab
    public Player player;

    public int startingHandSize = 5;

    private void OnEnable()
    {
        player.OnDeckInitialized += InitializeDeck;
    }

    private void OnDisable()
    {
        player.OnDeckInitialized -= InitializeDeck;
    }

    public void InitializeDeck()
    {
        cards = new List<Card>();
        foreach (var cardData in deckData.Cards)
        {
            Card newCard = Instantiate(cardPrefab); // Create a new Card object
            Card.player = player;
            newCard.name = "Card " + (cards.Count + 1).ToString();
            newCard.cardData = cardData; // Assign the SO_Card to the Card object
            cards.Add(newCard);
        }
        
        player.TriggerOnCardsSentToDrawPile(cards);
        drawPileController.drawPile = cards;
        player.TriggerOnReshuffleDrawPile();

        // Draw the starting hand
        player.TriggerOnCardsDrawn(startingHandSize);
    }

}
