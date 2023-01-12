using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UIWeaponData",menuName ="FPS Game/Shooter Pack/UIWeaponData")]
public class UIWeaponData : ScriptableObject
{
    [Range(0f,1f)]
    [SerializeField]
    private float damage;
    
    [Range(0f, 1f)]
    [SerializeField]
    private float rpm;

    [Range(0f, 1f)]
    [SerializeField]
    private float recoil;
    
    [Range(0f, 1f)]
    [SerializeField]
    private float mobility;
    
    [SerializeField]
    private int magazine;

    public float Damage => damage;

    public float RPM => rpm;

    public float Recoil => recoil;  

    public float Mobility => mobility;

    public int Magazine => magazine;


}
