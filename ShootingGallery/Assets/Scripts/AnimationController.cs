using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private string defaultStateName;
    [SerializeField] private string recoilTriggerName = "Recoil";
    [SerializeField] private string reloadTriggerName = "Reload";
    private Weapon weapon;

    private Animator animator;
    
    private void OnEnable()
    {
        weapon.ReloadStartedEvent += OnReloadStarted;
        weapon.ShootingStartedEvent += OnShootingStarted;
    }

    private void OnDisable()
    {
        weapon.ReloadStartedEvent -= OnReloadStarted;
        weapon.ShootingStartedEvent -= OnShootingStarted;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weapon= GetComponent<Weapon>();
    }

    public void OnReloadStarted()
    {
        animator.SetTrigger(reloadTriggerName);
    }

    private void OnShootingStarted()
    {
        animator.SetTrigger(recoilTriggerName);
    }

    public void SetDefaultAnimationState()
    {
        animator.Play(defaultStateName);
    }
}