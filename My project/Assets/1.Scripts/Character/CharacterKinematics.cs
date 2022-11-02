using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

/// <summary>
/// 캐릭터에 필요한 모든 역운동학을 처리합니다.
/// 유니티의 IK코드를 사용합니다.
/// </summary>
public class CharacterKinematics : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("캐릭터 애니메이터의 컴포넌트")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Title(label: "Settings Arm Left")]

    [Tooltip("왼팔 IK대상이 무엇인지 결정합니다")]
    [SerializeField]
    private Transform armLeftTarget;

    [Range(0.0f, 1.0f)]
    [Tooltip("왼쪽 팔에 대한 역운동학 가중치입니다. 위치")]
    [SerializeField]
    private float armLeftWeightPosition = 1.0f;

    [Range(0.0f, 1.0f)]
    [Tooltip("왼쪽 팔에 대한 역운동학 가중치입니다. 회전")]
    [SerializeField]
    private float armLeftWeightRoation = 1.0f;

    [Tooltip("왼쪽 팔의 계층, 루트 미드 팁")]
    [SerializeField]
    private Transform[] armLeftHierarchy;

    [Title(label: "Settings Arm Right")]

    [Title(label: "Settings Arm Right")]
    [Tooltip("오른팔 IK대상이 무엇인지 결정합니다")]
    [SerializeField]
    private Transform armRightTarget;

    [Range(0.0f, 1.0f)]
    [Tooltip("오른 팔에 대한 역운동학 가중치입니다. 위치")]
    [SerializeField]
    private float armRightWeightPosition = 1.0f;

    [Range(0.0f, 1.0f)]
    [Tooltip("오른 팔에 대한 역운동학 가중치입니다. 회전")]
    [SerializeField]
    private float armRightWeightRotation = 1.0f;

    [Tooltip("오른쪽 팔 계층, Root Mid tip")]
    [SerializeField]
    private Transform[] armRightHierarchy;

    [Title(label: "Generic")]
    [Tooltip("Hint")]
    [SerializeField]
    private Transform hint;

    [Range(0.0f, 1.0f)]
    [Tooltip("Hint 가중치")]
    [SerializeField]
    private float weighthint;

    #endregion

    #region FIELDS
    /// <summary>
    /// 목표 위치 오프셋을 유지하세여
    /// </summary>
    private bool maintainTargetPositionOffset;

    /// <summary>
    /// 목표 회전 오프셋을 유지하세여
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
        //왼쪽팔의 IK값을 가져옵니다
        alphaLeft = characterAnimator.GetFloat(AHashes.AlphaIKHandLeft);

        //오른팔의 IK값을 가져옵니다.
        alphaRight = characterAnimator.GetFloat(AHashes.AlphaIKHandRight);
    }

    private void LateUpdate()
    {
        if(characterAnimator == null)
        {
            Debug.LogError($"{this.gameObject}에 characterAnimator가 {characterAnimator}입니다");

            return;
        }

        Compute(alphaLeft, alphaRight);
    }

    #endregion

    #region METHODS

    /// <summary>
    /// 양쪽팔에 대한 역운동학을 계산합니다.
    /// </summary>
    private void Compute(float weightLeft = 1.0f,float weightRight = 1.0f )
    {
        ComputeOnce(armLeftHierarchy, armLeftTarget, armLeftWeightPosition * weightLeft, armLeftWeightRoation * weightLeft);

        ComputeOnce(armRightHierarchy, armRightTarget, armRightWeightPosition * weightRight, armRightWeightRotation * weightRight);

    }

    /// <summary>
    /// 한 팔 또는 계층에 대한 역운동학을 계산합니다.
    /// </summary>
    /// <param name="hierarchy">팔 계층. Root, Mid, Tip.</param>
    /// <param name="target">IK 타켓.</param>
    /// <param name="weightPosition">위치 가중치.</param>
    /// <param name="weightRotation">회전 가중치.</param>
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
        //방향벡터
        Vector3 ab = bPosition - aPosition;
        Vector3 bc = cPosition - bPosition;
        Vector3 ac = cPosition - aPosition;
        Vector3 at = tPosition - aPosition;

        //방향벡터 길이
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
