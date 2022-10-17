using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
// 둘 이상의 프레임에 대해 Spring 클래스에 적용되는 힘을 정의하는 데 사용됩니다.
public struct HeldForce
{
    #region PROPERTIES
    public Vector3 Force => force;

    public int Frames => frames;
    #endregion

    #region FIELDS SERIALZIED
    [Tooltip("프레임당 적용되는 힘")]
    [SerializeField]
    private Vector3 force;

    [Tooltip("힘을 가할 프레임")]
    [SerializeField]
    private int frames;

    #endregion
}
/// <summary>
/// Spring이 올바르게 작동하는 데 필요한 모든 설정을 정의합니다
/// Evaluate를 호출할 때 모든 데이터를 사용하기 위해 Spring에 직접 전달할 수 있습니다.
/// </summary>
[Serializable]
public struct SpringSettings 
{
    [Title(label: "Spring")]

    [BeginHorizontal(labelToWidthRatio: 0.15f)]
    [Tooltip("스프링의 탄력성을 결정합니다. 이 값이 낮을수록 더 많은 바운스를 볼 수 있습니다.")]
    [Range(0.0f, 100.0f)]
    public float damping;

    [EndHorizontal]
    [Tooltip("보간이 얼마나 뻣뻣한지 결정합니다. 값이 낮을수록 더 단단해집니다.")]
    [Range(0.0f, 200.0f)]
    public float stiffness;

    [Title(label: "Modifiers")]

    [BeginHorizontal(labelToWidthRatio: 0.15f)]
    [Tooltip("보간이 얼마나 무거운지 결정합니다.")]
    [Range(0.0f, 100.0f)]
    public float mass;

    [EndHorizontal]
    [Tooltip("보간 속도를 결정합니다. 값이 높을수록 속도가 빨라집니다.")]
    [Range(1.0f, 10.0f)]
    public float speed;

    /// <summary>
    /// 스프링 초기 설정값
    /// </summary>
    public static SpringSettings Default()
    {
        return new SpringSettings()
        {
            damping = 15.0f,
            mass = 1.0f,
            stiffness = 150.0f,
            speed = 1.0f
        };
    }
}
