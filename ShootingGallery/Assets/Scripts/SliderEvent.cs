using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SliderEvent : MonoBehaviour
{
    [SerializeField] private CameraSensitivity cameraSensitivity;
    [SerializeField] private Slider horizontalSlider;
    [SerializeField] private Slider verticalSlider;

    private void OnSliderValueChanged(float value)
    {
        // Handle slider value changed event
    }


}
