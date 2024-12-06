using UnityEngine;

public class BodyPartCollider : MonoBehaviour
{
    [Header("Config")]
    private EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enemyController != null)
        {
            if (collision.gameObject.tag == enemyController.playerTag)
                enemyController.OnBodyPartHit();
        }
    }
}
