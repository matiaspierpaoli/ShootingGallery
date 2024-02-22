using System.Collections;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public GunData gunData;
    [SerializeField] Transform firePoint;
    [SerializeField] private BulletController bulletController;
    [SerializeField] private bool isInstanceType;

    private Coroutine reloadingCoroutine;

    private void OnDisable()
    {
        if (reloadingCoroutine != null)
        {
            StopCoroutine(reloadingCoroutine);
            reloadingCoroutine = null;
        }
    }

    private void Start()
    {
        gunData = InstantiateGunData();
        gunData.currentAmmo = gunData.magSize;
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

        newGunData.isInstanceType = isInstanceType ? true : false;
        newGunData.isRaycastType = !isInstanceType;

        return newGunData;
    }

    public void Shoot()
    {
        if (gunData.currentAmmo > 0 && !gunData.reloading)
        {
            gunData.currentAmmo--;

            Debug.Log("Enemy shot!");

            if (gunData.isInstanceType)
                bulletController.CreateBullet(firePoint);

            if (gunData.currentAmmo == 0)
            {
                reloadingCoroutine = StartCoroutine(Reload());
            }
        }
        else if (!gunData.reloading)
        {
            Reload();
        }
    }

    private IEnumerator Reload()
    {
        Debug.Log("Enemy started realoading");
        gunData.reloading = true;       
        yield return new WaitForSeconds(gunData.reloadTime);

        Debug.Log("Enemy finished realoading");

        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }
}