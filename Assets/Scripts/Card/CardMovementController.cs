using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovementController : MonoBehaviour
{
    public CardPosition drawPilePosition;
    public List<CardPosition> handPositions;
    public CardPosition discardPilePosition;
    public CardPosition playerFieldPosition;
    public CardPosition selectedPosition;
    public Player player;

    private void Awake()
    {
        player.OnCardsDrawnToHand += MoveCardsToHand;
        player.OnCardDiscardedForMana += MoveCardToDiscardPile;
        player.OnCardsSentToDrawPile += MoveCardsToDrawPile;
        player.OnCardsSentToDiscardPile += MoveCardsToDiscardPile;
        player.OnHandCardHover += MoveCardUpOnHover;
        player.OffHandCardHover += MoveCardDownOffHover;
        player.OnSelectHandCard += MoveCardToSelectedPosition;
        player.OffSelectHandCard += MoveCardBackToHand;
        player.OnReplaceHandCard += MoveCardBackToHand;
        player.OnPlayHandCard += MoveCardToPlayfield;
        // Add more event subscriptions as needed
    }

    private void OnDestroy()
    {
        player.OnCardsDrawnToHand -= MoveCardsToHand;
        player.OnCardDiscardedForMana -= MoveCardToDiscardPile;
        player.OnCardsSentToDrawPile -= MoveCardsToDrawPile;
        player.OnCardsSentToDiscardPile -= MoveCardsToDiscardPile;
        player.OnHandCardHover -= MoveCardUpOnHover;
        player.OffHandCardHover -= MoveCardDownOffHover;
        player.OnSelectHandCard -= MoveCardToSelectedPosition;
        player.OffSelectHandCard -= MoveCardBackToHand;
        player.OnReplaceHandCard -= MoveCardBackToHand;
        player.OnPlayHandCard -= MoveCardToPlayfield;
        // Remove the event subscriptions when the object is destroyed
    }

    public void MoveCardToPosition(Card card, Vector3 position)
    {
        if (card != null)
        {
            card.targetPoint = position;
        }
        else
        {
            Debug.LogError("Card is null");
        }
    }

    // Similar checks can be added to other methods

    public void MoveCardToPositionAndRotation(Card card, Vector3 position, Quaternion rotation)
    {
        card.targetPoint = position;
        card.targetRotation = rotation;
    }

    private void MoveCardsToHand(List<Card> cards)
    {
        foreach (var card in cards)
        {
            MoveCardToPosition(card, handPositions[cards.IndexOf(card)].transform.position);
            card.cardLocation = CardLocation.Hand;
        }
    }

    private void MoveCardToDiscardPile(Card card)
    {
        MoveCardToPosition(card, discardPilePosition.transform.position);
        card.cardLocation = CardLocation.DiscardPile;
    }

    private void MoveCardsToDiscardPile(List<Card> cards)
    {
        foreach (var card in cards)
        {
            MoveCardToPosition(card, discardPilePosition.transform.position);
            card.cardLocation = CardLocation.DiscardPile;
        }
    }

    private void MoveCardsToDrawPile(List<Card> cards)
    {
        foreach (var card in cards)
        {
            MoveCardToPosition(card, drawPilePosition.transform.position);
            card.cardLocation = CardLocation.DrawPile;
            card.SetIsCardBackShown(true);
        }
    }

    private void MoveCardUpOnHover(Card card)
    {   
        Vector3 hoverPosition = new Vector3(card.transform.position.x, card.transform.position.y + 0.5f, card.transform.position.z);
        MoveCardToPosition(card, hoverPosition);
    }

    private void MoveCardDownOffHover(Card card)
    {
        Vector3 hoverPosition = new Vector3(card.transform.position.x, card.transform.position.y - 0.5f, card.transform.position.z);
        MoveCardToPosition(card, hoverPosition);
    }

    private void MoveCardToSelectedPosition(Card card)
    {
        MoveCardToPosition(card, selectedPosition.transform.position);
        card.cardLocation = CardLocation.Selected;
    }

    private void MoveCardBackToHand(Card card)
    { 
        Vector3 handPosition = GetLastActiveHandPosition().transform.position;
        MoveCardToPosition(card, handPosition);
        card.cardLocation = CardLocation.Hand;
    }

    private void MoveCardToPlayfield(Card card, CardPosition playfieldPosition)
    {
        MoveCardToPosition(card, playfieldPosition.transform.position);
        card.cardLocation = CardLocation.Playfield;
    }

    public CardPosition GetLastActiveHandPosition()
    {
        for (int i = handPositions.Count - 1; i >= 0; i--)
        {
            if (handPositions[i].gameObject.activeInHierarchy)
            {
                return handPositions[i];
            }
        }

        return null; // Return null if no active CardPosition is found
    }
}