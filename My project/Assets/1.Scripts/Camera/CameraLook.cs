using Photon.Pun;
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

    #endregion

    #region FIELDS

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

    #endregion

    private void Awake()
    {
        playerCharacter = GetComponent<CharacterBehaviour>();
        PV = GetComponent<PhotonView>();

    }
    #region UNITY
    private void Start()
    {

        //playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        rotationCharacter = playerCharacter.transform.localRotation;
        
        //카메라 초기 로케이션
        rotationCamera = lotateTransform.transform.localRotation;



    }


    private void LateUpdate()
    {
        if (!PV.IsMine)
        {

            Quaternion localRotation = lotateTransform.transform.localRotation;
            localRotation = Quaternion.Slerp(localRotation, YRotation, Time.deltaTime * 12.5f);
            lotateTransform.transform.localRotation = localRotation;
            return;

        }
        //프레임 입력 

        frameInput = playerCharacter.isCursorLocked() ? playerCharacter.GetInputLook() : default;
        //감도
        frameInput *= sensitivity;
        Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);
        Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);

        CameraLotation(rotationPitch, rotationYaw);

    }

    #endregion

    #region FUNCTION

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
