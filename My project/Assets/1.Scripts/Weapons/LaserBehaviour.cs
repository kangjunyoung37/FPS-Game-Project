using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaserBehaviour : MonoBehaviour
{
    #region GETTHERS

    /// <summary>
    /// 사용자 인터페이스에 쓰일 스프라이트를 리턴합니다.
    /// </summary>
    public abstract Sprite GetSprite();

    /// <summary>
    /// 캐릭터가 달리는 동안 레이저를 꺼야하는 경우 참을 리턴합니다.
    /// </summary>
    public abstract bool GetTurnOffWhileRunning();

    /// <summary>
    /// 캐릭터가 에임하는 동안 레이저를 꺼야하는 경우 참을 리턴합니다.
    /// </summary>
    public abstract bool GetTurnOffWhileAiming();

    /// <summary>
    /// 레이저 토글하기
    /// </summary>
    public abstract void Toggle();

    /// <summary>
    /// 레이저 재적용하기
    /// </summary>
    public abstract void Reapply();

    /// <summary>
    /// 레이저 숨기기
    /// </summary>
    public abstract void Hide();

    /// <summary>
    /// 레이저 Renderer끄기
    /// </summary>
    public abstract void FPLaserOff();
    #endregion
}
