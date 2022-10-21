
using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMotion : Motion
{
    #region FIELDS SERIALIZED

    [Tooltip("캐릭터의 Feel Manager")]
    [SerializeField, NotNull]
    private FeelManager feelManager;

    [Tooltip("캐릭터의 MovementBehaviour 컴포넌트")]
    [SerializeField, NotNull]
    private MovementBehaviour movementBehaviour;

    [Tooltip("캐릭터의 애니메이션 컴포넌트")]
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
    /// Rotation Spring
    /// </summary>
    private readonly Spring springRotation = new Spring();

    /// <summary>
    /// 현재 재생 중인 곡선
    /// </summary>
    private ACurves playedCurves;


    #endregion

    #region METHODS

    public override void Tick()
    {
        if(feelManager == null || movementBehaviour == null)
        { 
            Debug.LogError($"{this.gameObject}에 feelManager나 movementBehaviour 가 Null 입니다");
            return;
        }

        Feel feel = feelManager.Preset.GetFeel(motionType);
        if(feel == null)
        {
            Debug.LogError($"{this.gameObject}에 feel이 {feel}입니다");

            return;
        }
        //위치값
        Vector3 location = default;
        //로테이션값
        Vector3 rotation = default;

        FeelState state = feel.GetState(characterAnimator);

        //땅과 접지되어있지 않을 경우
        if(!movementBehaviour.IsGrounded())
        {
            //공중에 떠 있는 시간을 계산
            float airTime = Time.time - movementBehaviour.GetLastJumpTime();

            //공중에 있는게 점프로 인한 것인지 체크
            if(movementBehaviour.IsJumping())
            {
                //이 값은 점프 곡선이 완료되는 지점 즉 길이를 나타냅니다.
                var maxCurveLength = 0.0f;

                //state에서 점프 곡선 가져오기
                ACurves jumpingCurves = state.JumpingCurves;

                //애니메이션 곡선을 하나씩 돕니다.
                jumpingCurves.LocationCurves.ForEach(curve =>
                {
                    //가장 긴 곡선과 일치하도록 maxCurveLength 업데이트
                    if (curve.length > maxCurveLength)
                        maxCurveLength = curve.length;
                });

                jumpingCurves.RotationsCurves.ForEach(curve =>
                {
                    if (curve.length > maxCurveLength)
                        maxCurveLength = curve.length;

                });

                //점프 커브가 지금까지 재생을 완료했는지 확인하기 , 떨어지고 있는 상태
                if (Time.time - movementBehaviour.GetLastJumpTime() >= maxCurveLength)
                {
                    //떨어지고 있는 시간
                    airTime -= maxCurveLength;
                    //떨어지는 곡선을 사용
                    playedCurves = state.FallingCurves;
                }
                else
                    playedCurves = state.JumpingCurves;
            }
            //플레이어가 점프하지 않았으므로 떨어지는 곡선을 사용
            else
            {
                playedCurves = state.FallingCurves;
            }

            //위치 곡선 적용
            location += playedCurves.LocationCurves.EvaluateCurves(airTime);
            //회전 곡선 적용
            rotation += playedCurves.RotationsCurves.EvaluateCurves(airTime);
        }

        //Spring 위치 값 업데이트
        springLocation.UpdateEndValue(location);
        //Spring 회전 값 업데이트
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

    //오일러 앵글 가져오기
    public override Vector3 GetEulerAngles()
    {
        if (playedCurves == null)
            return default;

        return springRotation.Evaluate(playedCurves.RotationSpring);
    }

    #endregion
}
