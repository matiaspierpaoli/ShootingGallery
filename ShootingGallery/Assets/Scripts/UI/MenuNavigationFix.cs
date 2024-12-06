using UnityEngine;
using UnityEngine.EventSystems;

public class MenuNavigationFix : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GameObject defaultSelectedButton;
    private EventSystem eventSystem;
    private GameObject lastSelectedButton;

    private void Awake()
    {
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != null)
        {
            lastSelectedButton = eventSystem.currentSelectedGameObject;
        }
        else if (lastSelectedButton != null)
        {
            eventSystem.SetSelectedGameObject(lastSelectedButton);
        }
        else if (defaultSelectedButton != null)
        {
            eventSystem.SetSelectedGameObject(defaultSelectedButton);
        }
    }
}
