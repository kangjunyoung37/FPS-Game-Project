using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    #region FIELDS

    /// <summary>
    /// 장착된 무기
    /// </summary>
    private WeaponBehaviour weapon;

    #endregion


    #region UNITY

    private void Awake()
    {
        weapon = GetComponent<WeaponBehaviour>();
    }

    #endregion

    #region ANIMATION

    /// <summary>
    /// 이 무기에서 탄피를 배출합니다.
    /// </summary>
    private void OnEjectCasing()
    {
        if (weapon != null)
            weapon.EjectCasing();
    }

    #endregion
}
