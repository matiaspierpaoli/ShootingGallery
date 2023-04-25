using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    //[SerializeField] Camera cam;   
    
    [SerializeField] private float sensX = 10f;
    [SerializeField] private float sensY = 10f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Start()
    {
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

        //cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * sensX);
    }
}
