using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] Rigidbody rb;
    //private float traveledDistance;

    public void Fire(Vector3 direction)
    {
        //rb.velocity = transform.forward * speed;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    //private void Update()
    //{
    //    traveledDistance += Time.deltaTime * speed;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Button otherButton = collision.gameObject.GetComponent<Button>();

        if (otherButton != null)
        {
            otherButton.onClick.Invoke();
            Debug.Log("Button hit");

        }

        Destroy(gameObject);
    }
}
