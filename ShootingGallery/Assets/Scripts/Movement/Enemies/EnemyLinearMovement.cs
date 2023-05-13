using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLinearMovement : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float timeToReachPos;

    private bool isMoving = true;

    private float internalTimer = 0;

    private void Update()
    {
        if (isMoving)
            internalTimer += 1 / timeToReachPos * Time.deltaTime;
        else
            internalTimer -= 1 / timeToReachPos * Time.deltaTime;

        transform.position = Vector3.Lerp(pointA.position, pointB.position, internalTimer);

        if (transform.position == pointA.position || transform.position == pointB.position)
            isMoving = !isMoving;
    }
}
