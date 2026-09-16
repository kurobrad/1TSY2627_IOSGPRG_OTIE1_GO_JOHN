using System.IO.Pipes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SwipeDetector _swipeDetector;
    private Enemy _currentTargetEnemy;

    private void Update()
    {
        SwipeDirection? swipe = _swipeDetector.DetectSwipe();

        if (swipe.HasValue && _currentTargetEnemy != null)
        {
            _currentTargetEnemy.CheckSwipe(swipe.Value);
        }
    }

    public void SetCurrentTarget(Enemy enemy)
    {
        _currentTargetEnemy = enemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            SetCurrentTarget(enemy);
            enemy.SetCanBeHit(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (_currentTargetEnemy == enemy)
            {
                SetCurrentTarget(null);
            }
            enemy.SetCanBeHit(false);
        }
    }
}