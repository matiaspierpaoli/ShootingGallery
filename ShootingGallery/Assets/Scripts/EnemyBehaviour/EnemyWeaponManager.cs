using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject pistolPrefab;
    [SerializeField] private GameObject akPrefab;
    [SerializeField] private GameObject sniperPrefab;

    [SerializeField] private Transform pistolSpawn;
    [SerializeField] private Transform akSpawn;
    [SerializeField] private Transform sniperSpawn;

    [SerializeField] private Transform weaponHolderTransform;

    [SerializeField] private GameData gameData;

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
        DifficultyProbabilities probabilities;

        switch (gameData.difficulty)
        {
            case DifficultyLevel.Easy:
                probabilities = gameData.weaponProbabilities.easyProbabilities;
                break;
            case DifficultyLevel.Medium:
                probabilities = gameData.weaponProbabilities.mediumProbabilities;
                break;
            case DifficultyLevel.Hard:
                probabilities = gameData.weaponProbabilities.hardProbabilities;
                break;
            default:
                probabilities = gameData.weaponProbabilities.easyProbabilities; 
                break;
        }

        int totalProbability = probabilities.pistolProbability + probabilities.akProbability + probabilities.sniperProbability;
        int randomValue = Random.Range(1, totalProbability + 1);

        if (randomValue <= probabilities.pistolProbability)
        {
            return pistolPrefab;
        }
        else if (randomValue <= probabilities.pistolProbability + probabilities.akProbability)
        {
            return akPrefab;
        }
        else
        {
            return sniperPrefab;
        }
    }
}
