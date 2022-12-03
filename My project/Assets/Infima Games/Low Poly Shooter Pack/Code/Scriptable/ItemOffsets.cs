//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// 여러 상태에서 아이템의 간격띄우기하는 방법에 대한 데이터를 포합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_IO_Default", menuName = "Infima Games/Low Poly Shooter Pack/Item Offsets", order = 0)]
    public class ItemOffsets : ScriptableObject
    {
        /// <summary>
        /// Standing Location.
        /// </summary>
        public Vector3 StandingLocation => standingLocation;
        /// <summary>
        /// Standing Rotation.
        /// </summary>
        public Vector3 StandingRotation => standingRotation;
        
        /// <summary>
        /// Aiming Location.
        /// </summary>
        public Vector3 AimingLocation => aimingLocation;
        /// <summary>
        /// Aiming Rotation.
        /// </summary>
        public Vector3 AimingRotation => aimingRotation;
        
        /// <summary>
        /// Running Location.
        /// </summary>
        public Vector3 RunningLocation => runningLocation;
        /// <summary>
        /// Running Rotation.
        /// </summary>
        public Vector3 RunningRotation => runningRotation;
        
        /// <summary>
        /// Crouching Location.
        /// </summary>
        public Vector3 CrouchingLocation => crouchingLocation;
        /// <summary>
        /// Crouching Rotation.
        /// </summary>
        public Vector3 CrouchingRotation => crouchingRotation;
        
        /// <summary>
        /// Action Location.
        /// </summary>
        public Vector3 ActionLocation => actionLocation;
        /// <summary>
        /// Action Rotation.
        /// </summary>
        public Vector3 ActionRotation => actionRotation;
        
        [Title(label: "Standing Offset")]
        
        [Tooltip("서 있는동안 무기 뼈대의 위치 오프셋")]
        [SerializeField]
        private Vector3 standingLocation;
        
        [Tooltip("서 있는동안 무기 뼈대의 로테이션 오프셋")]
        [SerializeField]
        private Vector3 standingRotation;

        [Title(label: "Aiming Offset")]
        
        [Tooltip("조준하는 동안 무기 뼈대의 위치 오프셋")]
        [SerializeField]
        private Vector3 aimingLocation;
        
        [Tooltip("조준하는 동안 무기 뼈대의 로테이션 오프셋")]
        [SerializeField]
        private Vector3 aimingRotation;
        
        [Title(label: "Running Offset")]
        
        [Tooltip("달리고 있는 동안 무기 뼈대의 위치 오프셋")]
        [SerializeField]
        private Vector3 runningLocation;
        
        [Tooltip("달리고 있는 동안 무기 뼈대의 로테이션 오프셋")]
        [SerializeField]
        private Vector3 runningRotation;
        
        [Title(label: "Crouching Offset")]
        
        [Tooltip("웅크리고 있을 때 무기 뼈대의 위치 오프셋")]
        [SerializeField]
        private Vector3 crouchingLocation;
        
        [Tooltip("웅크리고 있을 때 무기 뼈대의 로테이션 오프셋")]
        [SerializeField]
        private Vector3 crouchingRotation;
        
        [Title(label: "Action Offset")]
        
        [Tooltip("수류탄을 던지거나 근접공격 행동을 취할 때의 무기 뼈대의 위치 오프셋.")]
        [SerializeField]
        private Vector3 actionLocation;
        
        [Tooltip("수류탄을 던지거나 근접공격 행동을 취할 때 무기 뼈대의 로테이션 오프셋")]
        [SerializeField]
        private Vector3 actionRotation;
    }
}