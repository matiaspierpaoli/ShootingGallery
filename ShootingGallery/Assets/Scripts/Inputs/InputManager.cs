using UnityEngine;
using UnityEngine.InputSystem;

//TODO: Documentation - Add summary
public class InputManager : MonoBehaviour
{
    //TODO: Fix - Should be event based
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private PlayerLook _playerLook;
    [SerializeField] private PauseScript _PauseManager;

    //TODO: TP2 - Remove unused methods/variables/classes
    [SerializeField] private SceneLoader _sceneLoader;

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

    public void OnPause(InputValue context)
    {
        _PauseManager.Pause();       
    }
}
