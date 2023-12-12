using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class SO_Deck : ScriptableObject
{
    [SerializeField]
    private List<SO_Card> cards;

    public List<SO_Card> Cards
    {
        get { return cards; }
        set
        {
            if (value.Count <= 10)
            {
                cards = value;
            }
            else
            {
                Debug.LogError("Deck cannot contain more than 10 cards.");
            }
        }
    }
}