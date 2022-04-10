using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Enemy : CharController
{
    [Header("Bomb")]
    [SerializeField] private float bombedCooldown = 5f;

    override protected void HandleMoving()
    {
        int randomMove = Random.Range(0, 4);

        MoveTo(randomMove);
    }

    override protected void HandleInteraction()
    {
        Tile[] neighbours = characterData.tile.neighbours;

        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i]?.character != null && neighbours[i].character.isPlayer)
            {
                HitPlayer(neighbours[i].character, i);
                break;
            }
        }
    }

    private void HitPlayer(Character player, int hitDirection)
    {
        animator.SetInteger("state", hitDirection);
        animator.SetTrigger("hit");

        player.charController.Die();
    }

    override public void GetBombed()
    {
        animator.SetTrigger("bombed");
        timeToMove = bombedCooldown;
    }
}
