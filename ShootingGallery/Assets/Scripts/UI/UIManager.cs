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
    [SerializeField] private PlayerData playerData;

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

    void Start()
    {
        victoryText.enabled = false;
        defeatText.enabled = false;

        DisableMainMenuButton();
        DisableReplayButton();

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

    // Update is called once per frame
    void Update()
    {
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

    public void EnableVictoryText()
    {
        victoryText.enabled = true;
    }

    public void EnableDefeatText()
    {
        defeatText.enabled = true;
    }

    public void DisableVictoryText()
    {
        victoryText.enabled = false;
    }

    public void DisableDefeatText()
    {
        defeatText.enabled = false;
    }

    public void EnableMainMenuButton()
    {
        mainMenuButton.SetActive(true);
    }

    public void EnableReplayButton()
    {
        replayButton.SetActive(true);
    }

    public void EnableExitChallengeButton()
    {
        exitChallengeButton.SetActive(true);
    }

    public void DisableMainMenuButton()
    {
        mainMenuButton.SetActive(false);
    }
    
    public void DisableReplayButton()
    {
        replayButton.SetActive(false);
    }
    
    public void DisableExitChallengeButton()
    {
        exitChallengeButton.SetActive(false);
    }

    void GetCurrentPointsText()
    {
        pointsText.text = "Points: " + playerData.points.ToString();
    }

    void GetCurrentEnemiesDefeatedText()
    {
        enemiesDefeatedText.text = "Enemies Defeated: " + _gameData.currentEnemiesDefeated.ToString() + "/" + _gameData.maxEnemiesToDefeat.ToString();
    }

    void GetCurrentPointsForAkText()
    {
        pointsForAKText.text = "AK: " + akData.cost.ToString() + " Points";
    }

    void GetCurrentPointsForSniperText()
    {
        pointsForSniperText.text = "Sniper: " + sniperData.cost.ToString() + " Points";
    }

    void GetCurrentAmmoText()
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
