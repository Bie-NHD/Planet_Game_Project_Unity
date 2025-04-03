using UnityEngine;
using UnityEngine.InputSystem;
public class UserInput : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 MoveInput { get; set; } 

    public static bool IsThrowPressed { get; set; }

    private InputAction _moveAction;
    private InputAction _throwAction;

    void Awake()
    { 
        PlayerInput = GetComponent<PlayerInput>();
        _moveAction = PlayerInput.actions["Move"];
        _throwAction = PlayerInput.actions["Throw"];
    
    }
  
    void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        IsThrowPressed = _throwAction.WasPressedThisFrame();
    }
}
