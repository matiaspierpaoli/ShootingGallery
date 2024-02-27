using UnityEngine;

public class Target : MonoBehaviour, IDamageable, IEnemy
{
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerStatsController playerStats;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float maxHealth;
    [SerializeField] private float eliminationCooldown = 5f;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AK.Wwise.Event dieSFX;
    [SerializeField] private AK.Wwise.Event damageSFX;

    private float currentHealth = 100f;
    private float timeToRespawn = 5f;
    
    private IPointsProvider pointsProvider;

    // Since there is a scene called tutorial where points and enemiesDefeated are not needed, check if both of these are referenced
    private bool hasPlayer;
    private bool hasGameManager;
    private bool hasAudioManager;

    private void Start()
    {
        hasPlayer = player != null;
        if (hasPlayer)
        {
            pointsProvider = player.GetComponent<IPointsProvider>();
            playerStats.SetLastEliminationTime(-eliminationCooldown);
        }


        hasGameManager = gameManager != null;
        hasAudioManager = audioManager != null;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;

        if (hasAudioManager)
            audioManager.PlaySoundEvent(damageSFX, gameObject);

        if (currentHealth <= 0)
        {
            if (hasPlayer && pointsProvider != null)
            {
                if (Time.time - playerStats.GetLastEliminationTime() <= eliminationCooldown)
                {
                    pointsProvider.AddPoints(2);
                }
                else
                {
                    pointsProvider.AddPoints(1);
                }

                playerStats.SetLastEliminationTime(Time.time);

                if (hasAudioManager)
                    audioManager.PlaySoundEvent(dieSFX, gameObject);

                gameObject.SetActive(false);
                if (transform.parent)
                    Debug.Log(transform.parent.gameObject.name + " eliminated in " + transform.parent.gameObject.tag);
                Invoke("Respawn", timeToRespawn);


                if (hasGameManager)
                    gameManager.AddOneEnemyDefeated();
            }
        }        
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        if (transform.parent)
            Debug.Log(transform.parent.gameObject.name + " respawned in " + transform.parent.gameObject.tag);
        currentHealth = maxHealth;
    }
}
