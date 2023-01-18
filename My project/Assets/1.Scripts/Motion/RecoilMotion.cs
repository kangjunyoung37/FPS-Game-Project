using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 반동 동작을 생성하고 적용합니다.
/// </summary>
public class RecoilMotion : Motion
{
    #region FIELDS SERIALIZED

    [Tooltip("캐릭터의 인벤토리 컴포넌트")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;

    [Tooltip("캐릭터의 CharacterBehaviour 컴포넌트")]
    [SerializeField,NotNull]
    private CharacterBehaviour characterBehaviour;

    [Title(label: "Settings")]

    [Tooltip("컴포넌트 적용 방식")]
    [SerializeField]
    private MotionType motionType;

    #endregion

    #region FIELDS

    /// <summary>
    /// 위치 반동 스프링값, 
    /// </summary>
    private readonly Spring recoilSpringLocation = new Spring();

    /// <summary>
    /// 회전 반동 스프링값
    /// </summary>
    private readonly Spring recoilSpringRotation = new Spring();

    /// <summary>
    /// 현재 반동 곡선
    /// </summary>
    private ACurves recoilCurves;

    #endregion

    #region METHODS

    public override void Tick()
    {
        if(inventoryBehaviour == null || characterBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}에 inventoryBehaviour = {inventoryBehaviour} ,characterBehaviour = {characterBehaviour}입니다. ");

            return;
        }

        //장착된 무기에서 애니메이션데이터를 가져옵니다.
        var animationDataBehaviour = inventoryBehaviour.GetEquipped().GetComponent<ItemAnimationDataBehaviour>();

        //없다면 실행될 필요가 없습니다.
        if (animationDataBehaviour == null)
            return;

        //필요한 반동 데이터를 가져옵니다.
        RecoilData recoilData = animationDataBehaviour.GetRecoilData(motionType);

        if (recoilData == null)
            return;

        //발사한 양
        int shotsFired = characterBehaviour.GetShotsFired();
        //서 있는 상태에서의 반동 값
        float recoilDataMultiplier = recoilData.StandingStateMultiplier;

        //위치 반동
        Vector3 recoilLocation = default;

        //회전 반동
        Vector3 recoilRotation = default;

        recoilCurves = recoilData.StandingState;

        //에임중이라면
        if(characterBehaviour.IsAiming())
        {
            //에임 반동 값을 사용합니다.
            recoilDataMultiplier = recoilData.AimingStateMultiplier;

            //반동 곡선을 변경
            recoilCurves = recoilData.AimingState;
        }

        if(recoilCurves != null)
        {
            //세가지의 곡선을 가지고 있어야 합니다.
            if(recoilCurves.LocationCurves.Length == 3)
            {
                //정확한 시간에 반동 곡선을 계산하여 반동 위치를 계산합니다.
                //정확한 시간은 방금 발사한 총알의 양이므로 특정 탄약 수를 고려해야합니다.
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

            //위치반동 더하기
            recoilLocation *= recoilCurves.LocationMultiplier * recoilDataMultiplier;
            //회전반동 더하기
            recoilRotation *= recoilCurves.RotationMultiplier * recoilDataMultiplier;

            //위치 반동 값을 업데이트합니다.
            recoilSpringLocation.UpdateEndValue(recoilLocation);
            //회전 반동 값을 업데이트합니다.
            recoilSpringRotation.UpdateEndValue(recoilRotation);
            
        }
    }
    #endregion

    #region FUNCTIONS

    //위치 스프링값 가져오기
    public override Vector3 GetLocation()
    {
        if (recoilCurves == null)
            return default;

        return recoilSpringLocation.Evaluate(recoilCurves.LocationSpring);
    }

    //회전 스프링값 가져오기
    public override Vector3 GetEulerAngles()
    {
        if (recoilCurves == null)
            return default;

        return recoilSpringRotation.Evaluate(recoilCurves.RotationSpring);

    }

    #endregion
}
