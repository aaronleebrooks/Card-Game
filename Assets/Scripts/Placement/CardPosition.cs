using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPosition : MonoBehaviour
{
    public Card activeCard;
    public TablePosition tablePosition;

    private void OnMouseDown()
    {
        if (tablePosition == TablePosition.Player && Card.selectedCard != null)
        {
            activeCard = Card.selectedCard;

            // Move the selected card to this CardPosition
            activeCard.AssignPositionAndRotation(transform.position, new Quaternion(0f, 0f, 0f, 0f));

            // Update the activeCard
            

            // Deselect the card
            activeCard.isSelected = false;
            activeCard.isInHand = false;
            activeCard.cardCollider.enabled = true;
            activeCard = null;
        }
    }
}
