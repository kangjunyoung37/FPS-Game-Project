using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 반동 동작을 재생할 때 필요한 모든 정보를 가지고 있다.
/// </summary>
[CreateAssetMenu(fileName = "SO_Recoil",menuName = "FPS Game/Shooter Pack/Recoil Data",order = 0)]
public class RecoilData : ScriptableObject
{

    #region PROPERTIES
    
    /// <summary>
    /// StandingStateMultiplier
    /// </summary>
    public float StandingStateMultiplier => standingStateMutilplier;

    /// <summary>
    /// Standing Curves
    /// </summary>
    public ACurves StandingState => standingState;

    /// <summary>
    /// AimingStateMultiplier
    /// </summary>
    public float AimingStateMultiplier => AimingStateMultiplier;

    /// <summary>
    /// Aiming Curves
    /// </summary>
    public ACurves AimingState => aimingState;

    #endregion

    #region FIELDS SERIALIZED
    [Title(label: "Standing State")]

    [Tooltip("서있는 상태에서 위치/회전 값에 곱하는 값입니다.")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float standingStateMutilplier = 1.0f;

    [Tooltip("서있는 상태")]
    [SerializeField, InLineEditor]
    private ACurves standingState;

    [Title(label: "Aiming State")]

    [Tooltip("조준하고 있는 장태에서 위치/회전 값에 곱하는 값입니다")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float aimingStateMultiplier = 1.0f;

    [Tooltip("Aiming State")]
    [SerializeField, InLineEditor]
    private ACurves aimingState;

    #endregion
}
