using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour, IPointsProvider, IDamageable
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Animator postProcessingAnimator;
    [SerializeField] private string redScreenBoolName;
    [SerializeField] private float redScreenTime;

    public static event System.Action TakeDamageEvent;

    private bool isRedScreenCoroutineRunning = false;

    private void Start()
    {
        _playerStats.points = 0;
    }

    public void AddPoints(int newPoints)
    {
        _playerStats.points += newPoints;
    }

    public void Damage(float damage)
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

    private IEnumerator TriggerRedScreen()
    {
        isRedScreenCoroutineRunning = true;
        postProcessingAnimator.SetBool(redScreenBoolName, true);
        yield return new WaitForSeconds(redScreenTime);
        postProcessingAnimator.SetBool(redScreenBoolName, false);
        isRedScreenCoroutineRunning = false;
    }
}
