using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    public List<Card> handOfCards;

    public List<CardPosition> handPositions;
    public Transform minPosition;
    public Transform maxPosition;
    public Player player;

    private void Awake()
    {
        player.OnCardsDrawnToHand += AddCardsToHand;
    }

    private void OnDestroy()
    {
        player.OnCardsDrawnToHand -= AddCardsToHand;
    }

    public void SetCardPositionsInHand()
    {
        float distanceBetweenCards = (maxPosition.position.x - minPosition.position.x) / (handOfCards.Count - 1);
        for (int i = 0; i < handPositions.Count; i++)
        {
            Vector3 cardPosition = new Vector3(minPosition.position.x + (distanceBetweenCards * i), minPosition.position.y, minPosition.position.z);
            handPositions[i].transform.position = cardPosition;
            handPositions[i].gameObject.SetActive(i < handOfCards.Count);
        }
    }

    public void AddCardToHand(Card card)
    {   
        card.cardLocation = CardLocation.Hand;
        Debug.Log("Card added to hand" + card.id.ToString());
        handOfCards.Add(card);
        SetCardPositionsInHand();
    }

    private void AddCardsToHand(List<Card> cards)
    {
        foreach (var card in cards)
        {
            AddCardToHand(card);
        }
    }
}
