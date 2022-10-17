using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAttachmentManagerBehaviour : MonoBehaviour
{

    #region UNITY FUNCTIONS
    protected virtual void Awake() { }
   
    protected virtual void Start() { }
   
    protected virtual void Update() { }

    protected virtual void LateUpdate() { }

    #endregion

    #region GETTERS

    /// <summary>
    /// 장착된 스코프를 리턴합니다.
    /// </summary>
    public abstract ScopeBehaviour GetEquippedScope();

    /// <summary>
    /// 장착된 스코프 default값을 리턴합니다.
    /// </summary>
    public abstract ScopeBehaviour GetEquippedScopeDefault();

    /// <summary>
    /// 장착된 탄창을 리턴합니다.
    /// </summary>
    public abstract MagazineBehaviour GetEquippedMagazine();

    /// <summary>
    /// 장착된 총구를 리턴합니다.
    /// </summary>
    public abstract MuzzleBehaviour GetEquippedMuzzle();

    /// <summary>
    /// 장착된 레이저를 리턴합니다.
    /// </summary>
    public abstract LaserBehaviour GetEquippedLaser();
    
    /// <summary>
    /// 장착된 그립을 리턴합니다.
    /// </summary>
    public abstract GripBehaviour GetEquippedGrip();
    #endregion
}

