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
}