using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��鸲 ���,��� ��鸲 ����� �����մϴ�
/// </summary>
public class SwayMotion : Motion
{
    #region FIELDS SERIALIZED

    [Tooltip("ĳ������ FeelManager ������Ʈ")]
    [SerializeField, NotNull]
    private FeelManager feelManager;

    [Tooltip("ĳ������ �ִϸ����� ������Ʈ")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Tooltip("ĳ������ inventoryBehaviour ������Ʈ")]
    [SerializeField,NotNull]
    private InventoryBehaviour inventoryBehaviour;

    [Tooltip("ĳ������ CharacterBehaviour ������Ʈ")]
    [SerializeField,NotNull]
    private CharacterBehaviour characterBehaviour;

    [Title(label:"Settings")]

    [Tooltip("��� Ÿ��")]
    [SerializeField]
    private MotionType motionType;
    #endregion

    #region FIELDS

    /// <summary>
    /// ��ġ Spring
    /// </summary>
    private readonly Spring springLocation = new Spring();

    /// <summary>
    /// ȸ�� Spring
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
            Debug.LogError($"{this.gameObject}�� feel Manager = {feelManager} ,characterBehaviour = {characterBehaviour},inventoryBehaviour = {inventoryBehaviour} , characterAnimator = {characterAnimator} ");

            return;
        }
        //���콺 ��ǲ �޾ƿ���
        Vector2 inputLook = Vector2.ClampMagnitude(characterBehaviour.GetInputLook(), 1);

        //Ű���� ��ǲ �޾ƿ���
        Vector2 movement = Vector2.ClampMagnitude(characterBehaviour.GetInputMovement(), 1);

        //feelManager���� feelPreset ��������
        FeelPreset feelPreset = feelManager.Preset;
        if (feelPreset == null)
            return;
        //feelPreset���� feel ��������
        Feel feel = feelPreset.GetFeel(motionType);
        if( feel == null) 
            return;

        //FeelState ���� �����ɴϴ�.
        feelState = feel.GetState(characterAnimator);

        //scopeBehaviour�� �����ɴϴ�.
        ScopeBehaviour scopeBehaviour = inventoryBehaviour.GetEquipped().GetAttachmentManager().GetEquippedScope();

        //feelState���� swayData�� �����ɴϴ�
        SwayData swayData = feelState.SwayData;
        if (swayData == null)
            return;

        //���� ���� ����� ��鸲�� ��Ÿ���ϴ�.
        Vector3 horizontalLocaiton = default;

        //�þ� ��鸲
        horizontalLocaiton += swayData.Look.Horizontal.locationCurves.EvaluateCurves(inputLook.x) * swayData.Look.Horizontal.locationMultiplier;
        //������ ��鸲
        horizontalLocaiton += swayData.Movement.Horizontal.locationCurves.EvaluateCurves(movement.x) * swayData.Movement.Horizontal.locationMultiplier;

        //���� ���� ����� ��鸲�� ��Ÿ���ϴ�.
        Vector3 verticalLocation = default;

        //�þ� ��鸲
        verticalLocation += swayData.Look.Vertical.locationCurves.EvaluateCurves(inputLook.y) * swayData.Look.Vertical.locationMultiplier;
        //������ ��鸲
        verticalLocation += swayData.Movement.Vertical.locationCurves.EvaluateCurves(movement.y) * swayData.Movement.Vertical.locationMultiplier;


        
        //���� ���� ����� ��鸲�� ��Ÿ���ϴ�.
        Vector3 horizontalRotation = default;
        
        //�þ� ��鸲
        horizontalRotation += swayData.Look.Horizontal.rotationCurves.EvaluateCurves(inputLook.x) * swayData.Look.Horizontal.rotationMultiplier;
        //������ ��鸲
        horizontalRotation += swayData.Movement.Horizontal.rotationCurves.EvaluateCurves(movement.x) * swayData.Movement.Horizontal.rotationMultiplier;

        //���� ���� ����� ��鸲�� ��Ÿ���ϴ�.
        Vector3 verticalRotation = default;
        
        //�þ� ��鸲
        verticalRotation += swayData.Look.Vertical.rotationCurves.EvaluateCurves(inputLook.y) * swayData.Look.Vertical.rotationMultiplier;
        //������ ��鸲
        verticalRotation += swayData.Movement.Vertical.rotationCurves.EvaluateCurves(movement.y) * swayData.Movement.Vertical.rotationMultiplier;

        //��ġ�� ������Ʈ
        springLocation.UpdateEndValue(scopeBehaviour.GetSwayMutiplier() * (horizontalLocaiton + verticalLocation));
        //ȸ���� ������Ʈ
        springRotation.UpdateEndValue(scopeBehaviour.GetSwayMutiplier() * (horizontalRotation + verticalRotation));

    }

    #endregion

    #region FUNCTIONS

    /// <summary>
    /// ��ġ�� ��������
    /// </summary>
    public override Vector3 GetLocation()
    {
        
        if (feelState.SwayData == null)
            return default;

        return springLocation.Evaluate(feelState.SwayData.SpringSettings);
    }

    /// <summary>
    /// ȸ���� ��������
    /// </summary>
    public override Vector3 GetEulerAngles()
    {
        if (feelState.SwayData == null)
            return default;
        return springRotation.Evaluate(feelState.SwayData.SpringSettings);
    }


    #endregion
}
