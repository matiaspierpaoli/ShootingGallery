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

    public void Fire(Vector3 direction)
    {
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(damage);

            Debug.Log("Target hit with bullet rigidbody");

        }

        if (collision.collider.TryGetComponent(out Button button))
        {
            button.onClick.Invoke();

            Debug.Log("Button hit with bullet rigidbody");
        }

        Destroy(gameObject);
    }
}
