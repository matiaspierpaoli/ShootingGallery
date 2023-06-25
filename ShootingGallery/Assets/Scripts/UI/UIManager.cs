using TMPro;
using UnityEngine;

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

    [SerializeField] private GameManager gameManager;

    public bool IsVictoryTextEnabled
    {
        get { return victoryText.enabled; }
        set { if (victoryText != null) victoryText.enabled = value; }
    }

    public bool IsDefeatTextEnabled
    {
        get { return defeatText.enabled; }
        set { if (defeatText != null) defeatText.enabled = value;}
    }

    public bool IsMainMenuButtonEnabled
    {       
        set { if (mainMenuButton != null) mainMenuButton.SetActive(value);}        
    }

    public bool IsReplayButtonEnabled
    {
        set { if (replayButton != null) replayButton.SetActive(value);}      
    }

    public bool IsExitChallengeButtonEnabled
    {
        set { if (exitChallengeButton != null) exitChallengeButton.SetActive(value);}       
    }

    private void Start()
    {
        IsVictoryTextEnabled = false;
        IsDefeatTextEnabled = false;

        IsMainMenuButtonEnabled = false;
        IsReplayButtonEnabled = false;
        IsExitChallengeButtonEnabled = false;

        GetCurrentPointsText();
        GetCurrentAmmoText();

        GetCurrentPointsForAkText();
        GetCurrentPointsForSniperText();

        DisableChallengeTexts();

    }

    public void DrawUI()
    {
        GetCurrentPointsText();                         
        GetCurrentTimeText();          
        GetCurrentEnemiesDefeatedText();                
        GetCurrentAmmoText();
    }

    public void EnableChallengeTexts()
    {
        pointsText.enabled = true;
        currentTimeText.enabled = true;
        enemiesDefeatedText.enabled = true;

        pointsForAKText.enabled = true;
        pointsForSniperText.enabled = true;
    }

    public void DisableChallengeTexts()
    {
        pointsText.enabled = false;
        currentTimeText.enabled = false;
        enemiesDefeatedText.enabled = false;

        pointsForAKText.enabled = false;
        pointsForSniperText.enabled = false;
    }

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

    public void GetCurrentAmmoText()
    {
        bulletsText.text = ""; // Reset ammo text to avoid wrong gun ammo in other area
         
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