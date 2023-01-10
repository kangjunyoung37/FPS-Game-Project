using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundCharacterBehaviour : StateMachineBehaviour
{
   private enum SoundType
    {
        //캐릭터 액션
        GrenadeThrow, Melee,
        //홀스터
        Holster, Unholster,
        //보통 재장전
        Reload,ReloadEmpty,
        //재장전 사이클
        ReloadOpen,ReloadInsert,ReloadClose,
        //발사
        Fire,FireEmpty,
        //볼트액션
        BoltAction,
    }

    #region FIELDS SERIALIZED

    [Title(label: "Setup")]
    [Tooltip("오디오가 재생되는 지연 시간")]
    [SerializeField]
    private float delay;

    [Tooltip("무기 사운드 재생 타입")]
    [SerializeField]
    private SoundType soundType;

    [Title(label: "Audio Settings")]

    [Tooltip("오디오 세팅")]
    [SerializeField]
    private AudioSettings audioSettings = new AudioSettings(1.0f, 0.0f, true);

    #endregion

    #region FIELDS

    /// <summary>
    /// 플레이어 캐릭터Behaviour
    /// </summary>
    private CharacterBehaviour playerCharacter;

    /// <summary>
    /// 플레이어 인벤토리
    /// </summary>
    private InventoryBehaviour playerInventory;

    /// <summary>
    /// 소리를 처리하는 서비스
    /// </summary>
    private IAudioManagerService audioManagerService;

    #endregion

    #region UNITY

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //캐릭터 컴포넌트를 가져온다.
        playerCharacter = animator.GetComponentInParent<CharacterBehaviour>();
        //인벤토리 가져오기
        playerInventory ??= playerCharacter.GetInventory();

        if (!(playerInventory.GetEquipped() is { } weaponBehaviour))
            return;

        audioManagerService ??= ServiceLocator.Current.Get<IAudioManagerService>();

        #region Select Correct Clip To Play

        AudioClip clip = soundType switch
        {
            //수류탄 던지기
            SoundType.GrenadeThrow => playerCharacter.GetAudioClipsGrenadeThrow().GetRandom(),
            //근접 공격
            SoundType.Melee => playerCharacter.GetAudioClipsMelee().GetRandom(),

            //무기 집어넣기
            SoundType.Holster => weaponBehaviour.GetAudioClipHolster(),
            //무기 꺼내기
            SoundType.Unholster => weaponBehaviour.GetAudioClipUnHolster(),

            //재장전
            SoundType.Reload => weaponBehaviour.GetAudioClipReload(),
            //빈 재장전 
            SoundType.ReloadEmpty => weaponBehaviour.GetAudioClipReloadEmpty(),

            //재장전 열기
            SoundType.ReloadOpen => weaponBehaviour.GetAudioClipReloadOpen(),
            //재장전 넣기
            SoundType.ReloadInsert => weaponBehaviour.GetAudioClipReloadInsert(),
            //재장전 닫기
            SoundType.ReloadClose => weaponBehaviour.GetAudioClipReloadClose(),

            //발사
            SoundType.Fire => weaponBehaviour.GetAudioClipFire(),
            //발사했는데 총알이 없을 때
            SoundType.FireEmpty => weaponBehaviour.GetAudioClipFireEmpty(),

            //볼트 액션
            SoundType.BoltAction => weaponBehaviour.GetAudioClipBoltAction(),

            //Defalut
            _ => default
        };

        #endregion
        //딜레이를 가지고 재생됩니다. 만약 딜레이가 0이면 바로 재생될겁니다.
        audioManagerService.PlayOneShotDelayed(clip, audioSettings, delay);
    }


    #endregion

}
