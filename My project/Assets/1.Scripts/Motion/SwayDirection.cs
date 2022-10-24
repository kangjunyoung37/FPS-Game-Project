using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ִϸ��̼ǿ� ����Ǵ� ��������
/// �ִϸ��̼� ��� ���ԵǾ��ֽ��ϴ�.
/// </summary>
[Serializable]
public struct SwayDirection
{
    [Title(label: "Location Settings")]

    [Range(0.0f, 10.0f)]
    [Tooltip("��ġ ��� ����Ǵ� ������")]
    [SerializeField]
    public float locationMultiplier;

    [Tooltip("�ִϸ��̼� ��ġ �")]
    [SerializeField]
    public AnimationCurve[] locationCurves;

    [Title(label: "Rotation Settings")]

    [Range(0.0f, 10.0f)]
    [Tooltip("�����̼� ��� ����Ǵ� ������")]
    [SerializeField]
    public float rotationMultiplier;

    [Tooltip("�ִϸ��̼� �����̼� �")]
    [SerializeField]
    public AnimationCurve[] rotationCurves;

}
