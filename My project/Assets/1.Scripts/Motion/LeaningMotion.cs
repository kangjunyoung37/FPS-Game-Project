using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaningMotion : Motion
{
    #region FIELDS SERIALIZED

    [Tooltip("캐릭터의 인벤토리 컴포넌트")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;

    [Tooltip("캐릭터의 CharacterBehaviour 컴포넌트")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Tooltip("캐릭터의 애니메이터 컴포넌트")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Title(label: "Settings")]

    [Tooltip("모션 타입")]
    [SerializeField]
    private MotionType motionType;

    #endregion

    #region FIEDLS

    /// <summary>
    /// 위치 스피링값
    /// </summary>
    private readonly Spring springLocation = new Spring();

    /// <summary>
    /// 회전 스피링값
    /// </summary>
    private readonly Spring springRotation = new Spring();

    /// <summary>
    /// 재생되고 있는 애니메이션 커브
    /// </summary>
    private ACurves leaningCurves;

    #endregion

    #region METHODS

    public override void Tick()
    {
        if(inventoryBehaviour == null || characterAnimator == null || characterBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}에 inventoryBehaviour,characterAnimator,characterBehaviour 중 null이 있습니다 {inventoryBehaviour},{characterBehaviour},{characterAnimator}");
            
            return;
        }
        //장비된 무기의 ItemAnimationDataBehaviour를 가져온다
        var animatorDataBehaviour = inventoryBehaviour.GetEquipped().GetComponent<ItemAnimationDataBehaviour>();

        //animatorDataBehaviour가 없다면 이 스크립트는 실행될 수 없습니다.
        if (animatorDataBehaviour == null)
            return;
        
        //LeaningData를 가져옵니다.
        LeaningData leaningData = animatorDataBehaviour.GetLeaningData();
        if (leaningData == null)
            return;

        //캐릭터가 조준하는지에 따라 올바른 기울기 곡선을 리턴합니다.
        leaningCurves = leaningData.GetCurves(motionType, characterBehaviour.IsAiming());

        if(leaningCurves == null)
        {
            //기울기 곡선이 없다면 default값으로 리셋을 해준다
            springLocation.UpdateEndValue(default);
            springRotation.UpdateEndValue(default);

            return;
        }

        //캐릭터애니메이터에서 캐릭터 기울기 값을 가져옵니다.
        float leaning = characterAnimator.GetFloat(AHashes.LeaningInput);

        //위치 업데이트
        springLocation.UpdateEndValue(leaningCurves.LocationCurves.EvaluateCurves(leaning));
        //회전 업데이트
        springRotation.UpdateEndValue(leaningCurves.RotationsCurves.EvaluateCurves(leaning));
    }

    #endregion

    #region FUNCTIONS

    //위치값 가져오기
    public override Vector3 GetLocation()
    {
        if (leaningCurves == null)
            return default;

        return springLocation.Evaluate(leaningCurves.LocationSpring);
    } 
    //회전값 가져오기
    public override Vector3 GetEulerAngles()
    {
        if (leaningCurves == null)
            return default;

        return springRotation.Evaluate(leaningCurves.RotationSpring);
    }
    #endregion
}
