using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    public List<Card> handOfCards = new List<Card>();
    public Transform minPosition;
    public Transform maxPosition;
    [HideInInspector]
    public List<Vector3> cardPositions = new List<Vector3>();

    void Start()
    {
        SetCardPositionsInHand();
    }

    public void SetCardPositionsInHand()
    {
        cardPositions.Clear();
        float distanceBetweenCards = (maxPosition.position.x - minPosition.position.x) / (handOfCards.Count - 1);
        for (int i = 0; i < handOfCards.Count; i++)
        {
            Vector3 cardPosition = new Vector3(minPosition.position.x + (distanceBetweenCards * i), minPosition.position.y, minPosition.position.z);
            cardPositions.Add(cardPosition);

            handOfCards[i].isInHand = true;
            handOfCards[i].handPosition = i;
            handOfCards[i].AssignPositionAndRotation(cardPosition, minPosition.rotation);
        }
    }

    public void AddCardToHand(Card card)
    {   
        card.isInHand = true;
        handOfCards.Add(card);
        SetCardPositionsInHand();
    }

    public void RemoveCardFromHand(Card card)
    {
        card.isInHand = false;
        handOfCards.Remove(card);
        SetCardPositionsInHand();
    }
}
