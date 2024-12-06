using TMPro;
using UnityEngine;

public class CameraSensitivity : MonoBehaviour
{
    [Header("Sensitivity Config")]
    [SerializeField] private TMP_Text horizontalSensitivityText;
    [SerializeField] private TMP_Text verticalSensitivityText;
    [SerializeField] private SensitivitySlider horizontalSensitivitySlider;
    [SerializeField] private SensitivitySlider verticalSensitivitySlider;

    [SerializeField] private float minSensitivity;
    [SerializeField] private float maxSensitivity;

    private float horizontalSensitivity;
    private float verticalSensitivity;

    public static event System.Action SensChangeEvent;

    private void OnEnable()
    {
        horizontalSensitivitySlider.OnValueChanged += OnHorizontalSensitivityChange;
        verticalSensitivitySlider.OnValueChanged += OnVerticalSensitivityChange;
    }

    private void OnDisable()
    {
        horizontalSensitivitySlider.OnValueChanged -= OnHorizontalSensitivityChange;
        verticalSensitivitySlider.OnValueChanged -= OnVerticalSensitivityChange;
    }

    private void OnHorizontalSensitivityChange(float value)
    {
        horizontalSensitivity = value;
        horizontalSensitivityText.text = value.ToString();

        horizontalSensitivity = Mathf.Clamp(horizontalSensitivity, minSensitivity, maxSensitivity);
        
        PlayerPrefs.SetFloat("horizontalSensitivity", horizontalSensitivity);
        Debug.Log("Horizontal sens set to:" + value);
        PlayerPrefs.Save();

        SensChangeEvent?.Invoke();
    }

    private void OnVerticalSensitivityChange(float value)
    {
        verticalSensitivity = value;
        verticalSensitivityText.text = value.ToString();

        verticalSensitivity = Mathf.Clamp(verticalSensitivity, minSensitivity, maxSensitivity);

        PlayerPrefs.SetFloat("verticalSensitivity", verticalSensitivity);
        Debug.Log("Vertical sens set to:" + value);
        PlayerPrefs.Save();

        SensChangeEvent?.Invoke();
    }
}
