using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ĳ���Ͱ� ��ǰ� ���������� ���⸦ ������ �� �ʿ��� ������ �����մϴ�.
/// </summary>
[CreateAssetMenu(fileName ="SO_Lower_Name",menuName = "FPS Game/Shooter Pack/Lower Data",order = 0)]
public class LowerData : ScriptableObject
{
    #region PROPERTIES

    /// <summary>
    /// ���� ����
    /// </summary>
    public SpringSettings Interpolation => interpolation;

    /// <summary>
    /// ��ġ ������
    /// </summary>
    public Vector3 LocationOffset => locationOffset;

    /// <summary>
    /// �����̼� ������
    /// </summary>
    public Vector3 RotationOffset => rotationOffset;

    #endregion

    #region FIELDS SERIALIZED

    [Title(label: "Interpolation")]

    [Tooltip("���� ����")]
    [SerializeField]
    private SpringSettings interpolation = SpringSettings.Default();

    [Title(label: "Offsets")]

    [Tooltip("���⸦ ���� ���¿��� ����� ��ġ �����°�")]
    [SerializeField]
    private Vector3 locationOffset;

    [Tooltip("���⸦ ���� ���¿��� ����� �����̼� ������")]
    [SerializeField]
    private Vector3 rotationOffset;

    #endregion
}
