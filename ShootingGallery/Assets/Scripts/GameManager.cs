using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private PauseScript _pauseManager;

    [SerializeField] private float maxTime;
    [SerializeField] private float maxEnemiesToDefeat;

    private void Start()
    {
        ResetGameData();

        if (Time.timeScale == 0f)
            _pauseManager.UnfreezeTime();
    }

    private void Update()
    {
        _gameData.currentTime += Time.deltaTime;

        if (!_gameData.practiceArea)
        {
            if (!_gameData.victory && !_gameData.defeat)
            {
                if (CheckWinCondition())
                    _UIManager.EnableVictoryText();

                if (CheckDefeatCondition())
                    _UIManager.EnableDefeatText();
            }
        }
    }

    private void ResetGameData()
    {
        _gameData.currentTime = 0f;
        _gameData.currentEnemiesDefeated = 0f;
        _gameData.maxTime = maxTime;
        _gameData.maxEnemiesToDefeat = maxEnemiesToDefeat;
        _gameData.victory = false;
        _gameData.defeat = false;
    }

    private bool CheckWinCondition()
    {
       if (_gameData.currentEnemiesDefeated >= _gameData.maxEnemiesToDefeat)
       {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _pauseManager.FreezeTime();
            _UIManager.EnableMainMenuButton();
            _gameData.victory = true;
            return true;
       }
       else 
            return false;
    }

    private bool CheckDefeatCondition()
    {
        if (_gameData.currentTime >= _gameData.maxTime)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _pauseManager.FreezeTime();
            _UIManager.EnableMainMenuButton();
            _gameData.defeat = true;
            return true;
        }
        else
            return false;
    }

    public void AddOneEnemyDefeated()
    {
        _gameData.currentEnemiesDefeated++;
    }
}
