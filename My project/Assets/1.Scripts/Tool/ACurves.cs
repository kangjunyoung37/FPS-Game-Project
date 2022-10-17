using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SO_AC_Default",menuName = "FPS Game/Animation Curves")]
public class ACurves : ScriptableObject
{
    #region PROPERTIES

    public SpringSettings LocationSpring => locationSpring;
    public AnimationCurve[] LocationCurves => locationCurves;
    public float LocationMultiplier => locationMutiplier;

    public SpringSettings RotationSpring => rotationSpring;
    public AnimationCurve[] RotationsCurves => rotationCurves;
    public float RotationMultiplier => rotationMutiplier;

    #endregion

    [Title(label: "Location Settings")]

    [Range(0.0f, 10.0f)]
    [Tooltip("위치 곡선에 적용되는 곱셈값입니다.")]
    [SerializeField]
    private float locationMutiplier = 1.0f;

    [Tooltip("위치에 대한 보간 설정입니다.")]
    [SerializeField]
    private SpringSettings locationSpring = SpringSettings.Default();

    [Tooltip("애니메이션 위치 곡선")]
    [SerializeField]
    private AnimationCurve[] locationCurves;

    [Title(label: "Rotation Settings")]

    [Range(0.0f, 10.0f)]
    [Tooltip("로테이션 곡선에 적용되는 곱셈값입니다")]
    [SerializeField]
    private float rotationMutiplier = 1.0f;

    [Tooltip("로테이션에 대한 보간 설정입니다.")]
    [SerializeField]
    private SpringSettings rotationSpring = SpringSettings.Default();


    [Tooltip("애니메이션 로테이션 곡선")]
    [SerializeField]
    private AnimationCurve[] rotationCurves;

}
