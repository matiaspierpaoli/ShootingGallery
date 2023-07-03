using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string defaultStateName;
    [SerializeField] private string recoilName;
    [SerializeField] private string reloadName;

    public static event System.Action<bool> AnimationEvent;

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
        AnimationEvent?.Invoke(true);

        StartCoroutine(CheckAnimationState());
    }

    private void OnShootingStarted()
    {
        animator.Play(recoilName);
        AnimationEvent?.Invoke(true);

        StartCoroutine(CheckAnimationState());
    }

    private IEnumerator CheckAnimationState()
    {
        yield return new WaitForEndOfFrame();

        while (animator.GetCurrentAnimatorStateInfo(0).IsName(defaultStateName) == false)
        {
            yield return null;
        }

        AnimationEvent?.Invoke(false);
    }
}