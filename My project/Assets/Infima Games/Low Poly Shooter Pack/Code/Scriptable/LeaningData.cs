//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// 기울기 데이터
    /// 플레이어가 기울어지면서 카메라와 무기 또한 기울어지게 작동하게 하는 것과 관련된 데이터들 
    /// </summary>
    [CreateAssetMenu(fileName = "SO_Leaning_Name", menuName = "Infima Games/Low Poly Shooter Pack/Leaning Data", order = 0)]
    public class LeaningData : ScriptableObject
    {
        #region FIELDS SERIALIZED
        
        [Title(label: "Item Curves")]
        
        [Tooltip("캐릭터가 조준하는 동안 기울일 때 아이템에서 재생되는 애니메이션 곡선.")]
        [SerializeField, InLineEditor]
        private ACurves itemAiming;
        
        [Tooltip("캐릭터가 서 있는 동안 기울일 때 아이템에서 재생되는 애니메이션 곡선")]
        [SerializeField, InLineEditor]
        private ACurves itemStanding;

        [Title(label: "Camera Curves")]
        
        [Tooltip("캐릭터가 조준하는 동안 기울일 때 카메라에서 재생되는 애니메이션 곡선")]
        [SerializeField, InLineEditor]  
        private ACurves cameraAiming;

        [Tooltip("캐릭터가 서 있는 동안 기울일 때 카메라에서 재생되는 애니메이션 곡선")]
        [SerializeField, InLineEditor]
        private ACurves cameraStanding;
        
        #endregion
        
        #region FUNCTIONS

        /// <summary>
        /// 요청한 모션 유형에 대한 곡선을 반환합니다.
        /// </summary>
        public ACurves GetCurves(MotionType motionType, bool aiming = false)
        {
            //Switch.
            return motionType switch
            {
                //MotionType.Camera.
                MotionType.Camera => aiming ? cameraAiming : cameraStanding,
                //MotionType.Item.
                MotionType.Item => aiming ? itemAiming : itemStanding,
                //Default.
                _ => itemStanding
            };
        }
        
        #endregion
    }
}