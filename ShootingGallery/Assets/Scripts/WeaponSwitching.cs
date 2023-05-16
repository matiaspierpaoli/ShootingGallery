using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    [Header("References")]
    private Transform[] weapons;
    [SerializeField] private PlayerData _playerData;

    [Header("Scriptable Objects")]
    [SerializeField] private GunData pistol;
    [SerializeField] private GunData ak;
    [SerializeField] private GunData sniper;

    private int selectedWeapon;

    void Start()
    {
        SetWeapon();
    }

    public void OnChangeWeapon1(InputValue value)
    {
        if (pistol.availiable)
        {
            selectedWeapon = 0;
            SelectFromIndex(selectedWeapon);
        }
    }

    public void OnChangeWeapon2(InputValue value)
    {
        if (ak.availiable)
        {
            selectedWeapon = 1;
            SelectFromIndex(selectedWeapon);
        }
    }

    public void OnChangeWeapon3(InputValue value)
    {
        if (sniper.availiable)
        {
            selectedWeapon = 2;
            SelectFromIndex(selectedWeapon);
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
        if (_playerData.points >= weapon.cost)
        {
            weapon.availiable = true;
            _playerData.points -= weapon.cost;
        }
    }
}
