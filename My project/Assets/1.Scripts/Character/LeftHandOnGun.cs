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
        //왼쪽 손이 오른쪽 손이랑 얼마나 차이나는지 확인
        Vector3 toLeftHand = leftHand.bone.position - rightHand.bone.position;
        Vector3 toleftHandRelative = rightHand.bone.InverseTransformDirection(toLeftHand);

        //오른 손 회전에 대한 왼손의 회전
        leftHandRotationRelative = Quaternion.Inverse(rightHand.bone.rotation) * leftHand.bone.rotation;

        //머리 회전하기
        aim.solver.IKPosition = look.solver.IKPosition;

        aim.solver.Update();

        //왼쪽 손위치 변경
        leftHand.position = rightHand.bone.position + rightHand.bone.TransformDirection(toleftHandRelative);
        leftHand.positionWeight = 1f;

        //해결하는 동안 오른 손이 움직이지 않도록 고정
        rightHand.position = rightHand.bone.position;
        rightHand.positionWeight = 1f;
        ik.solver.GetLimbMapping(FullBodyBipedChain.RightArm).maintainRotationWeight = 1f;

        ik.solver.Update();

        look.solver.Update();
    }
    //FBBIK를 끝낸 후에 왼손을 회전시키기
    private void OnPostFBBIK()
    {
        leftHand.bone.rotation = rightHand.bone.rotation * leftHandRotationRelative;
    }

}
