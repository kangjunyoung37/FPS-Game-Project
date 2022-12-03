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

    /// <summary>
    /// 총구 인덱스를 리턴합니다.
    /// </summary>
    public abstract int GetEquippedMuzzleIndex();
    
    /// <summary>
    /// 총 스코프 인덱스를 리턴합니다.
    /// </summary>
    public abstract int GetEquippedScopeIndex();

    /// <summary>
    /// 레이저 인덱스를 리턴합니다.
    /// </summary>
    public abstract int GetEquippedLaserIndex();
    
    /// <summary>
    /// 그립 인덱스를 리턴합니다.
    /// </summary>
    public abstract int GetEquippedGripIndex();

    /// <summary>
    /// 모든 그립 Renderer를 끕니다.
    /// </summary>
    public abstract void FPGripsOff();

    /// <summary>
    /// 모든 스코프 Renderer를 끕니다.
    /// </summary>
    public abstract void FPScopesOff();

    /// <summary>
    /// 모든 총구 Renderer를 끕니다.
    /// </summary>
    public abstract void FPMuzzlesOff();

    /// <summary>
    /// 모든 레이저 Renderer를 끕니다.
    /// </summary>
    public abstract void FPLasersOff();

    /// <summary>
    /// 모든 탄창 Renderer를 끕니다.
    /// </summary>
    public abstract void FPMagazinesOff();

    #endregion
}

