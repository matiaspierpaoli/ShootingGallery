using System.Collections;
using UnityEngine;

public class SoldierEnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Transform frontRaycastPosition;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float timeAfterAnimation = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float crouchAnimationLength = 4f;
    [SerializeField] private Animator soldierAnimationController;
    [SerializeField] private LayerMask structureLayer;
    [SerializeField] private float crouchProbability = 0.5f;
    [SerializeField] private string isSoldierCrouchingAnimationBool;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private EnemyWithFov enemyWithFov;

    private void Start()
    {       
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        enemyWithFov = GetComponent<EnemyWithFov>();
        StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        while (true)
        {
            if (enemyWithFov.PlayerInSight)
            {
                RotateTowardsPlayer();

                if (enemyWithFov.enemyWeapon.gunData.reloading)
                {
                    if (Vector3.Distance(transform.position, startingPosition) < 0.1)
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
                            if (targetPosition)
                            {
                                Debug.Log("Enemy Moving to Cover");
                                yield return StartCoroutine(MoveToPosition(targetPosition.position));
                                yield return new WaitForSeconds(timeAfterAnimation);
                            }
                        }
                    }
                }
            }
            else
            {
                if (transform.position != startingPosition)
                {
                    Debug.Log("Enemy Coming Back");
                    yield return StartCoroutine(MoveToPosition(startingPosition));
                    yield return new WaitForSeconds(timeAfterAnimation);
                }

                if (transform.rotation != startingRotation)
                    ResetRotation();
            }

            yield return null;
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        
        Quaternion yzRotationOnly = Quaternion.Euler(0f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, yzRotationOnly, rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ResetRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, startingRotation, rotationSpeed * Time.deltaTime);
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