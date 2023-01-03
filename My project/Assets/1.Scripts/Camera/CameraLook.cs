using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ī�޶� ȸ���� ó���մϴ�.
/// </summary>
public class CameraLook : MonoBehaviourPunCallbacks
{
    
    #region FIELDS SERIALIZED
    [Title(label: "Settinds")]

    [Tooltip("���콺 ����")]
    [SerializeField]
    private Vector2 sensitivity = new Vector2(1, 1);

    [Tooltip("ī�޶� ���� �� �ִ� �ּ� �� �ִ� ����/�Ʒ��� ȸ�� ����.")]
    [SerializeField]
    private Vector2 yClamp = new Vector2(-60, 60);

    [Title(label: "Interpolation")]

    [Tooltip("��� ȸ���� �����ؾ��ϴ���")]
    [SerializeField]
    private bool smooth;

    [Tooltip("ȸ���� �����Ǵ� �ӵ�")]
    [SerializeField]
    private float interpolationSpeed = 25.0f;

    [Tooltip("ȸ���� ������Ʈ")]
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
    /// �÷��̾��� CharacterBehaviour
    /// </summary>
    private CharacterBehaviour playerCharacter;

    /// <summary>
    /// �÷��̾� ĳ���� ȸ��
    /// </summary>
    private Quaternion rotationCharacter;

    /// <summary>
    /// �÷��̾� ī�޶� ȸ��
    /// </summary>
    private Quaternion rotationCamera;

    /// <summary>
    /// PhotonView
    /// </summary>
    private PhotonView PV;

    /// <summary>
    /// �Է� �ޱ�
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
        
        //ī�޶� �ʱ� �����̼�
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
        //������ �Է�      
        frameInput = playerCharacter.isCursorLocked() ? playerCharacter.GetInputLook() : default;

        //����
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
    /// �÷��̾ �װ� �� �ڿ� ī�޶� �����̱�.
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
            //local rotation ����
            localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime * interpolationSpeed);
            //Clamp
            localRotation = Clamp(localRotation, yClamp);
            //character rotation ����
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
