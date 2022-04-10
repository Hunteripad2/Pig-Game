using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Player : CharController
{
    [HideInInspector] private UIController UI;
    [HideInInspector] private Vector2 touchDownPosition;
    [HideInInspector] private Vector2 touchUpPosition;

    [Header("Controls")]
    [SerializeField] private float swipeThreshold = 2.5f;

    [Header("Items")]
    [SerializeField] private AudioSource collectItemSoundEffect;

    [Header("Bomb")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private AudioSource setBombSoundEffect;

    override protected void Start()
    {
        base.Start();

        UI = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
    }

    override protected void HandleMoving()
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

    override protected void HandleInteraction()
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

    override public void GetBombed()
    {
        UI.ShowGameOverScreen(false);
    }
}
