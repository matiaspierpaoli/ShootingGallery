using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string[] difficultyPlayerPrefNames;
    [SerializeField] private string[] difficultyNames;
    [SerializeField] private TextMeshProUGUI[] highScoreTexts;

    private void Awake()
    {
        for (int i = 0; i < difficultyPlayerPrefNames.Length; i++)
        {
            int highScore = PlayerPrefs.GetInt(difficultyPlayerPrefNames[i], 0);

            highScoreTexts[i].gameObject.SetActive(false);
            highScoreTexts[i].text = $"{difficultyNames[i]}: {highScore}";
        }
    }

    public void ToggleTexts(bool active)
    {
        foreach (var text in highScoreTexts)
        {
            text.gameObject.SetActive(active);
        }
    }
}
