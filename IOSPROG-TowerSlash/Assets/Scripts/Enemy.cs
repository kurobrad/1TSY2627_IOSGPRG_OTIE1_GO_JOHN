using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

public enum ArrowType
{
    Green,
    Red
}

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _enemySpeed = 4f;

    [Header("Arrow Settings")]
    [SerializeField] private SpriteRenderer _arrowRenderer;

    [Header("Arrow Sprites (0: Up, 1: Down, 2: Left, 3: Right)")]
    [SerializeField] private List<Sprite> _greenArrowSprites;
    [SerializeField] private List<Sprite> _redArrowSprites;

    private SwipeDirection _displayedDirection;
    private SwipeDirection _requiredSwipe;
    private ArrowType _arrowType;
    private bool _canBeHit = false;

    private void Start()
    {
        GenerateArrow();
    }

    private void Update()
    {
        transform.position += Vector3.down * _enemySpeed * Time.deltaTime;

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    public void SetCanBeHit(bool state)
    {
        _canBeHit = state;
    }

    public void CheckSwipe(SwipeDirection swipeDirection)
    {
        if (!_canBeHit) return;

        if (swipeDirection == _requiredSwipe)
        {
            KilledByPlayer();
        }
        else
        {
            Debug.Log("Wrong Swipe!");
        }
    }

    private void GenerateArrow()
    {
        _displayedDirection = (SwipeDirection)Random.Range(0, 4);
        _arrowType = (ArrowType)Random.Range(0, 2);

        switch (_arrowType)
        {
            case ArrowType.Green:
                _requiredSwipe = _displayedDirection;
                break;
            case ArrowType.Red:
                _requiredSwipe = GetOppositeDirection(_displayedDirection);
                break;
        }

        SetArrowSprite();
    }

    private SwipeDirection GetOppositeDirection(SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeDirection.Up: return SwipeDirection.Down;
            case SwipeDirection.Down: return SwipeDirection.Up;
            case SwipeDirection.Left: return SwipeDirection.Right;
            case SwipeDirection.Right: return SwipeDirection.Left;
            default: return SwipeDirection.Left;
        }
    }

    private void SetArrowSprite()
    {
        int directionIndex = (int)_displayedDirection;

        switch (_arrowType)
        {
            case ArrowType.Green:
                if (_greenArrowSprites.Count > directionIndex)
                    _arrowRenderer.sprite = _greenArrowSprites[directionIndex];
                break;
            case ArrowType.Red:
                if (_redArrowSprites.Count > directionIndex)
                    _arrowRenderer.sprite = _redArrowSprites[directionIndex];
                break;
        }
    }

    private void KilledByPlayer()
    {
        Debug.Log("Enemy Slashed!");
        Destroy(gameObject);
    }
}