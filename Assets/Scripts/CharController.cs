using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [HideInInspector] public Character characterData;
    [HideInInspector] protected float timeToMove;
    [HideInInspector] private Vector2 touchDownPosition;
    [HideInInspector] private Vector2 touchUpPosition;
    [HideInInspector] private UIController UI;
    [HideInInspector] public enum MoveDirection { right, left, up, down }

    [Header("Movement")]
    [SerializeField] private float swipeThreshold = 1f;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float tilePull = 0.1f;
    [SerializeField] private float moveCooldown;
    [SerializeField] private AudioSource moveSoundEffect;

    [Header("Items")]
    [SerializeField] private AudioSource collectItemSoundEffect;

    [Header("Bomb")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] protected float bombedCooldown = 5f;
    [SerializeField] private AudioSource setBombSoundEffect;

    [Header("Animation")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] protected Animator animator;

    private void Start()
    {
        characterData.charController = this;
        UI = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
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
        if (Input.touches.Length == 0)
        {
            return;
        }

        Touch touch = Input.touches[0];

        if (touch.phase == TouchPhase.Began)
        {
            touchDownPosition = touch.position;
            print(touchDownPosition);
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            touchUpPosition = touch.position;
            print(touchUpPosition);
            HandleTouch();
        }
    }

    private void HandleTouch()
    {
        float horizontalSwipeDistance = Mathf.Abs(touchDownPosition.x - touchUpPosition.x);
        float verticalSwipeDistance = Mathf.Abs(touchDownPosition.y - touchUpPosition.y);
        bool isVerticalSwipe = verticalSwipeDistance > horizontalSwipeDistance;
        bool isTap = horizontalSwipeDistance < swipeThreshold && verticalSwipeDistance < swipeThreshold;

        if (isTap)
        {
            SetBomb();
        }
        else if (isVerticalSwipe)
        {
            if (touchUpPosition.y > touchDownPosition.y)
            {
                MoveTo((int)MoveDirection.up);
            }
            else
            {
                MoveTo((int)MoveDirection.down);
            }
        }
        else
        {
            if (touchUpPosition.x > touchDownPosition.x)
            {
                MoveTo((int)MoveDirection.right);
            }
            else
            {
                MoveTo((int)MoveDirection.left);
            }
        }
    }

    private void SetBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, characterData.tile.position, Quaternion.identity);

        bomb.GetComponent<BombController>().tile = characterData.tile;

        setBombSoundEffect.Play();
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
        if (characterData.tile.item != null)
        {
            CollectItem(characterData.tile.item);
        }
    }

    private void CollectItem(GameObject item)
    {
        if (item.transform.parent.childCount == 1)
        {
            UI.ShowGameOverScreen(true);
        }

        Destroy(item);

        collectItemSoundEffect.Play();
    }

    virtual public void GetBombed()
    {
        Die();
    }

    public void Die()
    {
        UI.ShowGameOverScreen(false);
    }
}
