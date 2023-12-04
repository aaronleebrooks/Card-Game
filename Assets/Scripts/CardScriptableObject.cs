using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class SO_Card : ScriptableObject
{
    public int cardID;
    public int currentHealth;
    public int startingHealth;
    public int attack;
    public int cost;
    public int power;
    public string cardName;
    
    [TextArea(3, 10)]
    public string cardDescription;

    public Sprite imageBackground;
    public Sprite image;

}
