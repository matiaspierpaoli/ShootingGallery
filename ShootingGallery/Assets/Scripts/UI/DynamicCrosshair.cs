using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    [Header("UI Elements")]
    public Image crosshairImage;

    [Header("Timings")]
    public float fadeDuration = 0.2f; 
    public float killDisplayTime = 0.5f; 
    public float expansionDuration = 0.1f; 

    [Header("Visual Settings")]
    public Color defaultColor = new Color(1, 1, 1, 0.2f);
    public Color hitColor = new Color(1, 1, 1, 1f);
    public Color killColor = new Color(1, 0.5f, 0, 1f); 
    public float normalScale = 1.0f; 
    public float expandedScale = 1.5f;

    private Coroutine currentRoutine = null;

    private void Start()
    {
        ResetCrosshair();
    }

    public void OnHit()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        currentRoutine = StartCoroutine(HandleHit());
    }

    public void OnKill()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        currentRoutine = StartCoroutine(HandleKill());
    }

    private IEnumerator HandleHit()
    {
        crosshairImage.color = hitColor;
        crosshairImage.rectTransform.localScale = Vector3.one * normalScale;

        yield return new WaitForSeconds(fadeDuration);

        ResetCrosshair();
    }

    private IEnumerator HandleKill()
    {
        crosshairImage.color = killColor;
        yield return StartCoroutine(AnimateScale(expandedScale, expansionDuration));

        yield return new WaitForSeconds(killDisplayTime - expansionDuration);

        yield return StartCoroutine(AnimateScale(normalScale, expansionDuration));
        ResetCrosshair();
    }

    private IEnumerator AnimateScale(float targetScale, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = crosshairImage.rectTransform.localScale;
        Vector3 targetScaleVector = Vector3.one * targetScale;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            crosshairImage.rectTransform.localScale = Vector3.Lerp(initialScale, targetScaleVector, t);
            yield return null;
        }

        crosshairImage.rectTransform.localScale = targetScaleVector;
    }

    private void ResetCrosshair()
    {
        crosshairImage.color = defaultColor;
        crosshairImage.rectTransform.localScale = Vector3.one * normalScale;
    }
}
