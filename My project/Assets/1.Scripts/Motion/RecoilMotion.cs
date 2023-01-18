using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ݵ� ������ �����ϰ� �����մϴ�.
/// </summary>
public class RecoilMotion : Motion
{
    #region FIELDS SERIALIZED

    [Tooltip("ĳ������ �κ��丮 ������Ʈ")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;

    [Tooltip("ĳ������ CharacterBehaviour ������Ʈ")]
    [SerializeField,NotNull]
    private CharacterBehaviour characterBehaviour;

    [Title(label: "Settings")]

    [Tooltip("������Ʈ ���� ���")]
    [SerializeField]
    private MotionType motionType;

    #endregion

    #region FIELDS

    /// <summary>
    /// ��ġ �ݵ� ��������, 
    /// </summary>
    private readonly Spring recoilSpringLocation = new Spring();

    /// <summary>
    /// ȸ�� �ݵ� ��������
    /// </summary>
    private readonly Spring recoilSpringRotation = new Spring();

    /// <summary>
    /// ���� �ݵ� �
    /// </summary>
    private ACurves recoilCurves;

    #endregion

    #region METHODS

    public override void Tick()
    {
        if(inventoryBehaviour == null || characterBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}�� inventoryBehaviour = {inventoryBehaviour} ,characterBehaviour = {characterBehaviour}�Դϴ�. ");

            return;
        }

        //������ ���⿡�� �ִϸ��̼ǵ����͸� �����ɴϴ�.
        var animationDataBehaviour = inventoryBehaviour.GetEquipped().GetComponent<ItemAnimationDataBehaviour>();

        //���ٸ� ����� �ʿ䰡 �����ϴ�.
        if (animationDataBehaviour == null)
            return;

        //�ʿ��� �ݵ� �����͸� �����ɴϴ�.
        RecoilData recoilData = animationDataBehaviour.GetRecoilData(motionType);

        if (recoilData == null)
            return;

        //�߻��� ��
        int shotsFired = characterBehaviour.GetShotsFired();
        //�� �ִ� ���¿����� �ݵ� ��
        float recoilDataMultiplier = recoilData.StandingStateMultiplier;

        //��ġ �ݵ�
        Vector3 recoilLocation = default;

        //ȸ�� �ݵ�
        Vector3 recoilRotation = default;

        recoilCurves = recoilData.StandingState;

        //�������̶��
        if(characterBehaviour.IsAiming())
        {
            //���� �ݵ� ���� ����մϴ�.
            recoilDataMultiplier = recoilData.AimingStateMultiplier;

            //�ݵ� ��� ����
            recoilCurves = recoilData.AimingState;
        }

        if(recoilCurves != null)
        {
            //�������� ��� ������ �־�� �մϴ�.
            if(recoilCurves.LocationCurves.Length == 3)
            {
                //��Ȯ�� �ð��� �ݵ� ��� ����Ͽ� �ݵ� ��ġ�� ����մϴ�.
                //��Ȯ�� �ð��� ��� �߻��� �Ѿ��� ���̹Ƿ� Ư�� ź�� ���� ����ؾ��մϴ�.
                recoilLocation.x = recoilCurves.LocationCurves[0].Evaluate(shotsFired);
                recoilLocation.y = recoilCurves.LocationCurves[1].Evaluate(shotsFired);
                recoilLocation.z = recoilCurves.LocationCurves[2].Evaluate(shotsFired);

            }

            if(recoilCurves.RotationCurves.Length == 3)
            {
                recoilRotation.x = recoilCurves.RotationCurves[0].Evaluate(shotsFired);
                recoilRotation.y = recoilCurves.RotationCurves[1].Evaluate(shotsFired);
                recoilRotation.z = recoilCurves.RotationCurves[2].Evaluate(shotsFired);

            }

            //��ġ�ݵ� ���ϱ�
            recoilLocation *= recoilCurves.LocationMultiplier * recoilDataMultiplier;
            //ȸ���ݵ� ���ϱ�
            recoilRotation *= recoilCurves.RotationMultiplier * recoilDataMultiplier;

            //��ġ �ݵ� ���� ������Ʈ�մϴ�.
            recoilSpringLocation.UpdateEndValue(recoilLocation);
            //ȸ�� �ݵ� ���� ������Ʈ�մϴ�.
            recoilSpringRotation.UpdateEndValue(recoilRotation);
            
        }
    }
    #endregion

    #region FUNCTIONS

    //��ġ �������� ��������
    public override Vector3 GetLocation()
    {
        if (recoilCurves == null)
            return default;

        return recoilSpringLocation.Evaluate(recoilCurves.LocationSpring);
    }

    //ȸ�� �������� ��������
    public override Vector3 GetEulerAngles()
    {
        if (recoilCurves == null)
            return default;

        return recoilSpringRotation.Evaluate(recoilCurves.RotationSpring);

    }

    #endregion
}
