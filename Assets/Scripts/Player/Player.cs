using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public DeckController deckController;
    public PlayfieldController playfield;
    public int health;
    public string playerName;
    public ManaController manaController;
    public Card selectedCard;

    //Events
    public event Action OnDeckInitialized;
    public event Action<Card> OnCardSelected;
    public event Action<Card> OnCardDiscardedForMana;
    public event Action<int> OnCardsDrawn;
    public event Action OnDrawPileEmpty;
    public event Action OnReshuffleDrawPile;
    public event Action<Card, PlayfieldPosition> OnCardPlayed;
    public event Action<List<Card>> OnCardsSentToDiscardPile;
    public event Action<List<Card>> OnCardsSentToDrawPile;
    public event Action<List<Card>> OnCardsDrawnToHand;
    public event Action OnTurnPhaseChanged;
    public event Action<Card, List<Card>> OnCardTargeting; // First Card is the source, second Card is the target
    public event Action<Card> OnHandCardHover;
    public event Action<Card> OffHandCardHover;
    public event Action<Card> OnSelectHandCard;
    public event Action<Card> OffSelectHandCard;
    public event Action<Card> OnReplaceHandCard;
    public event Action<int> OnManaValueChanged;


    private void Awake()
    {
        OnDeckInitialized?.Invoke();
    }

    public bool HasEnoughMana(Card card)
    {
        return manaController.mana >= card.cost;
    }

        // Event Triggers
    public void TriggerOnDeckInitialized()
    {
        OnDeckInitialized?.Invoke();
    }

    public void TriggerOnCardSelected(Card card)
    {
        OnCardSelected?.Invoke(card);
    }

    public void TriggerOnCardDiscardedForMana(Card card)
    {
        Debug.Log("Discarded for mana " + card.name);
        OnCardDiscardedForMana?.Invoke(card);
    }

    public void TriggerOnCardsDrawn(int count)
    {
        OnCardsDrawn?.Invoke(count);
    }

    public void TriggerOnManaValueChanged(int mana)
    {
        OnManaValueChanged?.Invoke(mana);
    }

    public void TriggerOnDrawPileEmpty()
    {
        OnDrawPileEmpty?.Invoke();
    }

    public void TriggerOnReshuffleDrawPile()
    {
        OnReshuffleDrawPile?.Invoke();
    }

    public void TriggerOnCardPlayed(Card card, PlayfieldPosition position)
    {
        OnCardPlayed?.Invoke(card, position);
    }

    public void TriggerOnCardsDrawnToHand(List<Card> cards)
    {
        OnCardsDrawnToHand?.Invoke(cards);
        foreach (var card in cards)
        {
            if(card.isOwnedByPlayer)
            {
                card.SetIsCardBackShown(false);
            }
        }
    }

    public void TriggerOnTurnPhaseChanged()
    {
        OnTurnPhaseChanged?.Invoke();
    }

    public void TriggerOnCardTargeting(Card source, List<Card> targets)
    {
        OnCardTargeting?.Invoke(source, targets);
    }

    public void TriggerOnCardsSentToDiscardPile(List<Card> cards)
    {
        OnCardsSentToDiscardPile?.Invoke(cards);
    }

    public void TriggerOnCardsSentToDrawPile(List<Card> cards)
    {
        OnCardsSentToDrawPile?.Invoke(cards);
    }

    public void TriggerOnHandCardHover(Card card)
    {
        OnHandCardHover?.Invoke(card);
    }

    public void TriggerOffHandCardHover(Card card)
    {
        OffHandCardHover?.Invoke(card);
    }

    public void TriggerOnSelectHandCard(Card card)
    {
        OnSelectHandCard?.Invoke(card);
        card.SelectHighlight(true);
        card.isSelected = true;
        selectedCard = card;
    }

    public void TriggerOffSelectHandCard(Card card)
    {
        OffSelectHandCard?.Invoke(card);
        card.SelectHighlight(false);
        card.isSelected = false;
        selectedCard = null;
    }

    public void TriggerOnReplaceHandCard(Card card)
    {
        OnReplaceHandCard?.Invoke(card);
    }
}
