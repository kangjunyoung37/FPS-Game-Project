using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��ġ �� ȸ�� ��� �����ϰ� ������, SpringŬ������ ����Ͽ� �����ϴ� ������ �����ϰ� �ֽ��ϴ�.
/// ��� ����ϴ� motion��ũ��Ʈ�� ����մϴ�.
/// </summary>
[CreateAssetMenu(fileName ="SO_SD_Default",menuName ="FPS Game/Shooter Pack/SwayData")]
public class SwayData : ScriptableObject
{

    #region PROPERTIES

    /// <summary>
    /// �þ�
    /// </summary>
    public SwayType Look => look;

    /// <summary>
    /// ������
    /// </summary>
    public SwayType Movement => movement;

    /// <summary>
    /// ������ ����
    /// </summary>
    public SpringSettings SpringSettings => springSettings;

    #endregion

    #region FIELDS SERIALIZED

    [Title(label: "Look")]

    [Tooltip("�þ� ��鸲")]
    [SerializeField]
    private SwayType look;

    [Title(label: "Movement")]

    [Tooltip("������ ��鸲")]
    [SerializeField]
    private SwayType movement;

    [Title(label: "Spring Settings")]

    [Tooltip("��鸲�� ���� SpringSettings")]
    [SerializeField]
    private SpringSettings springSettings = SpringSettings.Default();

    #endregion
}
