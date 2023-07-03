using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string defaultStateName;
    [SerializeField] private string recoilName;
    [SerializeField] private string reloadName;

    private void OnEnable()
    {
        Gun.ReloadStartedEvent += OnReloadStarted;
        Gun.ShootingStartedEvent += OnShootingStarted;
    }

    private void OnDisable()
    {
        Gun.ReloadStartedEvent -= OnReloadStarted;
        Gun.ShootingStartedEvent -= OnShootingStarted;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnReloadStarted()
    {
        animator.Play(reloadName);
    }

    private void OnShootingStarted()
    {
        animator.Play(recoilName);
    }
}