//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// SwayMotion이 사용할 수평, 수직 SwayDirection 값에 대한 정보를 보유합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_ST_Default", menuName = "Infima Games/Low Poly Shooter Pack/Sway Type")]
    public class SwayType : ScriptableObject
    {
        #region PROPERTIES

        /// <summary>
        /// Horizontal.
        /// </summary>
        public SwayDirection Horizontal => horizontal;
        /// <summary>
        /// Vertical.
        /// </summary>
        public SwayDirection Vertical => vertical;
        
        #endregion
        
        #region FIELDS SERIALIZED
        
        [Title(label: "Horizontal")]
        
        [Tooltip("수평 Sway.")]
        [SerializeField]
        private SwayDirection horizontal;

        [Title(label: "Vertical")]
        
        [Tooltip("수직 Sway.")]
        [SerializeField]
        private SwayDirection vertical;
        
        #endregion
    }
}