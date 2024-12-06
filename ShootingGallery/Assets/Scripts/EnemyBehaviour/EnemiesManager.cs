using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private List<GameObject> enemies; // List of enemies in the scene
    [SerializeField] private GameData gameData;

    [Header("Enemies Ammount")]
    [SerializeField] private int easyDifficultyEnemies;
    [SerializeField] private int mediumDifficultyEnemies;
    [SerializeField] private int hardDifficultyEnemies;

    private void OnEnable()
    {
        GameManager.ReplayEvent += TurnOnRandomEnemiesBasedOnDifficulty;
        GameManager.ChallengeStartedEvent += TurnOnRandomEnemiesBasedOnDifficulty;
    }

    private void OnDisable()
    {
        GameManager.ReplayEvent -= TurnOnRandomEnemiesBasedOnDifficulty;
        GameManager.ChallengeStartedEvent -= TurnOnRandomEnemiesBasedOnDifficulty;
    }

    private void Awake()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetActive(false);
        }
    }

    private void TurnOnRandomEnemiesBasedOnDifficulty()
    {
        int numEnemiesToTurnOff = GetNumEnemiesToTurnOff();

        ShuffleEnemiesList();

        for (int i = 0; i < numEnemiesToTurnOff && i < enemies.Count; i++)
        {
            enemies[i].SetActive(true);
        }
    }

    private int GetNumEnemiesToTurnOff()
    {
        switch (gameData.difficulty)
        {
            case DifficultyLevel.Easy:
                return easyDifficultyEnemies;
            case DifficultyLevel.Medium:
                return mediumDifficultyEnemies;
            case DifficultyLevel.Hard:
                return hardDifficultyEnemies;
            default:
                return 0;
        }
    }

    private void ShuffleEnemiesList()
    {
        // Fisher-Yates shuffle algorithm to shuffle the list
        for (int i = 0; i < enemies.Count; i++)
        {
            int randomIndex = Random.Range(i, enemies.Count);
            GameObject temp = enemies[randomIndex];
            enemies[randomIndex] = enemies[i];
            enemies[i] = temp;
        }
    }

}
