using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// FeelState. 
/// FeelStateOffset, SwayData, ���� ���� ���� �ִϸ��̼� ��� ���ԵǾ��ִ�.
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
    /// ��鸲 ������
    /// </summary>
    public SwayData SwayData => swayData;

    /// <summary>
    /// ���� �ִϸ��̼� �
    /// </summary>
    public ACurves JumpingCurves => jumpingCurves;

    /// <summary>
    /// ���� �ִϸ��̼� �
    /// </summary>
    public ACurves FallingCurves => fallingCurves;

    /// <summary>
    /// ���� �ִϸ��̼� �
    /// </summary>
    public ACurves LandingCurves => landingCurves;


    #endregion

    #region FIELDS SERIALIZED

    [Title(label: "Offset")]

    [Tooltip("FeelState ������")]
    [SerializeField, InLineEditor]
    public FeelStateOffset offset;

    [Title(label: "Sway Data")]

    [Tooltip("��鸲�� ���õ� ����")]
    [SerializeField, InLineEditor]
    public SwayData swayData;

    [Title(label: "Jumping Curves")]

    [Tooltip("ĳ���Ͱ� ������ �� �÷��̵Ǵ� �ִϸ��̼� �")]
    [SerializeField, InLineEditor]
    public ACurves jumpingCurves;

    [Title(label: "Falling Curves")]

    [Tooltip("ĳ���Ͱ� ������ �� �÷��̵Ǵ� �ִϸ��̼� �")]
    [SerializeField, InLineEditor]
    public ACurves fallingCurves;

    [Title(label: "Landing Curves")]

    [Tooltip("ĳ���Ͱ� ������ �� �÷��̵Ǵ� �ִϸ��̼� �")]
    [SerializeField, InLineEditor]
    public ACurves landingCurves;

    #endregion
}
