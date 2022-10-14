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
    /// 무기 몸체에 표시할 때 사용할 스프라이트를 리턴합니다.
    /// </summary>
    public abstract Sprite GetSpriteBody();
    /// <summary>
    /// 곱셈 이동 속도 값을 반환합니다.
    /// </summary>
    public abstract float GetMultipleierMovementSpeed();

    /// <summary>
    /// 무기를 넣는 사운드 클립을 리턴합니다.
    /// </summary>
    public abstract AudioClip GetAudioClipHolster();

    /// <summary>
    /// 무기를 빼는 사운드 클립을 리턴합니다.
    /// </summary>
    public abstract AudioClip GetAudioClipUnHolster();

    /// <summary>
    /// 재장전 사운드 클립을 리턴합니다.
    /// </summary>
    public abstract AudioClip GetAudioClipReload();

    /// <summary>
    /// 빈 재장전 사운드를 리턴합니다.
    /// </summary>
    public abstract AudioClip GetAudioClipReloadEmpty();

    /// <summary>
    /// 재장전 여는 사운드를 리턴합니다.
    /// </summary>
    public abstract AudioClip GetAudioClipReloadOpen();

    /// <summary>
    /// 재장전 삽입 사운드를 리턴합니다
    /// </summary>
    public abstract AudioClip GetAudioClipReloadInsert();

    /// <summary>
    /// 재장전 닫히는 사운드를 리턴합니다.
    /// </summary>
    public abstract AudioClip GetAudioClipReloadClose();

    /// <summary>
    /// 총알이 없을 때 발사하면 나는 사운드를 리턴합니다.
    /// </summary>
    public abstract AudioClip GetAudioClipFireEmpty();

    /// <summary>
    /// 볼트 작동 소리를 리턴합니다.
    /// </summary>
    public abstract AudioClip GetAudioClipBoltAction();

    /// <summary>
    /// 총 발사 사운드
    /// </summary>
    public abstract AudioClip GetAudioClipFire();

    /// <summary>
    /// 현재 가지고 있는 탄약수를 리턴합니다.
    /// </summary>
    public abstract int GetAmmunitionCurrent();

    /// <summary>
    /// 가지고 있는 전체 탄약수를 리턴합니다.
    /// </summary>
    public abstract int GetAmmunitionTotal();

    /// <summary>
    /// 이 무기가 주기적으로 재장전되는지 여부를 결정하여 리턴합니다.
    /// </summary>
    public abstract bool HasCycledReload();

    /// <summary>
    /// 무기 에니메이터 컴포넌트를 리턴합니다.
    /// </summary>
    public abstract Animator GetAnimator();

    /// <summary>
    /// 자동발사 무기라면 참을 리턴합니다.
    /// </summary>
    public abstract bool IsAutomatic();

    /// <summary>
    /// 무기에 탄약이 남아있으면 참을 리턴합니다.
    /// </summary>
    public abstract bool HasAmmunition();

    /// <summary>
    /// 탄약이 가득차있다면 참을 리턴합니다.
    /// </summary>
    public abstract bool IsFull();

    /// <summary>
    /// 볼트 작동 무기일 경우 참을 리턴합니다.
    /// </summary>
    public abstract bool IsBoltAction();

    /// <summary>
    /// 비어 있을 때 무기를 자동으로 다시 재장전해야하는 경우 참을 리턴합니다.
    /// </summary>
    public abstract bool GetAutomaticallyReloadOnEmpty();

    /// <summary>
    /// 마지막 사격 이후 무기가 재장전이 시작될 때까지의 지연 시간을 리턴합니다.
    /// </summary>
    public abstract float GetAutomaticallyReloadOnEmptyDelay();

    /// <summary>
    /// 탄약이 FULL인 상태일 때 재장전을 할 수 있는지를 리턴합니다.
    /// </summary>
    public abstract bool CanReloadWhenFull();

    /// <summary>
    /// 무기의 발사 속도를 리턴합니다.
    /// </summary>
    public abstract float GetRateOfFire();

    /// <summary>
    /// 조준할 때 시야 Mutiplier를 리턴합니다.
    /// </summary>
    public abstract float GetFieldOfViewMutiplierAim();

    /// <summary>
    /// 무기 스코프 모드 조준시 Mutiplier를 리턴합니다.
    /// </summary>
    public abstract float GetFieldOfViewMutiplierAimWeapon();

    /// <summary>
    /// 이 무기를 장착한 캐릭터가 필요로 하는 RutimeAnimator를 리턴합니다.
    /// </summary>
    public abstract RuntimeAnimatorController GetAnimatorController();

    /// <summary>
    /// 무기 부착물 매니저 컴포넌트를 리턴합니다 
    /// </summary>
    //public abstract WeaponAttachmentManagerBehaviour GetAttachmentManager();
    #endregion

    #region METHODS

    /// <summary>
    /// 무기 발사하기
    /// </summary>
    /// <param name="spreadMultiplier">무기의 탄퍼짐에 곱할 값입니다.</param>
    public abstract void Fire(float spreadMutiplier = 1.0f);

    /// <summary>
    /// 무기 재장전
    /// </summary>
    public abstract void Reload();

    /// <summary>
    /// 탄약 채우기
    /// </summary>
    /// <param name="amount">탄약 양</param>
    public abstract void FillAmmunition(int amount);

    /// <summary>
    /// 슬라이드 백 포즈를 설정합니다.
    /// </summary>
    /// <param name="back"></param>
    public abstract void SetSlideBack(int back);

    /// <summary>
    /// 무기에서 탄피를 꺼냅니다. 일반적으로 애니메이션 이벤트에서 호출되지만 어디에서나 호출할 수 있습니다.
    /// </summary>
    public abstract void EjectCasing();
    #endregion
}
