using UnityEngine;

public class EnemyWithFov : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float viewRadius;
    [SerializeField] private float angleThreshold;
    [SerializeField] private float yOffset;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float shootDelay;
    [SerializeField] private Color fovColor;

    private bool playerInSight;
    private bool isShooting;

    private void Start()
    {
        InvokeRepeating("CheckPlayerInFOV", 0f, 0.5f);
    }

    private void CheckPlayerInFOV()
    {
        if (CanSeePlayer())
        {
            if (!playerInSight && !isShooting)
            {
                // Player entered the FOV
                StartShooting();
            }

            playerInSight = true;
        }
        else
        {
            if (playerInSight && isShooting)
            {
                // Player left the FOV
                StopShooting();
            }

            playerInSight = false;
        }
    }

    private bool CanSeePlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        if (hits.Length > 0)
        {
            Transform target = hits[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angleThreshold)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                Vector3 shootingPoint = transform.position + Vector3.up * yOffset;

                if (!Physics.Raycast(shootingPoint, directionToTarget, distanceToTarget, obstacleMask))
                {
                    Debug.DrawRay(shootingPoint, directionToTarget * distanceToTarget, Color.green);
                    return true;
                }
                else
                {
                    Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.red);
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
        Debug.DrawLine(transform.position, transform.position + leftBoundary * viewRadius, fovColor);
        Debug.DrawLine(transform.position, transform.position + rightBoundary * viewRadius, fovColor);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool isGlobal)
    {
        if (!isGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
