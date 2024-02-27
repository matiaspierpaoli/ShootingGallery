
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Object type References")]
    public GunData gunData;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private BulletController bulletController;

    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AK.Wwise.Event shootSFX;
    [SerializeField] private AK.Wwise.Event reloadSFX;

    [Header("Shooting")]
    public InputActionReference holdShootingActionReference;

    public static event System.Action ReloadStartedEvent;
    public static event System.Action ReloadFinishedEvent;
    public static event System.Action ShootingStartedEvent;

    private Coroutine shootingCoroutine;
    private Coroutine reloadingCoroutine;
    private bool canShoot = true;

    private void Awake()
    {
        ResetWeaponStats();
    }

    private void OnEnable()
    {
        if (gunData.isPlayerControlled)
        {
            holdShootingActionReference.action.started += OnHoldStarted;
            holdShootingActionReference.action.canceled += OnHoldCanceled;
        }

        canShoot = true;
        gunData.reloading = false;

        GameManager.ReplayEvent += ResetWeaponStats;
    }

    private void OnDisable()
    {
        if (gunData.isPlayerControlled)
        {
            holdShootingActionReference.action.started -= OnHoldStarted;
            holdShootingActionReference.action.canceled -= OnHoldCanceled;
        }

        if (reloadingCoroutine != null)
        {
            StopCoroutine(reloadingCoroutine);
            reloadingCoroutine = null;
        }

        GameManager.ReplayEvent += ResetWeaponStats;
    }

    private void Start()
    {
        if (!gunData.isPlayerControlled)
            gunData = InstantiateGunData();
        gunData.currentAmmo = gunData.magSize;

        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (!gunData.availiable && gunData.isPlayerControlled)
        {
            gameObject.SetActive(false);
            Debug.Log(gunData.name + " deactivated");
        }
    }

    private GunData InstantiateGunData()
    {
        GunData newGunData = ScriptableObject.CreateInstance<GunData>();

        newGunData.name = gunData.name;
        newGunData.damage = gunData.damage;
        newGunData.maxDistance = gunData.maxDistance;
        newGunData.shootCooldown = gunData.shootCooldown;
        newGunData.reloadTime = gunData.reloadTime;
        newGunData.magSize = gunData.magSize;
        newGunData.isInstanceType = gunData.isInstanceType;
        newGunData.isRaycastType = gunData.isRaycastType;
        newGunData.isPlayerControlled = gunData.isPlayerControlled;
        newGunData.availiable = gunData.availiable;

        return newGunData;
    }

    private void OnHoldStarted(InputAction.CallbackContext context)
    {
        if (gunData.currentAmmo != 0 && !gunData.reloading && canShoot)
        {
            if (shootingCoroutine == null)
            {
                gunData.holdingShootingAction = true;
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
        gunData.holdingShootingAction = false;
    }

    private IEnumerator ShootCoroutine()
    {
        while (gunData.currentAmmo > 0)
        {
            Shoot();
            yield return StartRecoil(gunData.shootCooldown);
        }
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(gunData.shootCooldown);
        canShoot = true;
    }

    public void Shoot(PlayerStatsController psc = null, Vector3? missDirection = null)
    {
        if (gunData.currentAmmo <= 0 || gunData.reloading)
        {
            if (!gunData.reloading)
            {
                Reload();
            }
            return;
        }

        gunData.currentAmmo--;
        Debug.Log("Shot fired!");

        if (gunData.isInstanceType)
        {
            if (missDirection != null)
            {
                bulletController.CreateBullet(firePoint, missDirection.Value);
            }
            else
            {
                bulletController.CreateBullet(firePoint);
            }
        }
        else if (gunData.isRaycastType)
        {
            if (gunData.isPlayerControlled)
            {
                RaycastShoot(cameraTransform);
            }
            else
            {
                RaycastShoot(firePoint);
            }
        }

        audioManager.PlaySoundEvent(shootSFX, gameObject);

        if (gunData.currentAmmo == 0)
        {
            reloadingCoroutine = StartCoroutine(Reload());
        }
    }

    private void RaycastShoot(Transform transform)
    {
        RaycastHit hit;
        Vector3 shootDir = GetShotDir(transform);
        if (Physics.Raycast(transform.position, shootDir, out hit, gunData.maxDistance))
        {
            Debug.DrawRay(transform.position, shootDir * gunData.maxDistance, Color.red, 0.1f);
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            IPlayer player = hit.collider.GetComponent<IPlayer>();
            IEnemy enemy = hit.collider.GetComponent<IEnemy>();

            if (gunData.isPlayerControlled)
            {
                Button button = hit.collider.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                    Debug.Log("Button hit with raycast shot");
                }                              
                else if (enemy != null && damageable != null)
                {
                    damageable.Damage(gunData.damage);
                    Debug.Log("Enemy hit with raycast shot");
                }
            }
            else
            {
                if (player != null && damageable != null)
                {
                    damageable.Damage(gunData.damage);
                    Debug.Log("Player hit with raycast shot");
                }
            }
        }
    }

    private Vector3 GetShotDir(Transform transform)
    {
        Vector3 targetPos = transform.position + transform.forward * gunData.maxDistance;

        Vector3 dir = targetPos - transform.position;

        return dir.normalized;
    }

    IEnumerator StartRecoil(float firePeriod)
    {
        if (gunData.isPlayerControlled)
            ShootingStartedEvent?.Invoke();
        yield return new WaitForSeconds(firePeriod);
    }

    private IEnumerator Reload()
    {
        Debug.Log(gunData.name + " started reloading");
        if (gunData.isPlayerControlled)
            ReloadStartedEvent?.Invoke();
        gunData.reloading = true;
        audioManager.PlaySoundEvent(reloadSFX, gameObject);
        yield return new WaitForSeconds(gunData.reloadTime);
        Debug.Log(gunData.name + " finished reloading");

        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;

        ReloadFinishedEvent?.Invoke();
    }

    private void ResetWeaponStats()
    {
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }
}
