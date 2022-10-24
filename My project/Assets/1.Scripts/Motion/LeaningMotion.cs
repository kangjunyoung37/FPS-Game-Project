using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaningMotion : Motion
{
    #region FIELDS SERIALIZED

    [Tooltip("ĳ������ �κ��丮 ������Ʈ")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;

    [Tooltip("ĳ������ CharacterBehaviour ������Ʈ")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Tooltip("ĳ������ �ִϸ����� ������Ʈ")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Title(label: "Settings")]

    [Tooltip("��� Ÿ��")]
    [SerializeField]
    private MotionType motionType;

    #endregion

    #region FIEDLS

    /// <summary>
    /// ��ġ ���Ǹ���
    /// </summary>
    private readonly Spring springLocation = new Spring();

    /// <summary>
    /// ȸ�� ���Ǹ���
    /// </summary>
    private readonly Spring springRotation = new Spring();

    /// <summary>
    /// ����ǰ� �ִ� �ִϸ��̼� Ŀ��
    /// </summary>
    private ACurves leaningCurves;

    #endregion

    #region METHODS

    public override void Tick()
    {
        if(inventoryBehaviour == null || characterAnimator == null || characterBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}�� inventoryBehaviour,characterAnimator,characterBehaviour �� null�� �ֽ��ϴ� {inventoryBehaviour},{characterBehaviour},{characterAnimator}");
            
            return;
        }
        //���� ������ ItemAnimationDataBehaviour�� �����´�
        var animatorDataBehaviour = inventoryBehaviour.GetEquipped().GetComponent<ItemAnimationDataBehaviour>();

        //animatorDataBehaviour�� ���ٸ� �� ��ũ��Ʈ�� ����� �� �����ϴ�.
        if (animatorDataBehaviour == null)
            return;
        
        //LeaningData�� �����ɴϴ�.
        LeaningData leaningData = animatorDataBehaviour.GetLeaningData();
        if (leaningData == null)
            return;

        //ĳ���Ͱ� �����ϴ����� ���� �ùٸ� ���� ��� �����մϴ�.
        leaningCurves = leaningData.GetCurves(motionType, characterBehaviour.IsAiming());

        if(leaningCurves == null)
        {
            //���� ��� ���ٸ� default������ ������ ���ش�
            springLocation.UpdateEndValue(default);
            springRotation.UpdateEndValue(default);

            return;
        }

        //ĳ���;ִϸ����Ϳ��� ĳ���� ���� ���� �����ɴϴ�.
        float leaning = characterAnimator.GetFloat(AHashes.LeaningInput);

        //��ġ ������Ʈ
        springLocation.UpdateEndValue(leaningCurves.LocationCurves.EvaluateCurves(leaning));
        //ȸ�� ������Ʈ
        springRotation.UpdateEndValue(leaningCurves.RotationsCurves.EvaluateCurves(leaning));
    }

    #endregion

    #region FUNCTIONS

    //��ġ�� ��������
    public override Vector3 GetLocation()
    {
        if (leaningCurves == null)
            return default;

        return springLocation.Evaluate(leaningCurves.LocationSpring);
    } 
    //ȸ���� ��������
    public override Vector3 GetEulerAngles()
    {
        if (leaningCurves == null)
            return default;

        return springRotation.Evaluate(leaningCurves.RotationSpring);
    }
    #endregion
}
