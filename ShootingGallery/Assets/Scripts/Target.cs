using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float maxHealth;

    private float currentHealth = 100f;
    private float timeToRespawn = 5f;

    private IPointsProvider pointsProvider;

    // Since there is a scene called tutorial where points and enemiesDefeated are not needed, check if both of these are referenced
    private bool hasPlayer;
    private bool hasGameManager;

    private void Start()
    {
        hasPlayer = player != null;
        if (hasPlayer)
        {
            pointsProvider = player.GetComponent<IPointsProvider>();
        }

        hasGameManager = gameManager != null;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if (hasPlayer && pointsProvider != null)
            {
                pointsProvider.AddPoints(1);
            }

            if (hasGameManager)
            {
                gameManager.GetComponent<GameManager>().AddOneEnemyDefeated();
                gameManager.AddOneEnemyDefeated();
            }

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
