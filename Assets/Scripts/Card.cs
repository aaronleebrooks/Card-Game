using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{

    public int currentHealth;
    public int startingHealth;
    public int attack;
    public int cost;
    public int power;
    public int cardID;
    public string cardName;
    public string cardDescription;

    // Texts
    public TMP_Text cardNameValue;
    public TMP_Text cardDescriptionValue;
    public TMP_Text cardCostValue;
    public TMP_Text cardAttackValue;
    public TMP_Text cardHealthValue;
    public TMP_Text cardPowerValue;



    // Start is called before the first frame update
    void Start()
    {
        cardNameValue.text = cardName;
        cardDescriptionValue.text = cardDescription;
        cardCostValue.text = cost.ToString();
        cardAttackValue.text = attack.ToString();
        cardHealthValue.text = startingHealth.ToString();
        cardPowerValue.text = power.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
