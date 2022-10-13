using UnityEngine;
/// <summary>
/// ī�޶� ȸ���� ó���մϴ�.
/// </summary>
public class CameraLook : MonoBehaviour
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
    private bool smooth;

    [Tooltip("ȸ���� �����Ǵ� �ӵ�")]
    [SerializeField]
    private float interpolationSpeed = 25.0f;
    #endregion

    #region FIELDS
    private CharacterBehaviour playerCharacter;

    private Rigidbody playerCharacterRigidbody;

    /// <summary>
    /// �÷��̾� ĳ���� ȸ��
    /// </summary>
    private Quaternion rotationCharacter;
    /// <summary>
    /// �÷��̾� ī�޶� ȸ��
    /// </summary>
    private Quaternion rotationCamera;

    #endregion

    #region UNITY
    private void Start()
    {
        playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();

        rotationCharacter = playerCharacter.transform.localRotation;

        //ī�޶� �ʱ� �����̼�
        rotationCamera = transform.localRotation;

        playerCharacterRigidbody = playerCharacter.GetComponent<Rigidbody>();
    }

   
    private void LateUpdate()
    {
        //������ �Է� 
        Vector2 frameInput = playerCharacter.isCursorLocked() ? playerCharacter.GetInputLook() : default;
        //����
        frameInput *= sensitivity;

        Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);

        Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);

        rotationCamera *= rotationPitch;
        rotationCamera = Clamp(rotationCamera);
        rotationCharacter *= rotationYaw;

        Quaternion localRotation = transform.localRotation;

        if(smooth)
        {
            //local rotation ����
            localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime);
            //Clamp
            localRotation = Clamp(localRotation);
            //character rotation ����
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
