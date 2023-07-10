using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///  Receive Input through new Input System and invoke events passing the corresponding values
/// </summary>
public class InputManager : MonoBehaviour
{
    public static event System.Action<Vector2> MoveEvent;
    public static event System.Action<Vector2> LookEvent;
    public static event System.Action PauseEvent;

    private Vector2 lookInput;

    private void Update()
    {
        if (lookInput.magnitude > 0)
            LookEvent?.Invoke(lookInput);
    }

    public void OnMove(InputValue context)
    {
        var movementInput = context.Get<Vector2>();

        MoveEvent?.Invoke(movementInput);
    }

    public void OnLook(InputValue context)
    {
        lookInput = context.Get<Vector2>();
    }

    public void OnPause(InputValue context)
    {
        if (context.isPressed)
            PauseEvent?.Invoke();
    }
}