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
    public static event System.Action NearExitCheat;
    public static event System.Action GodModeEvent;
    public static event System.Action FlashEvent;

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

    public void OnLookMouse(InputValue context)
    {
        var lookInput = context.Get<Vector2>();
        LookEvent?.Invoke(lookInput);
    }

    public void OnLookController(InputValue context)
    {
        lookInput = context.Get<Vector2>();
    }

    public void OnPause(InputValue context)
    {
        if (context.Get<float>() > 0.0f)
            PauseEvent?.Invoke();
    }
    
    public void OnNearExitCheat(InputValue context)
    {
        if (context.Get<float>() > 0.0f)
            NearExitCheat?.Invoke();
    }

    public void OnGodModeCheat(InputValue context)
    {
        if (context.Get<float>() > 0.0f)
            GodModeEvent?.Invoke();
    }
    
    public void OnFlashCheat(InputValue context)
    {
        if (context.Get<float>() > 0.0f)
            FlashEvent?.Invoke();
    }
}