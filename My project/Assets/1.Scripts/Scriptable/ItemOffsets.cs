using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="SO_IO_Default,",menuName = "FPS Game/Shooter Pack/Item Offsets",order = 0)]
public class ItemOffsets : ScriptableObject
{

    #region PROPERTY

    public Vector3 StandingLocation => standingLocation;
    public Vector3 StandingRotation => standingRotation;
    public Vector3 AimingLocation => aimingLocation;
    public Vector3 AimingRotation => aimingRotation;
    public Vector3 RunningLocation => runningLocation;
    public Vector3 RunningRotation => runningRotation;
    public Vector3 CrouchingLocation => crouchingLocation;
    public Vector3 CrouchingRotation => crouchingRotation;
    public Vector3 ActionLocation => actionLocation;
    public Vector3 ActionRotation => actionRotation;

    #endregion

    [Title(label:"Standing Offset")]

    [Tooltip("���ִ� ���� ���� ���� ��ġ ������")]
    [SerializeField]
    private Vector3 standingLocation;

    [Tooltip("���ִ� ���� ���� ���� �����̼� ������")]
    [SerializeField]
    private Vector3 standingRotation;

    [Title(label: "Aiming Offset")]

    [Tooltip("�����ϰ� �ִ� ���� ���� ���� ��ġ ������")]
    [SerializeField]
    private Vector3 aimingLocation;

    [Tooltip("�����ϰ� �ִ� ���� ���� ���� �����̼� ������")]
    [SerializeField]
    private Vector3 aimingRotation;

    [Title(label: "Running Offset")]

    [Tooltip("�޸��� �ִ� ���� ���� ���� ��ġ ������")]
    [SerializeField]
    private Vector3 runningLocation;

    [Tooltip("�޸��� �ִ� ���� ���� ���� �����̼� ������")]
    [SerializeField]
    private Vector3 runningRotation;

    [Title(label: "Crouching Offset")]

    [Tooltip("��ũ���� �ִ� ���� ���� ���� ��ġ ������")]
    [SerializeField]
    private Vector3 crouchingLocation;

    [Tooltip("��ũ���� �ִ� ���� ���� ���� �����̼� ������")]
    [SerializeField]
    private Vector3 crouchingRotation;

    [Title(label: "Action Offset")]

    [Tooltip("(����ź,����)action�� �����ϴ� ���� ���� ���� ��ġ ������")]
    [SerializeField]
    private Vector3 actionLocation;

    [Tooltip("(����ź,����)action�� �����ϴ� ���� ���� ���� �����̼� ������")]
    [SerializeField]
    private Vector3 actionRotation;

}
