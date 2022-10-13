using UnityEngine;
/// <summary>
/// 카메라 회전을 처리합니다.
/// </summary>
public class CameraLook : MonoBehaviour
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
    private bool smooth;

    [Tooltip("회전이 보간되는 속도")]
    [SerializeField]
    private float interpolationSpeed = 25.0f;
    #endregion

    #region FIELDS
    private CharacterBehaviour playerCharacter;

    private Rigidbody playerCharacterRigidbody;

    /// <summary>
    /// 플레이어 캐릭터 회전
    /// </summary>
    private Quaternion rotationCharacter;
    /// <summary>
    /// 플레이어 카메라 회전
    /// </summary>
    private Quaternion rotationCamera;

    #endregion

    #region UNITY
    private void Start()
    {
        playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();

        rotationCharacter = playerCharacter.transform.localRotation;

        //카메라 초기 로케이션
        rotationCamera = transform.localRotation;

        playerCharacterRigidbody = playerCharacter.GetComponent<Rigidbody>();
    }

   
    private void LateUpdate()
    {
        //프레임 입력 
        Vector2 frameInput = playerCharacter.isCursorLocked() ? playerCharacter.GetInputLook() : default;
        //감도
        frameInput *= sensitivity;

        Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);

        Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);

        rotationCamera *= rotationPitch;
        rotationCamera = Clamp(rotationCamera);
        rotationCharacter *= rotationYaw;

        Quaternion localRotation = transform.localRotation;

        if(smooth)
        {
            //local rotation 보간
            localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime);
            //Clamp
            localRotation = Clamp(localRotation);
            //character rotation 보간
            playerCharacter.transform.rotation = Quaternion.Slerp(playerCharacter.transform.rotation, rotationCharacter, Time.deltaTime * interpolationSpeed);


        }
        else
        {
            localRotation *= rotationPitch;
            localRotation = Clamp(localRotation);
            playerCharacter.transform.rotation *= rotationYaw;
        }
        transform.localRotation = localRotation;
    }
    #endregion
    #region FUNCTION
    private Quaternion Clamp(Quaternion rotation)
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
}
