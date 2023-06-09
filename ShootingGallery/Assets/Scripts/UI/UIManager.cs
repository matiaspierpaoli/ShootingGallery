using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text bulletsText;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text defeatText;
    [SerializeField] private PlayerStats playerData;

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

    [SerializeField] private Image[] selectedWeapons;
    [SerializeField] private Image[] unselectedWeapons;

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

    private void OnEnable()
    {
        WeaponSwitching.SwitchWeaponEvent += OnSwitchWeapon;
        Gun.ReloadStartedEvent += OnReload;
        Gun.ReloadFinishedEvent += OnReload;
        Gun.ShootingStartedEvent += OnShoot;
        LevelManager.ChangeAreaEvent += OnChangeArea;
    }

    private void OnDisable()
    {
        WeaponSwitching.SwitchWeaponEvent -= OnSwitchWeapon;
        Gun.ReloadStartedEvent -= OnReload;
        Gun.ReloadFinishedEvent -= OnReload;
        Gun.ShootingStartedEvent -= OnShoot;
        LevelManager.ChangeAreaEvent -= OnChangeArea;
    }

    private void Start()
    {
        IsVictoryTextEnabled = false;
        IsDefeatTextEnabled = false;

        for (int i = 0; i < selectedWeapons.Length; i++)
        {
            selectedWeapons[i].enabled = false;
        }

        for (int i = 0; i < unselectedWeapons.Length; i++)
        {
            unselectedWeapons[i].enabled = true;
        }

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
        bulletsText.text = ""; // Reset ammo text to avoid wrong gun ammo in other area before switching
         
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

    private void OnReload()
    {
        GetCurrentAmmoText();
    }

    private void OnShoot()
    {
        GetCurrentAmmoText();
    }

    private void OnChangeArea()
    {
        GetCurrentAmmoText();
    }

    private void OnSwitchWeapon(int weaponSelected)
    {
        SetSelectedWeaponImage(weaponSelected);
        GetCurrentAmmoText();
    }

    public void SetSelectedWeaponImage(int weaponSelected)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == weaponSelected)
            {
                if (weapons[weaponSelected].activeSelf)
                {
                    selectedWeapons[weaponSelected].enabled = true;
                    unselectedWeapons[weaponSelected].enabled = false;
                }
            }
            else
            {
                selectedWeapons[i].enabled = false;
                unselectedWeapons[i].enabled = true;
            }
        }      
    }
}