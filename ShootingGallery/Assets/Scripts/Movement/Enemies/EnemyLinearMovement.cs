using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLinearMovement : MonoBehaviour
{
    private Transform pointA;
    private Transform pointB;
    private float timeToReachPos;

    private bool isMoving = true;

    private float internalTimer = 0;

    public void SetPointA(Transform pointA)
    {
        this.pointA = pointA;
    }

    public void SetPointB(Transform pointB)
    {
        this.pointB = pointB;
    }

    public void SetTimeToReachPos(float timeToReachPos)
    {
        this.timeToReachPos = timeToReachPos;
    }

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
