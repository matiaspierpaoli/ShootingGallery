using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Transform playerCameraPosition;

    private void LateUpdate()
    {
        transform.position = playerCameraPosition.position + playerCameraPosition.TransformDirection(Vector3.zero);
    }
}
