using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    private void Start()
    {
        _playerData.points = 0;
    }

    public void AddPoints(int newPoints)
    {
        _playerData.points += newPoints;
    }
}
