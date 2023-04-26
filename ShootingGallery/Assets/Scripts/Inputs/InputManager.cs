using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private PlayerLook _playerLook;
    [SerializeField] private Gun _gun;

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

    public void OnShoot()
    {
        _gun.Shoot();
    }

}
