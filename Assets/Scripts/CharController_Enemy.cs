using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Enemy : CharController
{
    override protected void HandleMoving()
    {
        int randomMove = Random.Range(0, 4);

        MoveTo(randomMove);
    }

    override protected void HandleInteraction()
    {
        Tile[] neighbours = characterData.tile.neighbours;

        for (int i = 0; i < characterData.tile.neighbours.Length; i++)
        {
            if (neighbours[i]?.character != null && neighbours[i].character.isPlayer)
            {
                HitPlayer(neighbours[i].character, i);
            }
        }
    }

    private void HitPlayer(Character player, int hitDirection)
    {
        animator.SetInteger("state", hitDirection);
        animator.SetTrigger("hit");
        this.enabled = false;

        player.charController.Die();
    }
}
