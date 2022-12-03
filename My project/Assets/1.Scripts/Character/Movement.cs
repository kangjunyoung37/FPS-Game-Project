using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

/// <summary>
/// ĳ������ �������� ó���ϴ� �⺻ �������
/// �޸���, ��ũ����, ���� 
/// </summary>
public class Movement : MovementBehaviour
{

    
    #region FIELDS SERIALIZED

    [Title(label:"FIELD")]

    [Tooltip("TPĳ������ �ִϸ����� ��Ʈ�ѷ�")]
    [SerializeField , NotNull]
    private Animator TPAnimatorController;

    [Title(label: "Acceleration")]

    [Tooltip("ĳ������ �ӵ��� �󸶳� ������ �����ϴ���")]
    [SerializeField]
    private float acceleration = 9.0f;

    [Tooltip("���߿� ���� �� ���Ǵ� ���ӵ� ���Դϴ�.")]
    [SerializeField]
    private float accelerationInAir = 3.0f;

    [Tooltip("ĳ������ �ӵ��� �󸶳� ������ �����ϴ���")]
    [SerializeField]
    private float deceleration = 11.0f;

    [Title(label: "Speeds")]

    [Tooltip("ĳ���Ͱ� �ȴ� ������ �ӵ�")]
    [SerializeField]
    private float speedWalking = 4.0f;

    [Tooltip("�����߿� ĳ���Ͱ� �󸶳� ������ �����̴���")]
    [SerializeField]
    private float speedAiming = 3.2f;

    [Tooltip("��ũ���� ���� �� ĳ���Ͱ� �󸶳� ������ �����̴���")]
    [SerializeField]
    private float speedCrouching = 3.5f;

    [Tooltip("�޸��� ���� ĳ���Ͱ� �󸶳� ������ �����̴���")]
    [SerializeField]
    private float speedRunning = 6.8f;

    [Title(label: "Walking Multipliers")]

    [Tooltip("ĳ���Ͱ� ������ �̵��� �� �ȴ� �ӵ��� �������Դϴ�.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float walkingMultiplierForward = 1.0f;

    [Tooltip("ĳ���Ͱ� �翷���� �̵��� �� �ȴ� �ӵ��� �������Դϴ�")]
    [Range(0f, 1f)]
    [SerializeField]
    private float walkingMultiplierSideways = 1.0f;

    [Tooltip("ĳ���Ͱ� �ڷ� �̵��� �� �ȴ� �ӵ��� �������Դϴ�")]
    [Range(0f, 1f)]
    [SerializeField]
    private float walkingMultiplierBackwards = 1.0f;

    [Title(label: "Air")]

    [Tooltip("ĳ���Ͱ� ���߿� �ִ� ���� �÷��̾ ���� ������ �󸶳� ������ �� �ִ���")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float airControl = 0.8f;

    [Tooltip("ĳ������ �߷°�, ĳ���Ͱ� �󸶳� ������ ����������")]
    [SerializeField]
    float gravity = 1.1f;

    [Tooltip("ĳ������ �߷°� ĳ������ �������� �ӵ�")]
    [SerializeField]
    private float jumpGravity = 1.0f;

    [Tooltip("������ �� ��")]
    [SerializeField]
    private float jumpForce = 100.0f;

    [Tooltip("ĳ���Ͱ� �������濡�� ���ư��� �ʵ��� �ϱ� ���� ����Ǵ� ���Դϴ�.")]
    [SerializeField]
    private float stickToGroundForce = 0.03f;

    [Title(label: "Crouching")]

    [Tooltip("false��� ĳ���Ͱ� ��ũ���� ���� �����մϴ�")]
    [SerializeField]
    private bool canCrouch = true;

    [Tooltip("true�̸� �������� ���� ĳ���Ͱ� ��ũ�� �� �����ϴ�")]
    [SerializeField, ShowIf(nameof(canCrouch), true)]
    private bool canCrouchWhileFalling = false;

    [Tooltip("true�̸� ĳ���Ͱ� ��ũ���� �ִ� ���� jump�� �� �� �ֽ��ϴ�")]
    [SerializeField, ShowIf(nameof(canCrouch), true)]
    private bool canJumpWhileCrouching = true;

    [Tooltip("��ũ����  �ִµ��� ĳ������ ����")]
    [SerializeField, ShowIf(nameof(canCrouch), true)]
    private float crouchHeight = 1.0f;

    [Tooltip("���� ���θ����� �� �� ��ĥ �� �ִ� ������ ���̾��� ����ũ")]
    [SerializeField, ShowIf(nameof(canCrouch), true)]
    private LayerMask crouchOverlapsMask;

    [Title(label: "Rigidbody Push")]

    [Tooltip("�ٸ� ������ٵ� �� �� ����Ǵ� ���Դϴ�.")]
    [SerializeField]
    private float rigidbodyPushForce = 1.0f;

   

    #endregion

    #region FIELDS

    /// <summary>
    /// ĳ���� ��Ʈ�ѷ�
    /// </summary>
    private CharacterController controller;
    /// <summary>
    /// ĳ���� Behaviour
    /// </summary>
    private CharacterBehaviour playerCharacter;
    /// <summary>
    /// ������ ����
    /// </summary>
    private WeaponBehaviour equippedWeapon;
    /// <summary>
    /// ĳ������ �⺻ ����
    /// </summary>
    private float standingHeight;
    /// <summary>
    /// �ӵ�
    /// </summary>
    private Vector3 velocity;
    /// <summary>
    /// ĳ���Ͱ� ���� �ִ���
    /// </summary>
    private bool isGrounded;
    /// <summary>
    /// ������ �����ӿ� ĳ���Ͱ� ���� �־�����
    /// </summary>
    private bool wasGrounded;
    /// <summary>
    /// �����ϰ� �ִ� ������
    /// </summary>
    private bool jumping;
    /// <summary>
    /// ĳ���Ͱ� ��ũ���� �ִ���
    /// </summary>
    private bool crouching;
    /// <summary>
    /// ĳ���Ͱ� ���������� �����ߴ� �ð�
    /// </summary>
    private float lastJumpTime;

    private PhotonView PV;

    #endregion

    #region UNITY FUNCTIONS

    protected override void Awake()
    {

        controller = GetComponent<CharacterController>();
        //playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        playerCharacter = transform.GetComponent<CharacterBehaviour>();
        PV = transform.GetComponent<PhotonView>();
    }
   
    //���۽� FPS ��Ʈ�ѷ��� �ʱ�ȭ�մϴ�.
    protected override void Start()
    {

        standingHeight = controller.height;
    }

    protected override void Update()
    {
        if (!PV.IsMine)
            return;
        //�κ��丮���� ���⸦ ������
        equippedWeapon = playerCharacter.GetInventory().GetEquipped();
        //�� �����ӿ� ���� �־�����
        isGrounded = IsGrounded();
        
        if(isGrounded && !wasGrounded)
        {
            jumping = false;
            lastJumpTime = 0.0f;
        }
        else if(wasGrounded && !isGrounded)
            lastJumpTime = Time.time;

        MoveCharacter();

        wasGrounded = isGrounded;

        //TPAnimatorController.SetBool("Jumping", jumping);
        
    }

    [PunRPC]
    public override void PVJumping()
    {
        TPAnimatorController.SetBool("Jumping", jumping);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //ĳ���Ͱ� õ�忡 �ε����� ���� �ӵ��� 0���� ����ϴ�.
        if (hit.moveDirection.y > 0.0f && velocity.y > 0.0f)
            velocity.y = 0.0f;

        //�ε��� ��ü�� rigidBody
        Rigidbody hitRigidbody = hit.rigidbody;
        if (hitRigidbody == null)
            return;
        //�� ���ϱ�
        Vector3 force = (hit.moveDirection + Vector3.up * 0.35f) * velocity.magnitude * rigidbodyPushForce;
        hitRigidbody.AddForceAtPosition(force, hit.point);
    }

    #endregion

    #region METHODS

    private void MoveCharacter()
    {
        //������ input�� �޾ƿɴϴ�.
        Vector2 frameInput = Vector3.ClampMagnitude(playerCharacter.GetInputMovement(), 1.0f);

        //�÷��̾� ��ǲ�� ����Ͽ� ������ ����մϴ�.
        var desiredDirection = new Vector3(frameInput.x, 0.0f, frameInput.y);

        //�޸��� �ӵ��� ���
        if (playerCharacter.IsRunning())
            desiredDirection *= speedRunning;

        else
        {
            if (crouching)
                desiredDirection *= speedCrouching;
            else
            {   
                if (playerCharacter.IsAiming())
                    desiredDirection *= speedAiming;

                else
                {
                    //�ȴ� �ӵ��� ���մϴ�.
                    desiredDirection *= speedWalking;
                    //�翷���� ���� �������� ���մϴ�. ������ �̵��ϴ� ������ �� �ڿ������� �� �� �ֽ��ϴ�.
                    desiredDirection.x *= walkingMultiplierSideways;
                    //��ǲ���� 0���� ũ�ٸ� ������ �ƴϸ� �ڷ�
                    desiredDirection.z *= (frameInput.y > 0 ? walkingMultiplierForward : walkingMultiplierBackwards);
                }
            }
        }
        //���� ���⺤�ͷ� ��ȯ
        desiredDirection = transform.TransformDirection(desiredDirection);

        //���⸦ ��� ���� �� �ӵ��� ���մϴ�.
        if (equippedWeapon != null)
            desiredDirection *= equippedWeapon.GetMultipleierMovementSpeed();

        //�߷� ����
        if(isGrounded == false)
        {
            if (wasGrounded && !jumping)
                velocity.y = 0.0f;

            //������
            velocity += desiredDirection * (accelerationInAir * airControl * Time.deltaTime);
            //�߷�
            velocity.y -= (velocity.y >= 0 ? jumpGravity : gravity) * Time.deltaTime;

        }
        //���� ������ ���� ������
        else if(!jumping)
        {
            //���� ���� �� ������ �ӵ� ������Ʈ
            velocity = Vector3.Lerp(velocity, new Vector3(desiredDirection.x, velocity.y, desiredDirection.z), Time.deltaTime * (desiredDirection.sqrMagnitude > 0.0f ? acceleration : deceleration));
        }

        //����� �ӵ�
        Vector3 applied = velocity * Time.deltaTime;

        if (controller.isGrounded && !jumping)
            applied.y = -stickToGroundForce;
        //�����̱�
        controller.Move(applied);
    }

    /// <summary>
    /// ���� �ִ°�
    /// </summary>
    public override bool WasGrounded() => wasGrounded;

    /// <summary>
    /// �����ϰ� �ִ� ������
    /// </summary>
    public override bool IsJumping() => jumping;

    /// <summary>
    /// ��ũ�� �� �ִ��� 
    /// </summary>
    public override bool CanCrouch(bool newCrouching)
    {
        //�ʿ��� ��� ��ũ���� ���� �����մϴ�.
        if (canCrouch == false)
            return false;
        //���� ���߿� ���� �� ��ũ�� �� ���� ��� false�� ������
        if (isGrounded == false && canCrouchWhileFalling == false)
            return false;
        
        //��Ʈ�ѷ��� �׻� ��ũ���� ���� �� �ֽ��ϴ�. 
        if (newCrouching)
            return true;

        //�ߺ� Ȯ�� ��ġ
        Vector3 sphereLocation = transform.position + Vector3.up * standingHeight;

        return (Physics.OverlapSphere(sphereLocation, controller.radius, crouchOverlapsMask).Length == 0);

    }

    /// <summary>
    /// ��ũ���� �ִ� ������
    /// </summary>
    public override bool IsCrouching() => crouching;

    /// <summary>
    /// �����ϱ�
    /// </summary>
    public override void Jump()
    {
        //��ũ���� �ִ� �߿� ������ �� �� ����.
        if (crouching && !canJumpWhileCrouching)
            return;

        if (!isGrounded)
            return;

        jumping = true;

        velocity = new Vector3(velocity.x, Mathf.Sqrt(2.0f * jumpForce * jumpGravity), velocity.z);

        lastJumpTime = Time.time;
    }

    /// <summary>
    /// ��ũ����
    /// </summary>
    public override void Crouch(bool newcrouching)
    {
        crouching = newcrouching;

        controller.height = crouching ? crouchHeight : standingHeight;

        controller.center = controller.height / 2.0f * Vector3.up;
    }

    /// <summary>
    /// ��ũ���� �õ��ϱ�
    /// </summary>
    public override void TryCrouch(bool value)
    {
        if (value && CanCrouch(true))
            Crouch(true);

        else if (!value)
            StartCoroutine(nameof(TryUnCrouch));
    }

    public override void TryToggleCrouch() => TryCrouch(!crouching);

    /// <summary>
    /// ĳ���Ͱ� ��ũ���⸦ �õ��մϴ�.
    /// </summary>
    private IEnumerator TryUnCrouch()
    {
      
        //��ũ�� �� ���� ������ ��ٸ���
        yield return new WaitUntil(()=> CanCrouch(false));
        //��ũ�� �� ������ ��ũ����.
        Crouch(false);
    }

    #endregion

    #region GETTERS

    /// <summary>
    /// ���������� ������ �� �ð�
    /// </summary>
    public override float GetLastJumpTime() => lastJumpTime;

    /// <summary>
    /// ������ ���� �ӵ����� ���� �����ɴϴ�
    /// </summary>
    public override float GetMultiplierForward() => walkingMultiplierForward;

    /// <summary>
    /// �翷���� ���� �ӵ����� ���� �����ɴϴ�.
    /// </summary>
    public override float GetMultiplierSideways() => walkingMultiplierSideways;

    /// <summary>
    /// �ڷ� ���� �ӵ����� ���� �����ɴϴ�
    /// </summary>
    /// <returns></returns>
    public override float GetMultiplierBackwards() => walkingMultiplierBackwards;

    /// <summary>
    /// velocity���� �����մϴ�
    /// </summary>
    public override Vector3 GetVelocity() => controller.velocity;

    /// <summary>
    /// ���� �ִ����� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public override bool IsGrounded() => controller.isGrounded;


    public override void OnPhotonSerializeView(PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(jumping);
         
        }
        else
        {
            jumping = (bool)stream.ReceiveNext();
        }


    }

    #endregion

}
