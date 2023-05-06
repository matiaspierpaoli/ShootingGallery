using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private PlayerLook _playerLook;

    public void OnMove(InputValue context)
    {
        var movementInput = context.Get<Vector2>();
        _characterMovement.ProcessMove(movementInput);
    }

    public void OnLook(InputValue context)
    {
        var movementInput = context.Get<Vector2>();
        _playerLook.ProcessLook(movementInput);
    }
}
