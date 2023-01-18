using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class LeftHandOnGun : MonoBehaviour
{
    private AimIK aim;
    private FullBodyBipedIK ik;
    private LookAtIK look;
    private CharacterBehaviour characterBehaviour;

    private IKEffector leftHand => ik.solver.leftHandEffector;
    private IKEffector rightHand => ik.solver.rightHandEffector;
    
    private Quaternion leftHandRotationRelative;

    private void Awake()
    {
        characterBehaviour = transform.root.GetComponent<CharacterBehaviour>();
        aim = GetComponent<AimIK>();
        ik = GetComponent<FullBodyBipedIK>();
        look = GetComponent<LookAtIK>();
    }


    private void Start()
    {
        aim.Disable();
        ik.Disable();
        look.Disable();

        ik.solver.OnPostUpdate += OnPostFBBIK;
    }
    private void LateUpdate()
    {
        //���� ���� ������ ���̶� �󸶳� ���̳����� Ȯ��
        Vector3 toLeftHand = leftHand.bone.position - rightHand.bone.position;
        Vector3 toleftHandRelative = rightHand.bone.InverseTransformDirection(toLeftHand);

        //���� �� ȸ���� ���� �޼��� ȸ��
        leftHandRotationRelative = Quaternion.Inverse(rightHand.bone.rotation) * leftHand.bone.rotation;

        //�Ӹ� ȸ���ϱ�
        aim.solver.IKPosition = look.solver.IKPosition;

        aim.solver.Update();

        //���� ����ġ ����
        leftHand.position = rightHand.bone.position + rightHand.bone.TransformDirection(toleftHandRelative);
        leftHand.positionWeight = 1f;

        //�ذ��ϴ� ���� ���� ���� �������� �ʵ��� ����
        rightHand.position = rightHand.bone.position;
        rightHand.positionWeight = 1f;
        ik.solver.GetLimbMapping(FullBodyBipedChain.RightArm).maintainRotationWeight = 1f;

        ik.solver.Update();

        look.solver.Update();
    }
    //FBBIK�� ���� �Ŀ� �޼��� ȸ����Ű��
    private void OnPostFBBIK()
    {
        leftHand.bone.rotation = rightHand.bone.rotation * leftHandRotationRelative;
    }

}
