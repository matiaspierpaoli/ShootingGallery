using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSensitivity : MonoBehaviour
{
    public float horizontalSensitivity;
    public float verticalSensitivity;

    private float defaultHorizontalSensitivity = 75f;
    private float defaultVerticalSensitivity = 75f;

    public Scrollbar horizontalScrollbar;
    public Scrollbar verticalScrollbar;

    private float minSensitivity;
    private float maxSensitivity;

    private void Start()
    {
        minSensitivity = 0.2f;
        maxSensitivity = 150f;

        horizontalSensitivity = PlayerPrefs.GetFloat("horizontalSensitivity", defaultHorizontalSensitivity);
        horizontalSensitivity = PlayerPrefs.GetFloat("verticalSensitivity", defaultVerticalSensitivity);

        horizontalScrollbar.value = maxSensitivity / 2;
        verticalScrollbar.value = maxSensitivity / 2;
    }

    public void OnHorizontalSensitivityChange()
    {
        horizontalSensitivity = horizontalScrollbar.value;

        horizontalSensitivity = Mathf.Clamp(horizontalSensitivity, minSensitivity, maxSensitivity);
        
        PlayerPrefs.SetFloat("horizontalSensitivity", horizontalSensitivity);
        PlayerPrefs.Save();
    }

    public void OnVerticalSensitivityChange()
    {
        verticalSensitivity = verticalScrollbar.value;

        verticalSensitivity = Mathf.Clamp(verticalSensitivity, minSensitivity, maxSensitivity);

        PlayerPrefs.SetFloat("verticalSensitivity", verticalSensitivity);
        PlayerPrefs.Save();
    }
}
