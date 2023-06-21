using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialGameManager : MonoBehaviour
{
    [SerializeField] private Pause _pauseManager;
    [SerializeField] private GameObject[] weaponsGO;
    [SerializeField] private GunData[] _gunData; 

    [SerializeField] private TutorialData _tutorialData;

    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private PlayerLookController _playerLook;

    [SerializeField] private TMP_Text nextStepText;
    [SerializeField] private TMP_Text bulletsText;

    private float currentTutorialTimePractice = 0f;
    [SerializeField] private float maxTutorialTimePractice;

    private void Start()
    {
        ResetTutorialData();

        _tutorialData.isMovingPlayerAvailable = true;

        //TODO: Fix - Hardcoded value
        nextStepText.text = "Move Player with WASD or Left-Stick";

        for (int i = 0; i < _gunData.Length; i++)
        {
            _gunData[i].availiable = true;
        }

        for (int i = 0; i < weaponsGO.Length; i++)
        {
            if (i == 0)
                weaponsGO[i].SetActive(true);
            else
                weaponsGO[i].SetActive(false);
        }
    }

    private void Update()
    {
        //TODO: Fix - Should be event based
        GetCurrentAmmoText();
    }

    public void OnMove(InputValue context)
    {
        if (_tutorialData.isMovingPlayerAvailable)
        {
            var movementInput = context.Get<Vector2>();
            _characterMovement.SetMovementDir(movementInput);

            if (!_tutorialData.finishedMovingPlayer)
            {
                currentTutorialTimePractice ++;

                Debug.Log("Correctly Moved Player");

                if (currentTutorialTimePractice >= maxTutorialTimePractice)
                {                
                    _tutorialData.isMovingCameraAvailable = true;
                    _tutorialData.finishedMovingPlayer = true;

                    currentTutorialTimePractice = 0f;

                    nextStepText.text = "Next Step: Move Camera with mouse or Right-Stick";
                }
            }
        }
    }

    public void OnLook(InputValue context)
    {
        if (_tutorialData.isMovingCameraAvailable)
        {
            var movementInput = context.Get<Vector2>();
            _playerLook.ProcessLook(movementInput);

            if (!_tutorialData.finishedMovingCamera)
            {
                currentTutorialTimePractice += Time.deltaTime;

                Debug.Log("Correctly Moved Camera");

                if (currentTutorialTimePractice >= maxTutorialTimePractice)
                {
                    _tutorialData.isShootingAvailable = true;
                    _tutorialData.finishedMovingCamera = true;

                    currentTutorialTimePractice = 0f;

                    nextStepText.text = "Next Step: Shoot with left-click or R2/RT";

                }
            }
        }
    }

    public void OnShoot(InputValue context)
    {
        if (_tutorialData.isShootingAvailable)
        {
            if (!_tutorialData.finishedShooting)
            {
                currentTutorialTimePractice++;
                Debug.Log("Correctly Shooting");

                if (currentTutorialTimePractice >= 1)
                {
                    //_tutorialData.finishedMovingPlayer = true;

                    _tutorialData.isReloadingAvailable = true;
                    _tutorialData.finishedShooting = true;

                    currentTutorialTimePractice = 0f;

                    nextStepText.text = "Next Step: Reload with R or Square/X";
                }
            }
        }
    }

    public void OnReload(InputValue context)
    {
        if (_tutorialData.isReloadingAvailable)
        {
            if (!_tutorialData.finishedReloading)
            {
                currentTutorialTimePractice++;
                Debug.Log("Correctly Reloaded");

                if (currentTutorialTimePractice >= 1)
                {
                    _tutorialData.isPausingAvailable = true;
                    _tutorialData.finishedReloading = true;

                    currentTutorialTimePractice = 0f;

                    nextStepText.text = "Next Step: Pause with P or Options/Start";
                }
            }
        }
    }

    public void OnPause(InputValue context)
    {
        if (_tutorialData.isPausingAvailable)
        {
            if (!_tutorialData.finishedPausing)
            {
                currentTutorialTimePractice++;
                Debug.Log("Correctly Paused");

                if (currentTutorialTimePractice >= 1)
                {
                    _tutorialData.finishedPausing = true;
                    _tutorialData.isChanginWeaponsAvailable = true;
                    bulletsText.enabled = true;

                    currentTutorialTimePractice = 0f;

                    nextStepText.text = "Next Step: Change weapons with 1,2,3 or gamepad left,up,right arrows";
                }
            }
            else
                _pauseManager.PauseScreen();
        }
    }

    public void OnChangeWeapon1(InputValue context)
    {
        if (_tutorialData.isChanginWeaponsAvailable)
        {
            Debug.Log("Correctly Changed Weapon to 1");
            
            for (int i = 0; i < weaponsGO.Length; i++) 
            {
                if (i == 0)
                    weaponsGO[i].SetActive(true);
                else
                    weaponsGO[i].SetActive(false);
            }           
        }
    }

    public void OnChangeWeapon2(InputValue context)
    {
        if (_tutorialData.isChanginWeaponsAvailable)
        {
            Debug.Log("Correctly Changed Weapon to 2");

            for (int i = 0; i < weaponsGO.Length; i++)
            {
                if (i == 1)
                    weaponsGO[i].SetActive(true);
                else
                    weaponsGO[i].SetActive(false);
            }
        }
    }

    public void OnChangeWeapon3(InputValue context)
    {
        if (_tutorialData.isChanginWeaponsAvailable)
        {
            Debug.Log("Correctly Changed Weapon to 3");

            for (int i = 0; i < weaponsGO.Length; i++)
            {
                if (i == 2)
                    weaponsGO[i].SetActive(true);
                else
                    weaponsGO[i].SetActive(false);
            }
        }
    }

    void GetCurrentAmmoText()
    {
        for (int i = 0; i < weaponsGO.Length; i++)
        {
            if (i == 0)
            {
                if (weaponsGO[0].activeSelf)                  
                    bulletsText.text = _gunData[0].currentAmmo.ToString() + "/" + _gunData[0].magSize.ToString();               
            }
            else if (i == 1)
            {
                if (weaponsGO[1].activeSelf)                   
                    bulletsText.text = _gunData[1].currentAmmo.ToString() + "/" + _gunData[1].magSize.ToString();             
            }
            else
            {
                if (weaponsGO[2].activeSelf)                
                    bulletsText.text = _gunData[2].currentAmmo.ToString() + "/" + _gunData[2].magSize.ToString();              
            }
        }
    }

    void ResetTutorialData()
    {
        _tutorialData.isMovingPlayerAvailable = false;
        _tutorialData.isMovingCameraAvailable = false;
        _tutorialData.isShootingAvailable = false;
        _tutorialData.isReloadingAvailable = false;
        _tutorialData.isPausingAvailable = false;
        _tutorialData.isChanginWeaponsAvailable = false;

        _tutorialData.finishedMovingPlayer = false;
        _tutorialData.finishedMovingCamera = false;
        _tutorialData.finishedShooting = false;
        _tutorialData.finishedReloading = false;
        _tutorialData.finishedPausing = false;

        bulletsText.enabled = false;
    }
}
