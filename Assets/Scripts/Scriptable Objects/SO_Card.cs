using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class SO_Card : ScriptableObject
{
    public int id;
    public int startingHealth;
    public int attack;
    public int cost;
    public int power;
    public string title;
    
    [TextArea(3, 10)]
    public string description;

    public Sprite imageBackground;
    public Sprite image;

}
