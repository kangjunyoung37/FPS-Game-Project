using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;


public enum MotionType { Camera, Item }
[RequireComponent(typeof(MotionApplier))]
public abstract class Motion : MonoBehaviour
{
    #region PROPERTIES

    public float Alpha => alpha;

    #endregion


    #region FIELDS SERIALIZED

    [Title(label: "Motion")]

    [Tooltip("모션의 적용 정도를 보다 쉽게 제어하는데 사용됩니다.")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float alpha = 1.0f;

    [Title(label: "References")]

    [Tooltip("모션을 적용하는 모션어플라이어")]
    [SerializeField, NotNull]
    protected MotionApplier motionApplier;

    #endregion

    #region METHODS

    protected virtual void Awake()
    {
        if (motionApplier == null)
            motionApplier = GetComponent<MotionApplier>();

        if (motionApplier != null)
            motionApplier.Subscribe(this);
    }

    public abstract void Tick();

    #endregion

    #region FUNCTIONS

    /// <summary>
    /// 위치 가져오기
    /// </summary>
    public abstract Vector3 GetLocation();

    /// <summary>
    /// 오일러 앵글 가져오기
    /// </summary>
    public abstract Vector3 GetEulerAngles();

    #endregion
}
