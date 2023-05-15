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
 
    [SerializeField] private GameObject[] weapons;

    [SerializeField] private GunData pistolData;
    [SerializeField] private GunData akData;
    [SerializeField] private GunData sniperData;


    [SerializeField] private TMP_Text pointsForAKText;
    [SerializeField] private TMP_Text pointsForSniperText;
    [SerializeField] private TMP_Text currentTimeText;
    [SerializeField] private GameData _gameData;

    // Start is called before the first frame update
    void Start()
    {
        victoryText.enabled = false;
        defeatText.enabled = false;

        mainMenuButton.SetActive(false);

        GetCurrentPointsText();
        GetCurrentAmmoText();

        if (_gameData.tutorial)
        {
            pointsForAKText.enabled = false;
            pointsForSniperText.enabled = false;
            currentTimeText.enabled = false;
        }
        else
        {
            pointsForAKText.enabled = true;
            pointsForAKText.text = "Points for AK: " + akData.cost;
            pointsForSniperText.enabled = true;
            pointsForSniperText.text = "Points for Sniper: " + sniperData.cost;
            currentTimeText.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentPointsText();
        GetCurrentAmmoText();
        GetCurrentTimeText();
    }


    public void EnableVictoryText()
    {
        victoryText.enabled = true;
    }

    public void EnableDefeatText()
    {
        defeatText.enabled = true;
    }

    public void EnableMainMenuButton()
    {
        mainMenuButton.SetActive(true);
    }

    void GetCurrentPointsText()
    {
        pointsText.text = "Points: " + playerData.points.ToString();
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
                    if (pistolData.availiable)
                        bulletsText.text = akData.currentAmmo.ToString() + "/" + akData.magSize.ToString();
                }
            }
            else
            {
                if (weapons[2].activeSelf)
                {
                    if (pistolData.availiable)
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
