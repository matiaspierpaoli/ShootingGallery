using UnityEngine;

public class EnemyWithFov : MonoBehaviour
{
    [SerializeField] private Transform soldier;
    [SerializeField] private float viewRadius;
    [SerializeField] private float angleThreshold;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float zOffset;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float shootDelay;
    [SerializeField] private Color fovColor;
    
    public EnemyWeapon enemyWeapon;
    public bool PlayerInSight { get; private set; }
    private bool isShooting;

    private void Start()
    {
        InvokeRepeating("CheckPlayerInFOV", 0f, 0.5f);

        enemyWeapon = GetComponentInChildren<EnemyWeapon>();
    }

    private void CheckPlayerInFOV()
    {
        if (CanSeePlayer())
        {
            if (!PlayerInSight && !isShooting)
            {
                // Player entered the FOV
                StartShooting();
            }

            PlayerInSight = true;
        }
        else
        {
            if (PlayerInSight && isShooting)
            {
                // Player left the FOV
                StopShooting();
            }

            PlayerInSight = false;
        }
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

                Vector3 shootingPoint = soldier.position + Vector3.up * yOffset + Vector3.right * xOffset + Vector3.forward * zOffset;

                if (!Physics.Raycast(shootingPoint, directionToTarget, distanceToTarget, obstacleMask))
                {
                    Debug.DrawRay(shootingPoint, directionToTarget * distanceToTarget, Color.green);
                    return true;
                }
                else
                {
                    Debug.DrawRay(shootingPoint, directionToTarget * distanceToTarget, Color.red);
                }
            }
        }

        return false;
    }

    private void StartShooting()
    {
        isShooting = true;
        InvokeRepeating("ShootPlayer", 0f, shootDelay);
    }

    private void StopShooting()
    {
        isShooting = false;
        CancelInvoke("ShootPlayer");
    }

    private void ShootPlayer()
    {
        Debug.Log("Shooting at player!");
        enemyWeapon.Shoot();
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

        // Draw lines connecting the boundaries to the player (left and right)
        Debug.DrawLine(soldier.position, soldier.position + leftBoundary * viewRadius, fovColor);
        Debug.DrawLine(soldier.position, soldier.position + rightBoundary * viewRadius, fovColor);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool isGlobal)
    {
        if (!isGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
