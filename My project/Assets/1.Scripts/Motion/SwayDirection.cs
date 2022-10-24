using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이션에 적용되는 곱셈값과
/// 애니메이션 곡선이 포함되어있습니다.
/// </summary>
[Serializable]
public struct SwayDirection
{
    [Title(label: "Location Settings")]

    [Range(0.0f, 10.0f)]
    [Tooltip("위치 곡선에 적용되는 곱셈값")]
    [SerializeField]
    public float locationMultiplier;

    [Tooltip("애니메이션 위치 곡선")]
    [SerializeField]
    public AnimationCurve[] locationCurves;

    [Title(label: "Rotation Settings")]

    [Range(0.0f, 10.0f)]
    [Tooltip("로테이션 곡선에 적용되는 곱셈값")]
    [SerializeField]
    public float rotationMultiplier;

    [Tooltip("애니메이션 로테이션 곡선")]
    [SerializeField]
    public AnimationCurve[] rotationCurves;

}
