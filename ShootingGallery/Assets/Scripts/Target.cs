using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStatsController _player;
    private float health = 100f;

    public void Damage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        health -= damage;
        if (health <= 0)
        {
            _player.AddPoints(1);
            Destroy(gameObject);
        }
    }
}
