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

    [Tooltip("서있는 동안 무기 뼈대 위치 오프셋")]
    [SerializeField]
    private Vector3 standingLocation;

    [Tooltip("서있는 동안 무기 뼈대 로테이션 오프셋")]
    [SerializeField]
    private Vector3 standingRotation;

    [Title(label: "Aiming Offset")]

    [Tooltip("조준하고 있는 동안 무기 뼈대 위치 오프셋")]
    [SerializeField]
    private Vector3 aimingLocation;

    [Tooltip("조준하고 있는 동안 무기 뼈대 로테이션 오프셋")]
    [SerializeField]
    private Vector3 aimingRotation;

    [Title(label: "Running Offset")]

    [Tooltip("달리고 있는 동안 무기 뼈대 위치 오프셋")]
    [SerializeField]
    private Vector3 runningLocation;

    [Tooltip("달리고 있는 동안 무기 뼈대 로테이션 오프셋")]
    [SerializeField]
    private Vector3 runningRotation;

    [Title(label: "Crouching Offset")]

    [Tooltip("웅크리고 있는 동안 무기 뼈대 위치 오프셋")]
    [SerializeField]
    private Vector3 crouchingLocation;

    [Tooltip("웅크리고 있는 동안 무기 뼈대 로테이션 오프셋")]
    [SerializeField]
    private Vector3 crouchingRotation;

    [Title(label: "Action Offset")]

    [Tooltip("(수류탄,근접)action을 수행하는 동안 무기 뼈대 위치 오프셋")]
    [SerializeField]
    private Vector3 actionLocation;

    [Tooltip("(수류탄,근접)action을 수행하는 동안 무기 뼈대 로테이션 오프셋")]
    [SerializeField]
    private Vector3 actionRotation;

}
