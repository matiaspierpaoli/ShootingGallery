using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats playerStats;

    [Header("Scriptable Objects")]
    [SerializeField] private GunData pistol;
    [SerializeField] private GunData ak;
    [SerializeField] private GunData sniper;

    private Transform[] weapons;
    private Weapon[] weaponsComponent;
    private int selectedWeapon;

    public static event System.Action<int> SwitchWeaponEvent;

    private void Awake()
    {
        weaponsComponent = gameObject.GetComponentsInChildren<Weapon>();
    }

    private void Start()
    {
        SetWeapon();
    }

    public void OnChangeWeapon1(InputValue value)
    {
        if (pistol.availiable)
        {
            selectedWeapon = 0;
            SelectFromIndex(selectedWeapon);
            SwitchWeaponEvent?.Invoke(selectedWeapon);
        }
    }

    public void OnChangeWeapon2(InputValue value)
    {
        if (ak.availiable)
        {
            selectedWeapon = 1;
            SelectFromIndex(selectedWeapon);
            SwitchWeaponEvent?.Invoke(selectedWeapon);
        }
    }

    public void OnChangeWeapon3(InputValue value)
    {
        if (sniper.availiable)
        {
            selectedWeapon = 2;
            SelectFromIndex(selectedWeapon);
            SwitchWeaponEvent?.Invoke(selectedWeapon);
        }
    }

    public void OnReload(InputValue context)
    {
        HandleReload();
    }

    private void HandleReload()
    {
        Weapon weapon = weapons[selectedWeapon].GetComponent<Weapon>();
        if (weapon != null)
        {
            if (weapon.gunData.currentAmmo < weapon.gunData.magSize)
                weapon.StartReload();
        }
    }

    private void SetWeapon()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = transform.GetChild(i);
            weapons[i].gameObject.SetActive(false);
        }
    }

    public void SelectFromIndex(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == weaponIndex);
        }
    }

    public void AcquireWeapon(GunData weapon)
    {
        if (playerStats.points >= weapon.cost)
        {
            weapon.availiable = true;
            playerStats.points -= weapon.cost;
        }
    }
}
