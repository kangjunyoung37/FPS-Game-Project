using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    #region FIELDS

    /// <summary>
    /// ������ ����
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
    /// �� ���⿡�� ź�Ǹ� �����մϴ�.
    /// </summary>
    private void OnEjectCasing()
    {
        if (weapon != null)
            weapon.EjectCasing();
    }

    #endregion
}
