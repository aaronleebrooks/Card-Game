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
    public int id;
    public string title;
    public string description;

    public SpriteRenderer cardRenderer;
    public SpriteRenderer imageBackgroundRenderer;
    public SpriteRenderer imageRenderer;

    public Sprite cardBack;
    public Sprite cardFront;
    public bool isCardBackShown = true;

    public Vector3 targetPoint = Vector3.zero;
    public Quaternion targetRotation = Quaternion.identity;
    public float movementSpeed = 10f;
    public float rotationSpeed = 540f;
    public CardPosition cardPosition;

    public bool isSelected;

    public bool isHovered;
    public Collider cardCollider;
    public LayerMask tableLayer;

    public static Card selectedCard;

    public Player owner;
    public bool isOwnedByPlayer;

    // Texts
    public TMP_Text titleValue;
    public TMP_Text descriptionValue;
    public TMP_Text cardCostValue;
    public TMP_Text cardAttackValue;
    public TMP_Text cardHealthValue;
    public TMP_Text cardPowerValue;

    void Start()
    {
        SetupCard();
        owner.OnManaValueChanged += AffordableHighlight;
        cardCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (!cardPosition)
        {
            return;
        }

        if (isOwnedByPlayer &&
            cardPosition.cardLocation == CardLocation.Hand &&
            (isSelected || isHovered))
        {
            targetPoint = cardPosition.transform.position + new Vector3(0, 0, 0.5f);
        }
        else
        {
            targetPoint = cardPosition.transform.position;
        }

        transform.position = Vector3.Lerp(transform.position, targetPoint, movementSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.F))
        {
            SetIsCardBackShown(!isCardBackShown);
        }
    }

    private void OnDestroy()
    {
        owner.OnManaValueChanged -= AffordableHighlight;
    }

    public void SetupCard()
    {
        title = cardData.title;
        name = cardData.title;
        titleValue.text = cardData.title;
        id = cardData.id;
        descriptionValue.text = cardData.description;
        cost = cardData.cost;
        cardCostValue.text = cardData.cost.ToString();
        attack = cardData.attack;
        cardAttackValue.text = cardData.attack.ToString();
        startingHealth = cardData.startingHealth;
        cardHealthValue.text = cardData.startingHealth.ToString();
        power = cardData.power;
        cardPowerValue.text = cardData.power.ToString();
        imageRenderer.sprite = cardData.image;
        imageBackgroundRenderer.sprite = cardData.imageBackground;
        currentHealth = startingHealth;
    }

    public void SetIsCardBackShown(bool isCardBackShown)
    {
        this.isCardBackShown = isCardBackShown;
        FlipCard();
    }

    public void FlipCard()
    {
        if (isCardBackShown)
        {
            cardRenderer.sprite = cardBack;
            cardRenderer.sortingOrder = 6;
        }
        else
        {
            cardRenderer.sprite = cardFront;
            cardRenderer.sortingOrder = 0;
        }
    }

    private void OnMouseEnter()
    {
        if(cardPosition != null)
        {
            isHovered = true;
            switch (cardPosition.cardLocation)
            {
                case CardLocation.Hand:
                    if (isOwnedByPlayer)
                    {
                        owner.TriggerOnHandCardHover(this);
                    }
                    break;
                case CardLocation.Playfield:
                    //on hover in playerPlayfield
                    break;
                case CardLocation.DrawPile:
                    //on hover in draw pile
                    break;
                case CardLocation.DiscardPile:
                    //on hover in discard pile
                    break;
                case CardLocation.Store:
                    //on hover in store
                    break;
                default:
                    break;
            }
        }
    }

    private void OnMouseOver()
    {
        if(cardPosition != null)
        {
            switch (cardPosition.cardLocation)
            {
                case CardLocation.Hand:
                    //on hover in playerHand
                    break;
                case CardLocation.Playfield:
                    //on hover in playerPlayfield
                    break;
                case CardLocation.DrawPile:
                    //on hover in draw pile
                    break;
                case CardLocation.DiscardPile:
                    //on hover in discard pile
                    break;
                case CardLocation.Store:
                    //on hover in store
                    break;
                default:
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        if(cardPosition != null)
        {
            isHovered = false;
            switch (cardPosition.cardLocation)
            {
                case CardLocation.Hand:
                    if (isOwnedByPlayer)
                    {
                        owner.TriggerOffHandCardHover(this);
                    }
                    break;
                case CardLocation.Playfield:
                    //off hover in playerPlayfield
                    break;
                case CardLocation.DrawPile:
                    //off hover in draw pile
                    break;
                case CardLocation.DiscardPile:
                    //off hover in discard pile
                    break;
                case CardLocation.Store:
                    //off hover in store
                    break;
                default:
                    break;
            }
        }
    }

    private void OnMouseDown()
    {
        switch (cardPosition.cardLocation)
        {
            case CardLocation.Hand:
                if(isOwnedByPlayer)
                {
                    handleCardSelection(this);
                }
                break;
            case CardLocation.Playfield:
                //off hover in playerPlayfield
                break;
            case CardLocation.DrawPile:
                //off hover in draw pile
                break;
            case CardLocation.DiscardPile:
                //off hover in discard pile
                break;
            case CardLocation.Store:
                //off hover in store
                break;
            default:
                break;
        }
    }

    public void ReturnToHand()
    {
        isSelected = false;
        cardCollider.enabled = true;
        owner.TriggerOnReplaceHandCard(this);
    }

    public void SelectHighlight(bool isHighlighted)
    {
        if (isHighlighted)
        {
            cardRenderer.color = Color.yellow;
        }
        else
        {
            cardRenderer.color = Color.white;
        }
    }

    private void AffordableHighlight(int _)
    {
        if (owner.HasEnoughMana(this) &&
            cardPosition != null &&
            cardPosition.cardLocation == CardLocation.Hand)
        {
            cardRenderer.color = Color.green;
        }
        else
        {
            cardRenderer.color = Color.white;
        }
    }

    private void handleCardSelection(Card card) {
        if(isSelected) {
            owner.TriggerOffSelectHandCard(this);
            return;
        }

        // If there's already a selected card, deselect it
        if (owner.selectedCard != null && owner.selectedCard != this)
        {
            owner.TriggerOffSelectHandCard(owner.selectedCard);
        }

        owner.TriggerOnSelectHandCard(this);
    }

}
