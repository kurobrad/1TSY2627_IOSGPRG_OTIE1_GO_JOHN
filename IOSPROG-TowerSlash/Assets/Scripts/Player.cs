using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SwipeDetector _swipeDetector;

    // tracks the enemies in the slash range
    private List<Enemy> _enemiesInRange = new List<Enemy>();

    private void Start()
    {
        if (_swipeDetector == null)
        {
            _swipeDetector = FindFirstObjectByType<SwipeDetector>();
        }
    }

    private void Update()
    {
        // clean up any destroyed references
        _enemiesInRange.RemoveAll(enemy => enemy == null);

        if (_swipeDetector == null) return;

        SwipeDirection? swipe = _swipeDetector.DetectSwipe();

        if (swipe.HasValue && _enemiesInRange.Count > 0)
        {
            Enemy targetEnemy = GetClosestEnemy();
            if (targetEnemy != null)
            {
                targetEnemy.CheckSwipe(swipe.Value);
            }
        }
    }

    // finds the closest enemy to the player and prioritizes it
    private Enemy GetClosestEnemy()
    {
        Enemy closest = null;
        float lowestY = float.MaxValue;

        foreach (Enemy enemy in _enemiesInRange)
        {
            if (enemy == null) continue;

            if (enemy.transform.position.y < lowestY)
            {
                lowestY = enemy.transform.position.y;
                closest = enemy;
            }
        }

        return closest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !_enemiesInRange.Contains(enemy))
        {
            _enemiesInRange.Add(enemy);
            enemy.SetCanBeHit(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            _enemiesInRange.Remove(enemy);
            enemy.SetCanBeHit(false);
        }
    }
}