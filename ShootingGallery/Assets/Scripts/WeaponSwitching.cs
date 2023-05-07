using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] weapons;


    InputAction changeWeaponAction;

    private Dictionary<int, string> weaponBindings = new Dictionary<int, string>
    {
        {1, "Keyboard/1"}, // Map binding for "1" key to value of 1
        {2, "Keyboard/2"}, // Map binding for "2" key to value of 2
        {3, "Gamepad/buttonSouth"} // Map binding for gamepad button to value of 3
    };

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    // Start is called before the first frame update
    void Start()
    {
        SetWeapon();
        selectedWeapon = 1;
        SelectFromIndex(selectedWeapon);

        timeSinceLastSwitch = 0f;
    }

    public void OnChangeWeapon(InputValue value)
    {
        //int input = (int)value.Get<float>();

        //// Check if the input is coming from one of the two bindings for the changeWeaponAction
        //if (changeWeaponAction.bindings[0].isComposite || changeWeaponAction.bindings[1].isComposite)
        //{
        //    // Check if the input is coming from the "Weapon 1" binding
        //    if (value.GetBindingIndex() == changeWeaponAction.bindings[0].effectivePath[0])
        //    {
        //        selectedWeapon = 1;
        //        Debug.Log("Switched to weapon 1");
        //    }
        //    // Check if the input is coming from the "Weapon 2" binding
        //    else if (value.GetBindingIndex() == changeWeaponAction.bindings[1].effectivePath[0])
        //    {
        //        selectedWeapon = 2;
        //        Debug.Log("Switched to weapon 2");
        //    }
        //}
        //// If the input is not coming from a composite binding, just switch to weapon 1
        //else
        //{
        //    selectedWeapon = 1;
        //    Debug.Log("Switched to weapon 1");
        


        //if (input == 1)
        //{
        //    selectedWeapon = 0;
        //    Debug.Log("Switched to weapon 1");
        //}
        //else if (input == 2)
        //{
        //    selectedWeapon = 1;
        //    Debug.Log("Switched to weapon 2");
        //}

        SelectFromIndex(selectedWeapon);
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
    }

    public void SelectFromGameObject(GameObject weapon)
    {
        weapon.gameObject.SetActive(true);
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



}
