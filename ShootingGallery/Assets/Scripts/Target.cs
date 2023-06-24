using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float maxHealth;

    private float currentHealth = 100f;
    private float timeToRespawn = 5f;

    private IPointsProvider pointsProvider;

    // Since there is a scene called tutorial where points and enemiesDefeated are not needed, check if both of these are referenced
    private bool hasPlayer;
    private bool hasGameManager;

    private void Start()
    {
        hasPlayer = player != null;
        if (hasPlayer)
        {
            pointsProvider = player.GetComponent<IPointsProvider>();
        }

        hasGameManager = gameManager != null;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if (hasPlayer && pointsProvider != null)
            {
                pointsProvider.AddPoints(1);
            }

            if (hasGameManager)
            {
                gameManager.AddOneEnemyDefeated();
            }

            gameObject.SetActive(false);
            Debug.Log(transform.parent.gameObject.name + " eliminated in " + transform.parent.gameObject.tag);
            Invoke("Respawn", timeToRespawn);
        }
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        Debug.Log(transform.parent.gameObject.name + " respawned in " + transform.parent.gameObject.tag);
        currentHealth = maxHealth;
    }
}
