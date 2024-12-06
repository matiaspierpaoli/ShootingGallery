using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Config")]
    public GunData gunData;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private BulletController bulletController;
    [SerializeField] private ParticleSystem muzzleFlashParticleSystem;

    [Header("Body parts tags")]
    [SerializeField] private const string normalTag = "lowerBody";
    [SerializeField] private const string chestTag = "chest";
    [SerializeField] private const string headTag = "head";
    [SerializeField] private const string playerTag = "Player";

    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AK.Wwise.Event shootSFX;
    [SerializeField] private AK.Wwise.Event reloadSFX;

    [Header("Shooting")]
    public InputActionReference holdShootingActionReference;
    [SerializeField] private LayerMask playerRaycastLayer;
    [SerializeField] private LayerMask enemiesRaycastLayer;

    public event System.Action ReloadStartedEvent;
    public event System.Action ReloadFinishedEvent;
    public event System.Action ShootingStartedEvent;

    private Coroutine shootingCoroutine;
    private Coroutine reloadingCoroutine;
    public bool canShoot = true;

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
        newGunData.normalDamage = gunData.normalDamage;
        newGunData.chestDamage = gunData.chestDamage;
        newGunData.headDamage = gunData.headDamage;
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
        StartShooting();
    }

    private void OnHoldCanceled(InputAction.CallbackContext context)
    {
        StopShooting();
    }

    public void StartShooting()
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

    public void StopShooting()
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
                StartCoroutine(Reload());
            }
            return;
        }

        muzzleFlashParticleSystem.Play();

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
        LayerMask newRaycastLayers;
        newRaycastLayers = gunData.isPlayerControlled ?  enemiesRaycastLayer : playerRaycastLayer;

        RaycastHit hit;
        Vector3 shootDir = GetShotDir(transform);
        if (Physics.Raycast(transform.position, shootDir, out hit, gunData.maxDistance, newRaycastLayers))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable == null) damageable = hit.collider.GetComponentInParent<IDamageable>();
            IPlayer player = hit.collider.GetComponent<IPlayer>();

            if (gunData.isPlayerControlled)
            {
                Button button = hit.collider.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                    Debug.Log("Button hit with raycast shot");
                    Debug.DrawRay(transform.position, shootDir * gunData.maxDistance, Color.green, 0.1f);
                }                              
                else if (damageable != null)
                {
                    switch (hit.collider.gameObject.tag)
                    {
                        case normalTag:
                            damageable.Damage(gunData.normalDamage);
                            break;
                        case chestTag:
                            damageable.Damage(gunData.chestDamage);
                            break;
                        case headTag:
                            damageable.Damage(gunData.headDamage);
                            break;
                        default:
                            break;
                    }
                    Debug.DrawRay(transform.position, shootDir * gunData.maxDistance, Color.green, 0.1f);
                    Debug.Log("Enemy hit with raycast shot");
                }
            }
            else
            {
                if (player != null && damageable != null)
                {
                    damageable.Damage(gunData.normalDamage);
                    Debug.Log("Player hit with raycast shot");
                    Debug.DrawRay(transform.position, shootDir * gunData.maxDistance, Color.green, 0.1f);
                }
                else
                {
                    Debug.DrawRay(transform.position, shootDir * gunData.maxDistance, Color.red, 0.1f);
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, shootDir * gunData.maxDistance, Color.red, 0.1f);
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

    public void StartReload()
    {
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        if (gameObject.GetComponent<AnimationController>() != null)
            gameObject.GetComponent<AnimationController>().OnReloadStarted();
        Debug.Log(gunData.name + " started reloading");
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
