//using System.Collections;
//using UnityEngine;

//public class SoldierEnemyMovement : MonoBehaviour
//{
//    [SerializeField] private Transform player;
//    [SerializeField] private Transform targetPosition;
//    [SerializeField] private Transform frontRaycastPosition;
//    [SerializeField] private float moveSpeed = 3f;
//    [SerializeField] private float rotationSpeed = 2f;
//    [SerializeField] private float timeAfterAnimation = 2f;
//    [SerializeField] private float detectionRange = 5f;
//    [SerializeField] private float crouchAnimationLength = 4f;
//    [SerializeField] private Animator soldierAnimationController;
//    [SerializeField] private string crouchShootName = "crouchShoot";
//    [SerializeField] private LayerMask structureLayer;
//    [SerializeField] private float crouchProbability = 0.5f;
//    [SerializeField] private string isSoldierCrouchingAnimationBool;

//    private Vector3 startingPosition;
//    private Quaternion startingRotation;
//    private EnemyWithFov enemyWithFov;

//    private void Start()
//    {
//        startingPosition = transform.position;
//        startingRotation = transform.rotation;
//        enemyWithFov = GetComponent<EnemyWithFov>();
//        StartCoroutine(MovementCoroutine());
//    }

//    private IEnumerator MovementCoroutine()
//    {
//        while (true)
//        {
//            if (enemyWithFov.PlayerInSight)
//            {
                

//                if (enemyWithFov.enemyWeapon.gunData.reloading)
//                {
//                    if (Vector3.Distance(transform.position, startingPosition) < 0.1)
//                    {
//                        //bool crouch = CheckForStructures()/* && Random.value < crouchProbability*/;

//                        //if (crouch)
//                        //    soldierAnimationController.SetTrigger(crouchShootName);

//                        //if (crouch)
//                        //{
//                        //    Debug.Log("Crouching");
//                        //    soldierAnimationController.SetBool(isSoldierCrouchingAnimationBool, true);
//                        //    yield return new WaitForSeconds(crouchAnimationLength + timeAfterAnimation);
//                        //    soldierAnimationController.SetBool(isSoldierCrouchingAnimationBool, false);
//                        //}
//                        //else
//                        //{
//                        //    if (targetPosition)
//                        //    {
//                        //        Debug.Log("Enemy Moving to Cover");
//                        //        yield return StartCoroutine(MoveToPosition(targetPosition.position));
//                        //        yield return new WaitForSeconds(timeAfterAnimation);
//                        //    }
//                        //}
//                    }
//                }
//            }
//            else
//            {
//                if (transform.position != startingPosition)
//                {
//                    Debug.Log("Enemy Coming Back");
//                    yield return StartCoroutine(MoveToPosition(startingPosition));
//                    yield return new WaitForSeconds(timeAfterAnimation);
//                }

//                if (transform.rotation != startingRotation)
//                    ResetRotation();
//            }

//            yield return null;
//        }
//    }


//    private void ResetRotation()
//    {
//        transform.rotation = Quaternion.Slerp(transform.rotation, startingRotation, rotationSpeed * Time.deltaTime);
//    }

//    private IEnumerator MoveToPosition(Vector3 targetPosition)
//    {
//        float distance = Vector3.Distance(transform.position, targetPosition);
//        float duration = distance / moveSpeed;
//        float startTime = Time.time;
//        Vector3 startPosition = transform.position;

//        while (Time.time < startTime + duration)
//        {
//            float fraction = Mathf.Clamp01((Time.time - startTime) / duration);
//            transform.position = Vector3.Lerp(startPosition, targetPosition, fraction);
//            yield return null;
//        }

//        transform.position = targetPosition;
//    }

//}

////using System.Collections;
////using UnityEngine;

////public class SoldierEnemyMovement : MonoBehaviour
////{
////    [SerializeField] private Transform player;
////    [SerializeField] private Transform targetPosition;
////    [SerializeField] private Transform frontRaycastPosition;
////    [SerializeField] private float moveSpeed = 3f;
////    [SerializeField] private float rotationSpeed = 2f;
////    [SerializeField] private float timeAfterAnimation = 2f;
////    [SerializeField] private float detectionRange = 5f;
////    [SerializeField] private float crouchAnimationLength = 4f;
////    [SerializeField] private Animator soldierAnimationController;
////    [SerializeField] private string crouchShootName = "crouchShoot";
////    [SerializeField] private LayerMask structureLayer;
////    [SerializeField] private float crouchProbability = 0.5f;

////    private Vector3 startingPosition;
////    private Quaternion startingRotation;
////    private EnemyWithFov enemyWithFov;

////    private enum EnemyState { Idle, Pursuing, Crouching, Returning }
////    private EnemyState currentState = EnemyState.Idle;

////    private void Start()
////    {
////        startingPosition = transform.position;
////        startingRotation = transform.rotation;
////        enemyWithFov = GetComponent<EnemyWithFov>();
////        StartCoroutine(StateMachineCoroutine());
////    }

////    private IEnumerator StateMachineCoroutine()
////    {
////        while (true)
////        {
////            switch (currentState)
////            {
////                case EnemyState.Idle:
////                    HandleIdleState();
////                    break;

////                case EnemyState.Pursuing:
////                    yield return HandlePursuingState();
////                    break;

////                case EnemyState.Crouching:
////                    yield return HandleCrouchingState();
////                    break;

////                case EnemyState.Returning:
////                    yield return HandleReturningState();
////                    break;
////            }

////            yield return null;
////        }
////    }

////    private void HandleIdleState()
////    {
////        if (enemyWithFov.PlayerInSight)
////        {
////            currentState = EnemyState.Pursuing;
////        }
////        else if (transform.position != startingPosition)
////        {
////            currentState = EnemyState.Returning;
////        }
////    }

////    private IEnumerator HandlePursuingState()
////    {
////        RotateTowardsPlayer();

////        if (enemyWithFov.enemyWeapon.gunData.reloading)
////        {
////            bool crouch = CheckForStructures();
////            if (crouch)
////            {
////                currentState = EnemyState.Crouching;
////                yield break;
////            }

////            if (targetPosition)
////            {
////                yield return MoveToPosition(targetPosition.position);
////                yield return new WaitForSeconds(timeAfterAnimation);
////            }
////        }

////        currentState = EnemyState.Idle;
////    }

////    private IEnumerator HandleCrouchingState()
////    {
////        soldierAnimationController.SetTrigger(crouchShootName);
////        yield return new WaitForSeconds(crouchAnimationLength + timeAfterAnimation);
////        currentState = EnemyState.Idle;
////    }

////    private IEnumerator HandleReturningState()
////    {
////        yield return MoveToPosition(startingPosition);
////        ResetRotation();
////        currentState = EnemyState.Idle;
////    }

////    private void RotateTowardsPlayer()
////    {
////        Vector3 directionToPlayer = (player.position - transform.position).normalized;
////        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
////        targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f); // Solo rotación Y
////        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
////    }

////    private void ResetRotation()
////    {
////        transform.rotation = Quaternion.Slerp(transform.rotation, startingRotation, rotationSpeed * Time.deltaTime);
////    }

////    private IEnumerator MoveToPosition(Vector3 targetPosition)
////    {
////        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
////        {
////            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
////            yield return null;
////        }
////        transform.position = targetPosition;
////    }

////    private bool CheckForStructures()
////    {
////        if (Physics.Raycast(frontRaycastPosition.position, frontRaycastPosition.forward, out RaycastHit hit, detectionRange, structureLayer))
////        {
////            Debug.DrawLine(frontRaycastPosition.position, hit.point, Color.magenta);
////            return true;
////        }
////        else
////        {
////            Debug.DrawRay(transform.position, transform.forward * detectionRange, Color.green);
////            return false;
////        }
////    }
////}
