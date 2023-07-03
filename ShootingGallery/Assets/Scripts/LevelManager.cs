using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Determine which gun bools and text are correspondant to the floor in which the player is
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GunData pistolData;
    [SerializeField] private GunData akData;
    [SerializeField] private GunData sniperData;
    [SerializeField] private PlayerStats playerData;

    [SerializeField] private TMP_Text currentTimeText;
    [SerializeField] private TMP_Text currentAmmoText;

    [SerializeField] private Image[] selectedWeapons;
    [SerializeField] private Image[] unselectedWeapons;

    [SerializeField] private GameObject[] weapons;

    public static event System.Action ChangeAreaEvent;

    private void OnTriggerEnter(Collider other)
    {
        FloorSettingsContainer floorSettingsContainer = other.GetComponent<FloorSettingsContainer>();

        if (floorSettingsContainer != null)
        {
            FloorSettings floorSettings = floorSettingsContainer.floorSettings;

            pistolData.availiable = floorSettings.pistolAvailable;
            akData.availiable = floorSettings.akAvailable;
            sniperData.availiable = floorSettings.sniperAvailable;

            for (int i = 0; i < weapons.Length; i++)
            {
                if (pistolData.availiable)
                    weapons[0].SetActive(true);
                else
                    weapons[0].SetActive(false);

                if (akData.availiable)
                    weapons[1].SetActive(true);
                else
                    weapons[1].SetActive(false);

                if (sniperData.availiable)
                    weapons[2].SetActive(true);
                else
                    weapons[2].SetActive(false);
            }

            gameData.challengeStarted = floorSettings.challengeStarted;
            gameData.currentEnemiesDefeated = 0;
            gameData.currentTime = 0;
            playerData.points = 0;

            currentTimeText.enabled = floorSettings.currentTimeTextEnabled;
            currentAmmoText.enabled = floorSettings.currentAmmoTextEnabled;

            for (int i = 0; i < selectedWeapons.Length; i++)
            {
                if (!weapons[i].activeSelf)
                    selectedWeapons[i].enabled = false;
                else
                    selectedWeapons[i].enabled = true;
            }

            for (int i = 0; i < unselectedWeapons.Length; i++)
            {
                if (!weapons[i].activeSelf)
                    unselectedWeapons[i].enabled = true;
                else
                    unselectedWeapons[i].enabled = false;
            }

            ChangeAreaEvent?.Invoke();
        }
    }
}
