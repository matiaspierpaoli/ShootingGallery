using UnityEngine;

public class SceneObjectManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GameObject[] objectsToToggle;

    public void ToggleObjects(bool isActive)
    {
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(isActive);
        }
    }
}
