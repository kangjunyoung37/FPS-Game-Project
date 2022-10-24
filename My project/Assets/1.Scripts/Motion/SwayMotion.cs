using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 흔들림 모션,모든 흔들림 모션을 생성합니다
/// </summary>
public class SwayMotion : Motion
{
    #region FIELDS SERIALIZED

    [Tooltip("캐릭터의 FeelManager 컴포넌트")]
    [SerializeField, NotNull]
    private FeelManager feelManager;

    [Tooltip("캐릭터의 애니메이터 컴포넌트")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Tooltip("캐릭터의 inventoryBehaviour 컴포넌트")]
    [SerializeField,NotNull]
    private InventoryBehaviour inventoryBehaviour;

    [Tooltip("캐릭터의 CharacterBehaviour 컴포넌트")]
    [SerializeField,NotNull]
    private CharacterBehaviour characterBehaviour;

    [Title(label:"Settings")]

    [Tooltip("모션 타입")]
    [SerializeField]
    private MotionType motionType;
    #endregion

    #region FIELDS

    /// <summary>
    /// 위치 Spring
    /// </summary>
    private readonly Spring springLocation = new Spring();

    /// <summary>
    /// 회전 Spring
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
            Debug.LogError($"{this.gameObject}에 feel Manager = {feelManager} ,characterBehaviour = {characterBehaviour},inventoryBehaviour = {inventoryBehaviour} , characterAnimator = {characterAnimator} ");

            return;
        }
        //마우스 인풋 받아오기
        Vector2 inputLook = Vector2.ClampMagnitude(characterBehaviour.GetInputLook(), 1);

        //키보드 인풋 받아오기
        Vector2 movement = Vector2.ClampMagnitude(characterBehaviour.GetInputMovement(), 1);

        //feelManager에서 feelPreset 가져오기
        FeelPreset feelPreset = feelManager.Preset;
        if (feelPreset == null)
            return;
        //feelPreset에서 feel 가져오기
        Feel feel = feelPreset.GetFeel(motionType);
        if( feel == null) 
            return;

        //FeelState 값을 가져옵니다.
        feelState = feel.GetState(characterAnimator);

        //scopeBehaviour를 가져옵니다.
        ScopeBehaviour scopeBehaviour = inventoryBehaviour.GetEquipped().GetAttachmentManager().GetEquippedScope();

        //feelState에서 swayData를 가져옵니다
        SwayData swayData = feelState.SwayData;
        if (swayData == null)
            return;

        //수평 값에 적용된 흔들림을 나타냅니다.
        Vector3 horizontalLocaiton = default;

        //시야 흔들림
        horizontalLocaiton += swayData.Look.Horizontal.locationCurves.EvaluateCurves(inputLook.x) * swayData.Look.Horizontal.locationMultiplier;
        //움직임 흔들림
        horizontalLocaiton += swayData.Movement.Horizontal.locationCurves.EvaluateCurves(movement.x) * swayData.Movement.Horizontal.locationMultiplier;

        //수직 값에 적용된 흔들림을 나타냅니다.
        Vector3 verticalLocation = default;

        //시야 흔들림
        verticalLocation += swayData.Look.Vertical.locationCurves.EvaluateCurves(inputLook.y) * swayData.Look.Vertical.locationMultiplier;
        //움직임 흔들림
        verticalLocation += swayData.Movement.Vertical.locationCurves.EvaluateCurves(movement.y) * swayData.Movement.Vertical.locationMultiplier;


        
        //수평 값에 적용된 흔들림을 나타냅니다.
        Vector3 horizontalRotation = default;
        
        //시야 흔들림
        horizontalRotation += swayData.Look.Horizontal.rotationCurves.EvaluateCurves(inputLook.x) * swayData.Look.Horizontal.rotationMultiplier;
        //움직임 흔들림
        horizontalRotation += swayData.Movement.Horizontal.rotationCurves.EvaluateCurves(movement.x) * swayData.Movement.Horizontal.rotationMultiplier;

        //수직 값에 적용된 흔들림을 나타냅니다.
        Vector3 verticalRotation = default;
        
        //시야 흔들림
        verticalRotation += swayData.Look.Vertical.rotationCurves.EvaluateCurves(inputLook.y) * swayData.Look.Vertical.rotationMultiplier;
        //움직임 흔들림
        verticalRotation += swayData.Movement.Vertical.rotationCurves.EvaluateCurves(movement.y) * swayData.Movement.Vertical.rotationMultiplier;

        //위치값 업데이트
        springLocation.UpdateEndValue(scopeBehaviour.GetSwayMutiplier() * (horizontalLocaiton + verticalLocation));
        //회전값 업데이트
        springRotation.UpdateEndValue(scopeBehaviour.GetSwayMutiplier() * (horizontalRotation + verticalRotation));

    }

    #endregion

    #region FUNCTIONS

    /// <summary>
    /// 위치값 가져오기
    /// </summary>
    public override Vector3 GetLocation()
    {
        
        if (feelState.SwayData == null)
            return default;

        return springLocation.Evaluate(feelState.SwayData.SpringSettings);
    }

    /// <summary>
    /// 회전값 가져오기
    /// </summary>
    public override Vector3 GetEulerAngles()
    {
        if (feelState.SwayData == null)
            return default;
        return springRotation.Evaluate(feelState.SwayData.SpringSettings);
    }


    #endregion
}
