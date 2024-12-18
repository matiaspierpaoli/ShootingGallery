using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [Header("Config")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerStatsController playerStats;
    [SerializeField] private DynamicCrosshair dynamicCrosshair;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float maxHealth;
    [SerializeField] private float eliminationCooldown = 5f;

    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AK.Wwise.Event dieSFX;
    [SerializeField] private AK.Wwise.Event damageSFX;

    public event System.Action DieEvent;

    private float currentHealth = 100f;
    
    private IPointsProvider pointsProvider;

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

        dynamicCrosshair.OnHit();

        if (currentHealth <= 0)
        {
            if (hasPlayer && pointsProvider != null)
            {
                DieEvent?.Invoke();
                dynamicCrosshair.OnKill();
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

                if (transform.parent)
                    Debug.Log(transform.parent.gameObject.name + " eliminated in " + transform.parent.gameObject.tag);


                if (hasGameManager)
                    gameManager.AddOneEnemyDefeated();
            }
        }        
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
    }
}
