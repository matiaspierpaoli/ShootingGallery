using UnityEngine;
using UnityEngine.UI;

public class VerticalSensitivitySlider : MonoBehaviour
{
    private Slider slider;
    public static event System.Action<float> OnValueChanged;

    private float verticalSensitivity;
    private float defaultVerticalSensitivity = 75f;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        if (!PlayerPrefs.HasKey("verticalSensitivity"))
        {
            PlayerPrefs.SetFloat("verticalSensitivity", defaultVerticalSensitivity);
        }

        verticalSensitivity = PlayerPrefs.GetFloat("verticalSensitivity", defaultVerticalSensitivity);
        slider.value = verticalSensitivity;
        //OnValueChanged?.Invoke(slider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        OnValueChanged?.Invoke(value);
    }
}
