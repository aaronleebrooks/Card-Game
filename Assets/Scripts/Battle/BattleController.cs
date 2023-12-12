using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleController : MonoBehaviour
{       
    // Singleton instance
    public static BattleController Instance { get; private set; }
    public SO_Battle battleData;
    public int cardsDrawnPerTurn = 5;
    public Player player;
    public Player enemy;
    public Store store;
    public TurnPhase turnPhase;
    public int turnNumber;
    public bool isBattleOver;

    public static event Action<Card, Player> OnCardBought;

    private void Awake()
    {
        // If the instance field is not null and not this, destroy this instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Otherwise, this instance is the singleton instance
            Instance = this;
        }

        // Set up the battle
        store.storeDeck = battleData.storeDeck;
        enemy.deckController.deckData = battleData.enemyStartingDeck;
        enemy.playerName = battleData.enemyName;
    }

    public static void TriggerOnCardBought(Card card, Player player)
    {
        OnCardBought?.Invoke(card, player);
    }

}