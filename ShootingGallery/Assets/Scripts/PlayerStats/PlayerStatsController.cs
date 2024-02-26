using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStatsController : MonoBehaviour, IPointsProvider, IDamageable
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Animator postProcessingAnimator;
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


            if (_playerStats.health <= 0)
            {
                _playerStats.health = 0;
                Debug.Log("Player Died");
            }
        }      
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
            yellowScreenVignette.weight = 1;
        }
        else
        {
            Debug.Log("God Mode Disabled");
            yellowScreenVignette.weight = 0;
        }
    }
}
