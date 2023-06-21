using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour, IPointsProvider
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
}
