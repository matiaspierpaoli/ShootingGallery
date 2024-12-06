using System.Collections;
using UnityEngine;

/// <summary>
///  One type of enemy which moves linearly, from point A to point B and backwards using a coroutine.
///  If the gameObject is turned off, the coroutine also stopts 
/// </summary>
public class EnemyLinearMovement : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 1f;

    private bool isMovingToPointA = true;
    private bool isCoroutineRunning = false;

    private void Start()
    {
        StartMovementCoroutine();
    }

    private void OnEnable()
    {
        if (!isCoroutineRunning)
            StartMovementCoroutine();
    }

    private void OnDisable()
    {
        StopMovementCoroutine();
    }

    private void StartMovementCoroutine()
    {
        if (isCoroutineRunning)
            return;

        StartCoroutine(MovementCoroutine());
        isCoroutineRunning = true;
    }

    private void StopMovementCoroutine()
    {
        if (!isCoroutineRunning)
            return;

        StopAllCoroutines();
        isCoroutineRunning = false;
    }

    private IEnumerator MovementCoroutine()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToPoint(isMovingToPointA ? pointA.position : pointB.position));
            isMovingToPointA = !isMovingToPointA;
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
}
