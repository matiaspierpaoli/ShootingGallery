using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSensitivity : MonoBehaviour
{
    private float horizontalSensitivity;
    private float verticalSensitivity;

    private float defaultHorizontalSensitivity = 75f;
    private float defaultVerticalSensitivity = 75f;

    public Slider horizontalSlider;
    public Slider verticalSlider;

    private float minSensitivity;
    private float maxSensitivity;

    public static event System.Action SensChangeEvent;

    private void Start()
    {
        minSensitivity = 0.2f;
        maxSensitivity = 150f;

        horizontalSensitivity = PlayerPrefs.GetFloat("horizontalSensitivity", defaultHorizontalSensitivity);
        verticalSensitivity = PlayerPrefs.GetFloat("verticalSensitivity", defaultVerticalSensitivity);

        horizontalSlider.value = horizontalSensitivity;
        verticalSlider.value = verticalSensitivity;
    }

    public void OnHorizontalSensitivityChange()
    {
        horizontalSensitivity = horizontalSlider.value;

        horizontalSensitivity = Mathf.Clamp(horizontalSensitivity, minSensitivity, maxSensitivity);
        
        PlayerPrefs.SetFloat("horizontalSensitivity", horizontalSensitivity);
        PlayerPrefs.Save();

        SensChangeEvent?.Invoke();
    }

    public void OnVerticalSensitivityChange()
    {
        verticalSensitivity = verticalSlider.value;

        verticalSensitivity = Mathf.Clamp(verticalSensitivity, minSensitivity, maxSensitivity);

        PlayerPrefs.SetFloat("verticalSensitivity", verticalSensitivity);
        PlayerPrefs.Save();

        SensChangeEvent?.Invoke();
    }
}
