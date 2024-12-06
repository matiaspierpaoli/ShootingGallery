using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Slider slider;
    [SerializeField] private string playerPrefSensitivityString;
    public event System.Action<float> OnValueChanged;

    private float currentSensitivity;
    private float defaultVerticalSensitivity = 75f;

    private void Start()
    {
        if (!PlayerPrefs.HasKey(playerPrefSensitivityString))
        {
            PlayerPrefs.SetFloat(playerPrefSensitivityString, defaultVerticalSensitivity);
        }
        currentSensitivity = PlayerPrefs.GetFloat(playerPrefSensitivityString, defaultVerticalSensitivity);
        
        slider = GetComponent<Slider>();
        slider.value = currentSensitivity;
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        OnValueChanged?.Invoke(slider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        OnValueChanged?.Invoke(value);
    }
}
