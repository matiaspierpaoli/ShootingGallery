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
        //TODO: Fix - collider.TryGetComponent
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();

        if (damageable != null)
        {           
            damageable.Damage(damage);
           
            Debug.Log("Target hit with bullet rigidbody");
        }

        //TODO: Fix - collider.TryGetComponent
        Button otherButton = collision.gameObject.GetComponent<Button>();

        if (otherButton != null)
        {
            otherButton.onClick.Invoke();
            
            Debug.Log("Button hit with bullet rigidbody");

        }

        Destroy(gameObject);
    }
}
