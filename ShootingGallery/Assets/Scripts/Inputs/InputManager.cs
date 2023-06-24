using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
///  Receive Input through new Input System and invoke events passing the corresponding values
/// </summary>
public class InputManager : MonoBehaviour
{
    public static event System.Action<Vector2> MoveEvent;
    public static event System.Action<Vector2> LookEvent;
    public static event System.Action PauseEvent;

    public void OnMove(InputValue context)
    {
        var movementInput = context.Get<Vector2>();

        MoveEvent?.Invoke(movementInput);
    }

    public void OnLook(InputValue context)
    {
        var lookInput = context.Get<Vector2>();

        LookEvent?.Invoke(lookInput);
    }

    public void OnPause(InputValue context)
    {
        PauseEvent?.Invoke();
    }
}