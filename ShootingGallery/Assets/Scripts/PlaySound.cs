using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event eventSFX;

    public void PlaySFX()
    {
        eventSFX.Post(gameObject);
    } 
}
