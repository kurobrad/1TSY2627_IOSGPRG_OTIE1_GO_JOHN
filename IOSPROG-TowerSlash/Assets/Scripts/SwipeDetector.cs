using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 _fingerDownPos;
    private Vector2 _fingerUpPos;
    [SerializeField] private float _minDistanceForSwipe = 20f;

    public SwipeDirection? DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _fingerDownPos = touch.position;
                _fingerUpPos = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                _fingerUpPos = touch.position;
                return CalculateSwipe();
            }
        }

        // mouse click/drag inputs
        if (Input.GetMouseButtonDown(0)) _fingerDownPos = Input.mousePosition;
        if (Input.GetMouseButtonUp(0))
        {
            _fingerUpPos = Input.mousePosition;
            return CalculateSwipe();
        }

        return null;
    }

    private SwipeDirection? CalculateSwipe()
    {
        Vector2 delta = _fingerUpPos - _fingerDownPos;
        if (delta.magnitude < _minDistanceForSwipe) return null;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            return delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        else
            return delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
    }
}