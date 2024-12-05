using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStatsController playerStatsController;
    [SerializeField] private Transform player;
    [SerializeField] private Transform soldier;
    [SerializeField] private GameObject[] bodyparts;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform frontRaycastPosition;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask structureLayer;
    //[SerializeField] private float detectionRange = 5f;
    [SerializeField] private float timeToRespawn = 2f;

    [Header("Animations")]
    [SerializeField] private Animator soldierAnimationController;
    [SerializeField] private string shootBoolName = "shoot";
    [SerializeField] private string idleShootingBoolName = "idleShooting";
    [SerializeField] private string reloadTriggerName = "reload";
    [SerializeField] private string dyingTriggerName = "dying";
    [SerializeField] private string meleeTriggerName = "melee";
    [SerializeField] private float dyingAnimDuration;
    [SerializeField] private float meleeAnimDuration;
    [SerializeField] private string shootStateTagName = "Shoot";

    [Header("Movement")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 0.5f;

    private int currentPatrolIndex = 0;
    private bool isPatrolling = true;

    [Header("Gizmos")]
    [SerializeField] private float viewRadius;
    [SerializeField] private float angleThreshold;
    [SerializeField] private Color fovColor;

    private Weapon[] weapons;
    private Weapon weapon;
    private Target target;

    private bool isShooting = false;
    private bool isReloading = false;
    private bool isDying = false;
    private bool isMeleeAttacking = false;

    public string playerTag;

    private void OnEnable()
    {
        ResetAnimatorParameters();
    }

    private void Start()
    {
        weapons = GetComponentsInChildren<Weapon>();
        target = GetComponent<Target>();

        foreach (var childWeapon in weapons)
        {
            if (childWeapon.gameObject.activeSelf)
                weapon = childWeapon;
        }

        weapon.ReloadStartedEvent += HandleReloadStarted;
        target.DieEvent += HandleDie;

        if (patrolPoints.Length > 0)
        {
            transform.position = patrolPoints[patrolPoints.Length - 1].position; // Inicia en el primer punto
        }
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            isPatrolling = false;
            soldierAnimationController.SetBool(idleShootingBoolName, true);
            RotateTowardsPlayer();

            if (!isShooting && weapon.canShoot && !isReloading && !isDying && !isMeleeAttacking)
            {
                StartCoroutine(ShootAndWaitCoroutine());
            }
        }
        else
        {
            soldierAnimationController.SetBool(idleShootingBoolName, false);

            if (!isPatrolling)
                isPatrolling = true;

            if (!isDying)
                Patrol();
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        float step = moveSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, step);

        Vector3 direction = (targetPoint.position - transform.position).normalized;
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetPoint.position) < stopDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }

    }

    private void ResetAnimatorParameters()
    {
        soldierAnimationController.SetBool(shootBoolName, false);
        soldierAnimationController.ResetTrigger(reloadTriggerName);
        soldierAnimationController.ResetTrigger(dyingTriggerName);
        soldierAnimationController.ResetTrigger(meleeTriggerName);
    }

    private bool CanSeePlayer()
    {
        Collider[] hits = Physics.OverlapSphere(soldier.position, viewRadius, playerMask);

        if (hits.Length > 0)
        {
            Transform target = hits[0].transform;
            Vector3 directionToTarget = (target.position - soldier.position).normalized;

            if (Vector3.Angle(soldier.forward, directionToTarget) < angleThreshold)
            {
                float distanceToTarget = Vector3.Distance(soldier.position, target.position);

                if (!Physics.Raycast(shootingPoint.position, directionToTarget, distanceToTarget, structureLayer))
                {
                    Debug.DrawRay(shootingPoint.position, directionToTarget * distanceToTarget, Color.green);
                    return true;
                }
                else
                {
                    Debug.DrawRay(shootingPoint.position, directionToTarget * distanceToTarget, Color.red);
                }
            }
        }

        return false;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator ShootAndWaitCoroutine()
    {
        isShooting = true;
        soldierAnimationController.SetBool(shootBoolName, true);

        AnimatorStateInfo stateInfo;
        do
        {
            stateInfo = soldierAnimationController.GetCurrentAnimatorStateInfo(0); 
            yield return null; 
        } while (!stateInfo.IsTag(shootStateTagName));

        weapon.Shoot();

        yield return new WaitForSeconds(weapon.gunData.shootCooldown);

        soldierAnimationController.SetBool(shootBoolName, false);
        weapon.StopShooting();
        isShooting = false;
    }

    private void HandleReloadStarted()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        soldierAnimationController.SetTrigger(reloadTriggerName);

        yield return new WaitForSeconds(weapon.gunData.reloadTime);
        isReloading = false;
    }

    private void HandleDie()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        isDying = true;
        soldierAnimationController.SetTrigger(dyingTriggerName);

        yield return new WaitForSeconds(dyingAnimDuration);

        foreach (var bodyPart in bodyparts)
        {
            bodyPart.SetActive(false);
        }

        yield return new WaitForSeconds(timeToRespawn);

        foreach (var bodyPart in bodyparts)
        {
            Weapon bodyPartWeapon = bodyPart.GetComponent<Weapon>();
            if (bodyPartWeapon)
            {
                var isPreviousWeapon = (bodyPartWeapon == weapon);
                bodyPart.SetActive(isPreviousWeapon);
            }
            else
            {
                bodyPart.SetActive(true);
            }
        }
        target.Respawn();

        yield return new WaitForSeconds(2.0f);
        isDying = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        DrawFOV();
    }

    private void DrawFOV()
    {
        Vector3 leftBoundary = DirFromAngle(-angleThreshold, false);
        Vector3 rightBoundary = DirFromAngle(angleThreshold, false);

        Debug.DrawLine(soldier.position, soldier.position + leftBoundary * viewRadius, fovColor);
        Debug.DrawLine(soldier.position, soldier.position + rightBoundary * viewRadius, fovColor);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool isGlobal)
    {
        if (!isGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void OnBodyPartHit()
    {
        if (!isMeleeAttacking && !isDying)
            StartCoroutine(MeleeCoroutine());
    }

    private IEnumerator MeleeCoroutine()
    {
        isMeleeAttacking = true;
        soldierAnimationController.SetTrigger(meleeTriggerName);
        float elapsedTime = 0f;
        bool isHitActivated = false;

        while (elapsedTime < meleeAnimDuration)
        {
            if (elapsedTime >= meleeAnimDuration / 2 && !isHitActivated)
            {
                isHitActivated = true;
                playerStatsController.KillPlayer();
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isMeleeAttacking = false;

    }
}