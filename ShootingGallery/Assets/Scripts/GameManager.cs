using System;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
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
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private VictoryTrigger victoryTrigger;
    [SerializeField] private Transform nearExitTransform;
    [SerializeField] private GameObject weaponHolder;
    //[SerializeField] private ShopManager _shopManager;
    [SerializeField] private GameObject pauseManager;
    [SerializeField] private GameObject challengeButtons;
    [SerializeField] private string pistolName;

    [SerializeField] private GameObject challengeEscapeCollider;

    [SerializeField] private float easyDifficultyMultiplier = 1.0f;
    [SerializeField] private float mediumDifficultyMultiplier = 1.5f;
    [SerializeField] private float hardDifficultyMultiplier = 2.0f;

    [SerializeField] private int maxBaseScore = 1000; 
    [SerializeField] private int enemyPointValue = 10; 

    private int currentScore;
    private string highscoreKeyPrefix = "Highscore_";
    private bool winConditionActive = false;

    private ChallengeState currentState = ChallengeState.Inactive;

    public static event System.Action ReplayEvent;
    public static event System.Action ChallengeStartedEvent;

    private void OnEnable()
    {
        if (_gameData.isNextLevelCheatAvailiable)
            InputManager.NearExitCheat += HandleNearExitCheat;

        victoryTrigger.VictoryTriggerEvent += OnVictoryTriggerEvent;
    }

    private void OnDisable()
    {
        if (_gameData.isNextLevelCheatAvailiable)
            InputManager.NearExitCheat -= HandleNearExitCheat;
        victoryTrigger.VictoryTriggerEvent -= OnVictoryTriggerEvent;
    }

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
        ResetGameData();

        _gameData.challengeStarted = true;
        SetState(ChallengeState.Active);
        _gameData.difficulty = difficulty;
        _UIManager.EnableChallengeTexts();
        characterMovement.canMove = true;
        ChallengeStartedEvent?.Invoke();
        //UpdateShopState();
    }

    private void ResetGameData()
    {
        //_gameData.difficulty = DifficultyLevel.Easy;

        weaponHolder.SetActive(true);
        _gameData.currentTime = 0f;
        _gameData.currentEnemiesDefeated = 0f;
        _gameData.victory = false;
        _gameData.defeat = false;
        _gameData.challengeStarted = false;
        _UIManager.IsVictoryTextEnabled = false;
        _UIManager.IsDefeatTextEnabled = false;
        challengeButtons.SetActive(false);
        pauseManager.SetActive(true);

        winConditionActive = false;

        characterMovement.canMove = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _pauseManager.UnfreezeTime();

        //for (int i = 0; i < weapons.Length; i++)
        //{
        //    weapons[i].availiable = false;

        //    if (weapons[i].name == pistolName)
        //        weapons[i].availiable = true;
            
        //}

        player.points = 0;
        player.health = player.maxHealth;

        //UpdateShopState();
    }

    private void OnVictoryTriggerEvent() => winConditionActive = true;

    private bool CheckWinCondition()
    {
       if (winConditionActive)
       {
            CalculateScore();
            SaveHighscoreIfNeeded();
            return true;       
       }
       else 
            return false;
    }

    private bool CheckDefeatCondition()
    {
        if (player.health <= 0)
            return true;
        else
            return false;
    }

    private void FinishChallenge()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        _pauseManager.FreezeTime();

        weaponHolder.SetActive(false);

        challengeButtons.SetActive(true);
        pauseManager.SetActive(false);
        _UIManager.GetReticle().gameObject.SetActive(false);
        _UIManager.GetPointsText().enabled = true;

        _gameData.defeat = true;
        _gameData.challengeStarted = false;
        characterMovement.canMove = false;

        _UIManager.DrawUI();

        currentState = ChallengeState.Inactive;
    }

    public void AddOneEnemyDefeated()
    {
        _gameData.currentEnemiesDefeated++;
    }

    public void Replay()
    {
        ReplayEvent?.Invoke();
        ResetGameData();
        _gameData.challengeStarted = true;
        SetState(ChallengeState.Active);
        HandleChallengeEscapeCollider(true);
    }

    public void ExitChallenge()
    {
        ResetGameData();
        _UIManager.DisableChallengeTexts();
        SetState(ChallengeState.Inactive);
    }

    //public void UpdateShopState()
    //{
    //    if (_gameData.challengeStarted)
    //        _shopManager.SetState(ShopState.Active);
    //    else
    //        _shopManager.SetState(ShopState.Inactive);
    //}

    public void HandleChallengeEscapeCollider(bool active)
    {
        challengeEscapeCollider.SetActive(active);
    }

    public void OnEasyButtonClicked() => StartChallenge(DifficultyLevel.Easy);
    public void OnMediumButtonClicked() => StartChallenge(DifficultyLevel.Medium);
    public void OnHardButtonClicked() => StartChallenge(DifficultyLevel.Hard);

    public void SetEasyDifficulty()
    {
        _gameData.difficulty = DifficultyLevel.Easy;
    }

    public void SetMediumDifficulty()
    {
        _gameData.difficulty = DifficultyLevel.Medium;
    }

    public void SetHardDifficulty()
    {
        _gameData.difficulty = DifficultyLevel.Hard;
    }

    private void HandleNearExitCheat()
    {
        if (currentState == ChallengeState.Active)
        {
            characterMovement.transform.position = nearExitTransform.position;
            characterMovement.transform.rotation = nearExitTransform.rotation;
        }
    }

    private float GetDifficultyMultiplier(DifficultyLevel difficulty)
    {
        return difficulty switch
        {
            DifficultyLevel.Easy => easyDifficultyMultiplier,
            DifficultyLevel.Medium => mediumDifficultyMultiplier,
            DifficultyLevel.Hard => hardDifficultyMultiplier,
            _ => 1.0f,
        };
    }

    private void CalculateScore()
    {
        float timeFactor = Mathf.Clamp01(1 - (_gameData.currentTime / _gameData.maxTime));
        int timeScore = Mathf.RoundToInt(maxBaseScore * timeFactor);

        int enemyScore = Mathf.RoundToInt(_gameData.currentEnemiesDefeated * enemyPointValue);

        float difficultyMultiplier = GetDifficultyMultiplier(_gameData.difficulty);

        currentScore = Mathf.RoundToInt((timeScore + enemyScore) * difficultyMultiplier);
        player.points = currentScore;
    }

    private void SaveHighscoreIfNeeded()
    {
        string scoreKey = GetHighscoreKey(_gameData.difficulty);

        int highscore = PlayerPrefs.GetInt(scoreKey, 0);

        if (currentScore > highscore)
        {
            PlayerPrefs.SetInt(scoreKey, currentScore);
            PlayerPrefs.Save();
        }
    }

    private string GetHighscoreKey(DifficultyLevel difficulty)
    {
        return $"{highscoreKeyPrefix}{difficulty}";
    }

    public int GetHighscore(DifficultyLevel difficulty)
    {
        string scoreKey = GetHighscoreKey(difficulty);
        return PlayerPrefs.GetInt(scoreKey, 0);
    }
}