using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartColliderLogic : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.ReplayEvent += ToggleStartCollider;
    }

    private void OnDisable()
    {
        GameManager.ReplayEvent += ToggleStartCollider;
    }

    private void ToggleStartCollider()
    {
        gameObject.SetActive(true);
    }
}