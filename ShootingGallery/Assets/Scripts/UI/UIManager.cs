using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text bulletsText;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text defeatText;
    [SerializeField] private PlayerStats playerData;

    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject replayButton;
    [SerializeField] private GameObject exitChallengeButton;
 
    [SerializeField] private GameObject[] weapons;

    [SerializeField] private GunData pistolData;
    [SerializeField] private GunData akData;
    [SerializeField] private GunData sniperData;

    [SerializeField] private TMP_Text pointsForAKText;
    [SerializeField] private TMP_Text pointsForSniperText;
    [SerializeField] private TMP_Text currentTimeText;
    [SerializeField] private TMP_Text enemiesDefeatedText;
    [SerializeField] private GameData _gameData;

    public bool VictoryTextEnabled
    {
        get { return victoryText.enabled; }
        set { victoryText.enabled = value; }
    }

    public bool DefeatTextEnabled
    {
        get { return defeatText.enabled; }
        set { defeatText.enabled = value; }
    }

    public bool MainMenuButtonEnabled
    {
        set
        {
            bool newValue = value;
            mainMenuButton.SetActive(newValue);
        }
    }

    public bool ReplayButtonEnabled
    {
        set
        {
            bool newValue = value;
            replayButton.SetActive(newValue);
        }
    }

    public bool ExitChallengeButtonEnabled
    {
        set
        {
            bool newValue = value;
            exitChallengeButton.SetActive(newValue);
        }
    }

    private void Start()
    {
        VictoryTextEnabled = false;
        DefeatTextEnabled = false;

        MainMenuButtonEnabled = false;
        ReplayButtonEnabled = false;

        GetCurrentPointsText();
        GetCurrentAmmoText();

        GetCurrentPointsForAkText();
        GetCurrentPointsForSniperText();

        pointsText.enabled = false;
        pointsForAKText.enabled = false;
        pointsForSniperText.enabled = false;
        currentTimeText.enabled = false;
        enemiesDefeatedText.enabled = false;
        
    }

    private void Update()
    {
        //TODO: Fix - Should be event based
        DrawUI();
    }

    private void DrawUI()
    {
        if (_gameData.challengeStarted)
        {
            GetCurrentPointsText();
            pointsText.enabled = true;   
            
            GetCurrentTimeText();
            currentTimeText.enabled = true;

            GetCurrentEnemiesDefeatedText();
            enemiesDefeatedText.enabled = true;

            pointsForAKText.enabled = true;
            pointsForSniperText.enabled = true;

        }
        else
        {
            pointsText.enabled = false;
            currentTimeText.enabled = false;
            enemiesDefeatedText.enabled = false;
        }
        
        GetCurrentAmmoText();
    }

    //TODO: Fix - Should be native Setter/Getter
    //public void EnableVictoryText()
    //{
    //    victoryText.enabled = true;
    //}

    //public void EnableDefeatText()
    //{
    //    defeatText.enabled = true;
    //}

    //public void DisableVictoryText()
    //{
    //    victoryText.enabled = false;
    //}

    //public void DisableDefeatText()
    //{
    //    defeatText.enabled = false;
    //}

    //public void EnableMainMenuButton()
    //{
    //    mainMenuButton.SetActive(true);
    //}

    //public void EnableReplayButton()
    //{
    //    replayButton.SetActive(true);
    //}

    //public void EnableExitChallengeButton()
    //{
    //    exitChallengeButton.SetActive(true);
    //}

    //public void DisableMainMenuButton()
    //{
    //    mainMenuButton.SetActive(false);
    //}
    
    //public void DisableReplayButton()
    //{
    //    replayButton.SetActive(false);
    //}
    
    //public void DisableExitChallengeButton()
    //{
    //    exitChallengeButton.SetActive(false);
    //}

    private void GetCurrentPointsText()
    {
        pointsText.text = "Points: " + playerData.points.ToString();
    }

    private void GetCurrentEnemiesDefeatedText()
    {
        enemiesDefeatedText.text = "Enemies Defeated: " + _gameData.currentEnemiesDefeated.ToString() + "/" + _gameData.maxEnemiesToDefeat.ToString();
    }

    private void GetCurrentPointsForAkText()
    {
        pointsForAKText.text = "AK: " + akData.cost.ToString() + " Points";
    }

    private void GetCurrentPointsForSniperText()
    {
        pointsForSniperText.text = "Sniper: " + sniperData.cost.ToString() + " Points";
    }

    private void GetCurrentAmmoText()
    {
        for (int i = 0; i < weapons.Length; i++) 
        {
            if (i == 0)
            {
                if (weapons[0].activeSelf)
                {
                    if (pistolData.availiable)
                        bulletsText.text = pistolData.currentAmmo.ToString() + "/" + pistolData.magSize.ToString();
                }               
            }
            else if (i == 1)
            {
                if (weapons[1].activeSelf)
                {
                    if (akData.availiable)
                        bulletsText.text = akData.currentAmmo.ToString() + "/" + akData.magSize.ToString();
                }
            }
            else
            {
                if (weapons[2].activeSelf)
                {
                    if (sniperData.availiable)
                        bulletsText.text = sniperData.currentAmmo.ToString() + "/" + sniperData.magSize.ToString();
                }
            }
        }
    }

    private void GetCurrentTimeText()
    {
        int currentTime = (int)_gameData.currentTime;
        currentTimeText.text = "Current Time: " + currentTime.ToString() + "/" + _gameData.maxTime;        
    }
}
