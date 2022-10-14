using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehaviour : MonoBehaviour
{
    #region UNITY

    protected virtual void Awake() { }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void LateUpdate() { }


    #endregion

    #region GETTERS

    /// <summary>
    /// ���� ��ü�� ǥ���� �� ����� ��������Ʈ�� �����մϴ�.
    /// </summary>
    public abstract Sprite GetSpriteBody();
    /// <summary>
    /// ���� �̵� �ӵ� ���� ��ȯ�մϴ�.
    /// </summary>
    public abstract float GetMultipleierMovementSpeed();

    /// <summary>
    /// ���⸦ �ִ� ���� Ŭ���� �����մϴ�.
    /// </summary>
    public abstract AudioClip GetAudioClipHolster();

    /// <summary>
    /// ���⸦ ���� ���� Ŭ���� �����մϴ�.
    /// </summary>
    public abstract AudioClip GetAudioClipUnHolster();

    /// <summary>
    /// ������ ���� Ŭ���� �����մϴ�.
    /// </summary>
    public abstract AudioClip GetAudioClipReload();

    /// <summary>
    /// �� ������ ���带 �����մϴ�.
    /// </summary>
    public abstract AudioClip GetAudioClipReloadEmpty();

    /// <summary>
    /// ������ ���� ���带 �����մϴ�.
    /// </summary>
    public abstract AudioClip GetAudioClipReloadOpen();

    /// <summary>
    /// ������ ���� ���带 �����մϴ�
    /// </summary>
    public abstract AudioClip GetAudioClipReloadInsert();

    /// <summary>
    /// ������ ������ ���带 �����մϴ�.
    /// </summary>
    public abstract AudioClip GetAudioClipReloadClose();

    /// <summary>
    /// �Ѿ��� ���� �� �߻��ϸ� ���� ���带 �����մϴ�.
    /// </summary>
    public abstract AudioClip GetAudioClipFireEmpty();

    /// <summary>
    /// ��Ʈ �۵� �Ҹ��� �����մϴ�.
    /// </summary>
    public abstract AudioClip GetAudioClipBoltAction();

    /// <summary>
    /// �� �߻� ����
    /// </summary>
    public abstract AudioClip GetAudioClipFire();

    /// <summary>
    /// ���� ������ �ִ� ź����� �����մϴ�.
    /// </summary>
    public abstract int GetAmmunitionCurrent();

    /// <summary>
    /// ������ �ִ� ��ü ź����� �����մϴ�.
    /// </summary>
    public abstract int GetAmmunitionTotal();

    /// <summary>
    /// �� ���Ⱑ �ֱ������� �������Ǵ��� ���θ� �����Ͽ� �����մϴ�.
    /// </summary>
    public abstract bool HasCycledReload();

    /// <summary>
    /// ���� ���ϸ����� ������Ʈ�� �����մϴ�.
    /// </summary>
    public abstract Animator GetAnimator();

    /// <summary>
    /// �ڵ��߻� ������ ���� �����մϴ�.
    /// </summary>
    public abstract bool IsAutomatic();

    /// <summary>
    /// ���⿡ ź���� ���������� ���� �����մϴ�.
    /// </summary>
    public abstract bool HasAmmunition();

    /// <summary>
    /// ź���� �������ִٸ� ���� �����մϴ�.
    /// </summary>
    public abstract bool IsFull();

    /// <summary>
    /// ��Ʈ �۵� ������ ��� ���� �����մϴ�.
    /// </summary>
    public abstract bool IsBoltAction();

    /// <summary>
    /// ��� ���� �� ���⸦ �ڵ����� �ٽ� �������ؾ��ϴ� ��� ���� �����մϴ�.
    /// </summary>
    public abstract bool GetAutomaticallyReloadOnEmpty();

    /// <summary>
    /// ������ ��� ���� ���Ⱑ �������� ���۵� �������� ���� �ð��� �����մϴ�.
    /// </summary>
    public abstract float GetAutomaticallyReloadOnEmptyDelay();

    /// <summary>
    /// ź���� FULL�� ������ �� �������� �� �� �ִ����� �����մϴ�.
    /// </summary>
    public abstract bool CanReloadWhenFull();

    /// <summary>
    /// ������ �߻� �ӵ��� �����մϴ�.
    /// </summary>
    public abstract float GetRateOfFire();

    /// <summary>
    /// ������ �� �þ� Mutiplier�� �����մϴ�.
    /// </summary>
    public abstract float GetFieldOfViewMutiplierAim();

    /// <summary>
    /// ���� ������ ��� ���ؽ� Mutiplier�� �����մϴ�.
    /// </summary>
    public abstract float GetFieldOfViewMutiplierAimWeapon();

    /// <summary>
    /// �� ���⸦ ������ ĳ���Ͱ� �ʿ�� �ϴ� RutimeAnimator�� �����մϴ�.
    /// </summary>
    public abstract RuntimeAnimatorController GetAnimatorController();

    /// <summary>
    /// ���� ������ �Ŵ��� ������Ʈ�� �����մϴ� 
    /// </summary>
    //public abstract WeaponAttachmentManagerBehaviour GetAttachmentManager();
    #endregion

    #region METHODS

    /// <summary>
    /// ���� �߻��ϱ�
    /// </summary>
    /// <param name="spreadMultiplier">������ ź������ ���� ���Դϴ�.</param>
    public abstract void Fire(float spreadMutiplier = 1.0f);

    /// <summary>
    /// ���� ������
    /// </summary>
    public abstract void Reload();

    /// <summary>
    /// ź�� ä���
    /// </summary>
    /// <param name="amount">ź�� ��</param>
    public abstract void FillAmmunition(int amount);

    /// <summary>
    /// �����̵� �� ��� �����մϴ�.
    /// </summary>
    /// <param name="back"></param>
    public abstract void SetSlideBack(int back);

    /// <summary>
    /// ���⿡�� ź�Ǹ� �����ϴ�. �Ϲ������� �ִϸ��̼� �̺�Ʈ���� ȣ������� ��𿡼��� ȣ���� �� �ֽ��ϴ�.
    /// </summary>
    public abstract void EjectCasing();
    #endregion
}
