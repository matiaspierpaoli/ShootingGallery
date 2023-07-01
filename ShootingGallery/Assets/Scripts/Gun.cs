using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData _gunData;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private BulletController bulletController;
    
    [SerializeField] private string raycastGunTagName;
    [SerializeField] private string instanceGunTagName;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AK.Wwise.Event shootSFX;
    [SerializeField] private AK.Wwise.Event reloadSFX;

    public InputActionReference holdShootingActionReference;

    private Coroutine shootingCoroutine;

    public static event System.Action ReloadEvent;
    public static event System.Action ShootEvent;


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
        if (_gunData.currentAmmo != 0 && !_gunData.reloading)
        {
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootCoroutiune());
                ShootEvent?.Invoke();
            }
        }
    }

    private void OnHoldCanceled(InputAction.CallbackContext context)
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    public IEnumerator ShootCoroutiune()
    {
        while (true)
        {
            if (GameObject.FindGameObjectWithTag(instanceGunTagName))
            {
                if (_gunData.currentAmmo > 0)
                    bulletController.CreateBullet(firePoint);
            }
            else if (GameObject.FindGameObjectWithTag(raycastGunTagName))
            {
                RaycastShoot();
            }

            _gunData.currentAmmo--;
            if (_gunData.currentAmmo <= 0)
                _gunData.currentAmmo = 0;

            audioManager.PlaySoundEvent(shootSFX, gameObject);

            yield return new WaitForSeconds(_gunData.fireRate);
        }
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

    public void OnReload(InputValue context)
    {
        Debug.Log(_gunData.name + " started realoading");
        StartReload();
    }

    public void StartReload()
    {
        if (!_gunData.reloading)
        {
            StartCoroutine(Reload());
            audioManager.PlaySoundEvent(reloadSFX, gameObject);
        }
    }

    private IEnumerator Reload()
    {
        _gunData.reloading = true;
        yield return new WaitForSeconds(_gunData.reloadTime);

        Debug.Log(_gunData.name + " finished realoading");

        _gunData.currentAmmo = _gunData.magSize;
        _gunData.reloading = false;


        ReloadEvent?.Invoke();
    }

    private void ResetWeaponStats()
    {
        _gunData.currentAmmo = _gunData.magSize;
        _gunData.reloading = false;
    }
}
