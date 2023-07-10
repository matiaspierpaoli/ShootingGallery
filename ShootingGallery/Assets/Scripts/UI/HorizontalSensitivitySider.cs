using UnityEngine;
using UnityEngine.UI;

public class HorizontalSensitivitySider : MonoBehaviour
{
    private Slider slider;
    public static event System.Action<float> OnValueChanged;

    private float horizontalSensitivity;
    private float defaultHorizontalSensitivity = 75f;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        if (!PlayerPrefs.HasKey("horizontalSensitivity"))
        {
            PlayerPrefs.SetFloat("horizontalSensitivity", defaultHorizontalSensitivity);
        }

        horizontalSensitivity = PlayerPrefs.GetFloat("horizontalSensitivity", defaultHorizontalSensitivity);
        slider.value = horizontalSensitivity;
        //OnValueChanged?.Invoke(slider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        OnValueChanged?.Invoke(value);
    }
}
