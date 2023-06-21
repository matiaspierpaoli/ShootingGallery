using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private GunData pistolData;
    [SerializeField] private GunData akData;
    [SerializeField] private GunData sniperData;

    [SerializeField] private TMP_Text currentTimeText;
    [SerializeField] private TMP_Text currentAmmoText;

    private void OnTriggerEnter(Collider other)
    {
        //other.TryGetComponent(out IFloor floor)
        //TODO: TP2 - SOLID
        if (gameObject.tag == "neutralArea")
        {
            pistolData.availiable = false;
            akData.availiable = false;
            sniperData.availiable = false;

            _gameData.challengeStarted = false;
            _gameData.currentEnemiesDefeated = 0;
            _gameData.currentTime = 0;

            currentTimeText.enabled = false;
            currentAmmoText.enabled = false;
        }
        else if (gameObject.tag == "ChallengeArea")
        {
            pistolData.availiable = true;
            akData.availiable = false;
            sniperData.availiable = false;

            currentTimeText.enabled = true;
            currentAmmoText.enabled = true;
        }
        else if (gameObject.tag == "pistolPracticeArea")
        {
            pistolData.availiable = true;
            akData.availiable = false;
            sniperData.availiable = false;

            currentTimeText.enabled = false;
            currentAmmoText.enabled = true;
        }
        else if (gameObject.tag == "AKPracticeArea")
        {
            pistolData.availiable = false;
            akData.availiable = true;
            sniperData.availiable = false;

            currentTimeText.enabled = false;
            currentAmmoText.enabled = true;
        }
        else if (gameObject.tag == "sniperPracticeArea")
        {
            pistolData.availiable = false;
            akData.availiable = false;
            sniperData.availiable = true;

            currentTimeText.enabled = false;
            currentAmmoText.enabled = true;
        }
    }
}
