using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies = 3;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> objectivePoints = new List<Transform>();
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }

        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        Transform objectivePoint = GetRandomObjectivePoint();
        
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.AddComponent<EnemyLinearMovement>();

        enemy.GetComponent<EnemyLinearMovement>().SetPointA(spawnPoint);
        enemy.GetComponent<EnemyLinearMovement>().SetPointB(objectivePoint);
        enemy.GetComponent<EnemyLinearMovement>().SetTimeToReachPos(10);

        activeEnemies.Add(enemy);
    }

    private Transform GetRandomSpawnPoint()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];

        foreach (GameObject enemy in activeEnemies)
        {
            if (Vector3.Distance(spawnPoint.position, enemy.transform.position) < 2f)
            {
                return GetRandomSpawnPoint();
            }
        }

        return spawnPoint;
    }

    private Transform GetRandomObjectivePoint()
    {
        Transform objectivePoint = objectivePoints[UnityEngine.Random.Range(0, objectivePoints.Count)];

        foreach (GameObject enemy in activeEnemies)
        {
            if (Vector3.Distance(objectivePoint.position, enemy.transform.position) < 2f)
            {
                return GetRandomObjectivePoint();
            }
        }

        return objectivePoint;
    }

    public void EnemyDestroyed(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        Destroy(enemy);
        SpawnEnemy();
    }
}
