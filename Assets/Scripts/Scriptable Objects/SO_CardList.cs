using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card List", menuName = "Card List", order = 2)]
public class SO_CardList : ScriptableObject
{
    public List<SO_Card> Cards = new List<SO_Card>();

    public void AddCard(SO_Card newCard)
    {
        Cards.Add(newCard);
    }
}