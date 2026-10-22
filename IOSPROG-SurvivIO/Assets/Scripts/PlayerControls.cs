using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 7f;

    private Rigidbody2D _rb;
    private PlayerWeapon _playerWeapon;
    private Vector2 _moveInput;
    private Vector2 _aimInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerWeapon = GetComponent<PlayerWeapon>();
    }

    private void Update()
    {
        ReadJoystickInput();
        HandleRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void ReadJoystickInput()
    {
        if (Gamepad.current != null)
        {
            _moveInput = Gamepad.current.leftStick.ReadValue();
            _aimInput = Gamepad.current.rightStick.ReadValue();
        }
    }

    private void HandleMovement()
    {
        _rb.linearVelocity = _moveInput * _moveSpeed;
    }

    private void HandleRotation()
    {
        // aiming joystick rotation
        if (_aimInput.sqrMagnitude > 0.05f)
        {
            float targetAngle = Mathf.Atan2(_aimInput.y, _aimInput.x) * Mathf.Rad2Deg - 90f;
            _rb.rotation = targetAngle;
        }
        // movement direction rotation
        else if (_moveInput.sqrMagnitude > 0.05f)
        {
            float targetAngle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg - 90f;
            _rb.rotation = targetAngle;
        }
    }
}