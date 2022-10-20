using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �κ��丮 �߻� Ŭ����
/// </summary>
public abstract class InventoryBehaviour : MonoBehaviour
{
    #region GETTERS

    /// <summary>
    /// ���� �ε��� ������ �ε����� �����մϴ�.
    /// </summary>
    public abstract int GetLastIndex();

    /// <summary>
    /// ���� �ε��� ���� �ε����� �����մϴ�.
    /// </summary>
    public abstract int GetNextIndex();

    /// <summary>
    /// ���� �����ϰ� �ִ� ������ WeaponBehaviour�� �����մϴ�.
    /// </summary>
    public abstract WeaponBehaviour GetEquipped();

    /// <summary>
    /// ���� �����ϰ� �ִ� ������ �ε����� �����մϴ�.
    /// </summary>
    public abstract int GetEquippedIndex();

    #endregion

    #region METHODS
    /// <summary>
    /// �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="equippedAtStart">������ ���۵Ǹ� ������ index</param>
    public abstract void Init(int equippedAtStart = 0);

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="index">�κ��丮 ���� �ε���</param>
    /// <returns></returns>
    public abstract WeaponBehaviour Equip(int index);

    #endregion
}
