using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 위치 및 회전 곡선을 포함하고 있으며, Spring클래스를 사용하여 보간하는 설정을 포함하고 있습니다.
/// 곡선을 사용하는 motion스크립트에 사용합니다.
/// </summary>
[CreateAssetMenu(fileName ="SO_SD_Default",menuName ="FPS Game/Shooter Pack/SwayData")]
public class SwayData : ScriptableObject
{

    #region PROPERTIES

    /// <summary>
    /// 시야
    /// </summary>
    public SwayType Look => look;

    /// <summary>
    /// 움직임
    /// </summary>
    public SwayType Movement => movement;

    /// <summary>
    /// 스프링 설정
    /// </summary>
    public SpringSettings SpringSettings => springSettings;

    #endregion

    #region FIELDS SERIALIZED

    [Title(label: "Look")]

    [Tooltip("시야 흔들림")]
    [SerializeField]
    private SwayType look;

    [Title(label: "Movement")]

    [Tooltip("움직임 흔들림")]
    [SerializeField]
    private SwayType movement;

    [Title(label: "Spring Settings")]

    [Tooltip("흔들림을 위한 SpringSettings")]
    [SerializeField]
    private SpringSettings springSettings = SpringSettings.Default();

    #endregion
}
