using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPPlaySoundCharacterBahaviour : StateMachineBehaviour
{
    private enum SoundType
    {
        GrenadeThrow, Melee,
        //홀스터
        Holster, Unholster,
        //재장전 사이클
        ReloadOpen, ReloadInsert, ReloadClose,
        //볼트액션
        BoltAction,
    }

    #region FIELDS SERIALIZED

    [Title(label: "SetUp")]

    [SerializeField]
    private SoundType soundType;

    #endregion

    #region FUELDS
    
    private PhotonView PV;

    private AudioSource audioSource;

    private CharacterBehaviour playerCharacter;

    #endregion

    #region UNITY

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
     
        playerCharacter = animator.GetComponentInParent<CharacterBehaviour>();
        PV = playerCharacter.GetPhotonView();
        if (PV.IsMine)
            return;
        audioSource = playerCharacter.GetTPWeaponAudioSource();

        if (!(playerCharacter.GetWeaponBehaviour() is { } weaponBehaviour))
            return;

        #region Select Correct Clip to Play

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

            //재장전 열기
            SoundType.ReloadOpen => weaponBehaviour.GetAudioClipReloadOpen(),
            //재장전 넣기
            SoundType.ReloadInsert => weaponBehaviour.GetAudioClipReloadInsert(),
            //재장전 닫기
            SoundType.ReloadClose => weaponBehaviour.GetAudioClipReloadClose(),

            //볼트 액션
            SoundType.BoltAction => weaponBehaviour.GetAudioClipBoltAction(),

            //Defalut
            _ => default
        };

        audioSource.PlayOneShot(clip, 1.0f); 

        #endregion






    }


    #endregion
}
