using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// FeelState. 
/// FeelStateOffset, SwayData, 점프 낙하 착지 애니메이션 곡선이 포함되어있다.
/// </summary>
[Serializable]
public struct FeelState 
{
    #region PROPERTIES

    /// <summary>
    /// FeelStateOffset
    /// </summary>
    public FeelStateOffset Offset => offset;

    /// <summary>
    /// 흔들림 데이터
    /// </summary>
    public SwayData SwayData => swayData;

    /// <summary>
    /// 점프 애니메이션 곡선
    /// </summary>
    public ACurves JumpingCurves => jumpingCurves;

    /// <summary>
    /// 낙하 애니메이션 곡선
    /// </summary>
    public ACurves FallingCurves => fallingCurves;

    /// <summary>
    /// 착지 애니메이션 곡선
    /// </summary>
    public ACurves LandingCurves => landingCurves;


    #endregion

    #region FIELDS SERIALIZED

    [Title(label: "Offset")]

    [Tooltip("FeelState 오프셋")]
    [SerializeField, InLineEditor]
    public FeelStateOffset offset;

    [Title(label: "Sway Data")]

    [Tooltip("흔들림과 관련된 세팅")]
    [SerializeField, InLineEditor]
    public SwayData swayData;

    [Title(label: "Jumping Curves")]

    [Tooltip("캐릭터가 점프할 때 플레이되는 애니메이션 곡선")]
    [SerializeField, InLineEditor]
    public ACurves jumpingCurves;

    [Title(label: "Falling Curves")]

    [Tooltip("캐릭터가 떨어질 때 플레이되는 애니메이션 곡선")]
    [SerializeField, InLineEditor]
    public ACurves fallingCurves;

    [Title(label: "Landing Curves")]

    [Tooltip("캐릭터가 착지할 때 플레이되는 애니메이션 곡선")]
    [SerializeField, InLineEditor]
    public ACurves landingCurves;

    #endregion
}
