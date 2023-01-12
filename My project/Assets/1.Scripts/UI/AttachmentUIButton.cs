using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum AttachmentType
{
    Scope,
    Grip,
    LaserSight,
    Muzzle
}
public class AttachmentUIButton : MonoBehaviour
{

    public AttachmentType type = AttachmentType.Scope;

    public int index = 0;

    public WeaponType[] equipableWeapon;
    
    public bool CheckEquip(WeaponType equipWepon)
    {
        foreach(WeaponType weaponType in equipableWeapon)
        {
            if (equipWepon == weaponType)
                return true;
        }
        return false;
    }
}
