using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player; //player transform
    public Vector3 pivotOffset = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 camOffset = new Vector3(0.4f, 0.5f, -2.0f);//3인칭일때 카메라의 위치

    public float smooth = 10f;//카메라 반응속도
    public float horizontalAimingSpeed = 6.0f;//수평 회전 속도;
    public float verticalAimingSpeed = 6.0f;//수직 회전 속도;
    public float maxVerticalAngle = 30.0f;//카메라 수직 최대 각도
    public float minVerticalAngle = -60.0f;//최소 각도

    public float recoilAngleBounce = 5.0f;//사격 반동 값
    private float angleH = 0.0f;//마우스 이동에 따른 카메라 수평이동 수치
    private float angleV = 0.0f;//마우스 이동에 따른 카메라 수직 이동 수치
    private Transform cameraTransform;//카메라의 트랜스폼
    private Camera myCamera;
    private Vector3 relCameraPos; //플레이어로부터 카메라까지의 벡터
    private float relCameraPosMag; //플레이어로부터 카메라 사이의 거리
    private Vector3 smoothPivotOffset;//카메라 피봇영 보간용 벡터
    private Vector3 smoothCamOffset; //카메라 위치용 보간용 벡터
    private Vector3 targetPivotOffset; //카메라 피봇용 보간용 벡터
    private Vector3 targetCamOffset; // 카메라 위치용 보간용 벡터

    private float defaultFOV; //기본 시야값
    private float targetFOV; //타겟 시야값
    private float targetMaxVerticalAngle;//카메라 수직 최대 각도
    private float recoilAngle = 0f;//사격 반동 각도

    public float GetH
    {
        get
        {
            return angleH;
        }
    }

    private void Awake()
    {
        cameraTransform = transform;
        myCamera = cameraTransform.GetComponent<Camera>();

        cameraTransform.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
        cameraTransform.rotation = Quaternion.identity;

        relCameraPos = cameraTransform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;

        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;
        defaultFOV = myCamera.fieldOfView;
        angleH = player.eulerAngles.y;
        ResetTargetOffset();
        ResetFOV();
        ResetMaxVerticalAngle();
    }
    public void ResetTargetOffset()
    {
        //오프셋 초기화하기
        targetPivotOffset = pivotOffset;
        targetCamOffset = camOffset;
    }
    public void ResetFOV()
    {
        this.targetFOV = defaultFOV;
    }
    public void ResetMaxVerticalAngle()
    {
        targetMaxVerticalAngle = maxVerticalAngle;
    }
    public void BounceVertical(float degree)
    {
        recoilAngle = degree;
    }
    public void SetTargetOffset(Vector3 newPivotOffset, Vector3 newCamOffset)
    {
        targetPivotOffset = newPivotOffset;
        targetCamOffset = newCamOffset;

    }
    public void SetFOV(float customFOV)
    {
        this.targetFOV = customFOV;
    }
    bool ViewingPosCheck(Vector3 checkPos,float detlaPlayerHeight)
    {
        Vector3 target = player.position + (Vector3.up * detlaPlayerHeight);
        if(Physics.SphereCast(checkPos,0.2f,target-checkPos,out RaycastHit hit,relCameraPosMag))
        {
            if(hit.transform != player && hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }
    bool ReverseViewingPosCheck(Vector3 checkPos,float delataPlayerHeight,float maxDistance)
    {
        Vector3 origin = player.position + (Vector3.up * delataPlayerHeight);
        if(Physics.SphereCast(origin,0.2f,checkPos- origin,out RaycastHit hit, maxDistance))
        {
            if(hit.transform != player && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }
    bool DoubleViewingPosCheck(Vector3 checkPos, float offset)
    {
        float playerFocusHeight = player.GetComponent<CapsuleCollider>().height * 0.75f;
        return ViewingPosCheck(checkPos, playerFocusHeight) && ReverseViewingPosCheck(checkPos, playerFocusHeight, offset);
    }
    private void Update()
    {
        //마우스 이동 값
        angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f) * horizontalAimingSpeed;
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1f, 1f) * verticalAimingSpeed;
        //수직 이동 제한
        angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);
        //수직 바운스 값
        angleV = Mathf.LerpAngle(angleV, angleV + recoilAngle, 10f * Time.deltaTime);
        //카메라 회전
        Quaternion camYRotation = Quaternion.Euler(0.0f, angleH, 0.0f);
        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0.0f);//에이밍하는 로테이션
        cameraTransform.rotation = aimRotation;
        //set FOV
        myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, targetFOV, Time.deltaTime);

        Vector3 baseTempPosition = player.position + camYRotation * targetCamOffset;
        Vector3 noCollisionOffset = targetCamOffset; //조준할 때 오프셋값 
        for (float zOffset = targetCamOffset.z; zOffset <= 0f; zOffset += 0.5f)
        {
            noCollisionOffset.z = zOffset;
            if (DoubleViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset, Mathf.Abs(zOffset)) || zOffset == 0f)
            {
                break;
            }

        }
        //Repositon Camera
        smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
        smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smooth * Time.deltaTime);

        cameraTransform.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;
        if(recoilAngle > 0.0f)
        {
            recoilAngle -= recoilAngleBounce * Time.deltaTime;
        }
        else if(recoilAngle <0.0f)
        {
            recoilAngle += recoilAngleBounce * Time.deltaTime;
        }
    }
    public float GetCurrentPivotMagitude(Vector3 finalPivotOffset)
    {
        return Mathf.Abs((finalPivotOffset - smoothPivotOffset).magnitude);
    }
}
