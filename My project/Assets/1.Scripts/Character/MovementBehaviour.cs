using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이동 상호 작용을 처리합니다.
/// </summary>
public abstract class MovementBehaviour : MonoBehaviour
{
    #region UNITY

    protected virtual void Awake() { }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }

    #endregion

    #region GETTERS

    /// <summary>
    /// 캐릭터가 마지막으로 점프한 시간을 리턴합니다.
    /// </summary>
    public abstract float GetLastJumpTime();

    /// <summary>
    /// 앞으로 가는 곱셈값
    /// </summary>
    public abstract float GetMultiplierForward();

    /// <summary>
    /// 옆으로 가는 곱셈값
    /// </summary>
    public abstract float GetMultiplierSideways();

    /// <summary>
    /// 뒤로 가는 곱셈값
    /// </summary>
    public abstract float GetMultiplierBackwards();

    /// <summary>
    /// 현재 캐릭터가 가진 속도
    /// </summary>
    /// <returns></returns>
    public abstract Vector3 GetVelocity();

    /// <summary>
    /// 캐릭터가 땅에 있는지를 리턴합니다. 
    /// </summary>
    public abstract bool IsGrounded();

    /// <summary>
    /// 마지막 프레임의 IsGrounded 값을 리턴합니다.
    /// </summary>
    public abstract bool WasGrounded();

    /// <summary>
    /// 플레이어가 점프중이면 true를 리턴합니다.
    /// </summary>
    public abstract bool IsJumping();

    /// <summary>
    /// 캐릭터가 Crouching값을 newCrouching 값으로 설정할 수 있는 경우 
    /// true를 리턴합니다.
    /// </summary>
    public abstract bool CanCrouch(bool newCrouching);

    /// <summary>
    /// 캐릭터가 웅크리고 있는 중이라면 true를 리턴합니다.
    /// </summary>
    public abstract bool IsCrouching();
    #endregion

    #region METHODS

    /// <summary>
    /// 캐릭터 점프하기
    /// </summary>
    public abstract void Jump();

    /// <summary>
    /// 캐릭터 웅크리기
    /// </summary>
    public abstract void Crouch(bool newcrouching);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public abstract void TryCrouch(bool value);

    /// <summary>
    /// 웅크리고 있는 상태 전환하기
    /// </summary>
    public abstract void TryToggleCrouch();

    #endregion
}
