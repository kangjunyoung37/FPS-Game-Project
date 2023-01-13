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
    /// 플레이어의 이름을 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public abstract string GetPlayerName();

    /// <summary>
    /// running 값을 리턴합니다.
    /// </summary>
    public abstract bool GetRunning();

    /// <summary>
    /// TPRenController를 리턴합니다.
    /// </summary>
    public abstract TPRenController GetTPRenController();
    
    /// <summary>
    /// 캐릭터가 연속적으로 발사한 샷의 양을 반환하는 함수
    /// 반동을 적용하고 스프레드를 수정하기 위해 사용 
    /// </summary>
    public abstract int GetShotsFired();
    
    /// <summary>
    /// 캐릭터가 무기를 내리고 있는지 확인하는 함수
    /// </summary>
    public abstract bool IsLowered();
    
    /// <summary>
    /// 플레이어의 메인 카메라를 리턴
    /// </summary>
    public abstract Camera GetCameraWold();

    /// <summary>
    /// 플레이어의 딥 카메라를 리턴
    /// </summary>
    public abstract Camera GetCameraDepth();

    //인벤토리 구성요소에 대한 참조를 리턴
    public abstract InventoryBehaviour GetInventory();

    /// <summary>
    /// 폭탄의 남은 양을 반환
    /// </summary>
    public abstract int GetGrenadesCurrent();
    
    /// <summary>
    /// 폭탄의 총 개수를 반환
    /// </summary>
    public abstract int GetGrenadesTotal();

    /// <summary>
    /// 현재 캐릭터가 달리고 있는지 반환
    /// </summary>
    public abstract bool IsRunning();

    /// <summary>
    /// 캐릭터가 무기를 들고 있는지 반환
    /// </summary>
    public abstract bool IsHolstered();

    /// <summary>
    /// 캐릭터가 웅크리고 있는지 반환
    /// </summary>
    public abstract bool IsCrouching();

    /// <summary>
    /// 재장전중인지 반환
    /// </summary>
    public abstract bool IsReloading();

    /// <summary>
    /// 슈류탄을 던지고 있는지 반환
    /// </summary>
    public abstract bool IsTrowingGrenade();

    /// <summary>
    /// 근접공격중인지 반환
    /// </summary>
    public abstract bool IsMeleeing();
    
    /// <summary>
    /// 캐릭터가 에임중인지 반환
    /// </summary>
    public abstract bool IsAiming();

    /// <summary>
    /// 게임 커서가 잠겨있는지 반환
    /// </summary>
    public abstract bool isCursorLocked();

    /// <summary>
    /// 움직임 인풋을 반환
    /// </summary>
    public abstract Vector2 GetInputMovement();

    /// <summary>
    /// Look인풋을 반환(어디를 보고 있는지)
    /// </summary>
    public abstract Vector2 GetInputLook();

    /// <summary>
    /// 캐릭터가 수류탄을 던졌을때 플레이 되는 오디오 클립을 반환
    /// </summary>
    public abstract AudioClip[] GetAudioClipsGrenadeThrow();

    /// <summary>
    /// 캐릭터가 근접공격할때 플레이 되는 오디오 클립을 반환
    /// </summary>
    /// <returns></returns>
    public abstract AudioClip[] GetAudioClipsMelee();

    /// <summary>
    /// 플레이어가 검사중인지를 리턴
    /// </summary>
    public abstract bool IsInspecting();

    /// <summary>
    /// 플레이어가 발사버튼을 계속 누르고 있는지 반환
    /// </summary>
    public abstract bool isHoldingButtonFire();

    /// <summary>
    /// 플레이어의 포톤뷰를 리턴합니다.
    /// </summary>
    public abstract PhotonView GetPhotonView();

    /// <summary>
    /// TPWeapon에 있는 오디오 소스를 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public abstract AudioSource GetTPWeaponAudioSource();

    /// <summary>
    /// 지금 장착하고 있는 무기를 리턴합니다.
    /// </summary>
    public abstract WeaponBehaviour GetWeaponBehaviour();

    /// <summary>
    /// 플레이어가 죽었는지를 리턴합니다.
    /// </summary>
    public abstract bool GetPlayerDead();

    /// <summary>
    /// 플레이어의 팀을 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public abstract int GetPlayerTeam();

    /// <summary>
    /// 적의 위치를 받아옴
    /// </summary>
    public abstract Transform GetEnemyCharacterBehaviour();

    /// <summary>
    /// 메인 무기의 index를 리턴합니다.
    /// </summary>
    public abstract int GetMainWeaponIndex();

    /// <summary>
    /// 서브 무기의 index를 리턴합니다.
    /// </summary>
    public abstract int GetSubWeapnIndex();

    #endregion GETTER

    #region ANIMATION

    /// <summary>
    /// 장착한 무기에서 탄피를 꺼내는 애니메이션
    /// </summary>
    public abstract void EjectCasing();

    /// <summary>
    /// 캐릭터가 장착한 무기의 탄약을 일정량 채우거나 -1로 설정하면 완전히 채웁니다.
    /// </summary>
    /// <param name="amount">탄약의 양</param>
    public abstract void FillAmmunition(int amount);

    /// <summary>
    /// 수류탄을 던집니다.
    /// </summary>
    public abstract void Grenade();

    /// <summary>
    /// 장착 무기의 탄창을 활성화하거나 비활성화 합니다.
    /// </summary>
    public abstract void SetActiveMagzine(int active);

    /// <summary>
    /// 볼트 애니메이션 종료
    /// </summary>
    public abstract void AnimationEndedBolt();

    /// <summary>
    /// 재장전 애니메이션 종류
    /// </summary>
    public abstract void AnimationEndedReload();

    /// <summary>
    /// 수류탄 던지는 애니메이션 종료
    /// </summary>
    public abstract void AnimationEndedGrenadeThrow();

    /// <summary>
    /// 근접 공격 애니메이션 종료
    /// </summary>
    public abstract void AnimationEndedMelee();

    /// <summary>
    /// 검사 애니메이션 종료
    /// </summary>
    public abstract void AnimationEndedInspect();

    /// <summary>
    /// 무기를 드는 애니메이션 종료
    /// </summary>
    public abstract void AnimationEndedHolster();

    /// <summary>
    /// 장착중인 무기의 슬라이드 백 포즈를 설정합니다.
    /// </summary>
    public abstract void SetSlideBack(int back);

    /// <summary>
    /// 칼 Active 활성화
    /// </summary>
    public abstract void SetActiveKnife(int active);

    /// <summary>
    /// 탄창 떨어트리기
    /// </summary>
    public abstract void DropMagazine(bool drop);

    #endregion ANIMATION

    #region PhotonNetwork

    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

    #endregion

}
