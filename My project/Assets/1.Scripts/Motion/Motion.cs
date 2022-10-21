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

    [Tooltip("����� ���� ������ ���� ���� �����ϴµ� ���˴ϴ�.")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float alpha = 1.0f;

    [Title(label: "References")]

    [Tooltip("����� �����ϴ� ��Ǿ��ö��̾�")]
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
    /// ��ġ ��������
    /// </summary>
    public abstract Vector3 GetLocation();

    /// <summary>
    /// ���Ϸ� �ޱ� ��������
    /// </summary>
    public abstract Vector3 GetEulerAngles();

    #endregion
}
