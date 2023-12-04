using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    // Card properties
    public CardScriptableObject SO_Card;
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
        cardNameValue.text = SO_Card.cardName;
        cardDescriptionValue.text = SO_Card.cardDescription;
        cardCostValue.text = SO_Card.cost.ToString();
        cardAttackValue.text = SO_Card.attack.ToString();
        cardHealthValue.text = SO_Card.startingHealth.ToString();
        cardPowerValue.text = SO_Card.power.ToString();

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
