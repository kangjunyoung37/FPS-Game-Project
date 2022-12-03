using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMotion : Motion
{
    #region FIELDS SERIALIZED

    [Tooltip("캐릭터의 FeelManager 컴포넌트")]
    [SerializeField, NotNull]
    private FeelManager feelManager;

    [Tooltip("캐릭터의 movementBehaviour 컴포넌트")]
    [SerializeField, NotNull]
    private MovementBehaviour movementBehaviour;

    [Tooltip("캐릭터의 Animator")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Title(label: "Settings")]

    [Tooltip("모션의 타입")]
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
    /// 현재 재생되고 있는 애니메이션 곡선
    /// </summary>
    private ACurves playedCurves;

    /// <summary>
    /// 캐릭터가 마지막으로 땅에 착지했던 시간
    /// </summary>
    private float landingTime;

    #endregion

    #region METHODS

    public override void Tick()
    {
        if(feelManager == null || movementBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}에 feelManager가 {feelManager}이고,movementBehaviour가 {movementBehaviour}입니다");

            return;
        }

        Feel feel = feelManager.Preset.GetFeel(motionType);
        if(feel == null)
        {
            Debug.LogError($"{this.gameObject}에 feel 이 Null 입니다");

            return;
        }
        //위치
        Vector3 location = default;
        //회전
        Vector3 rotation = default;

        //landing Time 저장
        if (movementBehaviour.IsGrounded() && !movementBehaviour.WasGrounded())
            landingTime = Time.time;

        //착지 모션으로 적용
        playedCurves = feel.GetState(characterAnimator).LandingCurves;

        float evaluateTime = Time.time - landingTime;
        
        //위치 곡선 적용하기
        location += playedCurves.LocationCurves.EvaluateCurves(evaluateTime);
        //회전 곡선 적용하기
        rotation += playedCurves.RotationCurves.EvaluateCurves(evaluateTime);

        //스프링에 위치 적용하기
        springLocation.UpdateEndValue(location);
        //스프링에 회전 적용하기
        springRotation.UpdateEndValue(rotation);
        
    }

    #endregion

    #region FUNCTIONS

    //위치 가져오기
    public override Vector3 GetLocation()
    {
        if (playedCurves == null)
            return default;

        return springLocation.Evaluate(playedCurves.LocationSpring);
    }

    public override Vector3 GetEulerAngles()
    {
        if (playedCurves == null)
            return default;

        return springRotation.Evaluate(playedCurves.RotationSpring);
    }

    #endregion

}
