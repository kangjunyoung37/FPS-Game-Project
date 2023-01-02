using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    #region SerializeField

    [SerializeField]
    private float damagePercent;

    #endregion

    #region Getters

    public float GetDamagePercent() => damagePercent;

    #endregion


}
