using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    private GameObject _player;
    private GameObject _enemySpawner;
    private float health = 100f;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner");
    }

    public void Damage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        health -= damage;
        if (health <= 0)
        {
            _player.GetComponent<PlayerStatsController>().AddPoints(1);
            _enemySpawner.GetComponent<EnemySpawner>().EnemyDestroyed(gameObject);           
        }
    }
}
