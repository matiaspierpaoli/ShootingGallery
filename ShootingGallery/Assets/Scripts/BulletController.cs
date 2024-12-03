using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] Rigidbody rb;
    [SerializeField] private float normalDamage;
    [SerializeField] private string playerBulletTag;
    [SerializeField] private string enemyBulletTag;

    [SerializeField] private GameObject bulletPrefab;

    [Header("Body parts tags")]
    [SerializeField] private const string normalTag = "lowerBody";
    [SerializeField] private const string chestTag = "chest";
    [SerializeField] private const string headTag = "head";

    public void Fire(Vector3 direction)
    {
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    public void CreateBullet(Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Fire(firePoint.forward);
    }

    public void CreateBullet(Transform firePoint, Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Fire(direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag(playerBulletTag))
        {
            if (collision.collider.TryGetComponent(out Button button))
            {
                button.onClick.Invoke();

                Debug.Log("Button hit with bullet rigidbody");
            }
            else if (collision.collider.TryGetComponent(out IDamageable damageable) && collision.collider.TryGetComponent(out IEnemy enemy))
            {
                WeaponSwitching weaponActive = FindObjectOfType<WeaponSwitching>();
                Weapon[] weapons = weaponActive.gameObject.GetComponentsInChildren<Weapon>();
                GunData gunActive = new GunData();

                foreach (Weapon weapon in weapons)
                {
                    if (weapon.gameObject.activeSelf)
                    {
                        gunActive = weapon.gunData;
                        break;
                    }
                }

                switch (collision.gameObject.tag)
                {
                    case normalTag:
                        damageable.Damage(gunActive.normalDamage);
                        break;
                    case chestTag:
                        damageable.Damage(gunActive.chestDamage);
                        break;
                    case headTag:
                        damageable.Damage(gunActive.headDamage);
                        break;
                    default:
                        break;
                }

                Debug.Log("Enemy hit with bullet rigidbody");
            }

        }
        else if (gameObject.CompareTag(enemyBulletTag))
        {
            if (collision.collider.TryGetComponent(out IDamageable damageable) && collision.collider.TryGetComponent(out IPlayer player))
            {
                damageable.Damage(normalDamage);

                Debug.Log("Enemy hit with bullet rigidbody");
            }
        }
        Destroy(gameObject);
    }
}