using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̵� ��ȣ �ۿ��� ó���մϴ�.
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
    /// ĳ���Ͱ� ���������� ������ �ð��� �����մϴ�.
    /// </summary>
    public abstract float GetLastJumpTime();

    /// <summary>
    /// ������ ���� ������
    /// </summary>
    public abstract float GetMultiplierForward();

    /// <summary>
    /// ������ ���� ������
    /// </summary>
    public abstract float GetMultiplierSideways();

    /// <summary>
    /// �ڷ� ���� ������
    /// </summary>
    public abstract float GetMultiplierBackwards();

    /// <summary>
    /// ���� ĳ���Ͱ� ���� �ӵ�
    /// </summary>
    /// <returns></returns>
    public abstract Vector3 GetVelocity();

    /// <summary>
    /// ĳ���Ͱ� ���� �ִ����� �����մϴ�. 
    /// </summary>
    public abstract bool IsGrounded();

    /// <summary>
    /// ������ �������� IsGrounded ���� �����մϴ�.
    /// </summary>
    public abstract bool WasGrounded();

    /// <summary>
    /// �÷��̾ �������̸� true�� �����մϴ�.
    /// </summary>
    public abstract bool IsJumping();

    /// <summary>
    /// ĳ���Ͱ� Crouching���� newCrouching ������ ������ �� �ִ� ��� 
    /// true�� �����մϴ�.
    /// </summary>
    public abstract bool CanCrouch(bool newCrouching);

    /// <summary>
    /// ĳ���Ͱ� ��ũ���� �ִ� ���̶�� true�� �����մϴ�.
    /// </summary>
    public abstract bool IsCrouching();

    /// <summary>
    /// ĳ������ PVVelocity���� �����ɴϴ�.
    /// </summary>
    public abstract Vector3 GetPVVelocity();

    /// <summary>
    /// ĳ������ PVIsGrounded���� �����ɴϴ�.
    /// </summary>
    public abstract bool GetPVIsGrounded();

    #endregion

    #region METHODS

    /// <summary>
    /// ĳ���� �����ϱ�
    /// </summary>
    public abstract void Jump();

    /// <summary>
    /// ĳ���� ��ũ����
    /// </summary>
    public abstract void Crouch(bool newcrouching);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public abstract void TryCrouch(bool value);

    /// <summary>
    /// ��ũ���� �ִ� ���� ��ȯ�ϱ�
    /// </summary>
    public abstract void TryToggleCrouch();

    public abstract void OnPhotonSerializeView(PhotonStream stream, Photon.Pun.PhotonMessageInfo info);

    public abstract void PVJumping();
    #endregion
}
