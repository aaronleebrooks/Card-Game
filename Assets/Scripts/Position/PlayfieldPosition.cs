using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldPosition : CardPosition
{
    public bool HasCard = false;
    public Player player;

    private void OnMouseDown()
    {
        if (player.selectedCard != null && !HasCard && player.manaController.mana >= player.selectedCard.cost)
        {
            player.TriggerOnCardPlayed(player.selectedCard, this);
        }
    }

}
