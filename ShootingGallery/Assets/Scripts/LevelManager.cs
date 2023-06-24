using TMPro;
using UnityEngine;

/// <summary>
/// Determine which gun bools and text are correspondant to the floor in which the player is
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GunData pistolData;
    [SerializeField] private GunData akData;
    [SerializeField] private GunData sniperData;

    [SerializeField] private TMP_Text currentTimeText;
    [SerializeField] private TMP_Text currentAmmoText;

    private void OnTriggerEnter(Collider other)
    {
        FloorSettingsContainer floorSettingsContainer = other.GetComponent<FloorSettingsContainer>();

        if (floorSettingsContainer != null)
        {
            FloorSettings floorSettings = floorSettingsContainer.floorSettings;

            pistolData.availiable = floorSettings.pistolAvailable;
            akData.availiable = floorSettings.akAvailable;
            sniperData.availiable = floorSettings.sniperAvailable;

            gameData.challengeStarted = floorSettings.challengeStarted;
            gameData.currentEnemiesDefeated = 0;
            gameData.currentTime = 0;

            currentTimeText.enabled = floorSettings.currentTimeTextEnabled;
            currentAmmoText.enabled = floorSettings.currentAmmoTextEnabled;
        }
    }
}
