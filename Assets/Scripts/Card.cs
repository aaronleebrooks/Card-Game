using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    // Card properties
    public SO_Card cardData;
    public int currentHealth;
    public int startingHealth;
    public int attack;
    public int cost;
    public int power;
    public int cardID;
    public string cardName;
    public string cardDescription;
    public Sprite imageBackground;
    public Sprite image;

    public SpriteRenderer cardRenderer;
    public Sprite cardBack;
    public Sprite cardFront;
    public bool isCardBackShown = false;

    // Texts
    public TMP_Text cardNameValue;
    public TMP_Text cardDescriptionValue;
    public TMP_Text cardCostValue;
    public TMP_Text cardAttackValue;
    public TMP_Text cardHealthValue;
    public TMP_Text cardPowerValue;

    void Start()
    {
       SetupCard();    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlipCard();
        }
    }

    public void SetupCard()
    {
        cardNameValue.text = cardData.cardName;
        cardDescriptionValue.text = cardData.cardDescription;
        cardCostValue.text = cardData.cost.ToString();
        cardAttackValue.text = cardData.attack.ToString();
        cardHealthValue.text = cardData.startingHealth.ToString();
        cardPowerValue.text = cardData.power.ToString();

        currentHealth = startingHealth;

        FlipCard();
    }

    public void FlipCard()
    {
        if (isCardBackShown)
        {
            cardRenderer.sprite = cardFront;
            cardRenderer.sortingOrder = 0;
            isCardBackShown = false;
        }
        else
        {
            cardRenderer.sprite = cardBack;
            cardRenderer.sortingOrder = 6;
            isCardBackShown = true;
        }
    }
}
