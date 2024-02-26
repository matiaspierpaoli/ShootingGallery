using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float maxHealth;

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
                pointsProvider.AddPoints(1);

                if (hasAudioManager)
                    audioManager.PlaySoundEvent(dieSFX, gameObject);
            }

            if (hasGameManager)           
                gameManager.AddOneEnemyDefeated();
            
            gameObject.SetActive(false);
            if (transform.parent)
                Debug.Log(transform.parent.gameObject.name + " eliminated in " + transform.parent.gameObject.tag);
            Invoke("Respawn", timeToRespawn);
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
