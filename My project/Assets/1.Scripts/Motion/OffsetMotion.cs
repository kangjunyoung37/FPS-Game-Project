using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ϻ� ���� ���� ������� ������Ʈ�� �������ϴ� �� ���˴ϴ�.
/// </summary>
public class OffsetMotion : Motion
{
    #region FIEDLS SERIALIZED

    [Tooltip("ĳ���� feelManager ������Ʈ")]
    [SerializeField, NotNull]
    private FeelManager feelManager;

    [Tooltip("ĳ������ Animator ������Ʈ")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Tooltip("ĳ������ CharacterBehaviour ������Ʈ")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Tooltip("ĳ������ InventoryBehaviour ������Ʈ")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;

    [Title(label: "Settings")]

    [Tooltip("���� Ÿ��")]
    [SerializeField]
    private MotionType motionType;

    #endregion

    #region FIELDS

    /// <summary>
    /// ��� ��ġ�� ����ó���մϴ�.
    /// </summary>
    private readonly Spring springLocation = new Spring();

    /// <summary>
    /// ��� ȸ���� ����ó���մϴ�.
    /// </summary>
    private readonly Spring springRotation = new Spring();

    /// <summary>
    /// FeelState
    /// </summary>
    private FeelState feelState;

    #endregion

    #region METHODS

    public override void Tick()
    {
        if(feelManager == null || characterBehaviour == null || inventoryBehaviour == null || characterAnimator == null)
        {
            Debug.LogError($"{this.gameObject}��feelManager = {feelManager} characterBehaviour = {characterBehaviour} inventoryBehaviour = {inventoryBehaviour} characterAnimator = {characterAnimator} �Դϴ�" );

            return;
        }

        FeelPreset feelPreset = feelManager.Preset;
        if (feelPreset == null)
            return;
        
        Feel feel = feelPreset.GetFeel(motionType);
        if (feel == null)
            return;
        
        //������ ���⸦ ��������
        WeaponBehaviour weaponBehaviour = inventoryBehaviour.GetEquipped();
        if (weaponBehaviour == null)
            return;

        //������ ������ ������ ���� ��Ҹ� �����ɴϴ�.
        var itemAnimationDataBehaviour = weaponBehaviour.GetComponent<ItemAnimationDataBehaviour>();
        if (itemAnimationDataBehaviour == null)
            return;

        //������ ������ weaponAttachmentManagerBehaviour�� �����ɴϴ�
        WeaponAttachmentManagerBehaviour weaponAttachmentManagerBehaviour = weaponBehaviour.GetAttachmentManager();
        if(weaponAttachmentManagerBehaviour == null)
            return;

        //������ ������ scopeBehaviour�� �����ɴϴ�
        ScopeBehaviour scopeBehaviour = weaponAttachmentManagerBehaviour.GetEquippedScope();
        if (weaponAttachmentManagerBehaviour == null)
            return;

        //���⸦ ��� ������ ������
        ItemOffsets itemOffsets = itemAnimationDataBehaviour.GetItemOffsets();
        if (itemOffsets == null)
            return;

        //��ġ
        Vector3 location = default;

        //ȸ��
        Vector3 rotation = default;

        if(characterAnimator.GetBool(AHashes.Running))
        {
            //������ ���ϱ�
            location += itemOffsets.RunningLocation;
            rotation += itemOffsets.RunningRotation;

            feelState = feel.Running;
        }
        else
        {
            if(characterAnimator.GetBool(AHashes.Aim))
            {
                //������ ���ϱ�
                location += itemOffsets.AimingLocation;
                rotation += itemOffsets.AimingRotation;

                //������ ������ ���ϱ�
                location += scopeBehaviour.GetOffsetAimingLocation();
                rotation += scopeBehaviour.GetOffsetAimingRotation();

                feelState = feel.Aiming;
            }
            else
            {
                //��ũ���� �ִ� ����
                if(characterAnimator.GetBool(AHashes.Crouching))
                {
                    location += itemOffsets.CrouchingLocation;
                    rotation += itemOffsets.CrouchingRotation;

                    feelState = feel.Crouching;

                }
                //�� �ִ� ����
                else
                {
                    location += itemOffsets.StandingLocation;
                    rotation += itemOffsets.StandingRotation;

                    feelState = feel.Standing;
                }

            }
            
        }

        //�������� ������� ���� ���� �����ϴ� �ִϸ��̼� ��
        float alphaActionOffset = characterAnimator.GetFloat(AHashes.AlphaActionOffset);

        //����ź�� ���� ���� ���� ������ �� �� �����°��� ���մϴ�.
        location += itemOffsets.ActionLocation * alphaActionOffset;
        rotation += itemOffsets.ActionRotation * alphaActionOffset;

        //�����Ű��
        location += feelState.Offset.OffsetLocation;
        rotation += feelState.Offset.OffsetRotation;

        //��ġ �������� ������Ʈ 
        springLocation.UpdateEndValue(location);
        //ȸ�� �������� ������Ʈ
        springRotation.UpdateEndValue(rotation);

    }

    #endregion

    #region FUNCTIONS

    /// <summary>
    /// ����� ��ġ �� ��������
    /// </summary>
    public override Vector3 GetLocation()
    {
        //�������� ���ٸ� �⺻���� ����
        if (feelState.Offset == null)
            return default;

        return springLocation.Evaluate(feelState.Offset.SpringSettingsLocation);
    }

    /// <summary>
    /// ����� ȸ�� �� ��������
    /// </summary>
    public override Vector3 GetEulerAngles()
    {   
        //�������� ���ٸ� �⺻�� ����
        if (feelState.Offset == null)
            return default;

        return springRotation.Evaluate(feelState.Offset.SpringSettingsRotation);
                
    }

    #endregion
}
