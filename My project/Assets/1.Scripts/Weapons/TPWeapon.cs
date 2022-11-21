using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPWeapon : MonoBehaviour
{

    private Animator WeaponAnimator;

    private void Awake()
    {
        WeaponAnimator = transform.GetComponent<Animator>();  
    }
    public void Reload(string stateName)
    {
        WeaponAnimator.SetBool("Reloading", true);

        WeaponAnimator.Play(stateName, 0, 0.0f);
    }

    public void Fire()
    {
        const string stateName = "Fire";
        WeaponAnimator.Play(stateName, 0, 0.0f);
    }
}
