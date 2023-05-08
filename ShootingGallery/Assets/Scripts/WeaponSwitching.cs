using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [Header("Scriptable Objects")]
    [SerializeField] private GunData pistol;
    [SerializeField] private GunData ak;
    [SerializeField] private GunData sniper;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    // Start is called before the first frame update
    void Start()
    {
        SetWeapon();

        selectedWeapon = 0;
        SelectFromIndex(selectedWeapon);

        timeSinceLastSwitch = 0f;
    }

    public void OnChangeWeapon1(InputValue value)
    {
        if (pistol.availiable)
            selectedWeapon = 0;
    }

    public void OnChangeWeapon2(InputValue value)
    {
        if (ak.availiable)
            selectedWeapon = 1;
    }

    public void OnChangeWeapon3(InputValue value)
    {
        if (sniper.availiable)
            selectedWeapon = 2;
    }

    private void SetWeapon()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {
        timeSinceLastSwitch += Time.deltaTime;

        SelectFromIndex(selectedWeapon);
    }

    public void SelectFromIndex(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == weaponIndex);
        }

        timeSinceLastSwitch = 0f;

        OnWeaponSelected();
    }

    private void OnWeaponSelected()
    {
        //Debug.Log("Selected new weapon");
    }

    public void AcquireWeapon(GunData weapon)
    {
        weapon.availiable = true;
    }

}
