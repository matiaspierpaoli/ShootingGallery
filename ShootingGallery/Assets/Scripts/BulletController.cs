using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] Rigidbody rb;
    [SerializeField] private float damage;
    [SerializeField] private string playerBulletTag;
    [SerializeField] private string enemyBulletTag;

    [SerializeField] private GameObject bulletPrefab;

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
                damageable.Damage(damage); 

                Debug.Log("Enemy hit with bullet rigidbody");
            }

        }
        else if (gameObject.CompareTag(enemyBulletTag))
        {
            if (collision.collider.TryGetComponent(out IDamageable damageable) && collision.collider.TryGetComponent(out IPlayer player))
            {
                damageable.Damage(damage);

                Debug.Log("Enemy hit with bullet rigidbody");
            }
        }
        Destroy(gameObject);
    }
}