using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이 클래스는 무기를 낮추는 오프셋을 구동합니다.
public class LoweredMotion : Motion
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("캐릭터가 무기를 내리고 있는지 그 여부를 결정하는 컴포넌트")]
    [SerializeField, NotNull]
    private LowerWeapon lowerWeapon;

    [Title(label:"References Character")]

    [Tooltip("캐릭터의 characterBehaviour 컴포넌트")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Tooltip("캐릭터의 inventoryBehaviour 컴포넌트")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;

    #endregion

    #region FIELDS

    /// <summary>
    /// 무기 낮추기 위치 Spring, 캐릭터가 무기를 낮출 때 사용됩니다.
    /// </summary>
    private readonly Spring lowerdSpringLocation = new Spring();

    /// <summary>
    /// 무기 낮추기 회전 Spring, 캐릭터가 무기를 낮출 때 사용됩니다.
    /// </summary>
    private readonly Spring lowerdSpringRotation = new Spring();

    /// <summary>
    /// 현재 장착된 무기의 LowerData
    /// </summary>
    private LowerData lowerData;

    #endregion

    #region METHODS

    public override void Tick()
    {

        if(lowerWeapon == null || characterBehaviour == null || inventoryBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}에 lowerWeapon = {lowerWeapon}이고, characterBehaviour = {characterBehaviour}이고 inventoryBehaviour = {inventoryBehaviour}입니다");
            return;
        }

        //애니메이션의 데이터를 가져오기 위해 ItemAnimationDataBehaviour를 가져옴
        var animationData = inventoryBehaviour.GetEquipped().GetComponent<ItemAnimationDataBehaviour>();
        //없다면 리턴
        if (animationData == null)
            return;

        lowerData = animationData.GetLowerData();
        if(lowerData == null)
            return;
        //위치 Spring값 업데이트
        lowerdSpringLocation.UpdateEndValue(lowerWeapon.IsLowered() ? lowerData.LocationOffset : default);

        //회전 Spring값 업데이트
        lowerdSpringRotation.UpdateEndValue(lowerWeapon.IsLowered() ? lowerData.RotationOffset : default);

    }

    #endregion

    #region FUNCTIONS

    //위치값 적용해서 가져오기
    public override Vector3 GetLocation()
    {
        if(lowerData == null)
        {
            Debug.LogError($"{this.gameObject}에 lowerData가 Null입니다");

            return default;
        }

        return lowerdSpringLocation.Evaluate(lowerData.Interpolation);
    }
    //회전값 적용해서 가져오기
    public override Vector3 GetEulerAngles()
    {
        if(lowerData == null)
        {
            Debug.LogError($"{this.gameObject}에 lowerData가 Null입니다");

            return default;
        }

        return lowerdSpringRotation.Evaluate(lowerData.Interpolation);
    }

    #endregion
}
