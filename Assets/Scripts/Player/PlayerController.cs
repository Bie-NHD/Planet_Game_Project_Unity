using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10.0f;
    [SerializeField] private BoxCollider2D _boundaries;
    [SerializeField] private Transform _planetThrowTransform;
    [SerializeField] private bool _useDirectPosition = true;
    public float LeftBound => _leftBounds;
    public float RightBound => _rightBounds;

    private Bounds _bounds;
    private float _leftBounds;
    private float _rightBounds;
    private float _startingLeftBounds;
    private float _startingRightBounds;
    private float _offset;
    private Camera _mainCamera;

    //// Thêm biến để theo dõi vị trí đích cần di chuyển đến khi tap
    private Vector3 _targetPosition;

    private void Awake()
    {
        _bounds = _boundaries.bounds;
        //_offset = transform.position.x - _planetThrowTransform.position.x;
        _leftBounds = _bounds.min.x ;
        _rightBounds = _bounds.max.x ;
        _startingLeftBounds = _leftBounds;
        _startingRightBounds = _rightBounds;
        _mainCamera = Camera.main;

        _targetPosition = transform.position;
    }
                                                
    void Update()
    {
        // Xử lý khi người dùng nhấn và thả ngay lập tức (không phải kéo)
        if (UserInput.IsThrowPressed && !UserInput.IsDragging)
        {
            // Chuyển đổi vị trí tap từ screen sang world
            Vector3 worldTapPosition = _mainCamera.ScreenToWorldPoint(new Vector3(UserInput.TapPosition.x, UserInput.TapPosition.y, 10f));
            // Chỉ lấy giá trị X và giữ nguyên Y, Z
            _targetPosition = new Vector3(worldTapPosition.x, transform.position.y, transform.position.z);
            _targetPosition.x = Mathf.Clamp(_targetPosition.x, _leftBounds, _rightBounds);

            // Di chuyển ngay lập tức đến vị trí tap
            transform.position = _targetPosition;
            if (ThrowPlanetController.instance.CanThrow)
            {
                ThrowPlanetController.instance.ThrowPlanetAtPosition(_targetPosition);
            }
        }
        // Xử lý khi người dùng kéo (trường hợp IsDragging = true)
        else if (_useDirectPosition && Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed && UserInput.IsDragging)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));

            Vector3 newPosition = new Vector3(worldPosition.x, transform.position.y, transform.position.z);
            newPosition.x = Mathf.Clamp(newPosition.x, _leftBounds, _rightBounds);
            transform.position = newPosition;
        }
        // Trường hợp di chuyển bằng delta
        else
        {
            Vector3 newPosition = transform.position + new Vector3(UserInput.MoveInput.x * _moveSpeed * Time.deltaTime, 0f, 0f);
            newPosition.x = Mathf.Clamp(newPosition.x, _leftBounds, _rightBounds);
            transform.position = newPosition;
        }
    }

    public void ChangeBoundary(float extraWidth)
    {
        _leftBounds = _startingLeftBounds;
        _rightBounds = _startingRightBounds;
        float planetSize = ThrowPlanetController.instance.Bounds.size.x / 2;
        _leftBounds += planetSize + extraWidth;
        _rightBounds -= planetSize + extraWidth;
    }
   
}