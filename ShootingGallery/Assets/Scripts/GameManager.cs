using System;
using UnityEngine;

//TODO: Documentation - Add summary
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private Pause _pauseManager;
    [SerializeField] private GunData[] weapons;
    [SerializeField] private PlayerStats player;

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
        //TODO: TP2 - FSM
        if (_gameData.challengeStarted)
        {
            if (!_gameData.victory && !_gameData.defeat)
            {
                _gameData.currentTime += Time.deltaTime;
                //TODO: Fix - Should be event based
                if (CheckWinCondition())
                    _UIManager.VictoryTextEnabled = true;
                //_UIManager.EnableVictoryText();

                if (CheckDefeatCondition())
                    _UIManager.DefeatTextEnabled = true;
                //_UIManager.EnableDefeatText();
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
        _gameData.challengeStarted = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _pauseManager.UnfreezeTime();

        //_UIManager.DisableMainMenuButton();
        _UIManager.MainMenuButtonEnabled = false;
        
        //_UIManager.DisableReplayButton();
        _UIManager.ReplayButtonEnabled = false;

        //_UIManager.DisableExitChallengeButton();
        _UIManager.ExitChallengeButtonEnabled = false;

        //_UIManager.DisableVictoryText();
        _UIManager.VictoryTextEnabled = false;
              
        //_UIManager.DisableDefeatText();
        _UIManager.DefeatTextEnabled = false;

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].availiable = false;

            //TODO: Fix - Hardcoded value
            if (weapons[i].name == "Pistol")
                weapons[i].availiable = true;
        }

        player.points = 0;
    }

    private bool CheckWinCondition()
    {
       if (_gameData.currentEnemiesDefeated >= _gameData.maxEnemiesToDefeat)
       {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _pauseManager.FreezeTime();
            
            //_UIManager.EnableMainMenuButton();           
            _UIManager.MainMenuButtonEnabled = true;
            
            //_UIManager.EnableReplayButton();
            _UIManager.ReplayButtonEnabled = true;

            //_UIManager.EnableExitChallengeButton();
            _UIManager.ExitChallengeButtonEnabled = true;


            _gameData.victory = true;
            
            _gameData.challengeStarted = false;
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

            //_UIManager.EnableMainMenuButton();
            _UIManager.MainMenuButtonEnabled = true;

            //_UIManager.EnableReplayButton();
            _UIManager.ReplayButtonEnabled = true;
            
            //_UIManager.EnableExitChallengeButton();
            _UIManager.ExitChallengeButtonEnabled = true;

            _gameData.defeat = true;
            _gameData.challengeStarted = false;
            return true;
        }
        else
            return false;
    }

    public void AddOneEnemyDefeated()
    {
        _gameData.currentEnemiesDefeated++;
    }

    public void StartChallenge()
    {
        _gameData.challengeStarted = true;
    }

    public void Replay()
    {
        ResetGameData();
        _gameData.challengeStarted = true;
    }

    public void ExitChallenge()
    {
        ResetGameData();
    }
}
