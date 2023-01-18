using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MuzzleBehaviour : MonoBehaviour
{
    #region GETTERS

    /// <summary>
    /// 총소리 범위
    /// </summary>
    public abstract float GetShotSoundRange();

    /// <summary>
    /// 총구의 위치를 리턴합니다.
    /// </summary>
    public abstract Transform GetSocket();

    /// <summary>
    /// 인터페이스 쓰일 총구 Sprite를 리턴합니다.
    /// </summary>
    public abstract Sprite GetSprite();

    /// <summary>
    /// 발사할때 나는 사운드를 리턴합니다.
    /// </summary>
    public abstract AudioClip GetAudioClipFire();

    /// <summary>
    /// 발사할때 사용하기 위한 Particle를 리턴합니다.
    /// </summary>
    public abstract ParticleSystem GetParticleFire();

    /// <summary>
    /// 발사할 때 방출할 입자의 수를 리턴합니다.
    /// </summary>
    public abstract int GetParticleFireCount();

    /// <summary>
    /// 발사할때 사용되는 light 컴포넌트를 리턴합니다.
    /// </summary>
    public abstract Light GetFlashLight();

    /// <summary>
    /// 플래쉬가 사라지는데 걸리는 시간을 리턴합니다.
    /// </summary>
    public abstract float GetFlashLightDuration();
    #endregion

    #region METHODS
    /// <summary>
    /// 모든 총구 효과를 재생시킵니다.
    /// </summary>
    public abstract void Effect();

    /// <summary>
    /// 총구 랜더러를 끕니다.
    /// </summary>
    public abstract void FPMuzzleOff();
    #endregion
}
