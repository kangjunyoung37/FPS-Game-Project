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
        //Ȧ����
        Holster, Unholster,
        //������ ����Ŭ
        ReloadOpen, ReloadInsert, ReloadClose,
        //��Ʈ�׼�
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
            //����ź ������
            SoundType.GrenadeThrow => playerCharacter.GetAudioClipsGrenadeThrow().GetRandom(),
            //���� ����
            SoundType.Melee => playerCharacter.GetAudioClipsMelee().GetRandom(),

            //���� ����ֱ�
            SoundType.Holster => weaponBehaviour.GetAudioClipHolster(),
            //���� ������
            SoundType.Unholster => weaponBehaviour.GetAudioClipUnHolster(),

            //������ ����
            SoundType.ReloadOpen => weaponBehaviour.GetAudioClipReloadOpen(),
            //������ �ֱ�
            SoundType.ReloadInsert => weaponBehaviour.GetAudioClipReloadInsert(),
            //������ �ݱ�
            SoundType.ReloadClose => weaponBehaviour.GetAudioClipReloadClose(),

            //��Ʈ �׼�
            SoundType.BoltAction => weaponBehaviour.GetAudioClipBoltAction(),

            //Defalut
            _ => default
        };

        audioSource.PlayOneShot(clip, 1.0f); 

        #endregion






    }


    #endregion
}
