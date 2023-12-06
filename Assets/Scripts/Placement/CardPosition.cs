using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPosition : MonoBehaviour
{
    public Card activeCard;
    public TablePosition tablePosition;

    private HandController handController;

    void Start()
    {
        handController = FindObjectOfType<HandController>();   
    }

    private void OnMouseDown()
    {
        if (tablePosition == TablePosition.Player && Card.selectedCard != null)
        {
            activeCard = Card.selectedCard;

            // Move the selected card to this CardPosition
            activeCard.AssignPositionAndRotation(transform.position, Quaternion.identity);
            
            handController.RemoveCardFromHand(activeCard);

            // Deselect the card
            activeCard.isSelected = false;
            activeCard.cardCollider.enabled = true;
            activeCard = null;
        }
    }
}
