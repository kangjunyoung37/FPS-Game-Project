using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagazineBehaviour : MonoBehaviour
{
    #region GETTERS

    /// <summary>
    /// 총 탄약수를 리턴합니다.
    /// </summary>
    public abstract int GetAmmunitionTotal();

    /// <summary>
    /// 인터페이스에 쓰일 Sprite를 리턴합니다.
    /// </summary>
    public abstract Sprite GetSprite();

    /// <summary>
    /// MagazineRenderer를 끕니다.
    /// </summary>
    public abstract void FPMagazineOff();
    #endregion
}
