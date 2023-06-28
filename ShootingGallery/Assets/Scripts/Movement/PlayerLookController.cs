using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Get movement input through events and new input system and rotate tranform
/// </summary>
public class PlayerLookController : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    
    private float sensX;
    private float sensY;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private void OnEnable()
    {
        InputManager.LookEvent += OnLook;
        CameraSensitivity.SensChangeEvent += OnCameraSensitivityChangeX;
        CameraSensitivity.SensChangeEvent += OnCameraSensitivityChangeY;
    }

    private void OnDisable()
    {
        InputManager.LookEvent -= OnLook;
    }

    private void Start()
    {
        sensX = PlayerPrefs.GetFloat("horizontalSensitivity", 75f);
        sensY = PlayerPrefs.GetFloat("verticalSensitivity", 75f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnLook(Vector2 cameraInput)
    {
        ProcessLook(cameraInput);
    }

    public void OnCameraSensitivityChangeX()
    {
        sensX = PlayerPrefs.GetFloat("horizontalSensitivity", 75f);
    }

    public void OnCameraSensitivityChangeY()
    {
        sensY = PlayerPrefs.GetFloat("verticalSensitivity", 75f);
    }

    public void ProcessLook(Vector2 cameraInput)
    {
        float mouseX = cameraInput.x * Time.deltaTime * sensX;
        float mouseY = cameraInput.y * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
