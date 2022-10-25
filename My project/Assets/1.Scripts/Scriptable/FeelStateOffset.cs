using InfimaGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 위치 오프셋,로테이션 오프셋, 위치SpringSetting, RotationSpringSetting 포함되어있다.
/// 정확한 목적을 위해 FeelStates에서 사용됩니다
/// </summary>
[CreateAssetMenu(fileName ="SO_FSO_Default",menuName ="FPS Game/Shooter Pack/Feel State Offset",order = 0)]
public class FeelStateOffset : ScriptableObject
{
    #region PROPERTIES

    /// <summary>
    /// 위치 오프셋
    /// </summary>
    public Vector3 OffsetLocation => offsetLocation;

    /// <summary>
    /// 위치 보간 설정
    /// </summary>
    public SpringSettings SpringSettingsLocation => springSettingsLocation;

    /// <summary>
    /// 로테이션 오프셋
    /// </summary>
    public Vector3 OffsetRotation => offsetRotation;

    /// <summary>
    /// 로테이션 보간 설정 
    /// </summary>
    public SpringSettings SpringSettingsRotation => springSettingsRotation;

    #endregion


    #region FIELDS SERIALIZED

    [Title(label: "Location Offset")]

    [Tooltip("위치 오프셋")]
    [SerializeField]
    public Vector3 offsetLocation;

    [Tooltip("위치 보간과 관련된 스프링 설정 ")]
    [SerializeField]
    public SpringSettings springSettingsLocation = SpringSettings.Default();

    [Title("Rotation Offset")]

    [Tooltip("로테이션 오프셋")]
    [SerializeField]
    public Vector3 offsetRotation;

    [Tooltip("로테이션 보간과 관련된 스프링 설정")]
    [SerializeField]
    private SpringSettings springSettingsRotation = SpringSettings.Default();

    #endregion
}
