using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private string defaultStateName;
    [SerializeField] private string recoilName;
    [SerializeField] private string reloadName;
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

    private void OnReloadStarted()
    {
        animator.Play(reloadName);

        StartCoroutine(CheckAnimationState());
    }

    private void OnShootingStarted()
    {
        animator.Play(recoilName);

        StartCoroutine(CheckAnimationState());
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