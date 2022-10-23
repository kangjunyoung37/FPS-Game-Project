using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일부 상태 값을 기반으로 오브젝트를 오프셋하는 데 사용됩니다.
/// </summary>
public class OffsetMotion : Motion
{
    #region FIEDLS SERIALIZED

    [Tooltip("캐릭터 feelManager 컴포넌트")]
    [SerializeField, NotNull]
    private FeelManager feelManager;

    [Tooltip("캐릭터의 Animator 컴포넌트")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Tooltip("캐릭터의 CharacterBehaviour 컴포넌트")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Tooltip("캐릭터의 InventoryBehaviour 컴포넌트")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;

    [Title(label: "Settings")]

    [Tooltip("적용 타입")]
    [SerializeField]
    private MotionType motionType;

    #endregion

    #region FIELDS

    /// <summary>
    /// 모든 위치를 보간처리합니다.
    /// </summary>
    private readonly Spring springLocation = new Spring();

    /// <summary>
    /// 모든 회전을 보간처리합니다.
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
            Debug.LogError($"{this.gameObject}에feelManager = {feelManager} characterBehaviour = {characterBehaviour} inventoryBehaviour = {inventoryBehaviour} characterAnimator = {characterAnimator} 입니다" );

            return;
        }

        FeelPreset feelPreset = feelManager.Preset;
        if (feelPreset == null)
            return;
        
        Feel feel = feelPreset.GetFeel(motionType);
        if (feel == null)
            return;
        
        //장착된 무기를 가져오기
        WeaponBehaviour weaponBehaviour = inventoryBehaviour.GetEquipped();
        if (weaponBehaviour == null)
            return;

        //장착된 무기의 데이터 구성 요소를 가져옵니다.
        var itemAnimationDataBehaviour = weaponBehaviour.GetComponent<ItemAnimationDataBehaviour>();
        if (itemAnimationDataBehaviour == null)
            return;

        //장착된 무기의 weaponAttachmentManagerBehaviour를 가져옵니다
        WeaponAttachmentManagerBehaviour weaponAttachmentManagerBehaviour = weaponBehaviour.GetAttachmentManager();
        if(weaponAttachmentManagerBehaviour == null)
            return;

        //장착된 무기의 scopeBehaviour를 가져옵니다
        ScopeBehaviour scopeBehaviour = weaponAttachmentManagerBehaviour.GetEquippedScope();
        if (weaponAttachmentManagerBehaviour == null)
            return;

        //무기를 잡는 아이템 오프셋
        ItemOffsets itemOffsets = itemAnimationDataBehaviour.GetItemOffsets();
        if (itemOffsets == null)
            return;

        //위치
        Vector3 location = default;

        //회전
        Vector3 rotation = default;

        if(characterAnimator.GetBool(AHashes.Running))
        {
            //오프셋 더하기
            location += itemOffsets.RunningLocation;
            rotation += itemOffsets.RunningRotation;

            feelState = feel.Running;
        }
        else
        {
            if(characterAnimator.GetBool(AHashes.Aim))
            {
                //오프셋 더하기
                location += itemOffsets.AimingLocation;
                rotation += itemOffsets.AimingRotation;

                //스코프 오프셋 더하기
                location += scopeBehaviour.GetOffsetAimingLocation();
                rotation += scopeBehaviour.GetOffsetAimingRotation();

                feelState = feel.Aiming;
            }
            else
            {
                //웅크리고 있는 상태
                if(characterAnimator.GetBool(AHashes.Crouching))
                {
                    location += itemOffsets.CrouchingLocation;
                    rotation += itemOffsets.CrouchingRotation;

                    feelState = feel.Crouching;

                }
                //서 있는 상태
                else
                {
                    location += itemOffsets.StandingLocation;
                    rotation += itemOffsets.StandingRotation;

                    feelState = feel.Standing;
                }

            }
            
        }

        //오프셋을 사용하지 않을 때를 결정하는 애니메이션 값
        float alphaActionOffset = characterAnimator.GetFloat(AHashes.AlphaActionOffset);

        //수류탄을 던질 때와 근접 공격을 할 때 오프셋값을 더합니다.
        location += itemOffsets.ActionLocation * alphaActionOffset;
        rotation += itemOffsets.ActionRotation * alphaActionOffset;

        //적용시키기
        location += feelState.Offset.OffsetLocation;
        rotation += feelState.Offset.OffsetRotation;

        //위치 스프링값 업데이트 
        springLocation.UpdateEndValue(location);
        //회전 스프링값 업데이트
        springRotation.UpdateEndValue(rotation);

    }

    #endregion

    #region FUNCTIONS

    /// <summary>
    /// 적용된 위치 값 가져오기
    /// </summary>
    public override Vector3 GetLocation()
    {
        //오프셋이 없다면 기본값을 넣음
        if (feelState.Offset == null)
            return default;

        return springLocation.Evaluate(feelState.Offset.SpringSettingsLocation);
    }

    /// <summary>
    /// 적용된 회전 값 가져오기
    /// </summary>
    public override Vector3 GetEulerAngles()
    {   
        //오프셋이 없다면 기본값 넣음
        if (feelState.Offset == null)
            return default;

        return springRotation.Evaluate(feelState.Offset.SpringSettingsRotation);
                
    }

    #endregion
}
