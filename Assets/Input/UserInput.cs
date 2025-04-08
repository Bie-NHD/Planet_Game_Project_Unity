using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    public static Vector2 MoveInput { get; set; }
    public static bool IsThrowPressed { get; set; }
    public static bool IsDragging { get; set; }

    private InputAction _moveAction;
    private InputAction _throwAction;
    private bool _touchStarted = false;
    private float _dragThreshold = 2f; 
    private Vector2 _initialTouchPosition;

    void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        _moveAction = PlayerInput.actions["Move"];
        _throwAction = PlayerInput.actions["Throw"];
    }

    void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();

        // Kiểm tra khi bắt đầu chạm
        if (_throwAction.WasPressedThisFrame())
        {
            _touchStarted = true;
            _initialTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            IsDragging = false;
        }

        // Khi đang chạm và di chuyển
        if (_touchStarted && Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 currentPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            // Kiểm tra xem đã di chuyển đủ xa để coi là kéo chưa
            if (Vector2.Distance(_initialTouchPosition, currentPosition) > _dragThreshold)
            {
                IsDragging = true;
            }
        }

        // Kiểm tra khi thả tay
        if (_touchStarted && !Touchscreen.current.primaryTouch.press.isPressed)
        {
            _touchStarted = false;
            // Chỉ kích hoạt throw khi đã kéo trước đó
            IsThrowPressed = IsDragging;
            IsDragging = false;
        }
        else
        {
            IsThrowPressed = false;
        }
    }
}