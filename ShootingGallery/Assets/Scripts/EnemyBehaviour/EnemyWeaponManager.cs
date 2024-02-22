using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject pistolPrefab;
    [SerializeField] private GameObject akPrefab;
    [SerializeField] private GameObject sniperPrefab;

    [SerializeField, Range(0, 100)] private int pistolProbability = 50;
    [SerializeField, Range(0, 100)] private int akProbability = 30;
    [SerializeField, Range(0, 100)] private int sniperProbability = 20;

    [SerializeField] private Transform pistolSpawn;
    [SerializeField] private Transform akSpawn;
    [SerializeField] private Transform sniperSpawn;

    [SerializeField] private Transform weaponHolderTransform;

    private void Awake()
    {
        GameObject weaponPrefab = ChooseWeaponPrefab();
        GameObject weaponObject = Instantiate(weaponPrefab, weaponHolderTransform);

        Transform spawnTransform = GetSpawnTransform(weaponPrefab);
        if (spawnTransform != null)
        {
            weaponObject.transform.position = spawnTransform.position;
            weaponObject.transform.rotation = spawnTransform.rotation;
        }
        else
        {
            Debug.LogError("Spawn transform not found for weapon: " + weaponPrefab.name);
        }
    }

    private Transform GetSpawnTransform(GameObject weaponPrefab)
    {
        if (weaponPrefab == pistolPrefab)
        {
            return pistolSpawn;
        }
        else if (weaponPrefab == akPrefab)
        {
            return akSpawn;
        }
        else if (weaponPrefab == sniperPrefab)
        {
            return sniperSpawn;
        }
        else
        {
            Debug.LogError("Unknown weapon prefab: " + weaponPrefab.name);
            return null;
        }
    }

    private GameObject ChooseWeaponPrefab()
    {
        int randomValue = Random.Range(1, 101);

        if (randomValue <= pistolProbability)
        {
            return pistolPrefab;
        }
        else if (randomValue <= pistolProbability + akProbability)
        {
            return akPrefab;
        }
        else
        {
            return sniperPrefab;
        }
    }
}
