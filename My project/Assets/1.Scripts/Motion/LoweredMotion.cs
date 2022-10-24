using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� Ŭ������ ���⸦ ���ߴ� �������� �����մϴ�.
public class LoweredMotion : Motion
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("ĳ���Ͱ� ���⸦ ������ �ִ��� �� ���θ� �����ϴ� ������Ʈ")]
    [SerializeField, NotNull]
    private LowerWeapon lowerWeapon;

    [Title(label:"References Character")]

    [Tooltip("ĳ������ characterBehaviour ������Ʈ")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Tooltip("ĳ������ inventoryBehaviour ������Ʈ")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;

    #endregion

    #region FIELDS

    /// <summary>
    /// ���� ���߱� ��ġ Spring, ĳ���Ͱ� ���⸦ ���� �� ���˴ϴ�.
    /// </summary>
    private readonly Spring lowerdSpringLocation = new Spring();

    /// <summary>
    /// ���� ���߱� ȸ�� Spring, ĳ���Ͱ� ���⸦ ���� �� ���˴ϴ�.
    /// </summary>
    private readonly Spring lowerdSpringRotation = new Spring();

    /// <summary>
    /// ���� ������ ������ LowerData
    /// </summary>
    private LowerData lowerData;

    #endregion

    #region METHODS

    public override void Tick()
    {

        if(lowerWeapon == null || characterBehaviour == null || inventoryBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}�� lowerWeapon = {lowerWeapon}�̰�, characterBehaviour = {characterBehaviour}�̰� inventoryBehaviour = {inventoryBehaviour}�Դϴ�");
            return;
        }

        //�ִϸ��̼��� �����͸� �������� ���� ItemAnimationDataBehaviour�� ������
        var animationData = inventoryBehaviour.GetEquipped().GetComponent<ItemAnimationDataBehaviour>();
        //���ٸ� ����
        if (animationData == null)
            return;

        lowerData = animationData.GetLowerData();
        if(lowerData == null)
            return;
        //��ġ Spring�� ������Ʈ
        lowerdSpringLocation.UpdateEndValue(lowerWeapon.IsLowered() ? lowerData.LocationOffset : default);

        //ȸ�� Spring�� ������Ʈ
        lowerdSpringRotation.UpdateEndValue(lowerWeapon.IsLowered() ? lowerData.RotationOffset : default);

    }

    #endregion

    #region FUNCTIONS

    //��ġ�� �����ؼ� ��������
    public override Vector3 GetLocation()
    {
        if(lowerData == null)
        {
            Debug.LogError($"{this.gameObject}�� lowerData�� Null�Դϴ�");

            return default;
        }

        return lowerdSpringLocation.Evaluate(lowerData.Interpolation);
    }
    //ȸ���� �����ؼ� ��������
    public override Vector3 GetEulerAngles()
    {
        if(lowerData == null)
        {
            Debug.LogError($"{this.gameObject}�� lowerData�� Null�Դϴ�");

            return default;
        }

        return lowerdSpringRotation.Evaluate(lowerData.Interpolation);
    }

    #endregion
}
