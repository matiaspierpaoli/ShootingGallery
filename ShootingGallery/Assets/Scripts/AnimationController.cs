using System.Collections;
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
        //Gun.ReloadStartedEvent += OnReloadStarted;
        //Gun.ShootingStartedEvent += OnShootingStarted;

        weapon.ReloadStartedEvent += OnReloadStarted;
        weapon.ShootingStartedEvent += OnShootingStarted;
    }

    private void OnDisable()
    {
        //Gun.ReloadStartedEvent -= OnReloadStarted;
        //Gun.ShootingStartedEvent -= OnShootingStarted;

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
        //animator.Play(reloadTriggerName);

        //StartCoroutine(CheckAnimationState());
    }

    private void OnShootingStarted()
    {
        animator.SetTrigger(recoilTriggerName);
        //animator.Play(recoilTriggerName);

        //StartCoroutine(CheckAnimationState());
    }

    private IEnumerator CheckAnimationState()
    {
        yield return new WaitForEndOfFrame();

        while (animator.GetCurrentAnimatorStateInfo(0).IsName(defaultStateName) == false)
        {
            yield return null;
        }
    }

    public void SetDefaultAnimationState()
    {
        animator.Play(defaultStateName);
    }
}