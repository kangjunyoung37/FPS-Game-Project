//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// 반동 동작을 재생할 때 필요한 모든 정보를 가지고 있다.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_Recoil", menuName = "Infima Games/Low Poly Shooter Pack/Recoil Data", order = 0)]
    public class RecoilData : ScriptableObject
    {
        #region PROPERTIES

        /// <summary>
        /// StandingStateMultiplier.
        /// </summary>
        public float StandingStateMultiplier => standingStateMultiplier;
        /// <summary>
        /// Standing Curves.
        /// </summary>
        public ACurves StandingState => standingState;
        
        /// <summary>
        /// AimingStateMultiplier.
        /// </summary>
        public float AimingStateMultiplier => aimingStateMultiplier;
        /// <summary>
        /// Aiming Curves.
        /// </summary>
        public ACurves AimingState => aimingState;
        
        #endregion
        
        #region FIELDS SERIALIZED

        [Title(label: "Standing State")]
        
        [Tooltip("StandingState 위치/회전 값에 곱하는 값입니다.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float standingStateMultiplier = 1.0f;

        [Tooltip("Standing State.")]
        [SerializeField, InLineEditor]
        private ACurves standingState;

        [Title(label: "Aiming State")]

        [Tooltip("aimingState 위치/회전 값에 를 곱하는 값입니다.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float aimingStateMultiplier = 1.0f;
        
        [Tooltip("Aiming State.")]
        [SerializeField, InLineEditor]
        private ACurves aimingState;
        
        #endregion
    }
}