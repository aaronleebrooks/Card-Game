using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovementController : MonoBehaviour
{
    public CardPosition drawPilePosition;
    public List<HandPosition> handPositions;
    public CardPosition discardPilePosition;
    public List<PlayfieldPosition> playerFieldPositions;
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
        player.OffSelectHandCard += MoveCardOffSelectedPosition;
        player.OnReplaceHandCard += MoveCardBackToHand;
        player.OnCardPlayed += MoveCardToPlayfield;
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
        player.OffSelectHandCard -= MoveCardOffSelectedPosition;
        player.OnReplaceHandCard -= MoveCardBackToHand;
        player.OnCardPlayed -= MoveCardToPlayfield;
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
            HandPosition firstEmpty = GetFirstEmptyHandPosition();
            card.cardPosition = firstEmpty;
            firstEmpty.HasCard = true;
        }
    }

    private void MoveCardToDiscardPile(Card card)
    {
        Debug.Log("Moving card to discard pile" + card.name);
        player.TriggerOffSelectHandCard(card);
        card.cardCollider.enabled = false;
        card.cardPosition = discardPilePosition;
    }

    private void MoveCardsToDiscardPile(List<Card> cards)
    {
        foreach (var card in cards)
        {
            card.cardPosition = discardPilePosition;
        }
    }

    private void MoveCardsToDrawPile(List<Card> cards)
    {
        foreach (var card in cards)
        {
            card.cardPosition = drawPilePosition;
            card.SetIsCardBackShown(true);
        }
    }

    private void MoveCardUpOnHover(Card card)
    {   
        MoveCardToPosition(card, GetHightlightLocation(card.transform.position));
    }

    private void MoveCardDownOffHover(Card card)
    {
        MoveCardToPosition(card, GetNotHightlightLocation(card.transform.position));
    }

    private void MoveCardToSelectedPosition(Card card)
    {
        card.cardPosition.transform.position = GetHightlightLocation(card.transform.position);
    }

    private void MoveCardOffSelectedPosition(Card card)
    {
        card.cardPosition.transform.position = GetNotHightlightLocation(card.transform.position);
    }

    private void MoveCardBackToHand(Card card)
    { 
        HandPosition handPosition = GetLastActiveHandPosition();
        card.cardPosition = handPosition;
    }

    private void MoveCardToPlayfield(Card card, PlayfieldPosition playfieldPosition)
    {
        card.cardPosition = playfieldPosition;
    }

    public Vector3 GetHightlightLocation(Vector3 oldPosition) {
        Vector3 highlightPosition = new Vector3(oldPosition.x, oldPosition.y + 0.5f, oldPosition.z);
        return highlightPosition;
    }

    public Vector3 GetNotHightlightLocation(Vector3 oldPosition) {
        Vector3 highlightPosition = new Vector3(oldPosition.x, oldPosition.y - 0.5f, oldPosition.z);
        return highlightPosition;
    }

    public HandPosition GetLastActiveHandPosition()
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

    public HandPosition GetFirstEmptyHandPosition()
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