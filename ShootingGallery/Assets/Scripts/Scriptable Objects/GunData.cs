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

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    [HideInInspector] public bool reloading;

    [Header("Store")]
    public int cost;
}
