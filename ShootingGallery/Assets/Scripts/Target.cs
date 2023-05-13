using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStatsController _player;
    [SerializeField] private float maxHealth;
    private float currentHealth = 100f;
    private float timeToRespawn = 5f;

    public void Damage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            _player.AddPoints(1);

            gameObject.SetActive(false);
            Invoke("Respawn", timeToRespawn);
        }
    }

    private void Respawn()
    {
        gameObject.SetActive(true);

        currentHealth = maxHealth;
    }
}
