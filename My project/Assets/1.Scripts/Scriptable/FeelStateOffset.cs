using InfimaGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��ġ ������,�����̼� ������, ��ġSpringSetting, RotationSpringSetting ���ԵǾ��ִ�.
/// ��Ȯ�� ������ ���� FeelStates���� ���˴ϴ�
/// </summary>
[CreateAssetMenu(fileName ="SO_FSO_Default",menuName ="FPS Game/Shooter Pack/Feel State Offset",order = 0)]
public class FeelStateOffset : ScriptableObject
{
    #region PROPERTIES

    /// <summary>
    /// ��ġ ������
    /// </summary>
    public Vector3 OffsetLocation => offsetLocation;

    /// <summary>
    /// ��ġ ���� ����
    /// </summary>
    public SpringSettings SpringSettingsLocation => springSettingsLocation;

    /// <summary>
    /// �����̼� ������
    /// </summary>
    public Vector3 OffsetRotation => offsetRotation;

    /// <summary>
    /// �����̼� ���� ���� 
    /// </summary>
    public SpringSettings SpringSettingsRotation => springSettingsRotation;

    #endregion


    #region FIELDS SERIALIZED

    [Title(label: "Location Offset")]

    [Tooltip("��ġ ������")]
    [SerializeField]
    public Vector3 offsetLocation;

    [Tooltip("��ġ ������ ���õ� ������ ���� ")]
    [SerializeField]
    public SpringSettings springSettingsLocation = SpringSettings.Default();

    [Title("Rotation Offset")]

    [Tooltip("�����̼� ������")]
    [SerializeField]
    public Vector3 offsetRotation;

    [Tooltip("�����̼� ������ ���õ� ������ ����")]
    [SerializeField]
    private SpringSettings springSettingsRotation = SpringSettings.Default();

    #endregion
}
