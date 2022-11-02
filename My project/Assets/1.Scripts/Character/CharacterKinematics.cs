using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

/// <summary>
/// ĳ���Ϳ� �ʿ��� ��� ������� ó���մϴ�.
/// ����Ƽ�� IK�ڵ带 ����մϴ�.
/// </summary>
public class CharacterKinematics : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("ĳ���� �ִϸ������� ������Ʈ")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Title(label: "Settings Arm Left")]

    [Tooltip("���� IK����� �������� �����մϴ�")]
    [SerializeField]
    private Transform armLeftTarget;

    [Range(0.0f, 1.0f)]
    [Tooltip("���� �ȿ� ���� ����� ����ġ�Դϴ�. ��ġ")]
    [SerializeField]
    private float armLeftWeightPosition = 1.0f;

    [Range(0.0f, 1.0f)]
    [Tooltip("���� �ȿ� ���� ����� ����ġ�Դϴ�. ȸ��")]
    [SerializeField]
    private float armLeftWeightRoation = 1.0f;

    [Tooltip("���� ���� ����, ��Ʈ �̵� ��")]
    [SerializeField]
    private Transform[] armLeftHierarchy;

    [Title(label: "Settings Arm Right")]

    [Title(label: "Settings Arm Right")]
    [Tooltip("������ IK����� �������� �����մϴ�")]
    [SerializeField]
    private Transform armRightTarget;

    [Range(0.0f, 1.0f)]
    [Tooltip("���� �ȿ� ���� ����� ����ġ�Դϴ�. ��ġ")]
    [SerializeField]
    private float armRightWeightPosition = 1.0f;

    [Range(0.0f, 1.0f)]
    [Tooltip("���� �ȿ� ���� ����� ����ġ�Դϴ�. ȸ��")]
    [SerializeField]
    private float armRightWeightRotation = 1.0f;

    [Tooltip("������ �� ����, Root Mid tip")]
    [SerializeField]
    private Transform[] armRightHierarchy;

    [Title(label: "Generic")]
    [Tooltip("Hint")]
    [SerializeField]
    private Transform hint;

    [Range(0.0f, 1.0f)]
    [Tooltip("Hint ����ġ")]
    [SerializeField]
    private float weighthint;

    #endregion

    #region FIELDS
    /// <summary>
    /// ��ǥ ��ġ �������� �����ϼ���
    /// </summary>
    private bool maintainTargetPositionOffset;

    /// <summary>
    /// ��ǥ ȸ�� �������� �����ϼ���
    /// </summary>
    private bool maintainTargetRoationOffset;

    private float alphaLeft;

    private float alphaRight;

    #endregion

    #region CONSTANTS

    private const float kSqrEpsion = 1e-8f;

    #endregion

    #region UNITY

    private void Update()
    {
        //�������� IK���� �����ɴϴ�
        alphaLeft = characterAnimator.GetFloat(AHashes.AlphaIKHandLeft);

        //�������� IK���� �����ɴϴ�.
        alphaRight = characterAnimator.GetFloat(AHashes.AlphaIKHandRight);
    }

    private void LateUpdate()
    {
        if(characterAnimator == null)
        {
            Debug.LogError($"{this.gameObject}�� characterAnimator�� {characterAnimator}�Դϴ�");

            return;
        }

        Compute(alphaLeft, alphaRight);
    }

    #endregion

    #region METHODS

    /// <summary>
    /// �����ȿ� ���� ������� ����մϴ�.
    /// </summary>
    private void Compute(float weightLeft = 1.0f,float weightRight = 1.0f )
    {
        ComputeOnce(armLeftHierarchy, armLeftTarget, armLeftWeightPosition * weightLeft, armLeftWeightRoation * weightLeft);

        ComputeOnce(armRightHierarchy, armRightTarget, armRightWeightPosition * weightRight, armRightWeightRotation * weightRight);

    }

    /// <summary>
    /// �� �� �Ǵ� ������ ���� ������� ����մϴ�.
    /// </summary>
    /// <param name="hierarchy">�� ����. Root, Mid, Tip.</param>
    /// <param name="target">IK Ÿ��.</param>
    /// <param name="weightPosition">��ġ ����ġ.</param>
    /// <param name="weightRotation">ȸ�� ����ġ.</param>
    private void ComputeOnce(IReadOnlyList<Transform>hierarchy,Transform target, float weightPosition =1.0f, float weightRotation = 1.0f)
    {
        Vector3 targetOffsetPosition = Vector3.zero;
        Quaternion targetOffsetRotation = Quaternion.identity;

        if (maintainTargetPositionOffset)
            targetOffsetPosition = hierarchy[2].position - target.position;
        if (maintainTargetRoationOffset)
            targetOffsetRotation = Quaternion.Inverse(target.rotation) * hierarchy[2].rotation;

        Vector3 aPosition = hierarchy[0].position;
        Vector3 bPosition = hierarchy[1].position;
        Vector3 cPosition = hierarchy[2].position;
        Vector3 targetPos = target.position;
        Quaternion targetRot = target.rotation;
        Vector3 tPosition = Vector3.Lerp(cPosition, targetPos + targetOffsetPosition, weightPosition);
        Quaternion tRotation = Quaternion.Lerp(hierarchy[2].rotation, targetRot * targetOffsetRotation, weightRotation);
        bool hasHint = hint != null && weighthint > 0f;
        //���⺤��
        Vector3 ab = bPosition - aPosition;
        Vector3 bc = cPosition - bPosition;
        Vector3 ac = cPosition - aPosition;
        Vector3 at = tPosition - aPosition;

        //���⺤�� ����
        float abLen = ab.magnitude;
        float bcLen = bc.magnitude;
        float acLen = ac.magnitude;
        float atLen = at.magnitude;

        float oldAbcAngle = TrianlgeAngle(acLen, abLen, bcLen);
        float newAbcAngle = TrianlgeAngle(atLen, abLen, bcLen);

        Vector3 axis = Vector3.Cross(ab, bc);
        if(axis.sqrMagnitude < kSqrEpsion)
        {
            axis = hasHint ? Vector3.Cross(hint.position - aPosition, bc) : Vector3.zero;

            if (axis.sqrMagnitude < kSqrEpsion)
                axis = Vector3.Cross(at, bc);

            if (axis.sqrMagnitude < kSqrEpsion)
                axis = Vector3.up;
        }
        axis = Vector3.Normalize(axis);

        float a = 0.5f * (oldAbcAngle - newAbcAngle);
        float sin = Mathf.Sin(a);
        float cos = Mathf.Cos(a);
        Quaternion deltaR = new Quaternion(axis.x * sin, axis.y * sin, axis.z * sin, cos);
        hierarchy[1].rotation = deltaR * hierarchy[1].rotation;

        cPosition = hierarchy[2].position;
        ac = cPosition - aPosition;
        hierarchy[0].rotation = Quaternion.FromToRotation(ac, at) * hierarchy[0].rotation;

        if (hasHint)
        {
            float acSqrMag = ac.sqrMagnitude;
            if (acSqrMag > 0f)
            {
                bPosition = hierarchy[1].position;
                cPosition = hierarchy[2].position;
                ab = bPosition - aPosition;
                ac = cPosition - aPosition;

                Vector3 acNorm = ac / Mathf.Sqrt(acSqrMag);
                Vector3 ah = hint.position - aPosition;
                Vector3 abProj = ab - acNorm * Vector3.Dot(ab, acNorm);
                Vector3 ahProj = ah - acNorm * Vector3.Dot(ah, acNorm);

                float maxReach = abLen + bcLen;
                if (abProj.sqrMagnitude > (maxReach * maxReach * 0.001f) && ahProj.sqrMagnitude > 0f)
                {
                    Quaternion hintR = Quaternion.FromToRotation(abProj, ahProj);
                    hintR.x *= weighthint;
                    hintR.y *= weighthint;
                    hintR.z *= weighthint;
                    hintR = Quaternion.Normalize(hintR);
                    hierarchy[0].rotation = hintR * hierarchy[0].rotation;
                }

            }
        }

        hierarchy[2].rotation = tRotation;
    }

    private static float TrianlgeAngle(float aLen, float aLen1, float aLen2)
    {
        float c = Mathf.Clamp((aLen1 * aLen1 + aLen2 * aLen2 - aLen * aLen) / (aLen1 * aLen2) / 2.0f, -1.0f, 1.0f);
        return Mathf.Acos(c);
    }
    #endregion
}
