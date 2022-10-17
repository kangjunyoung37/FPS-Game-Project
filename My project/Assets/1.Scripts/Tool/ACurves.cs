using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SO_AC_Default",menuName = "FPS Game/Animation Curves")]
public class ACurves : ScriptableObject
{
    #region PROPERTIES

    public SpringSettings LocationSpring => locationSpring;
    public AnimationCurve[] LocationCurves => locationCurves;
    public float LocationMultiplier => locationMutiplier;

    public SpringSettings RotationSpring => rotationSpring;
    public AnimationCurve[] RotationsCurves => rotationCurves;
    public float RotationMultiplier => rotationMutiplier;

    #endregion

    [Title(label: "Location Settings")]

    [Range(0.0f, 10.0f)]
    [Tooltip("��ġ ��� ����Ǵ� �������Դϴ�.")]
    [SerializeField]
    private float locationMutiplier = 1.0f;

    [Tooltip("��ġ�� ���� ���� �����Դϴ�.")]
    [SerializeField]
    private SpringSettings locationSpring = SpringSettings.Default();

    [Tooltip("�ִϸ��̼� ��ġ �")]
    [SerializeField]
    private AnimationCurve[] locationCurves;

    [Title(label: "Rotation Settings")]

    [Range(0.0f, 10.0f)]
    [Tooltip("�����̼� ��� ����Ǵ� �������Դϴ�")]
    [SerializeField]
    private float rotationMutiplier = 1.0f;

    [Tooltip("�����̼ǿ� ���� ���� �����Դϴ�.")]
    [SerializeField]
    private SpringSettings rotationSpring = SpringSettings.Default();


    [Tooltip("�ִϸ��̼� �����̼� �")]
    [SerializeField]
    private AnimationCurve[] rotationCurves;

}
