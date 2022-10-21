using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적용 모드
/// 모션 어플라이어에 속한 Motion의 값을 어떻게 적용할건지 결정합니다.
/// </summary>
public enum ApplyMode { Override, Add }

/// <summary>
/// 모션리스트, Transform, 모션에 적용시키기,적용 모드가 포함되어 있습니다.
/// </summary>
public class MotionApplier : MonoBehaviour
{

    #region FIELDS SERIALIZED

    [Title(label: "Settings")]

    [Tooltip("Motion에 값을 적용하는 방법")]
    [SerializeField]
    private ApplyMode applyMode;

    #endregion

    #region FIELDS

    /// <summary>
    /// 등록되어 있는 모션들
    /// </summary>
    private readonly List<Motion> motions = new List<Motion>();

    /// <summary>
    /// Transform
    /// </summary>
    private Transform thisTransform;

    #endregion

    #region METHODS

    private void Awake()
    {
        //캐싱
        thisTransform = transform;
    }

    private void LateUpdate()
    {
        //마지막 위치(결과)
        Vector3 finalLocation = default;

        //마지막 오일러앵글(결과)
        Vector3 finalEulerAngles = default;

        motions.ForEach((motion =>
        {
            //Tick
            motion.Tick();

            //모션에 따라 위치 계산값을 받아와서 finalLocation에 넣기
            finalLocation += motion.GetLocation() * motion.Alpha;

            //모션에 따라 로테이션 계산값을 받아와서 finalRotation에 넣기
            finalEulerAngles += motion.GetEulerAngles() * motion.Alpha;

        }));

        //오버라이드 모드(덮어 씌우기)
        if(applyMode == ApplyMode.Override)
        {
            //로케이션 값 집어넣기
            thisTransform.localPosition = finalLocation;

            //EulerAngles 값 집어넣기
            thisTransform.localEulerAngles = finalEulerAngles;
        }
        else if(applyMode == ApplyMode.Add)
        {
            //로케이션 값 더하기
            thisTransform.localPosition += finalLocation;

            //EulerAngles 값 더하기
            thisTransform.localEulerAngles += finalEulerAngles;

        }

        //더하기 모드(더하기)
    }

    /// <summary>
    /// motion리스트에 motion 추가하기
    /// </summary>
    /// <param name="motion">모션</param>
    public void Subscribe(Motion motion) => motions.Add(motion);

    #endregion

}
