using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

/// <summary>
/// 캐릭터의 움직임을 처리하는 기본 구성요소
/// 달리기, 웅크리기, 점프 
/// </summary>
public class Movement : MovementBehaviour
{

    
    #region FIELDS SERIALIZED

    [Title(label:"FIELD")]

    [Tooltip("TP캐릭터의 애니메이터 컨트롤러")]
    [SerializeField , NotNull]
    private Animator TPAnimatorController;

    [Title(label: "Acceleration")]

    [Tooltip("캐릭터의 속도가 얼마나 빠르게 증가하는지")]
    [SerializeField]
    private float acceleration = 9.0f;

    [Tooltip("공중에 있을 때 사용되는 가속도 값입니다.")]
    [SerializeField]
    private float accelerationInAir = 3.0f;

    [Tooltip("캐릭터의 속도가 얼마나 빠르게 감소하는지")]
    [SerializeField]
    private float deceleration = 11.0f;

    [Title(label: "Speeds")]

    [Tooltip("캐릭터가 걷는 동안의 속도")]
    [SerializeField]
    private float speedWalking = 4.0f;

    [Tooltip("조준중에 캐릭터가 얼마나 빠르게 움직이는지")]
    [SerializeField]
    private float speedAiming = 3.2f;

    [Tooltip("웅크리고 있을 때 캐릭터가 얼마나 빠르게 움직이는지")]
    [SerializeField]
    private float speedCrouching = 3.5f;

    [Tooltip("달리는 동안 캐릭터가 얼마나 빠르게 움직이는지")]
    [SerializeField]
    private float speedRunning = 6.8f;

    [Title(label: "Walking Multipliers")]

    [Tooltip("캐릭터가 앞으로 이동할 때 걷는 속도의 배율값입니다.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float walkingMultiplierForward = 1.0f;

    [Tooltip("캐릭터가 양옆으로 이동할 때 걷는 속도의 배율값입니다")]
    [Range(0f, 1f)]
    [SerializeField]
    private float walkingMultiplierSideways = 1.0f;

    [Tooltip("캐릭터가 뒤로 이동할 때 걷는 속도의 배율값입니다")]
    [Range(0f, 1f)]
    [SerializeField]
    private float walkingMultiplierBackwards = 1.0f;

    [Title(label: "Air")]

    [Tooltip("캐릭터가 공중에 있는 동안 플레이어가 방향 변경을 얼마나 제어할 수 있는지")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float airControl = 0.8f;

    [Tooltip("캐릭터의 중력값, 캐릭터가 얼마나 빠르게 떨어지는지")]
    [SerializeField]
    float gravity = 1.1f;

    [Tooltip("캐릭터의 중력값 캐릭터의 떨어지는 속도")]
    [SerializeField]
    private float jumpGravity = 1.0f;

    [Tooltip("점프할 때 힘")]
    [SerializeField]
    private float jumpForce = 100.0f;

    [Tooltip("캐릭터가 내리막길에서 날아가지 않도록 하기 위해 적용되는 힘입니다.")]
    [SerializeField]
    private float stickToGroundForce = 0.03f;

    [Title(label: "Crouching")]

    [Tooltip("false라면 캐릭터가 웅크리는 것을 차단합니다")]
    [SerializeField]
    private bool canCrouch = true;

    [Tooltip("true이면 떨어지는 동안 캐릭터가 웅크릴 수 있읍니다")]
    [SerializeField, ShowIf(nameof(canCrouch), true)]
    private bool canCrouchWhileFalling = false;

    [Tooltip("true이면 캐릭터가 웅크리고 있는 동안 jump를 할 수 있습니다")]
    [SerializeField, ShowIf(nameof(canCrouch), true)]
    private bool canJumpWhileCrouching = true;

    [Tooltip("웅크리고  있는동안 캐릭터의 높이")]
    [SerializeField, ShowIf(nameof(canCrouch), true)]
    private float crouchHeight = 1.0f;

    [Tooltip("몸을 구부리려고 할 때 겹칠 수 있는 가능한 레이어의 마스크")]
    [SerializeField, ShowIf(nameof(canCrouch), true)]
    private LayerMask crouchOverlapsMask;

    [Title(label: "Rigidbody Push")]

    [Tooltip("다른 리지드바디에 들어갈 때 적용되는 힘입니다.")]
    [SerializeField]
    private float rigidbodyPushForce = 1.0f;

   

    #endregion

    #region FIELDS

    /// <summary>
    /// 캐릭터 컨트롤러
    /// </summary>
    private CharacterController controller;
    /// <summary>
    /// 캐릭터 Behaviour
    /// </summary>
    private CharacterBehaviour playerCharacter;
    /// <summary>
    /// 장착된 무기
    /// </summary>
    private WeaponBehaviour equippedWeapon;
    /// <summary>
    /// 캐릭터의 기본 높이
    /// </summary>
    private float standingHeight;
    /// <summary>
    /// 속도
    /// </summary>
    private Vector3 velocity;
    /// <summary>
    /// 캐릭터가 땅에 있는지
    /// </summary>
    private bool isGrounded;
    /// <summary>
    /// 마지막 프레임에 캐릭터가 땅에 있었는지
    /// </summary>
    private bool wasGrounded;
    /// <summary>
    /// 점프하고 있는 중인지
    /// </summary>
    private bool jumping;
    /// <summary>
    /// 캐릭터가 웅크리고 있는지
    /// </summary>
    private bool crouching;
    /// <summary>
    /// 캐릭터가 마지막으로 점프했던 시간
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
   
    //시작시 FPS 콘트롤러를 초기화합니다.
    protected override void Start()
    {

        standingHeight = controller.height;
    }

    protected override void Update()
    {
        if (!PV.IsMine)
            return;
        //인벤토리에서 무기를 가져옴
        equippedWeapon = playerCharacter.GetInventory().GetEquipped();
        //이 프레임에 땅에 있었는지
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
        //캐릭터가 천장에 부딪히면 상향 속도를 0으로 만듭니다.
        if (hit.moveDirection.y > 0.0f && velocity.y > 0.0f)
            velocity.y = 0.0f;

        //부딪힌 물체의 rigidBody
        Rigidbody hitRigidbody = hit.rigidbody;
        if (hitRigidbody == null)
            return;
        //힘 더하기
        Vector3 force = (hit.moveDirection + Vector3.up * 0.35f) * velocity.magnitude * rigidbodyPushForce;
        hitRigidbody.AddForceAtPosition(force, hit.point);
    }

    #endregion

    #region METHODS

    private void MoveCharacter()
    {
        //움직인 input을 받아옵니다.
        Vector2 frameInput = Vector3.ClampMagnitude(playerCharacter.GetInputMovement(), 1.0f);

        //플레이어 인풋을 사용하여 방향을 계산합니다.
        var desiredDirection = new Vector3(frameInput.x, 0.0f, frameInput.y);

        //달리는 속도를 계산
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
                    //걷는 속도를 곱합니다.
                    desiredDirection *= speedWalking;
                    //양옆으로 가는 배율값을 곱합니다. 옆으로 이동하는 느낌을 더 자연스럽게 할 수 있습니다.
                    desiredDirection.x *= walkingMultiplierSideways;
                    //인풋값이 0보다 크다면 앞으로 아니면 뒤로
                    desiredDirection.z *= (frameInput.y > 0 ? walkingMultiplierForward : walkingMultiplierBackwards);
                }
            }
        }
        //월드 방향벡터로 변환
        desiredDirection = transform.TransformDirection(desiredDirection);

        //무기를 들고 있을 때 속도를 곱합니다.
        if (equippedWeapon != null)
            desiredDirection *= equippedWeapon.GetMultipleierMovementSpeed();

        //중력 적용
        if(isGrounded == false)
        {
            if (wasGrounded && !jumping)
                velocity.y = 0.0f;

            //움직임
            velocity += desiredDirection * (accelerationInAir * airControl * Time.deltaTime);
            //중력
            velocity.y -= (velocity.y >= 0 ? jumpGravity : gravity) * Time.deltaTime;

        }
        //땅에 있을때 보통 움직임
        else if(!jumping)
        {
            //땅에 있을 때 움직임 속도 업데이트
            velocity = Vector3.Lerp(velocity, new Vector3(desiredDirection.x, velocity.y, desiredDirection.z), Time.deltaTime * (desiredDirection.sqrMagnitude > 0.0f ? acceleration : deceleration));
        }

        //적용된 속도
        Vector3 applied = velocity * Time.deltaTime;

        if (controller.isGrounded && !jumping)
            applied.y = -stickToGroundForce;
        //움직이기
        controller.Move(applied);
    }

    /// <summary>
    /// 땅에 있는가
    /// </summary>
    public override bool WasGrounded() => wasGrounded;

    /// <summary>
    /// 점프하고 있는 중인지
    /// </summary>
    public override bool IsJumping() => jumping;

    /// <summary>
    /// 웅크릴 수 있는지 
    /// </summary>
    public override bool CanCrouch(bool newCrouching)
    {
        //필요한 경우 웅크리는 것을 차단합니다.
        if (canCrouch == false)
            return false;
        //만약 공중에 있을 때 웅크릴 수 없는 경우 false를 리턴함
        if (isGrounded == false && canCrouchWhileFalling == false)
            return false;
        
        //컨트롤러는 항상 웅크리고 있을 수 있습니다. 
        if (newCrouching)
            return true;

        //중복 확인 위치
        Vector3 sphereLocation = transform.position + Vector3.up * standingHeight;

        return (Physics.OverlapSphere(sphereLocation, controller.radius, crouchOverlapsMask).Length == 0);

    }

    /// <summary>
    /// 웅크리고 있는 중인지
    /// </summary>
    public override bool IsCrouching() => crouching;

    /// <summary>
    /// 점프하기
    /// </summary>
    public override void Jump()
    {
        //웅크리고 있는 중에 점프를 할 수 없다.
        if (crouching && !canJumpWhileCrouching)
            return;

        if (!isGrounded)
            return;

        jumping = true;

        velocity = new Vector3(velocity.x, Mathf.Sqrt(2.0f * jumpForce * jumpGravity), velocity.z);

        lastJumpTime = Time.time;
    }

    /// <summary>
    /// 웅크리기
    /// </summary>
    public override void Crouch(bool newcrouching)
    {
        crouching = newcrouching;

        controller.height = crouching ? crouchHeight : standingHeight;

        controller.center = controller.height / 2.0f * Vector3.up;
    }

    /// <summary>
    /// 웅크리기 시도하기
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
    /// 캐릭터가 웅크리기를 시도합니다.
    /// </summary>
    private IEnumerator TryUnCrouch()
    {
      
        //웅크릴 수 있을 때까지 기다리며
        yield return new WaitUntil(()=> CanCrouch(false));
        //웅크릴 수 있으면 웅크린다.
        Crouch(false);
    }

    #endregion

    #region GETTERS

    /// <summary>
    /// 마지막으로 점프를 뛴 시간
    /// </summary>
    public override float GetLastJumpTime() => lastJumpTime;

    /// <summary>
    /// 앞으로 가는 속도배율 값을 가져옵니다
    /// </summary>
    public override float GetMultiplierForward() => walkingMultiplierForward;

    /// <summary>
    /// 양옆으로 가는 속도배율 값을 가져옵니다.
    /// </summary>
    public override float GetMultiplierSideways() => walkingMultiplierSideways;

    /// <summary>
    /// 뒤로 가는 속도배율 값을 가져옵니다
    /// </summary>
    /// <returns></returns>
    public override float GetMultiplierBackwards() => walkingMultiplierBackwards;

    /// <summary>
    /// velocity값을 리턴합니다
    /// </summary>
    public override Vector3 GetVelocity() => controller.velocity;

    /// <summary>
    /// 땅에 있는지를 리턴합니다.
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
