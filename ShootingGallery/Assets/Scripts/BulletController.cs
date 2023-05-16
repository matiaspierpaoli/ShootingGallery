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
        //rb.velocity = transform.forward * speed;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();

        if (damageable != null)
        {           
            damageable.Damage(damage);
            Debug.Log("Target hit");
        }

        Button otherButton = collision.gameObject.GetComponent<Button>();

        if (otherButton != null)
        {
            otherButton.onClick.Invoke();
            Debug.Log("Button hit");

        }

        Destroy(gameObject);
    }
}
