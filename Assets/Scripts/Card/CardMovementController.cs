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
            CardPosition firstEmpty = GetFirstEmptyHandPosition();
            card.cardLocation = CardLocation.Hand;
            card.cardPosition = firstEmpty;
            firstEmpty.HasCard = true;
        }
    }

    private void MoveCardToDiscardPile(Card card)
    {
        card.cardPosition = discardPilePosition;
        card.cardLocation = CardLocation.DiscardPile;
    }

    private void MoveCardsToDiscardPile(List<Card> cards)
    {
        foreach (var card in cards)
        {
            card.cardPosition = discardPilePosition;
            card.cardLocation = CardLocation.DiscardPile;
        }
    }

    private void MoveCardsToDrawPile(List<Card> cards)
    {
        foreach (var card in cards)
        {
            card.cardPosition = drawPilePosition;
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
        card.cardPosition = selectedPosition;
        card.cardLocation = CardLocation.Selected;
    }

    private void MoveCardBackToHand(Card card)
    { 
        CardPosition handPosition = GetLastActiveHandPosition();
        card.cardPosition = handPosition;
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

    public CardPosition GetFirstEmptyHandPosition()
    {
        foreach (var handPosition in handPositions)
        {
            if (!handPosition.gameObject.activeInHierarchy || handPosition.HasCard)
            {
                continue;
            }
            return handPosition;
        }
        return null; // Return null if no empty CardPosition is found
    }
}