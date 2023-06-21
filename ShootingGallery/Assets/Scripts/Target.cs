using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStatsController player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float maxHealth;

    private float currentHealth = 100f;
    private float timeToRespawn = 5f;

    public void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            player.AddPoints(1);
            gameManager.AddOneEnemyDefeated();

            gameObject.SetActive(false);
            //TODO: Fix - Bad log/Log out of context
            Debug.Log("Enemy eliminated");
            Invoke("Respawn", timeToRespawn);
        }
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        //TODO: Fix - Bad log/Log out of context
        Debug.Log("Enemy respawned");
        currentHealth = maxHealth;
    }
}
