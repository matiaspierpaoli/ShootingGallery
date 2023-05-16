using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialGameManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private TutorialUIManager _tutorialUIManager;
    [SerializeField] private PauseScript _pauseManager;
    [SerializeField] private GameObject[] weaponsGO;
    [SerializeField] private GunData[] _gunData; 
    [SerializeField] private PlayerData player;

    [SerializeField] private GameObject enemyHorizontalMovement;
    [SerializeField] private GameObject enemyVerticalMovement;
    [SerializeField] private GameObject enemyFBMovement;
    [SerializeField] private GameObject enemyRandomMovement;

    [SerializeField] private TutorialData _tutorialData;

    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private PlayerLook _playerLook;

    [SerializeField] private SceneLoader _sceneLoader;

    [SerializeField] private TMP_Text nextStepText;
    [SerializeField] private TMP_Text bulletsText;

    private float currentTutorialTimePractice = 0f;
    [SerializeField] private float maxTutorialTimePractice;

    private void Start()
    {
        //_pauseManager.FreezeTime();
        ResetTutorialData();

        _tutorialData.isMovingPlayerAvailable = true;

        nextStepText.text = "Move Player with WASD";

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
        GetCurrentAmmoText();
    }

    public void OnMove(InputValue context)
    {
        if (_tutorialData.isMovingPlayerAvailable)
        {
            var movementInput = context.Get<Vector2>();
            _characterMovement.ProcessMove(movementInput);

            if (!_tutorialData.finishedMovingPlayer)
            {
                currentTutorialTimePractice ++;

                Debug.Log("Correctly Moved Player");

                if (currentTutorialTimePractice >= maxTutorialTimePractice)
                {                
                    _tutorialData.isMovingCameraAvailable = true;
                    _tutorialData.finishedMovingPlayer = true;

                    currentTutorialTimePractice = 0f;

                    nextStepText.text = "Next Step: Move Camera with mouse";
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
                    //_tutorialData.finishedMovingCamera = true;

                    _tutorialData.isShootingAvailable = true;
                    _tutorialData.finishedMovingCamera = true;

                    currentTutorialTimePractice = 0f;

                    nextStepText.text = "Next Step: Shoot with left-ckick";

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

                    nextStepText.text = "Next Step: Reload with R";
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

                    nextStepText.text = "Next Step: Pause with P";
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

                    nextStepText.text = "Next Step: Change weapons with 1,2 or 3";
                }
            }
            else
                _pauseManager.Pause();
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

        _tutorialData.finishedKillingHorEnemy = false;
        _tutorialData.finishedKillingVerEnemy = false;
        _tutorialData.finishedKillingForEnemy = false;
        _tutorialData.finishedKillingRanEnemy = false;

        bulletsText.enabled = false;
    }


}