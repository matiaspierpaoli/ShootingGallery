using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraSensitivity : MonoBehaviour
{
    [SerializeField] private TMP_Text horizontalSensitivityText;
    [SerializeField] private TMP_Text verticalSensitivityText;

    private float horizontalSensitivity;
    private float verticalSensitivity;

    private float minSensitivity;
    private float maxSensitivity;

    public static event System.Action SensChangeEvent;

    private void Start()
    {
        minSensitivity = 0.2f;
        maxSensitivity = 150f;
    }

    private void OnEnable()
    {
        HorizontalSensitivitySider.OnValueChanged += OnHorizontalSensitivityChange;
        VerticalSensitivitySlider.OnValueChanged += OnVerticalSensitivityChange;

    }

    private void OnDisable()
    {
        HorizontalSensitivitySider.OnValueChanged -= OnHorizontalSensitivityChange;
        VerticalSensitivitySlider.OnValueChanged -= OnVerticalSensitivityChange;
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
        // Error here - if value is > 0, verticalSensitivity will still be 0
        verticalSensitivity = value;
        verticalSensitivityText.text = value.ToString();

        verticalSensitivity = Mathf.Clamp(verticalSensitivity, minSensitivity, maxSensitivity);

        PlayerPrefs.SetFloat("verticalSensitivity", verticalSensitivity);
        Debug.Log("Vertical sens set to:" + value);
        PlayerPrefs.Save();

        SensChangeEvent?.Invoke();
    }
}
