using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    private GameObject currentWeapon;

    void Start()
    {
        AttachWeapon();
    }

    public void AttachWeapon()
    {
        if (weaponPrefab == null) return;

        // Находим точку крепления
        Transform attachmentPoint = transform.Find("Armature/Hand_R/WeaponAttachment");

        // Создаем оружие
        currentWeapon = Instantiate(weaponPrefab, attachmentPoint);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
    }

    public void RemoveWeapon()
    {
        if (currentWeapon != null)
            Destroy(currentWeapon);
    }
}