//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// 위치 및 회전 곡선을 포함하고 있으며 Spring 클래스를 사용하여 보간하는 설정을 포함하고 있습니다.
    /// 곡선을 사용하는 절차적 motion에 사용합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_SD_Default", menuName = "Infima Games/Low Poly Shooter Pack/Sway Data")]
    public class SwayData : ScriptableObject
    {
        #region PROPERTIES

        /// <summary>
        /// Look.
        /// </summary>
        public SwayType Look => look;

        /// <summary>
        /// Movement.
        /// </summary>
        public SwayType Movement => movement;

        /// <summary>
        /// SpringSettings.
        /// </summary>
        public SpringSettings SpringSettings => springSettings;
        
        #endregion
        
        #region FIELDS SERIALIZED

        [Title(label: "Look")]
        
        [Tooltip("Look Sway.")]
        [SerializeField]
        private SwayType look;

        [Title(label: "Movement")]
        
        [Tooltip("Movement Sway.")]
        [SerializeField]
        private SwayType movement;
        
        [Title(label: "Spring Settings")]
        
        [Tooltip("Sway를 위한 SpringSettings")]
        [SerializeField]
        private SpringSettings springSettings = SpringSettings.Default();
        
        #endregion
    }
}