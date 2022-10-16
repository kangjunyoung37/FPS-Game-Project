using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagazineBehaviour : MonoBehaviour
{
    #region GETTERS

    /// <summary>
    /// �� ź����� �����մϴ�.
    /// </summary>
    public abstract int GetAmmunitionTotal();

    /// <summary>
    /// �������̽��� ���� Sprite�� �����մϴ�.
    /// </summary>
    public abstract Sprite GetSprite();
    #endregion
}
