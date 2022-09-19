using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player; //player transform
    public Vector3 pivotOffset = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 camOffset = new Vector3(0.4f, 0.5f, -2.0f);//3��Ī�϶� ī�޶��� ��ġ

    public float smooth = 10f;//ī�޶� �����ӵ�
    public float horizontalAimingSpeed = 6.0f;//���� ȸ�� �ӵ�;
    public float verticalAimingSpeed = 6.0f;//���� ȸ�� �ӵ�;
    public float maxVerticalAngle = 30.0f;//ī�޶� ���� �ִ� ����
    public float minVerticalAngle = -60.0f;//�ּ� ����

    public float recoilAngleBounce = 5.0f;//��� �ݵ� ��
    private float angleH = 0.0f;//���콺 �̵��� ���� ī�޶� �����̵� ��ġ
    private float angleV = 0.0f;//���콺 �̵��� ���� ī�޶� ���� �̵� ��ġ
    private Transform cameraTransform;//ī�޶��� Ʈ������
    private Camera myCamera;
    private Vector3 relCameraPos; //�÷��̾�κ��� ī�޶������ ����
    private float relCameraPosMag; //�÷��̾�κ��� ī�޶� ������ �Ÿ�
    private Vector3 smoothPivotOffset;//ī�޶� �Ǻ��� ������ ����
    private Vector3 smoothCamOffset; //ī�޶� ��ġ�� ������ ����
    private Vector3 targetPivotOffset; //ī�޶� �Ǻ��� ������ ����
    private Vector3 targetCamOffset; // ī�޶� ��ġ�� ������ ����

    private float defaultFOV; //�⺻ �þ߰�
    private float targetFOV; //Ÿ�� �þ߰�
    private float targetMaxVerticalAngle;//ī�޶� ���� �ִ� ����
    private float recoilAngle = 0f;//��� �ݵ� ����

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
        //������ �ʱ�ȭ�ϱ�
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
        //���콺 �̵� ��
        angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f) * horizontalAimingSpeed;
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1f, 1f) * verticalAimingSpeed;
        //���� �̵� ����
        angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);
        //���� �ٿ ��
        angleV = Mathf.LerpAngle(angleV, angleV + recoilAngle, 10f * Time.deltaTime);
        //ī�޶� ȸ��
        Quaternion camYRotation = Quaternion.Euler(0.0f, angleH, 0.0f);
        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0.0f);//���̹��ϴ� �����̼�
        cameraTransform.rotation = aimRotation;
        //set FOV
        myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, targetFOV, Time.deltaTime);

        Vector3 baseTempPosition = player.position + camYRotation * targetCamOffset;
        Vector3 noCollisionOffset = targetCamOffset; //������ �� �����°� 
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
