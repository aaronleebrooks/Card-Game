using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPosition : CardPosition
{
    public Player player;

    private void OnMouseDown()
    {
        if (player.selectedCard != null)
        {
            //show player deck?
        }
    }
}
