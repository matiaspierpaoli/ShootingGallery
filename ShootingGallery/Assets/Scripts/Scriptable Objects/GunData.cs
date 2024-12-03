using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header ("Info")]
    public new string name;
    public bool availiable;
    public bool isInstanceType;
    public bool isRaycastType;
    public bool isPlayerControlled;

    [Header("Shooting")]
    public float normalDamage;
    public float chestDamage;
    public float headDamage;
    public float maxDistance;
    public bool holdingShootingAction;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float shootCooldown;
    public float reloadTime;
    [HideInInspector] public bool reloading;

    [Header("Store")]
    public int cost;
}
