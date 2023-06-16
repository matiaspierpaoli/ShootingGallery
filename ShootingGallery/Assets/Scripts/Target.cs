using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    //TODO: TP2 - Syntax - Consistency in naming convention
    [SerializeField] private PlayerStatsController _player;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float maxHealth;
    //TODO: TP2 - Syntax - Consistency in naming convention
    private float currentHealth = 100f;
    private float timeToRespawn = 5f;

    public void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            _player.AddPoints(1);
            _gameManager.AddOneEnemyDefeated();

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
