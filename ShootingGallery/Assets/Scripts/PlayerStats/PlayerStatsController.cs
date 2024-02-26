using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour, IPointsProvider, IDamageable
{
    [SerializeField] private PlayerStats _playerStats;

    private void Start()
    {
        _playerStats.points = 0;
    }

    public void AddPoints(int newPoints)
    {
        _playerStats.points += newPoints;
    }

    public void Damage(float damage)
    {
        Debug.Log("Player Took Damage");
        _playerStats.health -= damage;

        if (_playerStats.health <= 0)
        {
            _playerStats.health = 0;
            Debug.Log("Player Died");
        }
    }
}
