using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
public class GameData : ScriptableObject
{
    public bool tutorial;
    public float currentTime;
    public float currentEnemiesDefeated;
    public float maxTime;
    public float maxEnemiesToDefeat;
    public bool victory;
    public bool defeat;

}
