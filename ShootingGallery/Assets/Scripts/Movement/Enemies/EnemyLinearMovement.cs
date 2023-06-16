using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Documentation - Add summary
public class EnemyLinearMovement : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float timeToReachPos;

    //TODO: Fix - Unclear name
    private bool isMoving = true;

    //TODO: Fix - Could be a coroutine.
    private float internalTimer = 0;

    private void Update()
    {
        if (isMoving)
            internalTimer += 1 / timeToReachPos * Time.deltaTime;
        else
            internalTimer -= 1 / timeToReachPos * Time.deltaTime;

        transform.position = Vector3.Lerp(pointA.position, pointB.position, internalTimer);

        //BUG: Comparing floats (vector3 = 3 floats) could generate problems. Please use small threshold.
        if (transform.position == pointA.position || transform.position == pointB.position)
            isMoving = !isMoving;
    }
}
