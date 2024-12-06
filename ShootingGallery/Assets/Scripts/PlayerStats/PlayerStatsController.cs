using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStatsController : MonoBehaviour, IPointsProvider, IDamageable, IPlayer
{
    [Header("Config")]
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Animator postProcessingAnimator;

    [Header("Post-Processing")]
    [SerializeField] private string redScreenBoolName;
    [SerializeField] private float redScreenTime;
    [SerializeField] private PostProcessVolume yellowScreenVignette;

    public static event System.Action TakeDamageEvent;

    private bool isRedScreenCoroutineRunning = false;

    private void OnEnable()
    {
        InputManager.GodModeEvent += OnToggleGodMode;
    }

    private void OnDisable()
    {
        InputManager.GodModeEvent -= OnToggleGodMode;
    }

    private void Start()
    {
        _playerStats.points = 0;
        _playerStats.isInmortal = false;
    }

    public void AddPoints(int newPoints)
    {
        _playerStats.points += newPoints;
    }

    public void SetLastEliminationTime(float lastEliminationTime)
    {
        _playerStats.lastEliminationTime = lastEliminationTime;
    }

    public float GetLastEliminationTime()
    {
        return _playerStats.lastEliminationTime;
    }

    public void Damage(float damage)
    {
        if (!_playerStats.isInmortal)
        {
            Debug.Log("Player Took Damage");
            _playerStats.health -= damage;
            TakeDamageEvent?.Invoke();

            if (!isRedScreenCoroutineRunning)
            {
                StartCoroutine(TriggerRedScreen());
            }
        }      
    }

    public void KillPlayer()
    {
        _playerStats.health = 0;
        Debug.Log("Player Died");
    }

    private IEnumerator TriggerRedScreen()
    {
        isRedScreenCoroutineRunning = true;
        postProcessingAnimator.SetBool(redScreenBoolName, true);
        yield return new WaitForSeconds(redScreenTime);
        postProcessingAnimator.SetBool(redScreenBoolName, false);
        isRedScreenCoroutineRunning = false;
    }

    private void OnToggleGodMode()
    {
        _playerStats.isInmortal = !_playerStats.isInmortal;

        if (_playerStats.isInmortal)
        {
            Debug.Log("God Mode Enabled");
            if (yellowScreenVignette)
                yellowScreenVignette.weight = 1;
        }
        else
        {
            Debug.Log("God Mode Disabled");
            if (yellowScreenVignette)
                yellowScreenVignette.weight = 0;
        }
    }
}
