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
    Red,
    Yellow
}

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _enemySpeed = 4f;

    [Header("Arrow Settings")]
    [SerializeField] private SpriteRenderer _arrowRenderer;
    [SerializeField] private float _yellowShuffleInterval = 0.15f;
    [SerializeField] private float _lockDistance = 1.8f;

    [Header("Arrow Sprites (0: Up, 1: Down, 2: Left, 3: Right)")]
    [SerializeField] private List<Sprite> _greenArrowSprites;
    [SerializeField] private List<Sprite> _redArrowSprites;
    [SerializeField] private List<Sprite> _yellowArrowSprites;

    [Header("Powerup Drop")]
    [SerializeField] private GameObject _extraLifePowerupPrefab;

    private SwipeDirection _displayedDirection;
    private SwipeDirection _requiredSwipe;
    private ArrowType _arrowType;
    private bool _canBeHit = false;
    private bool _isYellowLocked = false;
    private float _yellowTimer = 0f;
    private Transform _playerTransform;

    private void Start()
    {
        if (_arrowRenderer != null)
        {
            _arrowRenderer.transform.localPosition = new Vector3(1.2f, 0f, 0f);
            _arrowRenderer.transform.localRotation = Quaternion.identity;
        }

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            _playerTransform = playerObj.transform;
        }

        GenerateArrow();
    }

    private void Update()
    {
        transform.position += Vector3.down * _enemySpeed * Time.deltaTime;

        // yellow arrow rotates until close to player
        if (_arrowType == ArrowType.Yellow && !_isYellowLocked)
        {
            _yellowTimer += Time.deltaTime;
            if (_yellowTimer >= _yellowShuffleInterval)
            {
                _yellowTimer = 0f;
                _displayedDirection = (SwipeDirection)Random.Range(0, 4);
                _requiredSwipe = _displayedDirection; // sync requirement after locking
                SetArrowSprite();
            }

            if (_playerTransform != null)
            {
                float distanceToPlayer = Mathf.Abs(transform.position.y - _playerTransform.position.y);
                if (distanceToPlayer <= _lockDistance)
                {
                    LockYellowArrow();
                }
            }
        }

        if (transform.position.y < -2.82f)
        {
            Debug.Log("enemy passed! player damaged!");
            if (PlayerStats.instance != null)
            {
                PlayerStats.instance.TakeDamage(1);
            }
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
            // Wrong swipe: Log warning only — NO damage to player
            Debug.Log("wrong swipe");
        }
    }

    private void GenerateArrow()
    {
        _displayedDirection = (SwipeDirection)Random.Range(0, 4);
        _arrowType = (ArrowType)Random.Range(0, 3);

        switch (_arrowType)
        {
            case ArrowType.Green:
            case ArrowType.Yellow:
                // green and (yellow initially) = Same direction
                _requiredSwipe = _displayedDirection;
                break;
            case ArrowType.Red:
                // red = opposite
                _requiredSwipe = GetOppositeDirection(_displayedDirection);
                break;
        }

        SetArrowSprite();
    }
    
    // yellow arrow locking
    private void LockYellowArrow()
    {
        _isYellowLocked = true;
        _requiredSwipe = _displayedDirection;
        SetArrowSprite();
    }

    // opposite swiping mechanic
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
                if (_greenArrowSprites != null && _greenArrowSprites.Count > directionIndex)
                    _arrowRenderer.sprite = _greenArrowSprites[directionIndex];
                break;
            case ArrowType.Red:
                if (_redArrowSprites != null && _redArrowSprites.Count > directionIndex)
                    _arrowRenderer.sprite = _redArrowSprites[directionIndex];
                break;
            case ArrowType.Yellow:
                if (_yellowArrowSprites != null && _yellowArrowSprites.Count > directionIndex)
                    _arrowRenderer.sprite = _yellowArrowSprites[directionIndex];
                break;
        }
    }

    private void KilledByPlayer()
    {
        Debug.Log("enemy slained");

        if (Random.value <= 0.03f && _extraLifePowerupPrefab != null)
        {
            Instantiate(_extraLifePowerupPrefab, transform.position, Quaternion.identity);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.IncrementScore(100);
        }

        if (DashSystem.instance != null)
        {
            DashSystem.instance.OnEnemyKilled();
        }

        Destroy(gameObject);
    }
}