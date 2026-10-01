using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    [Header("Sensitivity")]
    [Tooltip("Minimum drag distance in screen percentage (0.04 = 4% of screen height)")]
    [SerializeField] private float _minSwipeDistancePercent = 0.04f;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private SwipeDirection? _bufferedSwipe = null;

    public SwipeDirection? DetectSwipe()
    {
        // poll input events
        ProcessTouchInput();
        ProcessMouseInput();

        SwipeDirection? swipeToReturn = _bufferedSwipe;
        _bufferedSwipe = null; // clear after consumptions so it registers once
        return swipeToReturn;
    }

    private void ProcessTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            _startPosition = touch.position;
            _endPosition = touch.position;
        }
        ;

        if (touch.phase == TouchPhase.Moved)
        {
            _endPosition = touch.position;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            _endPosition = touch.position;
            EvaluateSwipe();
        }
    }

    private void ProcessMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPosition = Input.mousePosition;
            _endPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endPosition = Input.mousePosition;
            EvaluateSwipe();
        }
    }

    private void EvaluateSwipe()
    {
        float minDistancePixels = Screen.height * _minSwipeDistancePercent;
        Vector2 delta = _endPosition - _startPosition;

        if (delta.magnitude < minDistancePixels) return;

        // determine dominant direction
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            _bufferedSwipe = delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            _bufferedSwipe = delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
        }
    }
}