using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPosition : CardPosition
{
    public Player player;

    private void OnMouseDown()
    {
        if (player.selectedCard != null)
        {
            player.TriggerOnCardDiscardedForMana(player.selectedCard);
        }
    }
}
