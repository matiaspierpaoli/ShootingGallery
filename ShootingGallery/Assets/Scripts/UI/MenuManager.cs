using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject[] menuButtons;
    [SerializeField] private GameObject returnNoButton;
    [SerializeField] private string[] difficultyPlayerPrefNames;
    [SerializeField] private string[] difficultyNames;
    [SerializeField] private TMP_Text[] highScoreTexts;
    [SerializeField] private GameObject[] panels;

    private bool isExitPanelActive = false;

    private void Awake()
    {
        for (int i = 0; i < difficultyPlayerPrefNames.Length; i++)
        {
            int highScore = PlayerPrefs.GetInt(difficultyPlayerPrefNames[i], -1);

            if (highScore == -1)
            {
                highScore = 0;
                PlayerPrefs.SetInt(difficultyPlayerPrefNames[i], highScore);
                PlayerPrefs.Save();
            }

            highScoreTexts[i].text = $"{difficultyNames[i]}: {highScore}";
        }
    }

    public void TogglePanelOn(GameObject panel)
    {
        if (isExitPanelActive) return;
        foreach (GameObject p in panels)
        {
            p.SetActive(false);
        }

        panel.SetActive(true);
    }

    public void TogglePanelOff(GameObject panel)
    {
        if (isExitPanelActive) return;
            panel.SetActive(false);
    }

    public void ToggleExitPanelBool(bool active)
    {
        isExitPanelActive = active;

        if (active)
        {
            foreach (GameObject button in menuButtons)
                button.GetComponent<Button>().interactable = false;

            ControlEventSystem(returnNoButton);
        }
        else
        {
            foreach (GameObject button in menuButtons)
                button.GetComponent<Button>().interactable = true;
            ControlEventSystem(menuButtons[0]);
        }
    }

    private void ControlEventSystem(GameObject defaultButton)
    {
        eventSystem.SetSelectedGameObject(defaultButton);
    }
}
