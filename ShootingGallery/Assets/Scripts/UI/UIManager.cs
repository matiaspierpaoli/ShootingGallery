using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        pointsText.text = "Points: " + playerData.points.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = "Points: " + playerData.points.ToString();
    }
}
