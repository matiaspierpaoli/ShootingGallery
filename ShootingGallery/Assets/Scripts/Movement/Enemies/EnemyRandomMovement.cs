using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  One type of enemy which moves linearly randomly, from point A to point B and backwards using a coroutine
/// </summary>
public class EnemyRandomMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> points = new List<Transform>();
    [SerializeField] private float speed = 1f;

    private Transform goalPoint;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToPoint(GetNewGoalPoint().position));
        }
    }

    private IEnumerator MoveToPoint(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private Transform GetNewGoalPoint()
    {
        List<Transform> newPointsList = new List<Transform>(points);
        newPointsList.Remove(goalPoint);

        int index = Random.Range(0, newPointsList.Count);
        goalPoint = newPointsList[index];
        return goalPoint;
    }
}
