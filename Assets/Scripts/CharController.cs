using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [HideInInspector] public Character characterData;
    [HideInInspector] protected float timeToMove;
    [HideInInspector] public enum MoveDirection { right, left, up, down }

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float tilePull = 0.1f;
    [SerializeField] private float moveCooldown;
    [SerializeField] private AudioSource moveSoundEffect;

    [Header("Animation")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] protected Animator animator;

    virtual protected void Start()
    {
        characterData.charController = this;
    }

    private void Update()
    {
        if (timeToMove > 0f)
        {
            timeToMove -= Time.deltaTime;
        }

        if (transform.position != characterData.tile.position)
        {
            HandlePosition();
        }
        else if (timeToMove <= 0f)
        {
            HandleInteraction();
            HandleMoving();
        }
    }

    virtual protected void HandleMoving()
    {
        return;
    }

    protected void MoveTo(int moveDirection)
    {
        Tile tile = characterData.tile.neighbours[moveDirection];

        if (tile == null || tile.character != null)
        {
            return;
        }

        characterData.SetTile(tile);
        animator.SetInteger("state", moveDirection);
        sprite.sortingOrder = tile.layer;

        timeToMove = moveCooldown;

        moveSoundEffect.Play();
    }

    private void HandlePosition()
    {
        if (Mathf.Abs(transform.position.x - characterData.tile.position.x) < tilePull && Mathf.Abs(transform.position.y - characterData.tile.position.y) < tilePull)
        {
            transform.position = characterData.tile.position;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, characterData.tile.position, movementSpeed * Time.deltaTime);
        }
    }

    virtual protected void HandleInteraction()
    {
        return;
    }

    virtual public void GetBombed()
    {
        return;
    }
}
