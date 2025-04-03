using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private BoxCollider2D _boundaries;
    [SerializeField] private Transform _planetThrowTransform;

    private Bounds _bounds;

    private float _leftBounds;
    private float _rightBounds;

    private float _startingLeftBounds;
    private float _startingRightBounds;

    private float _offset;

    private void Awake()
    {
        _bounds = _boundaries.bounds;
        _offset =  transform.position.x - _planetThrowTransform.position.x;

        _leftBounds = _bounds.min.x + _offset;
        _rightBounds = _bounds.max.x + _offset;

        _startingLeftBounds = _leftBounds;
        _startingRightBounds = _rightBounds;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position + new Vector3(UserInput.MoveInput.x * _moveSpeed * Time.deltaTime, 0f, 0f );
        newPosition.x = Mathf.Clamp(newPosition.x, _leftBounds, _rightBounds);
        transform.position = newPosition;
    }

    public void ChangeBoundary(float extraWidth)
    {
        _leftBounds = _startingLeftBounds;
        _rightBounds = _startingRightBounds;

        float planetSize = ThrowPlanetController.instance.Bounds.size.x / 2; // Chia đôi kích thước để tránh giới hạn quá mức
        _leftBounds += planetSize + extraWidth;
        _rightBounds -= planetSize + extraWidth;
    }
}
