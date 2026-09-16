using System.IO.Pipes;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 _swipeStartPos;
    private bool _isSwiping = false;

    [Header("Sensitivity Settings")]
    [SerializeField] private float _minSwipeDistance = 30f;

    public SwipeDirection? DetectSwipe()
    {
        // touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _swipeStartPos = touch.position;
                _isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Moved && _isSwiping)
            {
                Vector2 currentDelta = touch.position - _swipeStartPos;
                if (currentDelta.magnitude >= _minSwipeDistance)
                {
                    _isSwiping = false;
                    return CalculateDirection(currentDelta);
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _isSwiping = false;
            }
        }

        // mouse input
        if (Input.GetMouseButtonDown(0))
        {
            _swipeStartPos = Input.mousePosition;
            _isSwiping = true;
        }
        else if (Input.GetMouseButton(0) && _isSwiping)
        {
            Vector2 currentDelta = (Vector2)Input.mousePosition - _swipeStartPos;
            if (currentDelta.magnitude >= _minSwipeDistance)
            {
                _isSwiping = false;
                return CalculateDirection(currentDelta);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isSwiping = false;
        }

        return null;
    }

    private SwipeDirection CalculateDirection(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            return delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
        }
    }
}