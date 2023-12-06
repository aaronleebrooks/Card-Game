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
    public bool isCardBackShown = false;

    private Vector3 targetPoint;
    private Quaternion targetRotation;
    public float movementSpeed = 10f;
    public float rotationSpeed = 540f;

    public bool isInHand;
    public int handPosition;
    private HandController handController;

    [HideInInspector]
    public bool isSelected;
    public Collider cardCollider;
    public LayerMask tableLayer;

    public LayerMask placementLayer;
    public static Card selectedCard;


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
       handController = FindObjectOfType<HandController>();    
       cardCollider = GetComponent<Collider>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, movementSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.F))
        {
            FlipCard();
        }

        if (isSelected)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);

            if(Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }
        }
    }

    public void SetupCard()
    {
        titleValue.text = cardData.title;
        descriptionValue.text = cardData.description;
        cardCostValue.text = cardData.cost.ToString();
        cardAttackValue.text = cardData.attack.ToString();
        cardHealthValue.text = cardData.startingHealth.ToString();
        cardPowerValue.text = cardData.power.ToString();
        imageRenderer.sprite = cardData.image;
        imageBackgroundRenderer.sprite = cardData.imageBackground;

        currentHealth = startingHealth;

        FlipCard();
    }

    public void FlipCard()
    {
        if (isCardBackShown)
        {
            cardRenderer.sprite = cardBack;
            cardRenderer.sortingOrder = 6;
            isCardBackShown = true;
        }
        else
        {
            cardRenderer.sprite = cardFront;
            cardRenderer.sortingOrder = 0;
            isCardBackShown = false;
        }
    }

    public void AssignPositionAndRotation(Vector3 point, Quaternion rotation)
    {
        targetPoint = point;
        targetRotation = rotation;
    }

    private void OnMouseOver()
    {
        if (isInHand)
        {
            AssignPositionAndRotation(handController.cardPositions[handPosition] + new Vector3(0f, 1f, .5f), Quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (isInHand)
        {
            AssignPositionAndRotation(handController.cardPositions[handPosition], handController.minPosition.rotation);
        }
    }

    private void OnMouseDown()
    {
        if (isInHand)
        {
            if (isSelected)
            {
                // Deselect the card
                isSelected = false;
                cardCollider.enabled = true;
                selectedCard = null;
            }
            else
            {
                // If there's already a selected card, deselect it
                if (selectedCard != null)
                {
                    selectedCard.isSelected = false;
                    selectedCard.cardCollider.enabled = true;
                }

                // Select the new card
                isSelected = true;
                cardCollider.enabled = false;
                selectedCard = this;
            }
        }
    }

    public void ReturnToHand()
    {
        isSelected = false;
        cardCollider.enabled = true;
        AssignPositionAndRotation(handController.cardPositions[handPosition], handController.minPosition.rotation);
    }

}
