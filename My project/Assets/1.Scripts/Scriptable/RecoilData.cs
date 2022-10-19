using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ݵ� ������ ����� �� �ʿ��� ��� ������ ������ �ִ�.
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

    [Tooltip("���ִ� ���¿��� ��ġ/ȸ�� ���� ���ϴ� ���Դϴ�.")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float standingStateMutilplier = 1.0f;

    [Tooltip("���ִ� ����")]
    [SerializeField, InLineEditor]
    private ACurves standingState;

    [Title(label: "Aiming State")]

    [Tooltip("�����ϰ� �ִ� ���¿��� ��ġ/ȸ�� ���� ���ϴ� ���Դϴ�")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float aimingStateMultiplier = 1.0f;

    [Tooltip("Aiming State")]
    [SerializeField, InLineEditor]
    private ACurves aimingState;

    #endregion
}
