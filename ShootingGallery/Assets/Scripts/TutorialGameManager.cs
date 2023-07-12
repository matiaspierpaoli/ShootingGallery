using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialGameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponsGO;
    [SerializeField] private GunData[] _gunData;

    [SerializeField] private TutorialData _tutorialData;

    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private PlayerLookController _playerLook;

    private int currentStepIndex;
    [SerializeField] private TMP_Text[] stepText;

    [SerializeField] private TMP_Text bulletsText;

    private float currentTutorialTimePractice = 0f;
    [SerializeField] private float maxTutorialTimePractice;

    private bool isPaused = false;

    private void OnEnable()
    {
        InputManager.MoveEvent += OnMove;
        InputManager.PauseEvent += OnPause;
        Gun.ReloadFinishedEvent += SetNewAmmoText;
        Gun.ShootingStartedEvent += SetNewAmmoText;
        Gun.ReloadFinishedEvent += OnReload;
        Gun.ShootingStartedEvent += OnShoot;
        Pause.FinishedPause += OnUnPause;
    }

    private void OnDisable()
    {
        InputManager.MoveEvent -= OnMove;
        InputManager.PauseEvent -= OnPause;
        Gun.ReloadFinishedEvent -= OnReload;
        Gun.ShootingStartedEvent -= OnShoot;
        Gun.ReloadFinishedEvent -= SetNewAmmoText;
        Gun.ShootingStartedEvent -= SetNewAmmoText;
        Pause.FinishedPause -= OnUnPause;
    }

    private void Start()
    {
        ResetTutorialData();

        _tutorialData.isMovingPlayerAvailable = true;

        DisplayNextStepText();

        for (int i = 0; i < _gunData.Length; i++)
        {
            _gunData[i].availiable = true;
        }
    }

    private void OnMove(Vector2 movementInput)
    {
        if (_tutorialData.isMovingPlayerAvailable)
        {
            _characterMovement.SetMovementDir(movementInput);

            if (!_tutorialData.finishedMovingPlayer)
            {
                currentTutorialTimePractice++;

                Debug.Log("Correctly Moved Player");

                if (currentTutorialTimePractice >= maxTutorialTimePractice)
                {
                    _tutorialData.isShootingAvailable = true;
                    _tutorialData.finishedMovingPlayer = true;

                    currentTutorialTimePractice = 0f;

                    for (int i = 0; i < weaponsGO.Length; i++)
                    {
                        if (i == 0)
                            weaponsGO[i].SetActive(true);
                        else
                            weaponsGO[i].SetActive(false);
                    }

                    DisplayNextStepText();
                }
            }
        }
    }

    private void OnShoot()
    {
        if (_tutorialData.isShootingAvailable)
        {
            if (!_tutorialData.finishedShooting)
            {
                currentTutorialTimePractice++;
                Debug.Log("Correctly Shooting");

                if (currentTutorialTimePractice >= 1)
                {
                    _tutorialData.isReloadingAvailable = true;
                    _tutorialData.finishedShooting = true;

                    currentTutorialTimePractice = 0f;

                    DisplayNextStepText();
                    
                }
            }
        }
    }

    private void OnReload()
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

                    DisplayNextStepText();
                }
            }
        }
    }

    private void OnPause()
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

                    DisplayNextStepText();
                }
            }
            else
            {
                isPaused = true;
            }
        }
    }

    public void OnChangeWeapon1(InputValue context)
    {
        if (_tutorialData.isChanginWeaponsAvailable && !isPaused)
        {
            Debug.Log("Correctly Changed Weapon to 1");

            for (int i = 0; i < weaponsGO.Length; i++)
            {
                if (i == 0)
                    weaponsGO[i].SetActive(true);
                else
                    weaponsGO[i].SetActive(false);
            }

            DisplayNextStepText();
            SetNewAmmoText();
        }
    }

    public void OnChangeWeapon2(InputValue context)
    {
        if (_tutorialData.isChanginWeaponsAvailable && !isPaused)
        {
            Debug.Log("Correctly Changed Weapon to 2");

            for (int i = 0; i < weaponsGO.Length; i++)
            {
                if (i == 1)
                    weaponsGO[i].SetActive(true);
                else
                    weaponsGO[i].SetActive(false);
            }

            DisplayNextStepText();
            SetNewAmmoText();
        }
    }

    public void OnChangeWeapon3(InputValue context)
    {
        if (_tutorialData.isChanginWeaponsAvailable && !isPaused)
        {
            Debug.Log("Correctly Changed Weapon to 3");

            for (int i = 0; i < weaponsGO.Length; i++)
            {
                if (i == 2)
                    weaponsGO[i].SetActive(true);
                else
                    weaponsGO[i].SetActive(false);
            }

            DisplayNextStepText();
            SetNewAmmoText();
        }
    }

    public void SetNewAmmoText()
    {
        for (int i = 0; i < weaponsGO.Length; i++)
        {
            if (weaponsGO[i].activeSelf)
                bulletsText.text = _gunData[i].currentAmmo.ToString() + "/" + _gunData[i].magSize.ToString();
        }
    }

    private void ResetTutorialData()
    {
        currentStepIndex = 0;

        _tutorialData.isMovingPlayerAvailable = false;
        _tutorialData.isShootingAvailable = false;
        _tutorialData.isReloadingAvailable = false;
        _tutorialData.isPausingAvailable = false;
        _tutorialData.isChanginWeaponsAvailable = false;

        _tutorialData.finishedMovingPlayer = false;
        _tutorialData.finishedShooting = false;
        _tutorialData.finishedReloading = false;
        _tutorialData.finishedPausing = false;

        bulletsText.enabled = false;
    }

    private void DisplayNextStepText()
    {
        for (int i = 0; i < stepText.Length; i++)
        {
            if (currentStepIndex < stepText.Length) // If the number of steps index reached the limit, keep the last text in the array alive
            {
                if (i == currentStepIndex)
                    stepText[i].enabled = true;
                else
                    stepText[i].enabled = false;
            }
        }

        currentStepIndex++;
    }

    private void OnUnPause()
    {
        isPaused = false;
    }
}
