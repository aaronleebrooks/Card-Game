using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battle")]
public class SO_Battle : ScriptableObject
{
    public SO_Deck storeDeck;
    public SO_Deck enemyStartingDeck;
    public string enemyName;
}