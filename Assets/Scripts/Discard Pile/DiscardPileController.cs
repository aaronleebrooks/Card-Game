using System;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPileController : MonoBehaviour
{
    public List<Card> discardedCards;
    public Transform discardPilePosition;
    public Player player;

    private void OnEnable()
    {
        player.OnCardDiscardedForMana += DiscardForMana;
        player.OnCardsSentToDiscardPile += DiscardWithoutManaGain;
        player.OnDrawPileEmpty += MoveAllToDrawPile;
    }

    private void OnDisable()
    {
        player.OnCardDiscardedForMana -= DiscardForMana;
        player.OnCardsSentToDiscardPile -= DiscardWithoutManaGain;
        player.OnDrawPileEmpty -= MoveAllToDrawPile;
    }

    public void DiscardForMana(Card card)
    {
        discardedCards.Add(card);
        card.transform.position = discardPilePosition.position;
    }

    public void DiscardWithoutManaGain(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            discardedCards.Add(card);
            card.transform.position = discardPilePosition.position;
        }
    }

    private void MoveAllToDrawPile()
    {
        // Move all cards to the draw pile
        player.TriggerOnCardsSentToDiscardPile(discardedCards);
        discardedCards.Clear();
    }
}