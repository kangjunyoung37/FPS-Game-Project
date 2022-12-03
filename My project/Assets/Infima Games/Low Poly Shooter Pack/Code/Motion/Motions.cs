//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// MotionType.
    /// </summary>
    public enum MotionType { Camera, Item }
    
    /// <summary>
    /// 에셋의 무기나 카메라에 절차적 움직임을 적용하는 모든 컴포넌트의 기본 클래스 역할을 합니다
    /// 모션 어플리어를 통해 실행됩니다.
    /// </summary>
    [RequireComponent(typeof(MotionAppliers))]
    public abstract class Motions : MonoBehaviour
    {
        #region PROPERTIES
        
        /// <summary>
        /// Alpha.
        /// </summary>
        public float Alpha => alpha;
        
        #endregion
        
        #region FIELDS SERIALIZED
        
        [Title(label: "Motion")]
        
        [Tooltip("모션의 적용 정도를 보다 쉽게 제어하는 데 사용됩니다.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float alpha = 1.0f;

        [Title(label: "References")]
        
        [Tooltip("이 모션 값을 적용할 모션 motionApplier")]
        [SerializeField, NotNull]
        protected MotionAppliers motionApplier;
        
        #endregion
        
        #region METHODS
        
        /// <summary>
        /// Awake.
        /// </summary>
        protected virtual void Awake()
        {
            //Try to get the applier if we haven't assigned it.
            if (motionApplier == null)
                motionApplier = GetComponent<MotionAppliers>();
            
            //Subscribe.
            if(motionApplier != null)
                motionApplier.Subscribe(this);
        }

        /// <summary>
        /// Tick.
        /// </summary>
        public abstract void Tick();
        
        #endregion
        
        #region FUNCTIONS
        
        /// <summary>
        /// 위치 가져오기
        /// </summary>
        public abstract Vector3 GetLocation();
        /// <summary>
        /// 오일러 앵글 가져오기.
        /// </summary>
        public abstract Vector3 GetEulerAngles();
        
        #endregion
    }
}