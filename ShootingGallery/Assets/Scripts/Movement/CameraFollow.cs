using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
    //TODO: Fix - Unclear name
    [SerializeField] private Transform cameraPosition;

    //TODO: Fix - Should be LateUpdate
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
