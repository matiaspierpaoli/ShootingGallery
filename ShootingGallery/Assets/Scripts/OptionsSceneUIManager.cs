using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSceneUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text horizontalSensitivityText;
    [SerializeField] private TMP_Text verticalSensitivityText;

    [SerializeField] private Scrollbar horizontalScrollbar;
    [SerializeField] private Scrollbar verticalScrollbar;

    private void Update()
    {
        horizontalSensitivityText.text = horizontalScrollbar.value.ToString();
        verticalSensitivityText.text = verticalScrollbar.value.ToString();
    }

}
