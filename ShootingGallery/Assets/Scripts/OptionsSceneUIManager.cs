using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSceneUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text horizontalSensitivityText;
    [SerializeField] private TMP_Text verticalSensitivityText;

    [SerializeField] private Slider horizontalSlider;
    [SerializeField] private Slider verticalSlider;

    private void Update()
    {
        horizontalSensitivityText.text = horizontalSlider.value.ToString();
        verticalSensitivityText.text = verticalSlider.value.ToString();
    }

}
