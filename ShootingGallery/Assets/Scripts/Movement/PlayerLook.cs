using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    
    private float sensX;
    private float sensY;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Start()
    {
        sensX = PlayerPrefs.GetFloat("horizontalSensitivity", 75f);
        sensY = PlayerPrefs.GetFloat("verticalSensitivity", 75f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * Time.deltaTime * sensX;
        float mouseY = input.y * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation =  Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
