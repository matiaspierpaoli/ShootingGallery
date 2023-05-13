using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyRandomMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> points = new List<Transform>();
    [SerializeField] private float timeToReachPos;
    
    private Transform activePoint;
    private Transform goalPoint;

    float aux = 0f;

    private void Awake()
    {
        activePoint = GetRandomSpawnPoint();

        goalPoint = points[0];

        UpdateNewGoalPoint();
    }

    private Transform GetRandomSpawnPoint()
    {
        Transform spawnPoint = points[UnityEngine.Random.Range(0, points.Count)];

        return spawnPoint;
    }

    private void Update()
    {
        aux += Time.deltaTime;

        transform.position = Vector3.Lerp(activePoint.position, goalPoint.position, aux / timeToReachPos);

        if (aux > timeToReachPos)
        {
            aux = 0;
            UpdateNewGoalPoint();
        }
    }


    private void UpdateNewGoalPoint()
    {
        List<Transform> newPointsList = new List<Transform>(points);
        newPointsList.Remove(goalPoint);

        int index = Random.Range(0, newPointsList.Count);
        activePoint = goalPoint;
        goalPoint = newPointsList[index];
    }
}
