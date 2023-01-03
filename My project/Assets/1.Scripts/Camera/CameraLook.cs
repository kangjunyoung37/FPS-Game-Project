using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 카메라 회전을 처리합니다.
/// </summary>
public class CameraLook : MonoBehaviourPunCallbacks
{
    
    #region FIELDS SERIALIZED
    [Title(label: "Settinds")]

    [Tooltip("마우스 감도")]
    [SerializeField]
    private Vector2 sensitivity = new Vector2(1, 1);

    [Tooltip("카메라가 가질 수 있는 최소 및 최대 위쪽/아래쪽 회전 각도.")]
    [SerializeField]
    private Vector2 yClamp = new Vector2(-60, 60);

    [Title(label: "Interpolation")]

    [Tooltip("모양 회전을 보간해야하는지")]
    [SerializeField]
    private bool smooth;

    [Tooltip("회전이 보간되는 속도")]
    [SerializeField]
    private float interpolationSpeed = 25.0f;

    [Tooltip("회전될 오브젝트")]
    [SerializeField]
    private Transform lotateTransform;

    [SerializeField]
    private float lerpTime = 0.5f;

    [SerializeField]
    private float currentTime = 0;

    [SerializeField]
    private float defaultFOV = 100.0f;

    #endregion

    #region FIELDS

    /// <summary>
    /// 플레이어의 CharacterBehaviour
    /// </summary>
    private CharacterBehaviour playerCharacter;

    /// <summary>
    /// 플레이어 캐릭터 회전
    /// </summary>
    private Quaternion rotationCharacter;

    /// <summary>
    /// 플레이어 카메라 회전
    /// </summary>
    private Quaternion rotationCamera;

    /// <summary>
    /// PhotonView
    /// </summary>
    private PhotonView PV;

    /// <summary>
    /// 입력 받기
    /// </summary>
    private Vector2 frameInput;

    private Quaternion YRotation;
    private bool IsDead = false;
    private bool moveDeathCam = false;
    private Transform cameraTransform;
    private Transform targetTransform;
    private Vector3 deadCamPosition = new Vector3(0.0f,-0.5f,-1.5f);
    private Vector3 startPosition;
    private Quaternion onlyCamRotation;
    
    #endregion

    private void Awake()
    {
        playerCharacter = GetComponent<CharacterBehaviour>();
        cameraTransform = playerCharacter.GetCameraWold().transform;
        PV = GetComponent<PhotonView>();
        playerCharacter.OnCharacterDie += CharacterDie;
        startPosition = cameraTransform.localPosition;
    }
    #region UNITY
    private void Start()
    {

        //playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        rotationCharacter = playerCharacter.transform.localRotation;
        
        //카메라 초기 로케이션
        rotationCamera = lotateTransform.transform.localRotation;

        onlyCamRotation = cameraTransform.localRotation;

    }
    
    private void CharacterDie()
    {
        playerCharacter.GetCameraWold().fieldOfView = defaultFOV;
        targetTransform = playerCharacter.GetEnemyCharacterBehaviour().transform;
        IsDead = true;

    }

    private void LateUpdate()
    {
        //프레임 입력      
        frameInput = playerCharacter.isCursorLocked() ? playerCharacter.GetInputLook() : default;

        //감도
        frameInput *= sensitivity;
        Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);
        Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);
        
        if (IsDead)
        {   
            if(!moveDeathCam && targetTransform != null)
                StartCoroutine(nameof(MoveDeathCam));
            return;
        }
            
        if (!PV.IsMine)
        {

            Quaternion localRotation = lotateTransform.transform.localRotation;
            localRotation = Quaternion.Slerp(localRotation, YRotation, Time.deltaTime * 12.5f);
            lotateTransform.transform.localRotation = localRotation;
            return;

        }

        CameraLotation(rotationPitch, rotationYaw);
    }

    #endregion

    #region FUNCTION

    IEnumerator MoveDeathCam()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= lerpTime)
            currentTime = lerpTime;
        float t = currentTime / lerpTime;

        t = t * t * t * (t * (6f * t - 15f) + 10f);

        cameraTransform.localPosition = Vector3.Lerp(startPosition, deadCamPosition, t);
        Quaternion rotTarget = Quaternion.LookRotation(targetTransform.position - cameraTransform.position);
        cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, rotTarget, interpolationSpeed);

        yield return new WaitForSeconds(2.0f);

        moveDeathCam = true;

    }

    private Quaternion Clamp(Quaternion rotation,Vector2 yClamp)
    {
        rotation.x /= rotation.w;
        rotation.y /= rotation.w;
        rotation.z /= rotation.w;
        rotation.w = 1.0f;

        float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

        pitch = Mathf.Clamp(pitch, yClamp.x, yClamp.y);
        rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

        return rotation;

    }

    #endregion

    /// <summary>
    /// 플레이어가 죽고 난 뒤에 카메라만 움직이기.
    /// </summary>
    /// <param name="rotationPitch"></param>
    /// <param name="rotationYaw"></param>
    private void OnlyCamRotate(Quaternion rotationPitch,Quaternion rotationYaw)
    {
        

        onlyCamRotation *= rotationPitch;
        onlyCamRotation = Clamp(onlyCamRotation, yClamp);
        //onlyCamRotation *= rotationYaw;
        Quaternion localRotation = cameraTransform.transform.localRotation;
        localRotation = Quaternion.Slerp(localRotation, onlyCamRotation, Time.deltaTime * interpolationSpeed);
        localRotation = Clamp(localRotation,yClamp);
        cameraTransform.localRotation = localRotation;
        cameraTransform.parent.rotation *= rotationYaw;
        
    }

    private void CameraLotation(Quaternion rotationPitch, Quaternion rotationYaw)
    {

        rotationCamera *= rotationPitch;
        rotationCamera = Clamp(rotationCamera, yClamp);
        rotationCharacter *= rotationYaw;

        Quaternion localRotation = lotateTransform.transform.localRotation;
        //Quaternion localRotation = lotateTransform.localRotation;
        if (smooth)
        {
            //local rotation 보간
            localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime * interpolationSpeed);
            //Clamp
            localRotation = Clamp(localRotation, yClamp);
            //character rotation 보간
            playerCharacter.transform.rotation = Quaternion.Slerp(playerCharacter.transform.rotation, rotationCharacter, Time.deltaTime * interpolationSpeed);

        }
        else
        {
            localRotation *= rotationPitch;
            localRotation = Clamp(localRotation, yClamp);
            playerCharacter.transform.rotation *= rotationYaw;
        }

        lotateTransform.transform.localRotation = localRotation;
        //lotateTransform.localRotation = localRotation;

    }

    public void OnPhotonSerializeView(PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rotationCamera);
        }
        else
        {
            YRotation = (Quaternion)stream.ReceiveNext();

        }
    }


}
