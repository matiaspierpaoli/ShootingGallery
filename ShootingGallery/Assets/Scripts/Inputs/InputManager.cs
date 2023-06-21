using UnityEngine;
using UnityEngine.InputSystem;

//TODO: Documentation - Add summary
public class InputManager : MonoBehaviour
{
    //TODO: Fix - Should be event based
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private PlayerLookController _playerLook;
    [SerializeField] private Pause _PauseManager;

    public void OnMove(InputValue context)
    {
        var movementInput = context.Get<Vector2>();
        _characterMovement.SetMovementDir(movementInput);
    }

    public void OnLook(InputValue context)
    {
        var movementInput = context.Get<Vector2>();
        _playerLook.ProcessLook(movementInput);
    }

    public void OnPause(InputValue context)
    {
        _PauseManager.PauseScreen();       
    }
}
