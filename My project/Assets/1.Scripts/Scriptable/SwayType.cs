using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SwayMotion이 사용할 수평, 수직 SwayDiretion 값에 대한 정보를 가지고 있습니다.
/// </summary>
[CreateAssetMenu(fileName = "SO_ST_Default",menuName ="FPS Game/Shooter Pack/Sway Type")]
public class SwayType : ScriptableObject
{

    #region PROPERTIES

    /// <summary>
    /// Horizontal
    /// </summary>
    public SwayDirection Horizontal => horizontal;

    /// <summary>
    /// Vertical
    /// </summary>
    public SwayDirection Vertical => vertical;

    #endregion

    #region FILEDS SERIALIZED
    [Title(label: "Horizontal")]

    [Tooltip("Horizontal Sway")]
    [SerializeField]
    private SwayDirection horizontal;

    [Title(label: "Vertical")]

    [Tooltip("Vertical Sway")]
    [SerializeField]
    private SwayDirection vertical;

    #endregion
}
