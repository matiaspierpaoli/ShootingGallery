using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameData _gameData;

    [SerializeField] private GunData _gunData;
    [SerializeField] private Transform _cameraTransform;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;


    public InputActionReference holdShootingActionReference;

    private Coroutine shootingCoroutine;

    private void Awake()
    {
        ResetWeaponStats();
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
        if (_gunData.currentAmmo != 0)
        {
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootCoroutiune());
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
            if (GameObject.FindGameObjectWithTag("InstanceBulletGun"))
            {
                if (_gunData.currentAmmo > 0)
                    CreateBullet();
            }
            else if (GameObject.FindGameObjectWithTag("RaycastGun"))
            {
                if (RaycastShoot())
                {
                    //Debug.Log("Raycast hit");

                }
            }

            _gunData.currentAmmo--;
            if (_gunData.currentAmmo <= 0)
                _gunData.currentAmmo = 0;

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
                Debug.Log("Button hit");
            }

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(_gunData.damage, hit.point, hit.normal);
                Debug.Log("Target hit");
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
   
    private void CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Fire(transform.forward);
    }
    #endregion

    public void OnReload(InputValue context)
    {
        Debug.Log("Reload started");
        StartReload();
    }

    public void StartReload()
    {
        if (!_gunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        _gunData.reloading = true;
        yield return new WaitForSeconds(_gunData.reloadTime);

        Debug.Log("Reload finished");
        _gunData.currentAmmo = _gunData.magSize;
        _gunData.reloading = false;
    }

    private void ResetWeaponStats()
    {
        _gunData.currentAmmo = _gunData.magSize;
        _gunData.reloading = false;

        if (_gameData.practiceArea)
            ActivateAllWeapons();
        else
            ActivateOnlyFirstWeapon();
    }

    private void ActivateOnlyFirstWeapon()
    {
        if (_gunData.name == "Asault Rifle" || _gunData.name == "Sniper Rifle")
            _gunData.availiable = false;
        else
            _gunData.availiable = true;
    }

    private void ActivateAllWeapons()
    {
        _gunData.availiable = true;
    }

}
