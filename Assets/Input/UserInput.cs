using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    public static Vector2 MoveInput { get; set; }
    public static bool IsThrowPressed { get; set; }
    public static bool IsDragging { get; set; }
    public static Vector2 TapPosition { get; set; }

    private InputAction _moveAction;
    private InputAction _throwAction;
    private bool _touchStarted = false;
    private float _dragThreshold = 2f; 
    private Vector2 _initialTouchPosition;

    private float _touchStartTime;
    private float _quickTapThreshold = 0.2f;
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
            _touchStartTime = Time.time;
            _initialTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            TapPosition = _initialTouchPosition;
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
                TapPosition = currentPosition;
            }
        }

        // Kiểm tra khi thả tay
        if (_touchStarted && !Touchscreen.current.primaryTouch.press.isPressed)
        {
            float touchDuration = Time.time - _touchStartTime;
            _touchStarted = false;

            // Dù là kéo hay nhấn thả tại chỗ, đều kích hoạt throw
            if (IsDragging || touchDuration < _quickTapThreshold)
            {
                IsThrowPressed = true;
                // Giữ lại vị trí thả trong TapPosition
            }
            else
            {
                IsThrowPressed = false;
            }

            IsDragging = false;
        }
        else
        {
            IsThrowPressed = false;
        }
    }
}