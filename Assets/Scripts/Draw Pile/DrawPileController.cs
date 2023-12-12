using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPileController : MonoBehaviour
{
    public List<Card> drawPile;
    public Transform DrawCardPosition;
    public Player player;

    private void OnEnable()
    {
        player.OnCardsDrawn += DrawCards;
    }

    private void OnDisable()
    {
        player.OnCardsDrawn -= DrawCards;
    }

    private void DrawCards(int numberOfCards)
    {
        List<Card> drawnCards = new List<Card>();

        for (int i = 0; i < numberOfCards; i++)
        {
            if (drawPile.Count > 0)
            {
                // Draw a card from the draw pile
                Card drawnCard = drawPile[0];
                drawPile.RemoveAt(0);
                drawnCards.Add(drawnCard);
            }
            else
            {
                // If the draw pile is empty, trigger the OnDrawPileEmpty event
                player.TriggerOnDrawPileEmpty();

                // Wait for the OnCardsSentToDrawPile and OnReshuffleDrawPile events to finish
                // This assumes that these events are synchronous and will finish before the next line of code is executed
                // If they are asynchronous, you will need to use a different method to wait for them to finish

                // Draw a card from the reshuffled draw pile
                if (drawPile.Count > 0)
                {
                    // Draw a card from the reshuffled draw pile
                    Card drawnCard = drawPile[0];
                    drawPile.RemoveAt(0);
                    drawnCards.Add(drawnCard);
                }
            }
        }

        // Send the drawn cards to the player's hand
        player.TriggerOnCardsDrawnToHand(drawnCards);
    }

    private void ShuffleDrawPile()
    {
        for (int i = 0; i < drawPile.Count; i++)
        {
            Card temp = drawPile[i];
            int randomIndex = Random.Range(i, drawPile.Count);
            drawPile[i] = drawPile[randomIndex];
            drawPile[randomIndex] = temp;
        }
    }
}