using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScopeBehaviour : MonoBehaviour
{

    #region GETTERS

    /// <summary>
    /// 마우스 감도 곱셈값을 리턴합니다.
    /// </summary>
    public abstract float GetMutiplierMouseSensitivity();

    /// <summary>
    /// 곱셈Spread값을 리턴합니다.
    /// </summary>
    public abstract float GetMultiplierSpread();

    /// <summary>
    /// 조준 위치 오프셋 값을 리턴합니다.
    /// </summary>
    public abstract Vector3 GetOffsetAimingLocation();

    /// <summary>
    /// 조준 로테이션 오프셋 값을 리턴합니다.
    /// </summary>
    public abstract Vector3 GetOffsetAimingRotation();

    /// <summary>
    /// FOV 에임 곱셈값을 리턴합니다
    /// </summary>
    public abstract float GetFieldOfViewMutiplierAim();
    /// <summary>
    /// 무기 에임 FOV곱셈값을 리턴합니다.
    /// </summary>
    public abstract float GetFieldOfViewMutiplierAimWeapon();

    /// <summary>
    /// 인터페이스에 쓰일 Sprite값을 리턴합니다.
    /// </summary>
    public abstract Sprite GetSprite();

    /// <summary>
    /// 이 스코프로 조준하는 동안 무기 흔들림을 곱할 값을 리턴합니다.
    /// </summary>
    public abstract float GetSwayMutiplier();

    #endregion

    #region METHODS
    /// <summary>
    /// 스코프를 사용하는 캐릭터가 스코프를 조준할 때 호출됩니다.
    /// </summary>
    public abstract void OnAim();

    /// <summary>
    /// 스코프를 사용하는 캐릭터가 스코프 조준을 해제할 때 호출됩니다.
    /// </summary>
    public abstract void OnAimStop();

    /// <summary>
    /// 스코프 랜더러를 끕니다.
    /// </summary>
    public abstract void FPScopeRenOff();

    /// <summary>
    /// 랜더러 카메라를 끕니다.
    /// </summary>
    public abstract void RenDisable();
    #endregion
}
