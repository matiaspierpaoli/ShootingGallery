using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gunData;

    public void Shoot(InputValue context)
    {
        //context.isPressed

        Debug.Log("Shooting");
    }
}
