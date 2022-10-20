using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 추상 클래스
/// </summary>
public abstract class InventoryBehaviour : MonoBehaviour
{
    #region GETTERS

    /// <summary>
    /// 현재 인덱스 이전의 인덱스를 리턴합니다.
    /// </summary>
    public abstract int GetLastIndex();

    /// <summary>
    /// 현재 인덱스 다음 인덱스를 리턴합니다.
    /// </summary>
    public abstract int GetNextIndex();

    /// <summary>
    /// 현재 장착하고 있는 무기의 WeaponBehaviour를 리턴합니다.
    /// </summary>
    public abstract WeaponBehaviour GetEquipped();

    /// <summary>
    /// 현재 장착하고 있는 무기의 인덱스를 리턴합니다.
    /// </summary>
    public abstract int GetEquippedIndex();

    #endregion

    #region METHODS
    /// <summary>
    /// 초기화 함수
    /// </summary>
    /// <param name="equippedAtStart">게임이 시작되면 장착될 index</param>
    public abstract void Init(int equippedAtStart = 0);

    /// <summary>
    /// 장착된 무기
    /// </summary>
    /// <param name="index">인벤토리 무기 인덱스</param>
    /// <returns></returns>
    public abstract WeaponBehaviour Equip(int index);

    #endregion
}
