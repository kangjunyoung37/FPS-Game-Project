using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using System;

public abstract class CharacterBehaviour : MonoBehaviourPunCallbacks, IPunObservable
{

    public Action OnCharacterDie;

    #region UNITY
    protected virtual void Awake(){}
    protected virtual void Start() {}
    protected virtual void Update() {}
    protected virtual void LateUpdate() {}
    #endregion UNITY

    #region GETTER

    /// <summary>
    /// �÷��̾��� �̸��� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public abstract string GetPlayerName();

    /// <summary>
    /// running ���� �����մϴ�.
    /// </summary>
    public abstract bool GetRunning();

    /// <summary>
    /// TPRenController�� �����մϴ�.
    /// </summary>
    public abstract TPRenController GetTPRenController();
    
    /// <summary>
    /// ĳ���Ͱ� ���������� �߻��� ���� ���� ��ȯ�ϴ� �Լ�
    /// �ݵ��� �����ϰ� �������带 �����ϱ� ���� ��� 
    /// </summary>
    public abstract int GetShotsFired();
    
    /// <summary>
    /// ĳ���Ͱ� ���⸦ ������ �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    public abstract bool IsLowered();
    
    /// <summary>
    /// �÷��̾��� ���� ī�޶� ����
    /// </summary>
    public abstract Camera GetCameraWold();

    /// <summary>
    /// �÷��̾��� �� ī�޶� ����
    /// </summary>
    public abstract Camera GetCameraDepth();

    //�κ��丮 ������ҿ� ���� ������ ����
    public abstract InventoryBehaviour GetInventory();

    /// <summary>
    /// ��ź�� ���� ���� ��ȯ
    /// </summary>
    public abstract int GetGrenadesCurrent();
    
    /// <summary>
    /// ��ź�� �� ������ ��ȯ
    /// </summary>
    public abstract int GetGrenadesTotal();

    /// <summary>
    /// ���� ĳ���Ͱ� �޸��� �ִ��� ��ȯ
    /// </summary>
    public abstract bool IsRunning();

    /// <summary>
    /// ĳ���Ͱ� ���⸦ ��� �ִ��� ��ȯ
    /// </summary>
    public abstract bool IsHolstered();

    /// <summary>
    /// ĳ���Ͱ� ��ũ���� �ִ��� ��ȯ
    /// </summary>
    public abstract bool IsCrouching();

    /// <summary>
    /// ������������ ��ȯ
    /// </summary>
    public abstract bool IsReloading();

    /// <summary>
    /// ����ź�� ������ �ִ��� ��ȯ
    /// </summary>
    public abstract bool IsTrowingGrenade();

    /// <summary>
    /// �������������� ��ȯ
    /// </summary>
    public abstract bool IsMeleeing();
    
    /// <summary>
    /// ĳ���Ͱ� ���������� ��ȯ
    /// </summary>
    public abstract bool IsAiming();

    /// <summary>
    /// ���� Ŀ���� ����ִ��� ��ȯ
    /// </summary>
    public abstract bool isCursorLocked();

    /// <summary>
    /// ������ ��ǲ�� ��ȯ
    /// </summary>
    public abstract Vector2 GetInputMovement();

    /// <summary>
    /// Look��ǲ�� ��ȯ(��� ���� �ִ���)
    /// </summary>
    public abstract Vector2 GetInputLook();

    /// <summary>
    /// ĳ���Ͱ� ����ź�� �������� �÷��� �Ǵ� ����� Ŭ���� ��ȯ
    /// </summary>
    public abstract AudioClip[] GetAudioClipsGrenadeThrow();

    /// <summary>
    /// ĳ���Ͱ� ���������Ҷ� �÷��� �Ǵ� ����� Ŭ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public abstract AudioClip[] GetAudioClipsMelee();

    /// <summary>
    /// �÷��̾ �˻��������� ����
    /// </summary>
    public abstract bool IsInspecting();

    /// <summary>
    /// �÷��̾ �߻��ư�� ��� ������ �ִ��� ��ȯ
    /// </summary>
    public abstract bool isHoldingButtonFire();

    /// <summary>
    /// �÷��̾��� ����並 �����մϴ�.
    /// </summary>
    public abstract PhotonView GetPhotonView();

    /// <summary>
    /// TPWeapon�� �ִ� ����� �ҽ��� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public abstract AudioSource GetTPWeaponAudioSource();

    /// <summary>
    /// ���� �����ϰ� �ִ� ���⸦ �����մϴ�.
    /// </summary>
    public abstract WeaponBehaviour GetWeaponBehaviour();

    /// <summary>
    /// �÷��̾ �׾������� �����մϴ�.
    /// </summary>
    public abstract bool GetPlayerDead();

    /// <summary>
    /// �÷��̾��� ���� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public abstract int GetPlayerTeam();

    /// <summary>
    /// ���� ��ġ�� �޾ƿ�
    /// </summary>
    public abstract Transform GetEnemyCharacterBehaviour();

    /// <summary>
    /// ���� ������ index�� �����մϴ�.
    /// </summary>
    public abstract int GetMainWeaponIndex();

    /// <summary>
    /// ���� ������ index�� �����մϴ�.
    /// </summary>
    public abstract int GetSubWeapnIndex();

    #endregion GETTER

    #region ANIMATION

    /// <summary>
    /// ������ ���⿡�� ź�Ǹ� ������ �ִϸ��̼�
    /// </summary>
    public abstract void EjectCasing();

    /// <summary>
    /// ĳ���Ͱ� ������ ������ ź���� ������ ä��ų� -1�� �����ϸ� ������ ä��ϴ�.
    /// </summary>
    /// <param name="amount">ź���� ��</param>
    public abstract void FillAmmunition(int amount);

    /// <summary>
    /// ����ź�� �����ϴ�.
    /// </summary>
    public abstract void Grenade();

    /// <summary>
    /// ���� ������ źâ�� Ȱ��ȭ�ϰų� ��Ȱ��ȭ �մϴ�.
    /// </summary>
    public abstract void SetActiveMagzine(int active);

    /// <summary>
    /// ��Ʈ �ִϸ��̼� ����
    /// </summary>
    public abstract void AnimationEndedBolt();

    /// <summary>
    /// ������ �ִϸ��̼� ����
    /// </summary>
    public abstract void AnimationEndedReload();

    /// <summary>
    /// ����ź ������ �ִϸ��̼� ����
    /// </summary>
    public abstract void AnimationEndedGrenadeThrow();

    /// <summary>
    /// ���� ���� �ִϸ��̼� ����
    /// </summary>
    public abstract void AnimationEndedMelee();

    /// <summary>
    /// �˻� �ִϸ��̼� ����
    /// </summary>
    public abstract void AnimationEndedInspect();

    /// <summary>
    /// ���⸦ ��� �ִϸ��̼� ����
    /// </summary>
    public abstract void AnimationEndedHolster();

    /// <summary>
    /// �������� ������ �����̵� �� ��� �����մϴ�.
    /// </summary>
    public abstract void SetSlideBack(int back);

    /// <summary>
    /// Į Active Ȱ��ȭ
    /// </summary>
    public abstract void SetActiveKnife(int active);

    /// <summary>
    /// źâ ����Ʈ����
    /// </summary>
    public abstract void DropMagazine(bool drop);

    #endregion ANIMATION

    #region PhotonNetwork

    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

    #endregion

}
