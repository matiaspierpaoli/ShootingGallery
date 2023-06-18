using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Fix - Isn't this the same as the target class? If the only diff is the points, that should be worked through an event or external class
public class TutorialTarget : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float currentHealth = 100f;
    private float timeToRespawn = 5f;

    public void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("Enemy eliminated");
            Invoke("Respawn", timeToRespawn);
        }
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        Debug.Log("Enemy respawned");
        currentHealth = maxHealth;
    }
}
