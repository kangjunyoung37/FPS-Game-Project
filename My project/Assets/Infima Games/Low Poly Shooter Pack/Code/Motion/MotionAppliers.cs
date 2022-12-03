//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Collections.Generic;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// 적용 모드
    /// MotionApplier에 속한 Motion의 값을 어떻게 적용할건지 결정합니다.
    /// </summary>
    public enum ApplyMode { Override, Add }
    
    /// <summary>
    /// 이 구성 요소의 설정에 따라 속한 모션 구성 요소의 모든 위치, 회전 값을 적용합니다.
    /// </summary>
    public class MotionAppliers : MonoBehaviour
    {
        #region FIELDS SERIALIZED
        
        [Title(label: "Settings")]

        [Tooltip("모션 구성 요소에 값을 적용하는 방법")]
        [SerializeField]
        private ApplyMode applyMode;
        
        #endregion
        
        #region FIELDS
        
        /// <summary>
        /// Subscribed Motions.
        /// </summary>
        private readonly List<Motions> motions = new List<Motions>();

        /// <summary>
        /// This Transform.
        /// </summary>
        private Transform thisTransform;

        #endregion
        
        #region METHODS

        /// <summary>
        /// Awake.
        /// </summary>
        private void Awake()
        {
            //Cache.
            thisTransform = transform;
        }
        /// <summary>
        /// LateUpdate.
        /// </summary>
        private void LateUpdate()
        {
            //마지막 위치
            Vector3 finalLocation = default;
            //마지막 오일러 앵글.
            Vector3 finaEulerAngles = default;
            
            //모든 모션들 돌면서
            motions.ForEach((motion =>
            {
                //Tick.
                motion.Tick();
                
                //Add Location.
                finalLocation += motion.GetLocation() * motion.Alpha;
                //Add Rotation.
                finaEulerAngles += motion.GetEulerAngles() * motion.Alpha;
            }));

            //Override Mode.
            if(applyMode == ApplyMode.Override)
            {
                //Location 설정.
                thisTransform.localPosition = finalLocation;
                //EulerAngles 설정.
                thisTransform.localEulerAngles = finaEulerAngles;
            }
            //Add Mode.
            else if (applyMode == ApplyMode.Add)
            {
                //Location 더하기.
                thisTransform.localPosition += finalLocation;
                //Euler Angles 더하기.
                thisTransform.localEulerAngles += finaEulerAngles;
            }
        }
        
        /// <summary>
        /// MotionApplier에 모션을 구독합니다.
        /// Motio 결과는 Motion Applier에 의해 계산되고 적용됩니다.
        /// </summary>
        public void Subscribe(Motions motion) => motions.Add(motion);
        
        #endregion
    }
}