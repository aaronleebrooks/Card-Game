using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManaController : MonoBehaviour
{
    public Player player;
    public TMP_Text manaText;

    private int _mana;
    public int mana
    {
        get { return _mana; }
        set
        {
            _mana = value;
            player.TriggerOnManaValueChanged(_mana);
        }
    }
    
    private void Start()
    {
        player.OnCardDiscardedForMana += AddMana;
        BattleController.OnCardBought += SubtractMana;
        player.OnCardPlayed += SubtractMana;
        // Subscribe to other events as needed...
    }

    private void OnDestroy()
    {
        player.OnCardDiscardedForMana -= AddMana;
        BattleController.OnCardBought -= SubtractMana;
        player.OnCardPlayed -= SubtractMana;
        // Unsubscribe from other events as needed...
    }
    private void Update()
    {
        manaText.text = mana.ToString();
    }

    private void AddMana(Card card)
    {
        mana += card.power;
    }

    private void SubtractMana(Card card, PlayfieldPosition playfieldPosition)
    {
        mana -= card.cost;
    }

    private void SubtractMana(Card card, Player purchasePlayer)
    {
        if(purchasePlayer != player) {
            return;
        }
        mana -= card.cost;
    }
}