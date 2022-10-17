using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
// �� �̻��� �����ӿ� ���� Spring Ŭ������ ����Ǵ� ���� �����ϴ� �� ���˴ϴ�.
public struct HeldForce
{
    #region PROPERTIES
    public Vector3 Force => force;

    public int Frames => frames;
    #endregion

    #region FIELDS SERIALZIED
    [Tooltip("�����Ӵ� ����Ǵ� ��")]
    [SerializeField]
    private Vector3 force;

    [Tooltip("���� ���� ������")]
    [SerializeField]
    private int frames;

    #endregion
}
/// <summary>
/// Spring�� �ùٸ��� �۵��ϴ� �� �ʿ��� ��� ������ �����մϴ�
/// Evaluate�� ȣ���� �� ��� �����͸� ����ϱ� ���� Spring�� ���� ������ �� �ֽ��ϴ�.
/// </summary>
[Serializable]
public struct SpringSettings 
{
    [Title(label: "Spring")]

    [BeginHorizontal(labelToWidthRatio: 0.15f)]
    [Tooltip("�������� ź�¼��� �����մϴ�. �� ���� �������� �� ���� �ٿ�� �� �� �ֽ��ϴ�.")]
    [Range(0.0f, 100.0f)]
    public float damping;

    [EndHorizontal]
    [Tooltip("������ �󸶳� �������� �����մϴ�. ���� �������� �� �ܴ������ϴ�.")]
    [Range(0.0f, 200.0f)]
    public float stiffness;

    [Title(label: "Modifiers")]

    [BeginHorizontal(labelToWidthRatio: 0.15f)]
    [Tooltip("������ �󸶳� ���ſ��� �����մϴ�.")]
    [Range(0.0f, 100.0f)]
    public float mass;

    [EndHorizontal]
    [Tooltip("���� �ӵ��� �����մϴ�. ���� �������� �ӵ��� �������ϴ�.")]
    [Range(1.0f, 10.0f)]
    public float speed;

    /// <summary>
    /// ������ �ʱ� ������
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
