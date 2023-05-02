using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData _gunData;
    [SerializeField] private Transform _cameraTransform;
    
    public virtual IEnumerator ShootCoroutiune()
    {
        if (Shoot())
            Debug.Log("Raycast hit");

        yield break;
    }

    private bool Shoot()
    {
        RaycastHit hit;

        Vector3 shootDir = GetShotDir();

        if (Physics.Raycast(_cameraTransform.position, shootDir, out hit, _gunData.maxDistance))
        {
            IDamageable aux = hit.collider.GetComponent<IDamageable>();

            if (aux != null)
            {
                aux.Damage(_gunData.damage, hit.point, hit.normal);
            }

            return true;
        }
        else
        {
            return false;
        }


    }

    private Vector3 GetShotDir()
    {
        Vector3 targetPos = _cameraTransform.position + _cameraTransform.forward * _gunData.maxDistance;

        Vector3 dir = targetPos - _cameraTransform.position;

        return dir.normalized;
    }
   
}
