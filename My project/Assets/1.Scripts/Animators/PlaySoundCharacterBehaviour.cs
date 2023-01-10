using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundCharacterBehaviour : StateMachineBehaviour
{
   private enum SoundType
    {
        //ĳ���� �׼�
        GrenadeThrow, Melee,
        //Ȧ����
        Holster, Unholster,
        //���� ������
        Reload,ReloadEmpty,
        //������ ����Ŭ
        ReloadOpen,ReloadInsert,ReloadClose,
        //�߻�
        Fire,FireEmpty,
        //��Ʈ�׼�
        BoltAction,
    }

    #region FIELDS SERIALIZED

    [Title(label: "Setup")]
    [Tooltip("������� ����Ǵ� ���� �ð�")]
    [SerializeField]
    private float delay;

    [Tooltip("���� ���� ��� Ÿ��")]
    [SerializeField]
    private SoundType soundType;

    [Title(label: "Audio Settings")]

    [Tooltip("����� ����")]
    [SerializeField]
    private AudioSettings audioSettings = new AudioSettings(1.0f, 0.0f, true);

    #endregion

    #region FIELDS

    /// <summary>
    /// �÷��̾� ĳ����Behaviour
    /// </summary>
    private CharacterBehaviour playerCharacter;

    /// <summary>
    /// �÷��̾� �κ��丮
    /// </summary>
    private InventoryBehaviour playerInventory;

    /// <summary>
    /// �Ҹ��� ó���ϴ� ����
    /// </summary>
    private IAudioManagerService audioManagerService;

    #endregion

    #region UNITY

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ĳ���� ������Ʈ�� �����´�.
        playerCharacter = animator.GetComponentInParent<CharacterBehaviour>();
        //�κ��丮 ��������
        playerInventory ??= playerCharacter.GetInventory();

        if (!(playerInventory.GetEquipped() is { } weaponBehaviour))
            return;

        audioManagerService ??= ServiceLocator.Current.Get<IAudioManagerService>();

        #region Select Correct Clip To Play

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

            //������
            SoundType.Reload => weaponBehaviour.GetAudioClipReload(),
            //�� ������ 
            SoundType.ReloadEmpty => weaponBehaviour.GetAudioClipReloadEmpty(),

            //������ ����
            SoundType.ReloadOpen => weaponBehaviour.GetAudioClipReloadOpen(),
            //������ �ֱ�
            SoundType.ReloadInsert => weaponBehaviour.GetAudioClipReloadInsert(),
            //������ �ݱ�
            SoundType.ReloadClose => weaponBehaviour.GetAudioClipReloadClose(),

            //�߻�
            SoundType.Fire => weaponBehaviour.GetAudioClipFire(),
            //�߻��ߴµ� �Ѿ��� ���� ��
            SoundType.FireEmpty => weaponBehaviour.GetAudioClipFireEmpty(),

            //��Ʈ �׼�
            SoundType.BoltAction => weaponBehaviour.GetAudioClipBoltAction(),

            //Defalut
            _ => default
        };

        #endregion
        //�����̸� ������ ����˴ϴ�. ���� �����̰� 0�̸� �ٷ� ����ɰ̴ϴ�.
        audioManagerService.PlayOneShotDelayed(clip, audioSettings, delay);
    }


    #endregion

}
