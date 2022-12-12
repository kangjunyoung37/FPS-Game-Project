using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Interface;
using Photon.Pun;
using RootMotion.FinalIK;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// 주요 캐릭터의 구성요소
/// </summary>
[RequireComponent(typeof(CharacterKinematicss))]
public class Character : CharacterBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("캐릭터의 LowerWeapon 컴포넌트")]
    [SerializeField]
    private LowerWeapon lowerWeapon;

    [Tooltip("캐릭터의 movement 컴포넌트")]
    [SerializeField]
    private MovementBehaviour movementBehaviour;
    
    [Title(label: "Inventory")]

    [Tooltip("게임이 시작될 때 장착될 무기 index")]
    [SerializeField]
    private int weaponIndexEquippedAtStart;

    [Tooltip("인벤토리")]
    [SerializeField]
    private InventoryBehaviour inventory;

    [Title(label: "Grenade")]

    [Tooltip("true라면 ,캐릭터의 수류탄이 소진되지 않습니다.")]
    [SerializeField]
    private bool grenadesUnlimited;

    [Tooltip("시작할 때 수류탄의 총량")]
    [SerializeField]
    private int grenadeTotal = 10;

    [Tooltip("캐릭터 카메라 오프셋으로부터 수류탄이 스폰됩니다")]
    [SerializeField]
    private float grenadeSpawnOffset = 1.0f;

    [Tooltip("수류탄 Prefab, 수류탄을 던질 때 스폰됩니다")]
    [SerializeField]
    private GameObject grenadePrefab;

    [Title(label: "Knife")]

    [Tooltip("칼 게임오브젝트")]
    [SerializeField]
    private GameObject knife;

    [Title(label: "Cameras")]

    [Tooltip("보통 카메라")]
    [SerializeField]
    private Camera cameraWorld;

    [Tooltip("무기만 보이는 카메라. Depth")]
    [SerializeField]
    private Camera cameraDepth;

    [Title(label: "Animation")]

    [Tooltip("회전 애니메이션이 얼마나 부드러운지 결정합니다.")]
    [SerializeField]
    private float dampTimeTurning = 0.4f;

    [Tooltip("locomotion blendSpace가 얼마나 부드러운지 결정합니다")]
    [SerializeField]
    private float dampTimeLocomotion = 0.15f;

    [Tooltip("조준 전환을 얼마나 부드럽게 플레이하는지 결정합니다.")]
    [SerializeField]
    private float dampTimeAiming = 0.3f;

    [Tooltip("달리기 오프셋의 보간 속도입니다")]
    [SerializeField]
    private float runningInterpolationSpeed = 12.0f;

    [Tooltip("캐릭터의 무기가 조준되는 속도를 결정합니다.")]
    [SerializeField]
    private float aimingSpeedMultiplier = 1.0f;

    [Title(label:"Character Animator")]

    [Tooltip("FP캐릭터의 애니메이터")]
    [SerializeField]
    private Animator characterAnimator;

    [Tooltip("TP캐릭터의 애니메이터")]
    [SerializeField]
    private Animator TPcharacterAnimator;

    [Title(label: "Field Of View")]

    [Tooltip("기본 FOV")]
    [SerializeField]
    private float fieldOfView = 100.0f;

    [Tooltip("달리는 동안의 FOV 곱셈값")]
    [SerializeField]
    private float fieldOfViewRunningMultiplier = 1.05f;

    [Tooltip("무기별 FOV")]
    [SerializeField]
    private float fieldOfViewWeapon = 55.0f;

    [Title(label: "Audio Clips")]

    [Tooltip("근접 무기 오디오 클립")]
    [SerializeField]
    private AudioClip[] audioClipsMelee;

    [Tooltip("수류탄 던지는 오디오 클립")]
    [SerializeField]
    private AudioClip[] audioClipsGrenadeThrow;

    [Title(label: "Input Options")]

    [Tooltip("true인 경우 running input이 입력되고 있는 동안 활성화 됩니다")]
    [SerializeField]
    private bool holdToRun = true;

    [Tooltip("true인 경우 aiming input이 입력되고 있는 동안 활성화 됩니다.")]
    [SerializeField]
    private bool holdToAim = true;

    [Title(label: "Drop Magazine")]

    [Tooltip("캐릭터가 가지고 있는 무기의 탄창")]
    [SerializeField]
    private Transform magazineTransform;

    [Tooltip("탄창 프리펩")]
    [SerializeField]
    private GameObject prefabMagazine;

    [Title(label:"Renderer Controller")]

    [Tooltip("TP렌더러 컨트롤러")]
    [SerializeField]
    private TPRenController tPRenController;

    [Tooltip("FP렌더러 컨트롤러")]
    [SerializeField]
    private FPRenController fPRenController;

    [Title(label: "Leaning Input")]
    [SerializeField]
    private LeaningInput leaningInput;

    #endregion

    #region FIELDS

    /// <summary>
    /// 캐릭터가 조준하고 있으면 참입니다.
    /// </summary>
    private bool aiming;

    /// <summary>
    /// 마지막 프레임의 조준값입니다.
    /// </summary>
    private bool wasAiming;

    /// <summary>
    /// 캐릭터가 달리고 있으면 참입니다.
    /// </summary>
    private bool running;

    /// <summary>
    /// 캐릭터가 무기를 넣은 경우 참입니다.
    /// </summary>
    private bool holstered;

    /// <summary>
    /// 마지막으로 쏜 시간
    /// </summary>
    private float lastShotTime;

    /// <summary>
    /// 오버레이 레이어 인덱스, 발사 애니메이션같은 것을 재생하는데 유용합니다.
    /// </summary>
    private int layerOverlay;

    /// <summary>
    /// Holster Layer 인덱스, holster 애니메이션을 재생하는데 유용합니다.
    /// </summary>
    private int layerHolster;
    /// <summary>
    /// Actions Layer 인덱스, 재장전같은 action을 재생하는데 유용합니다.
    /// </summary>
    private int layerActions;

    /// <summary>
    /// TP캐릭터의 레이어 인덱스 발사 애니메이션 같은 것을 재생하는데 유용합니다.
    /// </summary>
    private int TPlayerOverlay;

    /// <summary>
    /// TP캐릭터의 레이어 인덱스, holster 애니메이션을 재생하는데 유용합니다.
    /// </summary>
    private int TPlayerHolster;

    /// <summary>
    /// TP캐릭터의 레이어 인덱스, 재장전 같은 action을 재생하는데 유용합니다.
    /// </summary>
    private int TPlayerActions; 
 

    /// <summary>
    /// 현재 장착된 무기
    /// </summary>
    private WeaponBehaviour equippedWeapon;

    /// <summary>
    /// 무기에 장착된 부착물 매니저
    /// </summary>
    private WeaponAttachmentManagerBehaviour weaponAttachmentManager;

    /// <summary>
    /// 캐릭터 무기에 장착된 스코프
    /// </summary>
    private ScopeBehaviour equippedWeaponScope;

    /// <summary>
    /// 캐릭터 무기에 장착된 탄창
    /// </summary>
    private MagazineBehaviour equipeedWeaponMagazine;

    /// <summary>
    /// 캐릭터가 재장전중이면 true입니다.
    /// </summary>
    private bool reloading;

    /// <summary>
    /// 무기를 검사중이면 true입니다.
    /// </summary>
    private bool inspecting;

    /// <summary>
    /// 수류탄을 던지는중이면 true입니다.
    /// </summary>
    private bool throwingGrenade;

    /// <summary>
    /// 캐릭터가 근접공격중이면 true입니다.
    /// </summary>
    private bool meleeing;

    /// <summary>
    /// 캐릭터가 무기를 들고 있으면 true입니다.
    /// </summary>
    private bool holstering;

    /// <summary>
    /// 조준을 나타내는 값. 조준하지 않으면 0이고 완전히 조준 중이면 1입니다.
    /// </summary>
    private float aimingAlpha;

    /// <summary>
    /// 주어진 시간에 웅크리고 있는 상태가 얼마나 보이는지를 나타냅니다.
    /// </summary>
    private float crouchingAlpha;

    /// <summary>
    /// 주어진 시간에 달리고 있는 상태가 얼마나 보이는지를 나타냅니다.
    /// </summary>
    private float runningAlpha;

    /// <summary>
    /// 시야 축값
    /// </summary>
    private Vector2 axisLook;

    /// <summary>
    /// 움직임 축값
    /// </summary>
    private Vector2 axisMovement;

    /// <summary>
    /// 캐릭터가 볼트 액션 애니메이션을 실행하는 중이라면 True입니다.
    /// </summary>
    private bool bolting;

    /// <summary>
    /// 현재 남아있는 수류탄
    /// </summary>
    private int grenadeCount;

    /// <summary>
    /// 만약 조준 버튼을 누르고 있으면 true입니다.
    /// </summary>
    private bool holdingButtonAim;

    /// <summary>
    /// 달리는 버튼을 누르고 있으면 true입니다.
    /// </summary>
    private bool holdingButtonRun;

    /// <summary>
    /// 발사 버튼을 누르고 있으면 true입니다.
    /// </summary>
    private bool holdingButtonFire;

    /// <summary>
    /// 게임 커서가 잠겨있으면 참입니다. "ESC"를 누르면 사용됩니다.
    /// </summary>
    private bool cursorLocked;

    /// <summary>
    /// 연달아 발사한 총알의 양, 반동에 적용하는데 사용합니다.
    /// </summary>
    private int shotsFired;

    /// <summary>
    /// 탄창의 MeshFilter
    /// </summary>
    private MeshFilter meshFilter;

    /// <summary>
    /// 탄창의 MeshRender
    /// </summary>
    private MeshRenderer meshRenderer;

    /// <summary>
    /// Aim ik
    /// </summary>
    private AimIK aimik;

    /// <summary>
    /// 캐릭터의 앞 방향
    /// </summary>
    private Vector3 CharacterForward;

    /// <summary>
    /// 무기를 꺼내는 중인지
    /// </summary>
    [NonSerialized]
    public bool ishostering = false;

    /// <summary>
    /// TP장착 무기
    /// </summary>
    private TPWeapon TPEquipWeapon;
    
    /// <summary>
    /// PhotonView
    /// </summary>
    private PhotonView PV;

    /// <summary>
    /// 싱크 포지션
    /// </summary>
    private Vector3 syncPos;

    /// <summary>
    /// 싱크 로테이션
    /// </summary>
    private Quaternion syncRot;
    
    /// <summary>
    /// 카메라 조정
    /// </summary>
    private CameraLook CL;

    private float movementValue;
    #endregion

    #region UNITY

    protected override void Awake()
    {
        #region Lock Cursor

        //게임이 시작될 때 항상 커서가 잠겨 있는지 확인하세여
        cursorLocked = true;

        UpdateCursorState();

        #endregion

        //캐싱
        //movementBehaviour = GetComponent<MovementBehaviour>();

        //캐싱
        PV = transform.GetComponent<PhotonView>();
        CL = GetComponent<CameraLook>();
        meshFilter = magazineTransform.GetComponent<MeshFilter>();
        meshRenderer = magazineTransform.GetComponent<MeshRenderer>();
        aimik = TPcharacterAnimator.transform.GetComponent<AimIK>();
        //인벤토리 초기화
        inventory.Init(weaponIndexEquippedAtStart);
        
        //새로 고치기
        RefreshWeaponSetup();
        
    }

    protected override void Start()
    {

        if (!PV.IsMine)
        {
            //Destroy(cameraWorld.gameObject);
            //Destroy(cameraDepth.gameObject);
            cameraWorld.enabled = false;
            cameraDepth.enabled = false;
            cameraWorld.GetComponent<AudioListener>().enabled = false;

            fPRenController.FPRenOff();
            equippedWeapon.FPWPOff();
        }
        else
        {
            tPRenController.TPRenderOff();
            TPEquipWeapon.TPWeaponOff();
        }

        //수류탄 양 최대로 설정
        grenadeCount = grenadeTotal;
        //칼을 숨깁니다.
        if (knife != null)
            knife.SetActive(false);

        //레이어 인덱스 캐싱
        layerHolster = characterAnimator.GetLayerIndex("Layer Holster");
        layerActions = characterAnimator.GetLayerIndex("Layer Actions");
        layerOverlay = characterAnimator.GetLayerIndex("Layer Overlay");
        TPlayerHolster = TPcharacterAnimator.GetLayerIndex("Layer Holster");
        TPlayerActions = TPcharacterAnimator.GetLayerIndex("Layer Actions");
        TPlayerOverlay = TPcharacterAnimator.GetLayerIndex("Layer Overlay");
    }

    protected override void Update()
    {
        PVAnimatorUpdate();
        if (!PV.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, syncPos, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Slerp(transform.rotation, syncRot, Time.deltaTime * 15f);
            return;
        }
            
        aiming = holdingButtonAim && CanAim();
        running = holdingButtonRun && CanRun();
        if(aimik == null)
        {
            Debug.LogError("에러");
        }
        else
        {
            IKSolver solver = aimik.GetIKSolver();
        }
        
        switch(aiming)
        {
            //시작
            case true when !wasAiming:
                equippedWeaponScope.OnAim();
                break;
            //멈춤
            case false when wasAiming:
                equippedWeaponScope.OnAimStop();
                break;

        }

        //발사 버튼을 계속 누르고 있으면
        if(holdingButtonFire)
        {
            if(CanPlayAnimationFire() && equippedWeapon.HasAmmunition() && equippedWeapon.IsAutomatic())
            {
                if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
                {
                    //PV.RPC("Fire", RpcTarget.All);
                    Fire();
                }

            }
            else
            {
                //발사된 샷을 재설정합니다. 탄약이 이미 바닥났을 때 반동/확산이 최대로
                //유지되지 않도록 합니다.
                shotsFired = 0;
            }
        }

        //애니메이터 업데이트
        UpdateAnimator();

        //Animator를 사용하여 조준 값을 보간하기 때문에 여기에서 가져와야 합니다.
        aimingAlpha = characterAnimator.GetFloat(AHashes.AimingAlpha);
        //웅크리고 있는 알파를 보간
        crouchingAlpha = Mathf.Lerp(crouchingAlpha, movementBehaviour.IsCrouching() ? 1.0f : 0.0f,Time.deltaTime * 12.0f);
        //달리기 앞라를 보간
        runningAlpha = Mathf.Lerp(runningAlpha, running ? 1.0f : 0.0f, Time.deltaTime * runningInterpolationSpeed);

        //달리기 Fov곱셈값
        float runningFieldOfView = Mathf.Lerp(1.0f, fieldOfViewRunningMultiplier, runningAlpha);
        //조준 여부에 따라 월드 카메라의 시야를 보간합니다.
        cameraWorld.fieldOfView = Mathf.Lerp(fieldOfView, fieldOfView * equippedWeapon.GetFieldOfViewMutiplierAim(), aimingAlpha) * runningFieldOfView;
        //조준 여부에 따라 딥 카메라의 시야를 보간합니다.
        cameraDepth.fieldOfView = Mathf.Lerp(fieldOfViewWeapon, fieldOfViewWeapon * equippedWeapon.GetFieldOfViewMutiplierAimWeapon(), aimingAlpha);

        wasAiming = aiming;


    }
    protected override void LateUpdate()
    {
       
        if (reloading || inspecting || ishostering || meleeing || throwingGrenade )
        {

            CharacterForward = aimik.solver.transform.InverseTransformDirection(transform.forward);
            float x = aimik.solver.axis.x;
            float y = aimik.solver.axis.y;
            float z = aimik.solver.axis.z;

            x = Mathf.Lerp(x, CharacterForward.x, Time.deltaTime * 15);
            y = Mathf.Lerp(y, CharacterForward.y, Time.deltaTime * 15);
            z = Mathf.Lerp(z, CharacterForward.z, Time.deltaTime * 15);
       
            aimik.solver.axis = new Vector3(x, y, z);
        }
        else if (aimik.solver.axis == new Vector3(0.0f, 0.0f, 1f))
            return;
        else
        {

            float x = aimik.solver.axis.x;
            float y = aimik.solver.axis.y;
            float z = aimik.solver.axis.z;

            x = Mathf.Lerp(x, 0.0f, Time.deltaTime * 8.0f);
            y = Mathf.Lerp(y, 0.0f, Time.deltaTime * 8.0f);
            z = Mathf.Lerp(z, 1.0f, Time.deltaTime * 8.0f);
            aimik.solver.axis = new Vector3(x, y, z);
        }
    }
    #endregion

    #region GETTERS

    /// <summary>
    /// 쏜 총알의 수
    /// </summary>
    public override int GetShotsFired() => shotsFired;

    /// <summary>
    /// 무기 낮췄는가
    /// </summary>
    public override bool IsLowered()
    {
        if (lowerWeapon == null)
            return false;

        return lowerWeapon.IsLowered();
    }

    /// <summary>
    /// 월드 카메라 가져오기
    /// </summary>
    public override Camera GetCameraWold() => cameraWorld;

    /// <summary>
    /// 딥 카메라 가져오기
    /// </summary>
    public override Camera GetCameraDepth() => cameraDepth;

    /// <summary>
    /// 인벤토리 가져오기
    /// </summary>
    public override InventoryBehaviour GetInventory() => inventory;

    /// <summary>
    /// 현재 가지고 있는 수류탄의 개수
    /// </summary>
    public override int GetGrenadesCurrent() => grenadeCount;

    /// <summary>
    /// 수류탄 총 개수
    /// </summary>
    public override int GetGrenadesTotal() => grenadeTotal;

    /// <summary>
    /// 달리고 있는 중인지
    /// </summary>
    public override bool IsRunning() => running;

    /// <summary>
    /// 총을 집어넣었는가
    /// </summary>
    public override bool IsHolstered() => holstered;

    /// <summary>
    /// 웅크리고 있는 중인지
    /// </summary>
    public override bool IsCrouching() => movementBehaviour.IsCrouching();

    /// <summary>
    /// 재장전하고 있는 중인지
    /// </summary>
    public override bool IsReloading() => reloading;

    /// <summary>
    /// 수류탄을 던지고 있는 중인지
    /// </summary>
    public override bool IsTrowingGrenade() => throwingGrenade;

    /// <summary>
    /// 근접 공격을 하고 있는 중인지
    /// </summary>
    public override bool IsMeleeing() => meleeing;

    /// <summary>
    /// 조준 중인지 
    /// </summary>
    public override bool IsAiming() => aiming;

    /// <summary>
    /// 커서가 잠겨있는지
    /// </summary>
    public override bool isCursorLocked() => cursorLocked;

    /// <summary>
    /// 키보드 입력 받아오기
    /// </summary>
    public override Vector2 GetInputMovement() => axisMovement;

    /// <summary>
    /// 마우스 입력 받아오기
    /// </summary>
    public override Vector2 GetInputLook() => axisLook;

    /// <summary>
    /// 수류탄 던지는 사운드 가져오기
    /// </summary>
    public override AudioClip[] GetAudioClipsGrenadeThrow() => audioClipsGrenadeThrow;

    /// <summary>
    /// 근접무기 사운드 가져오기 
    /// </summary>
    public override AudioClip[] GetAudioClipsMelee() => audioClipsMelee;

    /// <summary>
    /// 검사중인지
    /// </summary>
    public override bool IsInspecting() => inspecting;

    /// <summary>
    /// 발사버튼을 계속 누르고 있는지
    /// </summary>
    public override bool isHoldingButtonFire() => holdingButtonFire;

    public override void DropMagazine(bool drop = true)
    {
        magazineTransform.gameObject.SetActive(!drop);

        if (!drop)
            return;

        //새로운 탄창 생성
        GameObject spawnedMagazine = Instantiate(prefabMagazine, magazineTransform.position, magazineTransform.rotation);

        spawnedMagazine.GetComponent<MeshRenderer>().sharedMaterials = meshRenderer.sharedMaterials;

        spawnedMagazine.GetComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;

        Destroy(spawnedMagazine, 5.0f);
    }

    #endregion

    #region METHODS

    /// <summary>
    /// 애니메이터 업데이트하기
    /// </summary>
    private void UpdateAnimator()
    {
        #region Reload Stop

        //현재 재장전중인지 체크한다.
        const string boolNameReloading = "Reloading";
        if(characterAnimator.GetBool(boolNameReloading))
        {
            //재장전할 총알이 하나라도 있다면 값을 변경할 수 있습니다.
            if (equippedWeapon.GetAmmunitionTotal() - equippedWeapon.GetAmmunitionCurrent() < 1)
            {
                //캐릭터 애니메이터 업데이트
                characterAnimator.SetBool(boolNameReloading, false);
                TPcharacterAnimator.SetBool(boolNameReloading, false);
                equippedWeapon.GetAnimator().SetBool(boolNameReloading, false);
                TPEquipWeapon.GetAnimator().SetBool(boolNameReloading, false);
            }

        }

        #endregion

        //캐릭터가 기울이는 애니메이션을 얼마나 적용해야하는지 
        float leaningValue = Mathf.Clamp01(axisMovement.y);
       
        characterAnimator.SetFloat(AHashes.LeaningForward, leaningValue, 0.5f, Time.deltaTime);

        //캐릭터가 이동하는 애니메이션을 얼마나 적용해야하는지 
        movementValue = Mathf.Clamp01(Mathf.Abs(axisMovement.x) + Mathf.Abs(axisMovement.y));
        characterAnimator.SetFloat(AHashes.Movement, movementValue, dampTimeLocomotion, Time.deltaTime);
        //TPcharacterAnimator.SetFloat(AHashes.Movement, movementValue, dampTimeLocomotion, Time.deltaTime);
        //조준하는 스피드
        characterAnimator.SetFloat(AHashes.AimingSpeedMultiplier, aimingSpeedMultiplier);

        //회전을 기반으로 재생할 회전 애니메이션의 양을 결정합니다.
        characterAnimator.SetFloat(AHashes.Turning, Mathf.Abs(axisLook.x), dampTimeTurning, Time.deltaTime);

        //수평 움직임 
        characterAnimator.SetFloat(AHashes.Horizontal, axisMovement.x, dampTimeLocomotion, Time.deltaTime);
        //TPcharacterAnimator.SetFloat(AHashes.Horizontal, axisMovement.x, dampTimeLocomotion, Time.deltaTime);
        //수직 움직임
        characterAnimator.SetFloat(AHashes.Vertical, axisMovement.y, dampTimeLocomotion, Time.deltaTime);
        //TPcharacterAnimator.SetFloat(AHashes.Vertical, axisMovement.y, dampTimeLocomotion, Time.deltaTime);

        //조준값을 보간을 이용하여 업데이트
        characterAnimator.SetFloat(AHashes.AimingAlpha, Convert.ToSingle(aiming), dampTimeAiming, Time.deltaTime);
        TPcharacterAnimator.SetFloat(AHashes.AimingAlpha, Convert.ToSingle(aiming), dampTimeAiming, Time.deltaTime);

        //이동 재생 속도를 설정합니다.
        const string playRateLocomotionBool = "Play Rate Locomotion";
        characterAnimator.SetFloat(playRateLocomotionBool, movementBehaviour.IsGrounded() ? 1.0f : 0.0f, 0.2f, Time.deltaTime);

        #region Movement Play Rates

        //이동 배율에 따라 애니메이션의 재생 속도를 변경할 수 있습니다.
        characterAnimator.SetFloat(AHashes.PlayRateLocomotionForward, movementBehaviour.GetMultiplierForward(), 0.2f, Time.deltaTime);
        characterAnimator.SetFloat(AHashes.PlayRateLocomotionSideways, movementBehaviour.GetMultiplierSideways(), 0.2f, Time.deltaTime);
        characterAnimator.SetFloat(AHashes.PlayRateLocomotionBackwards, movementBehaviour.GetMultiplierBackwards(), 0.2f, Time.deltaTime);

        TPcharacterAnimator.SetFloat(AHashes.PlayRateLocomotionForward, movementBehaviour.GetMultiplierForward(), 0.2f, Time.deltaTime);
        TPcharacterAnimator.SetFloat(AHashes.PlayRateLocomotionSideways, movementBehaviour.GetMultiplierSideways(), 0.2f, Time.deltaTime);
        TPcharacterAnimator.SetFloat(AHashes.PlayRateLocomotionBackwards, movementBehaviour.GetMultiplierBackwards(), 0.2f, Time.deltaTime);
        #endregion

        //조준 애니메이터 업데이트
        characterAnimator.SetBool(AHashes.Aim, aiming);
        //달리기 애니메이터 업데이트
        characterAnimator.SetBool(AHashes.Running, running);
        //웅크리기 애니메이터 업데이트
        characterAnimator.SetBool(AHashes.Crouching, movementBehaviour.IsCrouching());
    }

    private void PVAnimatorUpdate()
    {
        TPcharacterAnimator.SetBool(AHashes.Aim, aiming);
        TPcharacterAnimator.SetBool(AHashes.Running, running);
        TPcharacterAnimator.SetFloat(AHashes.Horizontal, axisMovement.x, dampTimeLocomotion, Time.deltaTime);
        TPcharacterAnimator.SetFloat(AHashes.Vertical, axisMovement.y, dampTimeLocomotion, Time.deltaTime);
        TPcharacterAnimator.SetFloat(AHashes.Movement, movementValue, dampTimeLocomotion, Time.deltaTime);
        TPcharacterAnimator.SetBool(AHashes.Crouching, movementBehaviour.IsCrouching());
    }

    /// <summary>
    /// 검사하기
    /// </summary>
    [PunRPC]
    private void Inspect()
    {
        //상태
        inspecting = true;
        //재생
        characterAnimator.CrossFade("Inspect", 0.0f, layerActions, 0);
        TPcharacterAnimator.CrossFade("Inspect", 0.0f, TPlayerActions, 0);
    }

    /// <summary>
    /// 발사하기
    /// </summary>
    private void Fire()
    {
        //발사되는 총알의 양을 늘립니다.반동을 적용하므로 최신 상태로 유지해야합니다.
        shotsFired++;

        //발사 시간을 저장하여 발사 속도를 올바르게 계산할 수 있습니다.
        lastShotTime = Time.time;
        //조준하는 경우 스코프의 확산 배율도 전달해야 합니다.
        equippedWeapon.Fire(aiming ? equippedWeaponScope.GetMultiplierSpread() : 1.0f);
        //TP발사 애니메이션 재생
        TPEquipWeapon.Fire();
        if(equippedWeapon.GetAmmunitionCurrent() == 0)
        {
            TPEquipWeapon.SetSlideBack(1);
        }
        //발사 애니메이션 재생
        const string stateName = "Fire";
        characterAnimator.CrossFade(stateName, 0.05f, layerOverlay, 0);
        //FireAnimation();
        PV.RPC("FireAnimation", RpcTarget.All);

        //탄약이 있는 경우 볼트 액션 애니메이션을 재생합니다.
        if (equippedWeapon.IsBoltAction() && equippedWeapon.HasAmmunition())
            UpdateBolt(true);
        //필요한 경우 무기를 자동으로 재장전합니다. 유탄 발사기 또는 로켓 발사기와 같은 것에 매우 유용합니다.
        if (!equippedWeapon.HasAmmunition() && equippedWeapon.GetAutomaticallyReloadOnEmpty())
            StartCoroutine(nameof(TryReloadAutomatic));
    }

    [PunRPC]
    private void FireAnimation()
    {
        //TP발사 애니메이션 재생
        TPcharacterAnimator.CrossFade("Fire", 0.05f, TPlayerOverlay, 0);
        if (!PV.IsMine)
        {
            equippedWeapon.InstateProjectile(aiming ? equippedWeaponScope.GetMultiplierSpread() : 1.0f);
            TPEquipWeapon.MuzzleFire();
        }
    }

    [PunRPC]
    private void ReloadAnimation(string stateName)
    {
        TPcharacterAnimator.Play(stateName, TPlayerActions, 0.0f);
        TPcharacterAnimator.SetBool(AHashes.Reloading, reloading = true);
        TPEquipWeapon.Reload(stateName);
    }

    /// <summary>
    /// 재장전 애니메이션 재생하기
    /// </summary>
    private void PlayReloadAnimation()
    {
        #region Animation

        string stateName = equippedWeapon.HasCycledReload() ? "Reload Open" : (equippedWeapon.HasAmmunition() ? "Reload" : "Reload Empty");
        //플레이
        characterAnimator.Play(stateName, layerActions, 0.0f);
       
        #endregion

        characterAnimator.SetBool(AHashes.Reloading, reloading = true);

        equippedWeapon.Reload();
        PV.RPC("ReloadAnimation", RpcTarget.All, stateName);


    }

    /// <summary>
    /// 자동 재장전
    /// </summary>
    private IEnumerator TryReloadAutomatic()
    {
        //자동 재장전 시간 기다리기
        yield return new WaitForSeconds(equippedWeapon.GetAutomaticallyReloadOnEmptyDelay());

        //재장전
        PlayReloadAnimation();
    }

    /// <summary>
    /// 무기 장착하기
    /// </summary>
    /// <param name="index">장착될 인벤토리 인덱스</param>
    private IEnumerator Equip(int index = 0)
    {
        ishostering = true;
        //만약 holster가 아니면 holster를 합니다.만약 이미 준비가 되어 있다면 기다릴 필요가 없습니다.
        if (!holstered)
        {
            SetHolstered(holstering = true);

            //기다리기
            yield return new WaitUntil(() => holstering == false);
        }
        //이미 무기를 들고 있는 경우라면 무기를 집어 넣습니다.
        SetHolstered(false);

        characterAnimator.Play("Unholster", layerHolster, 0);
        //TPcharacterAnimator.Play("Unholster", TPlayerHolster, 0);
        //새로운 장비 장착
        inventory.Equip(index);

        //새로 고침
        RefreshWeaponSetup();
    }

    [PunRPC]
    private void EquipWeapon(int index = 0)
    {
        StartCoroutine(nameof(Equip),index);
    }
    /// <summary>
    /// 무기 새로고침하기
    /// </summary>
    private void RefreshWeaponSetup()
    {
        //장착된 무기가 없다면 return
        if ((equippedWeapon = inventory.GetEquipped()) == null)
            return;
        if ((TPEquipWeapon = inventory.EquipTPWeapon()) == null)
            return;
        //애니메이터 컨트롤러를 업데이트합니다.
        characterAnimator.runtimeAnimatorController = equippedWeapon.GetAnimatorController();
        TPcharacterAnimator.runtimeAnimatorController = TPEquipWeapon.Controller;
        aimik.solver.transform = TPEquipWeapon.transform.GetChild(2);
        
        //부착물 매니저를 가져옵니다
        weaponAttachmentManager = equippedWeapon.GetAttachmentManager();
        if (weaponAttachmentManager == null)
            return;
        //스코프정보를 가져옵니다.
        equippedWeaponScope = weaponAttachmentManager.GetEquippedScope();

        //장착된 탄창정보를 가져옵니다.
        equipeedWeaponMagazine = weaponAttachmentManager.GetEquippedMagazine();
    }

    /// <summary>
    /// 총알이 없는데 발사할 때
    /// </summary>
    private void FireEmpty()
    {
        //발사를 하지는 않지만 발사속도에는 이 값이 필요합니다
        lastShotTime = Time.time;
        //재생하기
        characterAnimator.CrossFade("Fire Empty", 0.05f, layerOverlay, 0);
        TPcharacterAnimator.CrossFade("Fire Empty", 0.05f, layerOverlay, 0);
    }

    /// <summary>
    /// 커서 상태를 업데이트합니다.
    /// </summary>
    private void UpdateCursorState()
    {
        //커서 보이는 상태를 업데이트 합니다.
        Cursor.visible = !cursorLocked;
        //커서상태 업데이트 합니다.
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    /// <summary>
    /// 수류탄 던지기
    /// </summary>
    private void PlayGrenadeThrow()
    {
        //상태 업데이트
        throwingGrenade = true;

        //애니메이션 재생
        characterAnimator.CrossFade("Grenade Throw", 0.15f, characterAnimator.GetLayerIndex("Layer Actions Arm Left"), 0.0f);
        characterAnimator.CrossFade("Grenade Throw", 0.05f, characterAnimator.GetLayerIndex("Layer Actions Arm Right"), 0.0f);
        TPcharacterAnimator.CrossFade("Grenade Throw", 0.15f, TPcharacterAnimator.GetLayerIndex("Layer Actions Arm Left"), 0.0f);
        TPcharacterAnimator.CrossFade("Grenade Throw", 0.05f, TPcharacterAnimator.GetLayerIndex("Layer Actions Arm Right"), 0.0f);


    }

    /// <summary>
    /// 근접 무기 애니메이션 재생
    /// </summary>
    private void PlayMelee()
    {
        //상태 업데이트
        meleeing = true;
        
        //애니메이션 재생
        characterAnimator.CrossFade("Knife Attack", 0.05f, characterAnimator.GetLayerIndex("Layer Actions Arm Left"), 0.0f);
        characterAnimator.CrossFade("Knife Attack", 0.05f, characterAnimator.GetLayerIndex("Layer Actions Arm Right"), 0.0f);
        TPcharacterAnimator.CrossFade("Knife Attack", 0.05f, TPcharacterAnimator.GetLayerIndex("Layer Actions Arm Left"), 0.0f);
        TPcharacterAnimator.CrossFade("Knife Attack", 0.05f, TPcharacterAnimator.GetLayerIndex("Layer Actions Arm Right"), 0.0f);

    }
    /// <summary>
    ///  볼트 애니메이션 재생
    /// </summary>
    /// <param name="value"></param>
    private void UpdateBolt(bool value)
    {
        //상태 업데이트
        characterAnimator.SetBool(AHashes.Bolt, bolting = value);
        TPcharacterAnimator.SetBool(AHashes.Bolt, bolting = value);
        
    }

    /// <summary>
    /// 캐릭터의 애니메이터 값에 따라서 Holstered 업데이트하기
    /// </summary>
    private void SetHolstered(bool value = true)
    {
        holstered = value;

        const string boolName = "Holstered";
        characterAnimator.SetBool(boolName, holstered);
        TPcharacterAnimator.SetBool(boolName, holstered);
    }
    #endregion

    #region ACTION CHECKS

    /// <summary>
    /// 발사할 수 있는지
    /// </summary>
    private bool CanPlayAnimationFire()
    {
        //무기 넣기, 무기 넣는 중
        if (holstered || holstering)
            return false;

        //근접공격, 수류탄
        if (meleeing || throwingGrenade)
            return false;

        //재장전, 볼팅
        if (reloading || bolting)
            return false;

        //검사
        if (inspecting)
            return false;

        return true;
    }
    /// <summary>
    /// 재장전 애니메이션을 재생할 수 있는지 
    /// </summary>
    private bool CanPlayAnimationReload()
    {
        //재장전 중
        if (reloading)
            return false;

        //근접 공격 중
        if (meleeing)
            return false;

        //수류탄 던지는 중
        if (throwingGrenade)
            return false;

        //검사중인지
        if (inspecting)
            return false;
        
        //탄창이 가득차 있을 때
        if (!equippedWeapon.CanReloadWhenFull() && equippedWeapon.IsFull())
            return false;

        return true;
    }

    /// <summary>
    /// 수류탄 던지는 애니메이션을 던질 수 있는지
    /// </summary>
    private bool CanPlayAnimationGrenadeThrow()
    {
        //무기를 집어 넣었거나, 집어넣는 중
        if (holstered || holstering)
            return false;

        //근접 공격 중이거나, 수류탄을 던지는 중
        if (meleeing || throwingGrenade)
            return false;

        //재장전 중이거나 볼트액션중이면
        if (reloading || bolting)
            return false;

        //검사중이면
        if (inspecting)
            return false;

        //가진 수류탄의 개수가 0개이거나 무한이 아니라면
        if (!grenadesUnlimited && grenadeCount == 0)
            return false;

        return true;
    }
    /// <summary>
    /// 근접 공격을 할 수 있는지
    /// </summary>
    private bool CanPlayAnimationMelee()
    {
        //무기를 집어 넣었거나, 집어넣는 중이면
        if (holstered || holstering)
            return false;

        //근접 공격 중이거나 수류탄을 던지는 중이면
        if (meleeing || throwingGrenade)
            return false;

        //재장전 중이거나 볼트액션중이면
        if (reloading || bolting)
            return false;

        //검사중이면
        if (inspecting)
            return false;

        return true;

    }

    /// <summary>
    /// 무기를 집어넣는 애니메이션을 재생할 수 있는지
    /// </summary>
    private bool CanPlayAnimationHolster()
    {
        //근접공격중이거나 수류탄을 던지는 중이거나
        if (meleeing || throwingGrenade)
            return false;

        //재장전중이거나 볼트액션중이거나
        if (reloading || bolting)
            return false;

        //검사중이거나
        if (inspecting)
            return false;
        
        return true;
    }
    
    /// <summary>
    /// 무기를 바꿀 수 있는지 
    /// </summary>
    private bool CanChangeWeapon()
    {
        //무기를 집어넣는 중이면
        if (holstering)
            return false;

        //근접 공격중인지, 수류탄을 던지는 중인지
        if (meleeing || throwingGrenade)
            return false;

        //재장전 중인지, 볼트액션중인지
        if (reloading || bolting)
            return false;

        //검사중인지
        if (inspecting)
            return false;

        return true;
    }
    /// <summary>
    /// 검사 애니메이션을 재생할 수 있는지
    /// </summary>
    private bool CanPlayAnimationInspect()
    {
        //무기를 집어 넣었거나 집어넣는 중이면
        if (holstered || holstering)
            return false;

        //근접공격중이거나 수류탄을 던지는 중이면
        if (meleeing || throwingGrenade)
            return false;
        
        //재장전 중이거나 볼트액션중이면
        if (reloading || bolting)
            return false;

        //검사중이면
        if (inspecting)
            return false;

        if (IsCrouching())
            return false;

        return true;
    }
    
    /// <summary>
    /// 조준할 수 있는지
    /// </summary>
    private bool CanAim()
    {
        //무기를 집어 넣었거나 ,검사중이면
        if (holstered || inspecting)
            return false;

        //근접공격중이거나 수류탄을 던지고 있으면
        if (meleeing || throwingGrenade)
            return false;

        //재장전 중이거나, 무기를 집어넣는 중이면
        if ((!equippedWeapon.CanReloadAimed() && reloading) || holstering)
            return false;

        return true;
    }

    /// <summary>
    /// 달릴 수 있는지
    /// </summary>
    private bool CanRun()
    {
        //검사중이거나 볼트액션중이면
        if (inspecting || bolting)
            return false;

        //웅크리고 있는 중인지
        if (movementBehaviour.IsCrouching())
            return false;

        //근접 공격중이거나 수류탄을 던지고 있으면
        if (meleeing || throwingGrenade)
            return false;
        
        //재장전 중인지 조준중인지
        if (reloading || aiming)
            return false;

        //발사 버튼을 누르기 있고 총알이 남아 있다면
        if (holdingButtonFire && equippedWeapon.HasAmmunition())
            return false;
        
        //뒤로 달리거나 완전히 옆으로 움직이는 동안 차단합니다.
        if(axisMovement.y <= 0 || Math.Abs(Math.Abs(axisMovement.x)-1) < 0.01f)
        {
            return false;
        }

        return true;

    }

    #endregion

    #region INPUT

    //발사하기 Input
    public void OnTryFire(InputAction.CallbackContext context)
    {
        //커서가 잠겨있지 않다면
        if (!cursorLocked || !PV.IsMine)
            return;

        switch(context)
        {
            //실행 시작 시 호출
            case { phase: InputActionPhase.Started }:
                holdingButtonFire = true;
                shotsFired = 0;
                break;
            //완전히 실행시 호출
            case { phase: InputActionPhase.Performed }:
                if (!CanPlayAnimationFire())
                    break;

                if (equippedWeapon.HasAmmunition())
                {

                    if (equippedWeapon.IsAutomatic())
                    {
                        //발사된 샷을 재정설합니다. 탄약이 바닥났을 때 반동/확산이 최대로 유지되지 않도록 합니다.
                        shotsFired = 0;

                        break;
                    }
                    if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
                        Fire();
                }
                //총알이 없는 경우
                else
                    FireEmpty();
                break;
            //실행 종료시
            case { phase: InputActionPhase.Canceled }:
               
                holdingButtonFire = false;

                shotsFired = 0;
                break;
        }
    }

    /// <summary>
    /// 재장전하기
    /// </summary>
    public void OnTryPlayReload(InputAction.CallbackContext context)
    {
        //커서가 잡겨있지 않다면 리턴
        if (!cursorLocked || !PV.IsMine)
            return;

        //재장전할 수 없다면 
        if (!CanPlayAnimationReload())
            return;
        
        switch(context)
        {
            case { phase: InputActionPhase.Performed }:

                PlayReloadAnimation();
                break;
        }    

    }

    //검사하기
    public void OnTryInspect(InputAction.CallbackContext context)
    {
        //커서가 잠겨있지 않다면
        if (!cursorLocked || !PV.IsMine)
            return;

        if (!CanPlayAnimationInspect())
            return;

        switch(context)
        {
            case { phase: InputActionPhase.Performed }:
                PV.RPC("Inspect",RpcTarget.All);
                break;
        }
    }

    /// <summary>
    /// 조준하기, 
    /// </summary>
    public void OnTryAiming(InputAction.CallbackContext context)
    {
        if (!cursorLocked || !PV.IsMine)
            return;

        switch(context.phase)
        {
            
            case InputActionPhase.Started:
                if (holdToAim)
                    holdingButtonAim = true;
                break;
            case InputActionPhase.Performed:
                if (!holdToAim)
                    holdingButtonAim = !holdingButtonAim;
                break;
            case InputActionPhase.Canceled:
                if (holdToAim)
                    holdingButtonAim = false;
                break;
        }
    }

    /// <summary>
    /// 무기 집어넣기
    /// </summary>
    public void OnTryHolster(InputAction.CallbackContext context)
    {
        if (!cursorLocked || !PV.IsMine)
            return;

        if (!CanPlayAnimationHolster())
            return;

        switch(context.phase)
        {
            
            case InputActionPhase.Started:
                
                if(holstered)
                {
                    //UnHolster
                    SetHolstered(false);
                    //Holstering
                    holstering = true;
                }
                break;
            
            case InputActionPhase.Performed:
                
                SetHolstered(!holstered);
                holstering = true;
                break;
        }

    }

    /// <summary>
    /// 수류탄 던지기 
    /// </summary>
    public void OnTryThrowGrenade(InputAction.CallbackContext context)
    {
        if (!cursorLocked || !PV.IsMine)
            return;

        switch(context.phase)
        {

            case InputActionPhase.Performed:

                if (CanPlayAnimationGrenadeThrow())
                    PlayGrenadeThrow();
                break;
        }

    }

    /// <summary>
    /// 근접공격하기
    /// </summary>
    public void OnTryMelee(InputAction.CallbackContext context)
    {
        if (!cursorLocked || !PV.IsMine)
            return;

        switch(context.phase)
        {
            case InputActionPhase.Performed:
                //근접공격을 할 수 있다면
                if (CanPlayAnimationMelee())
                    //근접 공격 실행
                    PlayMelee();
                break;
        }
    }

    /// <summary>
    /// 달리기
    /// </summary>
    public void OnTryRun(InputAction.CallbackContext context)
    {
        if (!cursorLocked || !PV.IsMine)
            return;

        switch(context.phase)
        {
            
            case InputActionPhase.Performed:
                //만약 토글을 사용할 경우 사용합니다.
                if (!holdToRun)
                    holdingButtonRun = !holdingButtonRun;
                break;

            case InputActionPhase.Started:
                if (holdToRun)
                    holdingButtonRun = true;
                break;
            
            case InputActionPhase.Canceled:
                if (holdToRun)
                    holdingButtonRun = false;
                break;
        }
    }

    /// <summary>
    /// 점프하기
    /// </summary>
    public void OnTryJump(InputAction.CallbackContext context)
    {
        if (!cursorLocked || !PV.IsMine)
            return;

        switch(context.phase)
        {
            case InputActionPhase.Performed:

                movementBehaviour.Jump();
                break;
        }
    }

    /// <summary>
    /// 다음 인벤토리 무기
    /// </summary>
    public void OnTryInventoryNext(InputAction.CallbackContext context)
    {
        if (!cursorLocked || !PV.IsMine)
            return;

        if (inventory == null)
            return;

        switch(context.phase)
        {
            case InputActionPhase.Performed:
                //스크롤 휠 방향을 사용하여 인벤토리에 대한 인덱스 증가 방향을 가져옵니다.
                float scrollValue = context.valueType.IsEquivalentTo(typeof(Vector2)) ? Mathf.Sign(context.ReadValue<Vector2>().y) : 1.0f;
                //scrollValue가 양수이면 다음index 음수이면 전index
                int indexNext = scrollValue > 0 ? inventory.GetNextIndex() : inventory.GetLastIndex();
                //현재 장착한 무기 인덱스
                int indexCurrent = inventory.GetEquippedIndex();
                //무기를 바꿀수 있고, 현재 인덱스과 다음 인덱스와 같지 않다면
                if (CanChangeWeapon() && (indexCurrent != indexNext))
                    PV.RPC("EquipWeapon", RpcTarget.All, indexNext);
                break;              

        }
    }

    /// <summary>
    /// 커서 잠구기
    /// </summary>
    public void OnLockCursor(InputAction.CallbackContext context)
    {
        switch(context.phase)
        {
            case InputActionPhase.Performed:
                //커서 잠금 값을 토글합니다.
                cursorLocked = !cursorLocked;
                //커서 상태 업데이트
                UpdateCursorState();
                break;
        }
    }

    /// <summary>
    /// 움직이기
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        //입력 값 받아오기
        axisMovement = cursorLocked ? context.ReadValue<Vector2>() : default;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //마우스 입력 받아오기
        axisLook = cursorLocked ? context.ReadValue<Vector2>() : default;

        if (equippedWeapon == null)
            return;

        if (equippedWeaponScope == null)
            return;
        //조준하고 있다면 장착한 무기의 스코프에 마우스 감도 배율을 곱합니다.
        axisLook *= aiming ? equippedWeaponScope.GetMutiplierMouseSensitivity() : 1.0f;

    }

    #endregion

    #region ANIMATION EVENTS

    /// <summary>
    /// 탄피 배출
    /// </summary>
    public override void EjectCasing()
    {
        if (equippedWeapon != null)
            equippedWeapon.EjectCasing();

    }

    /// <summary>
    /// 탄약 채우기
    /// </summary>
    /// <param name="amount">탄약 양</param>
    public override void FillAmmunition(int amount)
    {
        //양만큼 탄약을 채우도록 무기에 알립니다.
        if (equippedWeapon != null)
            equippedWeapon.FillAmmunition(amount);
    }

    /// <summary>
    /// 수류탄 던지기
    /// </summary>
    public override void Grenade()
    {
        if (grenadePrefab == null)
            return;

        if (cameraWorld == null)
            return;

        if (!grenadesUnlimited)
            grenadeCount--;

        //카메라의 위치를 가져옵니다.
        Transform cTransform = cameraWorld.transform;
        //던지는 위치를 계산합니다.

        Vector3 position = cTransform.position;
        position += cTransform.forward * grenadeSpawnOffset;
        //던지기
        Instantiate(grenadePrefab, position, cTransform.rotation);
    }

    /// <summary>
    /// 탄창 활성화시키기 
    /// </summary>
    public override void SetActiveMagzine(int active)
    {
        //탄창 활성화하기
        equipeedWeaponMagazine.gameObject.SetActive(active != 0);
    }

    /// <summary>
    /// 볼트 액션 끝내기
    /// </summary>
    public override void AnimationEndedBolt()
    {
        UpdateBolt(false);
    }

    /// <summary>
    /// 재장전 애니메이션 끝내기
    /// </summary>
    public override void AnimationEndedReload()
    {
        reloading = false;
    }

    /// <summary>
    /// 수류탄 던지기 애니메이션 종료
    /// </summary>
    public override void AnimationEndedGrenadeThrow()
    {
        throwingGrenade = false;
    }

    /// <summary>
    /// 근접 공격 애니메이션 종료
    /// </summary>
    public override void AnimationEndedMelee()
    {
        meleeing = false;
    }

    /// <summary>
    /// 검사 애니메이션 종료
    /// </summary>
    public override void AnimationEndedInspect()
    {
        inspecting = false;
    }

    /// <summary>
    /// 무기 넣는 애니메이션 종료
    /// </summary>
    public override void AnimationEndedHolster()
    {
        holstering = false;
    }

    /// <summary>
    /// 슬라이드백
    /// </summary>
    public override void SetSlideBack(int back)
    {
        if(equippedWeapon != null)
            equippedWeapon.SetSlideBack(back);
        if (TPEquipWeapon != null)
            TPEquipWeapon.SetSlideBack(back);
    }

    /// <summary>
    /// 칼 활성화하기
    /// </summary>
    public override void SetActiveKnife(int active)
    {
        //활성화하기
        knife.SetActive(active != 0);
    }


    #endregion

    #region PunCallBack

    public override void OnPhotonSerializeView(PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
  
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(axisMovement);
            stream.SendNext(movementValue);
            stream.SendNext(reloading);
            stream.SendNext(inspecting);
            stream.SendNext(ishostering);
            stream.SendNext(meleeing);
            stream.SendNext(throwingGrenade);
            stream.SendNext(shotsFired);
            stream.SendNext(aiming);
        }
        else
        {
            syncPos = (Vector3)stream.ReceiveNext();
            if(Vector3.Distance(transform.position, syncPos) > 5f)
            {
                transform.position = syncPos;
            }
            syncRot = (Quaternion)stream.ReceiveNext();
            axisMovement = (Vector2)stream.ReceiveNext();
            movementValue = (float)stream.ReceiveNext();
            reloading = (bool)stream.ReceiveNext();
            inspecting = (bool)stream.ReceiveNext();
            ishostering= (bool)stream.ReceiveNext();
            meleeing= (bool)stream.ReceiveNext();
            throwingGrenade= (bool)stream.ReceiveNext();
            shotsFired = (int)stream.ReceiveNext();
            aiming = (bool)stream.ReceiveNext();
        }

        CL.OnPhotonSerializeView(stream, info);
        movementBehaviour.OnPhotonSerializeView(stream, info);
        weaponAttachmentManager.OnPhotonSerializeView(stream, info);
        leaningInput.OnPhotonSerializeView(stream, info);

    }

    #endregion
}
