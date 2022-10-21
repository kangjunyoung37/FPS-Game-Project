using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���
/// ��� ���ö��̾ ���� Motion�� ���� ��� �����Ұ��� �����մϴ�.
/// </summary>
public enum ApplyMode { Override, Add }

/// <summary>
/// ��Ǹ���Ʈ, Transform, ��ǿ� �����Ű��,���� ��尡 ���ԵǾ� �ֽ��ϴ�.
/// </summary>
public class MotionApplier : MonoBehaviour
{

    #region FIELDS SERIALIZED

    [Title(label: "Settings")]

    [Tooltip("Motion�� ���� �����ϴ� ���")]
    [SerializeField]
    private ApplyMode applyMode;

    #endregion

    #region FIELDS

    /// <summary>
    /// ��ϵǾ� �ִ� ��ǵ�
    /// </summary>
    private readonly List<Motion> motions = new List<Motion>();

    /// <summary>
    /// Transform
    /// </summary>
    private Transform thisTransform;

    #endregion

    #region METHODS

    private void Awake()
    {
        //ĳ��
        thisTransform = transform;
    }

    private void LateUpdate()
    {
        //������ ��ġ(���)
        Vector3 finalLocation = default;

        //������ ���Ϸ��ޱ�(���)
        Vector3 finalEulerAngles = default;

        motions.ForEach((motion =>
        {
            //Tick
            motion.Tick();

            //��ǿ� ���� ��ġ ��갪�� �޾ƿͼ� finalLocation�� �ֱ�
            finalLocation += motion.GetLocation() * motion.Alpha;

            //��ǿ� ���� �����̼� ��갪�� �޾ƿͼ� finalRotation�� �ֱ�
            finalEulerAngles += motion.GetEulerAngles() * motion.Alpha;

        }));

        //�������̵� ���(���� �����)
        if(applyMode == ApplyMode.Override)
        {
            //�����̼� �� ����ֱ�
            thisTransform.localPosition = finalLocation;

            //EulerAngles �� ����ֱ�
            thisTransform.localEulerAngles = finalEulerAngles;
        }
        else if(applyMode == ApplyMode.Add)
        {
            //�����̼� �� ���ϱ�
            thisTransform.localPosition += finalLocation;

            //EulerAngles �� ���ϱ�
            thisTransform.localEulerAngles += finalEulerAngles;

        }

        //���ϱ� ���(���ϱ�)
    }

    /// <summary>
    /// motion����Ʈ�� motion �߰��ϱ�
    /// </summary>
    /// <param name="motion">���</param>
    public void Subscribe(Motion motion) => motions.Add(motion);

    #endregion

}
