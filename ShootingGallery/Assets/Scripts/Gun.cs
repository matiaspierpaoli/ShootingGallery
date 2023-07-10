using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Object type References")]
    [SerializeField] private GunData _gunData;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private BulletController bulletController;

    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AK.Wwise.Event shootSFX;
    [SerializeField] private AK.Wwise.Event reloadSFX;

    [Header("Shooting")]
    public InputActionReference holdShootingActionReference;
    private Coroutine shootingCoroutine;

    public static event System.Action ReloadStartedEvent;
    public static event System.Action ReloadFinishedEvent;
    public static event System.Action ShootingStartedEvent;

    private bool canShoot = true;

    private void Awake()
    {
        ResetWeaponStats();
    }

    private void Update()
    {
        if (!_gunData.availiable)
        {
            gameObject.SetActive(false);
            Debug.Log(_gunData.name + " deactivated");
        }
    }

    private void OnEnable()
    {
        holdShootingActionReference.action.started += OnHoldStarted;
        holdShootingActionReference.action.canceled += OnHoldCanceled;
    }

    private void OnDisable()
    {
        holdShootingActionReference.action.started -= OnHoldStarted;
        holdShootingActionReference.action.canceled -= OnHoldCanceled;
    }

    #region Shooting
    private void OnHoldStarted(InputAction.CallbackContext context)
    {
        if (_gunData.currentAmmo != 0 && !_gunData.reloading && canShoot)
        {
            if (shootingCoroutine == null)
            {
                _gunData.holdingShootingAction = true;
                shootingCoroutine = StartCoroutine(ShootCoroutine());
            }
        }
    }

    private void OnHoldCanceled(InputAction.CallbackContext context)
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
            StartCoroutine(ShootCooldown());
        }

        _gunData.holdingShootingAction = false;
    }

    public IEnumerator ShootCoroutine()
    {
        while (_gunData.currentAmmo > 0)
        {
            if (_gunData.isInstanceType)
                bulletController.CreateBullet(firePoint);

            if (_gunData.isRaycastType)
                RaycastShoot();

            _gunData.currentAmmo--;
            audioManager.PlaySoundEvent(shootSFX, gameObject);
            yield return StartRecoil(_gunData.shootCooldown);
        }
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(_gunData.shootCooldown);
        canShoot = true;
    }

    private bool RaycastShoot()
    {
        RaycastHit hit;

        Vector3 shootDir = GetShotDir();

        if (Physics.Raycast(_cameraTransform.position, shootDir, out hit, _gunData.maxDistance))
        {
            Button button = hit.collider.GetComponent<Button>();

            if (button != null)
            {
                button.onClick.Invoke();
                Debug.Log("Button hit with raycast shot");
            }

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(_gunData.damage);
                Debug.Log("Target hit with raycast shot");
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 GetShotDir()
    {
        Vector3 targetPos = _cameraTransform.position + _cameraTransform.forward * _gunData.maxDistance;

        Vector3 dir = targetPos - _cameraTransform.position;

        return dir.normalized;
    }
    #endregion

    #region Recoil

    IEnumerator StartRecoil(float firePeriod)
    {
        ShootingStartedEvent?.Invoke();
        yield return new WaitForSeconds(firePeriod);
        
    }

    #endregion

    public void OnReload(InputValue context)
    {
        if (!_gunData.reloading && _gunData.currentAmmo < _gunData.magSize)
        {
            if (shootingCoroutine != null)
                StopCoroutine(shootingCoroutine);
            StartCoroutine(Reload());
            audioManager.PlaySoundEvent(reloadSFX, gameObject);
        }
    }

    private IEnumerator Reload()
    {
        Debug.Log(_gunData.name + " started realoading");
        _gunData.reloading = true;
        ReloadStartedEvent?.Invoke();
        yield return new WaitForSeconds(_gunData.reloadTime);

        Debug.Log(_gunData.name + " finished realoading");

        _gunData.currentAmmo = _gunData.magSize;
        _gunData.reloading = false;
        ReloadFinishedEvent?.Invoke();
    }

    private void ResetWeaponStats()
    {
        _gunData.currentAmmo = _gunData.magSize;
        _gunData.reloading = false;
    }
}
