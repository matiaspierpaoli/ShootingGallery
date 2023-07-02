using System;
using UnityEngine;
using UnityEngine.Playables;

public enum ChallengeState
{
    Inactive,
    Active
}

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}

/// <summary>
/// Using states, manage challenge methods such as StartChallenge/Replay/Exit 
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private Pause _pauseManager;
    [SerializeField] private GunData[] weapons;
    [SerializeField] private PlayerStats player;
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private string pistolName;

    [SerializeField] private float maxTime;
    [SerializeField] private float maxEnemiesToDefeat;

    [SerializeField] private GameObject escapeCollider;

    [SerializeField] private DifficultyLevel currentDifficulty;
    [SerializeField] private float easyDifficultyTime;
    [SerializeField] private float mediumDifficultyTime;
    [SerializeField] private float hardDifficultyTime;
    [SerializeField] private float easyDifficultyEnemies;
    [SerializeField] private float mediumDifficultyEnemies;
    [SerializeField] private float hardDifficultyEnemies;

    private ChallengeState currentState = ChallengeState.Inactive;

    private void Start()
    {
        ResetGameData();

        if (Time.timeScale == 0f)
            _pauseManager.UnfreezeTime();
    }

    private void Update()
    {
        switch (currentState)
        {
            case ChallengeState.Inactive:
                break;
            case ChallengeState.Active:
                HandleActiveState();
                break;
        }
    }

    private void HandleActiveState()
    {
        if (!_gameData.victory && !_gameData.defeat)
        {
            _gameData.currentTime += Time.deltaTime;
            _UIManager.DrawUI();

            if (CheckWinCondition())
            {
                _UIManager.IsVictoryTextEnabled = true;               
                FinishChallenge();
            }

            if (CheckDefeatCondition())
            {
                _UIManager.IsDefeatTextEnabled = true;
                FinishChallenge();
            }
        }
    }

    public void SetState(ChallengeState newState)
    {
        currentState = newState;
    }

    public void StartChallenge(DifficultyLevel difficulty)
    {
        _gameData.challengeStarted = true;
        SetState(ChallengeState.Active);
        currentDifficulty = difficulty;
        SetTimeLimit(difficulty);
        SetEnemiesAmmount(difficulty);
        _UIManager.EnableChallengeTexts();
        UpdateShopState();
    }

    private void ResetGameData()
    {
        currentDifficulty = DifficultyLevel.Easy;

        _gameData.currentTime = 0f;
        SetTimeLimit(currentDifficulty);
        _gameData.currentEnemiesDefeated = 0f;
        SetEnemiesAmmount(currentDifficulty);
        _gameData.victory = false;
        _gameData.defeat = false;
        _gameData.challengeStarted = false;

        _UIManager.IsMainMenuButtonEnabled = false;
        _UIManager.IsReplayButtonEnabled = false;
        _UIManager.IsExitChallengeButtonEnabled = false;
        _UIManager.IsVictoryTextEnabled = false;
        _UIManager.IsDefeatTextEnabled = false;

        escapeCollider.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _pauseManager.UnfreezeTime();

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].availiable = false;

            if (weapons[i].name == pistolName)
                weapons[i].availiable = true;
            
        }

        player.points = 0;

        UpdateShopState();
    }

    private bool CheckWinCondition()
    {
       if (_gameData.currentEnemiesDefeated >= _gameData.maxEnemiesToDefeat)
            return true;       
       else 
            return false;
    }

    private bool CheckDefeatCondition()
    {
        if (_gameData.currentTime >= _gameData.maxTime)
            return true;
        else
            return false;
    }

    private void FinishChallenge()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        _pauseManager.FreezeTime();


        _UIManager.IsMainMenuButtonEnabled = true;
        _UIManager.IsReplayButtonEnabled = true;
        _UIManager.IsExitChallengeButtonEnabled = true;

        _gameData.defeat = true;
        _gameData.challengeStarted = false;

        currentState = ChallengeState.Inactive;
    }

    public void AddOneEnemyDefeated()
    {
        _gameData.currentEnemiesDefeated++;
    }

    public void Replay()
    {
        ResetGameData();
        _gameData.challengeStarted = true;
        SetState(ChallengeState.Active);
    }

    public void ExitChallenge()
    {
        ResetGameData();
        _UIManager.DisableChallengeTexts();
        SetState(ChallengeState.Inactive);
    }

    public void UpdateShopState()
    {
        if (_gameData.challengeStarted)
            _shopManager.SetState(ShopState.Active);
        else
            _shopManager.SetState(ShopState.Inactive);
    }

    public void HandleEscapeCollider(bool active)
    {
        escapeCollider.SetActive(active);
    }

    private void SetTimeLimit(DifficultyLevel difficulty)
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                _gameData.maxTime = easyDifficultyTime;
                break;
            case DifficultyLevel.Medium:
                _gameData.maxTime = mediumDifficultyTime;
                break;
            case DifficultyLevel.Hard:
                _gameData.maxTime = hardDifficultyTime;
                break;
            default:
                _gameData.maxTime = easyDifficultyTime; 
                break;
        }
    }

    private void SetEnemiesAmmount(DifficultyLevel difficulty)
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                _gameData.maxEnemiesToDefeat = easyDifficultyEnemies;
                break;
            case DifficultyLevel.Medium:
                _gameData.maxEnemiesToDefeat = mediumDifficultyEnemies;
                break;
            case DifficultyLevel.Hard:
                _gameData.maxEnemiesToDefeat = hardDifficultyEnemies;
                break;
            default:
                _gameData.maxEnemiesToDefeat = easyDifficultyEnemies;
                break;
        }
    }

    public void OnEasyButtonClicked() => StartChallenge(DifficultyLevel.Easy);
    public void OnMediumButtonClicked() => StartChallenge(DifficultyLevel.Medium);
    public void OnHardButtonClicked() => StartChallenge(DifficultyLevel.Hard);
}