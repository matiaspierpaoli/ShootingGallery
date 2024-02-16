using System.Collections;
using UnityEngine;

public class SoldierEnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Transform frontRaycastPosition;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float timeAfterAnimation = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float crouchAnimationLength = 4f;
    [SerializeField] private LayerMask structureLayer;
    [SerializeField] private float crouchProbability = 0.5f;
    [SerializeField] private Animator soldierAnimationController;
    [SerializeField] private string isSoldierCrouchingAnimationBool;

    private Vector3 startingPosition;
    private EnemyWithFov enemyWithFov;

    private void Start()
    {
        startingPosition = transform.position;
        enemyWithFov = GetComponent<EnemyWithFov>();
        StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        while (true)
        {
            if (enemyWithFov.PlayerInSight)
            {
                if (transform.position == startingPosition)
                {
                    bool crouch = CheckForStructures() && Random.value < crouchProbability;

                    if (crouch)
                    {
                        Debug.Log("Crouching");
                        soldierAnimationController.SetBool(isSoldierCrouchingAnimationBool, true);
                        yield return new WaitForSeconds(crouchAnimationLength + timeAfterAnimation);
                        soldierAnimationController.SetBool(isSoldierCrouchingAnimationBool, false);
                    }
                    else
                    {
                        Debug.Log("Enemy Moving to Cover");
                        yield return StartCoroutine(MoveToPosition(targetPosition.position));
                        yield return new WaitForSeconds(timeAfterAnimation);
                    }
                }              
            }
            else
            {
                Debug.Log("Enemy Coming Back");
                yield return StartCoroutine(MoveToPosition(startingPosition));
                yield return new WaitForSeconds(timeAfterAnimation);
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / moveSpeed;
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Time.time < startTime + duration)
        {
            float fraction = Mathf.Clamp01((Time.time - startTime) / duration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, fraction);
            yield return null;
        }

        transform.position = targetPosition;
    }

    private bool CheckForStructures()
    {
        // Raycast to check for structures in front of the enemy at ground level
        RaycastHit hit;
        if (Physics.Raycast(frontRaycastPosition.position, frontRaycastPosition.forward, out hit, detectionRange, structureLayer))
        {
            Debug.DrawLine(frontRaycastPosition.position, hit.point, Color.magenta);
            return true; 
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * detectionRange, Color.green);
            return false; 
        }
    }
}