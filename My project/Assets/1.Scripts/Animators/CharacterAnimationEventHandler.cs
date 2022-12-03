using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 캐릭터에서 가져온 모든 애니메이션을 관리합니다.
/// </summary>
public class CharacterAnimationEventHandler : MonoBehaviour
{

    #region FIELDS

    /// <summary>
    /// 캐릭터 컴포넌트
    /// </summary>
    private CharacterBehaviour playerCharacter;
    private Character Character;

    #endregion

    #region UNITY

    private void Awake()
    {
        //캐릭터 Behaviour 컴포넌트 가져오기
        //playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        playerCharacter = transform.gameObject.GetComponentInParent<CharacterBehaviour>();
        
        Character = playerCharacter.GetComponent<Character>();
        
    }

    #endregion

    #region ANIMATION

    /// <summary>
    /// 캐릭터의 장비된 무기에서 탄피를 꺼냅니다.
    /// </summary>
    private void OnEjectCasing()
    {
        if (playerCharacter != null)
            playerCharacter.EjectCasing();
    }

    /// <summary>
    /// 캐릭터에 장착된 무기의 탄약을 일정량 채우거나 0으로 설정한 경우 완전히 채웁니다.
    /// </summary>
    private void OnAmmunitionFill(int amount = 0)
    {
        if (playerCharacter != null)
            playerCharacter.FillAmmunition(amount);
    }

    /// <summary>
    /// 캐릭터의 나이프를 활성화합니다.
    /// </summary>
    private void OnSetActiveKnife(int active)
    {
        if (playerCharacter != null)
            playerCharacter.SetActiveKnife(active);
    }

    /// <summary>
    /// 정확한 위치에서 수류탄을 스폰합니다.
    /// </summary>
    private void OnGrenade()
    {
        if (playerCharacter != null)
            playerCharacter.Grenade();
    }

    /// <summary>
    /// 장착된 무기의 탄창을 활성화합니다.
    /// </summary>
    private void OnSetActiveMagazine(int active)
    {
        if (playerCharacter != null)
            playerCharacter.SetActiveMagzine(active);
    }

    /// <summary>
    /// 볼트 액션 애니메이션을 끝냅니다.
    /// </summary>
    private void OnAnimationEndedBolt()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedBolt();
    }

    /// <summary>
    /// 재장전 애니메이션을 끝냅니다.
    /// </summary>
    private void OnAnimationEndedReload()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedReload();
    }

    /// <summary>
    /// 수류탄 던지는 애니메이션을 끝냅니다.
    /// </summary>
    private void OnAnimationEndedGrenadeThrow()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedGrenadeThrow();
    }

    /// <summary>
    /// 근접 공격 애니메이션을 끝냅니다.
    /// </summary>
    private void OnAnimationEndedMelee()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedMelee();
    }

    /// <summary>
    /// 검사 애니메이션을 끝냅니다.
    /// </summary>
    private void OnAnimationEndedInspect()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedInspect();
    }

    /// <summary>
    /// 집어 넣는 애니메이션을 끝냅니다.
    /// </summary>
    private void OnAnimationEndedHolster()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedHolster();
    }

    /// <summary>
    /// 캐릭터가 장비한 무기의 슬라이드 백 포즈를 설정합니다.
    /// </summary>
    private void OnSlideBack(int back)
    {
        if (playerCharacter != null)
            playerCharacter.SetSlideBack(back);
    }

    private void OnDropMagazine(int drop = 0)
    {
        if (playerCharacter != null)
            playerCharacter.DropMagazine(drop == 0);
    }

    private void isgofalse()
    {
        Character.ishostering = false;
    }

    #endregion
}
