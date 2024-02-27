using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float points;
    public float health;
    public float maxHealth;
    public float lastEliminationTime;
    public bool isInmortal;
}
