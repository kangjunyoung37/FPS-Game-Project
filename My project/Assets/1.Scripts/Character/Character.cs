using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �ֿ� ĳ������ �������
/// </summary>
[RequireComponent(typeof(CharacterKinematics))]
public class Character : CharacterBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("ĳ������ LowerWeapon ������Ʈ")]
    [SerializeField]
    private LowerWeapon lowerWeapon;

    [Title(label: "Inventory")]

    [Tooltip("������ ���۵� �� ������ ���� index")]
    [SerializeField]
    private int weaponIndexEquippedAtStart;

    [Tooltip("�κ��丮")]
    [SerializeField]
    private InventoryBehaviour inventory;

    [Title(label: "Grenade")]

    [Tooltip("true��� ,ĳ������ ����ź�� �������� �ʽ��ϴ�.")]
    [SerializeField]
    private bool grenadesUnlimited;

    [Tooltip("������ �� ����ź�� �ѷ�")]
    [SerializeField]
    private int grenadeTotal = 10;

    [Tooltip("ĳ���� ī�޶� ���������κ��� ����ź�� �����˴ϴ�")]
    [SerializeField]
    private float grenadeSpawnOffset = 1.0f;

    [Tooltip("����ź Prefab, ����ź�� ���� �� �����˴ϴ�")]
    [SerializeField]
    private GameObject grenadePrefab;

    [Title(label: "Knife")]

    [Tooltip("Į ���ӿ�����Ʈ")]
    [SerializeField]
    private GameObject knife;

    [Title(label: "Cameras")]

    [Tooltip("���� ī�޶�")]
    [SerializeField]
    private Camera cameraWorld;

    [Tooltip("���⸸ ���̴� ī�޶�. Depth")]
    [SerializeField]
    private Camera cameraDepth;

    [Title(label: "Animation")]

    [Tooltip("ȸ�� �ִϸ��̼��� �󸶳� �ε巯���� �����մϴ�.")]
    [SerializeField]
    private float dampTimeTurning = 0.4f;

    [Tooltip("locomotion blendSpace�� �󸶳� �ε巯���� �����մϴ�")]
    [SerializeField]
    private float dampTimeLocomotion = 0.15f;

    [Tooltip("���� ��ȯ�� �󸶳� �ε巴�� �÷����ϴ��� �����մϴ�.")]
    [SerializeField]
    private float dampTimeAiming = 0.3f;

    [Tooltip("�޸��� �������� ���� �ӵ��Դϴ�")]
    [SerializeField]
    private float runningInterpolationSpeed = 12.0f;

    [Tooltip("ĳ������ ���Ⱑ ���صǴ� �ӵ��� �����մϴ�.")]
    [SerializeField]
    private float aimingSpeedMultiplier = 1.0f;

    [Title(label:"Character Animator")]
    [SerializeField]
    private Animator characterAnimator;

    [Title(label: "Field Of View")]

    [Tooltip("�⺻ FOV")]
    [SerializeField]
    private float fieldOfView = 100.0f;

    [Tooltip("�޸��� ������ FOV ������")]
    [SerializeField]
    private float fieldOfViewRunningMultiplier = 1.05f;

    [Tooltip("���⺰ FOV")]
    [SerializeField]
    private float fieldOfViewWeapon = 55.0f;

    [Title(label: "Audio Clips")]

    [Tooltip("���� ���� ����� Ŭ��")]
    [SerializeField]
    private AudioClip[] audioClipsMelee;

    [Tooltip("����ź ������ ����� Ŭ��")]
    [SerializeField]
    private AudioClip[] audioClipsGrenadeThrow;

    [Title(label: "Input Options")]

    [Tooltip("true�� ��� running input�� �Էµǰ� �ִ� ���� Ȱ��ȭ �˴ϴ�")]
    [SerializeField]
    private bool holdToRun = true;

    [Tooltip("true�� ��� aiming input�� �Էµǰ� �ִ� ���� Ȱ��ȭ �˴ϴ�.")]
    [SerializeField]
    private bool holdToAim = true;

    #endregion

    #region FIELDS

    /// <summary>
    /// ĳ���Ͱ� �����ϰ� ������ ���Դϴ�.
    /// </summary>
    private bool aiming;

    /// <summary>
    /// ������ �������� ���ذ��Դϴ�.
    /// </summary>
    private bool wasAiming;

    /// <summary>
    /// ĳ���Ͱ� �޸��� ������ ���Դϴ�.
    /// </summary>
    private bool running;

    /// <summary>
    /// ĳ���Ͱ� ���⸦ ���� ��� ���Դϴ�.
    /// </summary>
    private bool holstered;

    /// <summary>
    /// ���������� �� �ð�
    /// </summary>
    private float lastShotTime;

    /// <summary>
    /// �������� ���̾� �ε���, �߻� �ִϸ��̼ǰ��� ���� ����ϴµ� �����մϴ�.
    /// </summary>
    private int layerOverlay;

    /// <summary>
    /// Holster Layer �ε���, holster �ִϸ��̼��� ����ϴµ� �����մϴ�.
    /// </summary>
    private int layerHolster;

    /// <summary>
    /// Actions Layer �ε���, ���������� action�� ����ϴµ� �����մϴ�.
    /// </summary>
    private int layerActions;

    /// <summary>
    /// �̵����� ������Ʈ
    /// </summary>
    private MovementBehaviour movementBehaviour;

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    private WeaponBehaviour equippedWeapon;

    /// <summary>
    /// ���⿡ ������ ������ �Ŵ���
    /// </summary>
    private WeaponAttachmentManagerBehaviour weaponAttachmentManager;

    /// <summary>
    /// ĳ���� ���⿡ ������ ������
    /// </summary>
    private ScopeBehaviour equippedWeaponScope;

    /// <summary>
    /// ĳ���� ���⿡ ������ źâ
    /// </summary>
    private MagazineBehaviour equipeedWeaponMagazine;

    /// <summary>
    /// ĳ���Ͱ� ���������̸� true�Դϴ�.
    /// </summary>
    private bool reloading;

    /// <summary>
    /// ���⸦ �˻����̸� true�Դϴ�.
    /// </summary>
    private bool inspecting;

    /// <summary>
    /// ����ź�� ���������̸� true�Դϴ�.
    /// </summary>
    private bool throwingGrenade;

    /// <summary>
    /// ĳ���Ͱ� �����������̸� true�Դϴ�.
    /// </summary>
    private bool meleeing;

    /// <summary>
    /// ĳ���Ͱ� ���⸦ ��� ������ true�Դϴ�.
    /// </summary>
    private bool holstering;

    /// <summary>
    /// ������ ��Ÿ���� ��. �������� ������ 0�̰� ������ ���� ���̸� 1�Դϴ�.
    /// </summary>
    private float aimingAlpha;

    /// <summary>
    /// �־��� �ð��� ��ũ���� �ִ� ���°� �󸶳� ���̴����� ��Ÿ���ϴ�.
    /// </summary>
    private float crouchingAlpha;

    /// <summary>
    /// �־��� �ð��� �޸��� �ִ� ���°� �󸶳� ���̴����� ��Ÿ���ϴ�.
    /// </summary>
    private float runningAlpha;

    /// <summary>
    /// �þ� �ప
    /// </summary>
    private Vector2 axisLook;

    /// <summary>
    /// ������ �ప
    /// </summary>
    private Vector2 axisMovement;

    /// <summary>
    /// ĳ���Ͱ� ��Ʈ �׼� �ִϸ��̼��� �����ϴ� ���̶�� True�Դϴ�.
    /// </summary>
    private bool bolting;

    /// <summary>
    /// ���� �����ִ� ����ź
    /// </summary>
    private int grenadeCount;

    /// <summary>
    /// ���� ���� ��ư�� ������ ������ true�Դϴ�.
    /// </summary>
    private bool holdingButtonAim;

    /// <summary>
    /// �޸��� ��ư�� ������ ������ true�Դϴ�.
    /// </summary>
    private bool holdingButtonRun;

    /// <summary>
    /// �߻� ��ư�� ������ ������ true�Դϴ�.
    /// </summary>
    private bool holdingButtonFire;

    /// <summary>
    /// ���� Ŀ���� ��������� ���Դϴ�. "ESC"�� ������ ���˴ϴ�.
    /// </summary>
    private bool cursorLocked;

    /// <summary>
    /// ���޾� �߻��� �Ѿ��� ��, �ݵ��� �����ϴµ� ����մϴ�.
    /// </summary>
    private int shotsFired;

    #endregion

    #region UNITY

    protected override void Awake()
    {
        #region Lock Cursor

        //������ ���۵� �� �׻� Ŀ���� ��� �ִ��� Ȯ���ϼ���
        cursorLocked = true;

        UpdateCursorState();

        #endregion

        //ĳ��
        movementBehaviour = GetComponent<MovementBehaviour>();

        //�κ��丮 �ʱ�ȭ
        inventory.Init(weaponIndexEquippedAtStart);

        //���� ��ġ��
        RefreshWeaponSetup();

    }

    protected override void Start()
    {
        //����ź �� �ִ�� ����
        grenadeCount = grenadeTotal;
        //Į�� ����ϴ�.
        if (knife != null)
            knife.SetActive(false);

        //���̾� �ε��� ĳ��
        layerHolster = characterAnimator.GetLayerIndex("Layer Holster");
        layerActions = characterAnimator.GetLayerIndex("Layer Actions");
        layerOverlay = characterAnimator.GetLayerIndex("Layer Overlay");
    }

    protected override void Update()
    {
        aiming = holdingButtonAim && CanAim();
        running = holdingButtonRun && CanRun();

        switch(aiming)
        {
            //����
            case true when !wasAiming:
                equippedWeaponScope.OnAim();
                break;
            //����
            case false when wasAiming:
                equippedWeaponScope.OnAimStop();
                break;

        }
        //�߻� ��ư�� ��� ������ ������
        if(holdingButtonFire)
        {
            if(CanPlayAnimationFire() && equippedWeapon.HasAmmunition() && equippedWeapon.IsAutomatic())
            {
                if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
                    Fire();
            }
            else
            {
                //�߻�� ���� �缳���մϴ�. ź���� �̹� �ٴڳ��� �� �ݵ�/Ȯ���� �ִ��
                //�������� �ʵ��� �մϴ�.
                shotsFired = 0;
            }
        }
        //�ִϸ����� ������Ʈ
        UpdateAnimator();

        //Animator�� ����Ͽ� ���� ���� �����ϱ� ������ ���⿡�� �����;� �մϴ�.
        aimingAlpha = characterAnimator.GetFloat(AHashes.AimingAlpha);
        //��ũ���� �ִ� ���ĸ� ����
        crouchingAlpha = Mathf.Lerp(crouchingAlpha, movementBehaviour.IsCrouching() ? 1.0f : 0.0f,Time.deltaTime * 12.0f);
        //�޸��� �ն� ����
        runningAlpha = Mathf.Lerp(runningAlpha, running ? 1.0f : 0.0f, Time.deltaTime * runningInterpolationSpeed);

        //�޸��� Fov������
        float runningFieldOfView = Mathf.Lerp(1.0f, fieldOfViewRunningMultiplier, runningAlpha);
        //���� ���ο� ���� ���� ī�޶��� �þ߸� �����մϴ�.
        cameraWorld.fieldOfView = Mathf.Lerp(fieldOfView, fieldOfView * equippedWeapon.GetFieldOfViewMutiplierAim(), aimingAlpha) * runningFieldOfView;
        //���� ���ο� ���� �� ī�޶��� �þ߸� �����մϴ�.
        cameraDepth.fieldOfView = Mathf.Lerp(fieldOfViewWeapon, fieldOfViewWeapon * equippedWeapon.GetFieldOfViewMutiplierAimWeapon(), aimingAlpha);

        wasAiming = aiming;

    }
    #endregion

    #region GETTERS

    /// <summary>
    /// �� �Ѿ��� ��
    /// </summary>
    public override int GetShotsFired() => shotsFired;

    /// <summary>
    /// ���� ����°�
    /// </summary>
    public override bool IsLowered()
    {
        if (lowerWeapon == null)
            return false;

        return lowerWeapon.IsLowered();
    }

    /// <summary>
    /// ���� ī�޶� ��������
    /// </summary>
    public override Camera GetCameraWold() => cameraWorld;

    /// <summary>
    /// �� ī�޶� ��������
    /// </summary>
    public override Camera GetCameraDepth() => cameraDepth;

    /// <summary>
    /// �κ��丮 ��������
    /// </summary>
    public override InventoryBehaviour GetInventory() => inventory;

    /// <summary>
    /// ���� ������ �ִ� ����ź�� ����
    /// </summary>
    public override int GetGrenadesCurrent() => grenadeCount;

    /// <summary>
    /// ����ź �� ����
    /// </summary>
    public override int GetGrenadesTotal() => grenadeTotal;

    /// <summary>
    /// �޸��� �ִ� ������
    /// </summary>
    public override bool IsRunning() => running;

    /// <summary>
    /// ���� ����־��°�
    /// </summary>
    public override bool IsHolstered() => holstered;

    /// <summary>
    /// ��ũ���� �ִ� ������
    /// </summary>
    public override bool IsCrouching() => movementBehaviour.IsCrouching();

    /// <summary>
    /// �������ϰ� �ִ� ������
    /// </summary>
    public override bool IsReloading() => reloading;

    /// <summary>
    /// ����ź�� ������ �ִ� ������
    /// </summary>
    public override bool IsTrowingGrenade() => throwingGrenade;

    /// <summary>
    /// ���� ������ �ϰ� �ִ� ������
    /// </summary>
    public override bool IsMeleeing() => meleeing;

    /// <summary>
    /// ���� ������ 
    /// </summary>
    public override bool IsAiming() => aiming;

    /// <summary>
    /// Ŀ���� ����ִ���
    /// </summary>
    public override bool isCursorLocked() => cursorLocked;

    /// <summary>
    /// Ű���� �Է� �޾ƿ���
    /// </summary>
    public override Vector2 GetInputMovement() => axisMovement;

    /// <summary>
    /// ���콺 �Է� �޾ƿ���
    /// </summary>
    public override Vector2 GetInputLook() => axisLook;

    /// <summary>
    /// ����ź ������ ���� ��������
    /// </summary>
    public override AudioClip[] GetAudioClipsGrenadeThrow() => audioClipsGrenadeThrow;

    /// <summary>
    /// �������� ���� �������� 
    /// </summary>
    public override AudioClip[] GetAudioClipsMelee() => audioClipsMelee;

    /// <summary>
    /// �˻�������
    /// </summary>
    public override bool IsInspecting() => inspecting;

    /// <summary>
    /// �߻��ư�� ��� ������ �ִ���
    /// </summary>
    public override bool isHoldingButtonFire() => holdingButtonFire;

    #endregion

    #region METHODS

    /// <summary>
    /// �ִϸ����� ������Ʈ�ϱ�
    /// </summary>
    private void UpdateAnimator()
    {
        #region Reload Stop

        //���� ������������ üũ�Ѵ�.
        const string boolNameReloading = "Reloading";
        if(characterAnimator.GetBool(boolNameReloading))
        {
            //�������� �Ѿ��� �ϳ��� �ִٸ� ���� ������ �� �ֽ��ϴ�.
            if (equippedWeapon.GetAmmunitionTotal() - equippedWeapon.GetAmmunitionCurrent() < 1)
            {
                //ĳ���� �ִϸ����� ������Ʈ
                characterAnimator.SetBool(boolNameReloading, false);

                equippedWeapon.GetAnimator().SetBool(boolNameReloading, false);
            }

        }

        #endregion

        //ĳ���Ͱ� ����̴� �ִϸ��̼��� �󸶳� �����ؾ��ϴ��� 
        float leaningValue = Mathf.Clamp01(axisMovement.y);
       
        characterAnimator.SetFloat(AHashes.LeaningForward, leaningValue, 0.5f, Time.deltaTime);

        //ĳ���Ͱ� �̵��ϴ� �ִϸ��̼��� �󸶳� �����ؾ��ϴ��� 
        float movementValue = Mathf.Clamp01(Mathf.Abs(axisMovement.x) + Mathf.Abs(axisMovement.y));
        characterAnimator.SetFloat(AHashes.Movement, movementValue, dampTimeLocomotion, Time.deltaTime);

        //�����ϴ� ���ǵ�
        characterAnimator.SetFloat(AHashes.AimingSpeedMultiplier, aimingSpeedMultiplier);

        //ȸ���� ������� ����� ȸ�� �ִϸ��̼��� ���� �����մϴ�.
        characterAnimator.SetFloat(AHashes.Turning, Mathf.Abs(axisLook.x), dampTimeTurning, Time.deltaTime);

        //���� ������ 
        characterAnimator.SetFloat(AHashes.Horizontal, axisMovement.x, dampTimeLocomotion, Time.deltaTime);

        //���� ������
        characterAnimator.SetFloat(AHashes.Vertical, axisMovement.y, dampTimeLocomotion, Time.deltaTime);

        //���ذ��� ������ �̿��Ͽ� ������Ʈ
        characterAnimator.SetFloat(AHashes.AimingAlpha, Convert.ToSingle(aiming), dampTimeAiming, Time.deltaTime);

        //�̵� ��� �ӵ��� �����մϴ�.
        const string playRateLocomotionBool = "Play Rate Locomotion";
        characterAnimator.SetFloat(playRateLocomotionBool, movementBehaviour.IsGrounded() ? 1.0f : 0.0f, 0.2f, Time.deltaTime);

        #region Movement Play Rates

        //�̵� ������ ���� �ִϸ��̼��� ��� �ӵ��� ������ �� �ֽ��ϴ�.
        characterAnimator.SetFloat(AHashes.PlayRateLocomotionForward, movementBehaviour.GetMultiplierForward(), 0.2f, Time.deltaTime);
        characterAnimator.SetFloat(AHashes.PlayRateLocomotionSideways, movementBehaviour.GetMultiplierSideways(), 0.2f, Time.deltaTime);
        characterAnimator.SetFloat(AHashes.PlayRateLocomotionBackwards, movementBehaviour.GetMultiplierBackwards(), 0.2f, Time.deltaTime);

        #endregion

        //���� �ִϸ����� ������Ʈ
        characterAnimator.SetBool(AHashes.Aim, aiming);
        //�޸��� �ִϸ����� ������Ʈ
        characterAnimator.SetBool(AHashes.Running, running);
        //��ũ���� �ִϸ����� ������Ʈ
        characterAnimator.SetBool(AHashes.Crouching, movementBehaviour.IsCrouching());

    }

    /// <summary>
    /// �˻��ϱ�
    /// </summary>
    private void Inspect()
    {
        //����
        inspecting = true;
        //���
        characterAnimator.CrossFade("Inspect", 0.0f, layerActions, 0);
    }

    /// <summary>
    /// �߻��ϱ�
    /// </summary>
    private void Fire()
    {
        //�߻�Ǵ� �Ѿ��� ���� �ø��ϴ�.�ݵ��� �����ϹǷ� �ֽ� ���·� �����ؾ��մϴ�.
        shotsFired++;

        //�߻� �ð��� �����Ͽ� �߻� �ӵ��� �ùٸ��� ����� �� �ֽ��ϴ�.
        lastShotTime = Time.time;
        //�����ϴ� ��� �������� Ȯ�� ������ �����ؾ� �մϴ�.
        equippedWeapon.Fire(aiming ? equippedWeaponScope.GetMultiplierSpread() : 1.0f);

        //�߻� �ִϸ��̼� ���
        const string stateName = "Fire";
        characterAnimator.CrossFade(stateName, 0.05f, layerOverlay, 0);

        //ź���� �ִ� ��� ��Ʈ �׼� �ִϸ��̼��� ����մϴ�.
        if (equippedWeapon.IsBoltAction() && equippedWeapon.HasAmmunition())
            UpdateBolt(true);
        //�ʿ��� ��� ���⸦ �ڵ����� �������մϴ�. ��ź �߻�� �Ǵ� ���� �߻��� ���� �Ϳ� �ſ� �����մϴ�.
        if (!equippedWeapon.HasAmmunition() && equippedWeapon.GetAutomaticallyReloadOnEmpty())
            StartCoroutine(nameof(TryReloadAutomatic));
    }
    
    /// <summary>
    /// ������ �ִϸ��̼� ����ϱ�
    /// </summary>
    private void PlayReloadAnimation()
    {
        #region Animation

        string stateName = equippedWeapon.HasCycledReload() ? "Reload Open" : (equippedWeapon.HasAmmunition() ? "Reload" : "Reload Empty");
        //�÷���
        characterAnimator.Play(stateName, layerActions, 0.0f);

        #endregion

        characterAnimator.SetBool(AHashes.Reloading, reloading = true);

        equippedWeapon.Reload();
    }

    /// <summary>
    /// �ڵ� ������
    /// </summary>
    private IEnumerator TryReloadAutomatic()
    {
        //�ڵ� ������ �ð� ��ٸ���
        yield return new WaitForSeconds(equippedWeapon.GetAutomaticallyReloadOnEmptyDelay());

        //������
        PlayReloadAnimation();
    }

    /// <summary>
    /// ���� �����ϱ�
    /// </summary>
    /// <param name="index">������ �κ��丮 �ε���</param>
    private IEnumerator Equip(int index = 0)
    {
        //���� holster�� �ƴϸ� holster�� �մϴ�.���� �̹� �غ� �Ǿ� �ִٸ� ��ٸ� �ʿ䰡 �����ϴ�.
        if(!holstered)
        {
            SetHolstered(holstering = true);

            //��ٸ���
            yield return new WaitUntil(() => holstering == false);
        }
        //�̹� ���⸦ ��� �ִ� ����� ���⸦ ���� �ֽ��ϴ�.
        SetHolstered(false);

        characterAnimator.Play("Unholster", layerHolster, 0);

        //���ο� ��� ����
        inventory.Equip(index);

        //���� ��ħ
        RefreshWeaponSetup();
    }

    /// <summary>
    /// ���� ���ΰ�ħ�ϱ�
    /// </summary>
    private void RefreshWeaponSetup()
    {
        //������ ���Ⱑ ���ٸ� return
        if ((equippedWeapon = inventory.GetEquipped()) == null)
            return;
        //�ִϸ����� ��Ʈ�ѷ��� ������Ʈ�մϴ�.
        characterAnimator.runtimeAnimatorController = equippedWeapon.GetAnimatorController();

        //������ �Ŵ����� �����ɴϴ�
        weaponAttachmentManager = equippedWeapon.GetAttachmentManager();
        if (weaponAttachmentManager == null)
            return;
        //������������ �����ɴϴ�.
        equippedWeaponScope = weaponAttachmentManager.GetEquippedScope();

        //������ źâ������ �����ɴϴ�.
        equipeedWeaponMagazine = weaponAttachmentManager.GetEquippedMagazine();
    }

    /// <summary>
    /// �Ѿ��� ���µ� �߻��� ��
    /// </summary>
    private void FireEmpty()
    {
        //�߻縦 ������ ������ �߻�ӵ����� �� ���� �ʿ��մϴ�
        lastShotTime = Time.time;
        //����ϱ�
        characterAnimator.CrossFade("Fire Empty", 0.05f, layerOverlay, 0);
    }

    /// <summary>
    /// Ŀ�� ���¸� ������Ʈ�մϴ�.
    /// </summary>
    private void UpdateCursorState()
    {
        //Ŀ�� ���̴� ���¸� ������Ʈ �մϴ�.
        Cursor.visible = !cursorLocked;
        //Ŀ������ ������Ʈ �մϴ�.
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    /// <summary>
    /// ����ź ������
    /// </summary>
    private void PlayGrenadeThrow()
    {
        //���� ������Ʈ
        throwingGrenade = true;

        //�ִϸ��̼� ���
        characterAnimator.CrossFade("Grenade Throw", 0.15f, characterAnimator.GetLayerIndex("Layer Actions Arm Left"), 0.0f);
        characterAnimator.CrossFade("Grenade Throw", 0.05f, characterAnimator.GetLayerIndex("Layer Actions Arm Right"), 0.0f);

    }

    /// <summary>
    /// ���� ���� �ִϸ��̼� ���
    /// </summary>
    private void PlayMelee()
    {
        //���� ������Ʈ
        meleeing = true;
        
        //�ִϸ��̼� ���
        characterAnimator.CrossFade("Knife Attack", 0.05f, characterAnimator.GetLayerIndex("Layer Actions Arm Left"), 0.0f);
        characterAnimator.CrossFade("Knife Attack", 0.05f, characterAnimator.GetLayerIndex("Layer Actions arm Right"), 0.0f);

    }
    /// <summary>
    ///  ��Ʈ �ִϸ��̼� ���
    /// </summary>
    /// <param name="value"></param>
    private void UpdateBolt(bool value)
    {
        //���� ������Ʈ
        characterAnimator.SetBool(AHashes.Bolt, bolting = value);
    }

    /// <summary>
    /// ĳ������ �ִϸ����� ���� ���� Holstered ������Ʈ�ϱ�
    /// </summary>
    private void SetHolstered(bool value = true)
    {
        holstered = value;

        const string boolName = "Holstered";
        characterAnimator.SetBool(boolName, holstered);
    }
    #endregion

    #region ACTION CHECKS

    /// <summary>
    /// �߻��� �� �ִ���
    /// </summary>
    private bool CanPlayAnimationFire()
    {
        //���� �ֱ�, ���� �ִ� ��
        if (holstered || holstering)
            return false;

        //��������, ����ź
        if (meleeing || throwingGrenade)
            return false;

        //������, ����
        if (reloading || bolting)
            return false;

        //�˻�
        if (inspecting)
            return false;

        return true;
    }
    /// <summary>
    /// ������ �ִϸ��̼��� ����� �� �ִ��� 
    /// </summary>
    private bool CanPlayAnimationReload()
    {
        //������ ��
        if (reloading)
            return false;

        //���� ���� ��
        if (meleeing)
            return false;

        //����ź ������ ��
        if (throwingGrenade)
            return false;

        //�˻�������
        if (inspecting)
            return false;
        
        //źâ�� ������ ���� ��
        if (!equippedWeapon.CanReloadWhenFull() && equippedWeapon.IsFull())
            return false;

        return true;
    }

    /// <summary>
    /// ����ź ������ �ִϸ��̼��� ���� �� �ִ���
    /// </summary>
    private bool CanPlayAnimationGrenadeThrow()
    {
        //���⸦ ���� �־��ų�, ����ִ� ��
        if (holstered || holstering)
            return false;

        //���� ���� ���̰ų�, ����ź�� ������ ��
        if (meleeing || throwingGrenade)
            return false;

        //������ ���̰ų� ��Ʈ�׼����̸�
        if (reloading || bolting)
            return false;

        //�˻����̸�
        if (inspecting)
            return false;

        //���� ����ź�� ������ 0���̰ų� ������ �ƴ϶��
        if (!grenadesUnlimited && grenadeCount == 0)
            return false;

        return true;
    }
    /// <summary>
    /// ���� ������ �� �� �ִ���
    /// </summary>
    private bool CanPlayAnimationMelee()
    {
        //���⸦ ���� �־��ų�, ����ִ� ���̸�
        if (holstered || holstering)
            return false;

        //���� ���� ���̰ų� ����ź�� ������ ���̸�
        if (meleeing || throwingGrenade)
            return false;

        //������ ���̰ų� ��Ʈ�׼����̸�
        if (reloading || bolting)
            return false;

        //�˻����̸�
        if (inspecting)
            return false;

        return true;

    }

    /// <summary>
    /// ���⸦ ����ִ� �ִϸ��̼��� ����� �� �ִ���
    /// </summary>
    private bool CanPlayAnimationHolster()
    {
        //�����������̰ų� ����ź�� ������ ���̰ų�
        if (meleeing || throwingGrenade)
            return false;

        //���������̰ų� ��Ʈ�׼����̰ų�
        if (reloading || bolting)
            return false;

        //�˻����̰ų�
        if (inspecting)
            return false;
        
        return true;
    }
    
    /// <summary>
    /// ���⸦ �ٲ� �� �ִ��� 
    /// </summary>
    private bool CanChangeWeapon()
    {
        //���⸦ ����ִ� ���̸�
        if (holstering)
            return false;

        //���� ����������, ����ź�� ������ ������
        if (meleeing || throwingGrenade)
            return false;

        //������ ������, ��Ʈ�׼�������
        if (reloading || bolting)
            return false;

        //�˻�������
        if (inspecting)
            return false;

        return true;
    }
    /// <summary>
    /// �˻� �ִϸ��̼��� ����� �� �ִ���
    /// </summary>
    private bool CanPlayAnimationInspect()
    {
        //���⸦ ���� �־��ų� ����ִ� ���̸�
        if (holstered || holstering)
            return false;

        //�����������̰ų� ����ź�� ������ ���̸�
        if (meleeing || throwingGrenade)
            return false;
        
        //������ ���̰ų� ��Ʈ�׼����̸�
        if (reloading || bolting)
            return false;

        //�˻����̸�
        if (inspecting)
            return false;

        return true;
    }
    
    /// <summary>
    /// ������ �� �ִ���
    /// </summary>
    private bool CanAim()
    {
        //���⸦ ���� �־��ų� ,�˻����̸�
        if (holstered || inspecting)
            return false;

        //�����������̰ų� ����ź�� ������ ������
        if (meleeing || throwingGrenade)
            return false;

        //������ ���̰ų�, ���⸦ ����ִ� ���̸�
        if ((!equippedWeapon.CanReloadAimed() && reloading) || holstering)
            return false;

        return true;
    }

    /// <summary>
    /// �޸� �� �ִ���
    /// </summary>
    private bool CanRun()
    {
        //�˻����̰ų� ��Ʈ�׼����̸�
        if (inspecting || bolting)
            return false;

        //��ũ���� �ִ� ������
        if (movementBehaviour.IsCrouching())
            return false;

        //���� �������̰ų� ����ź�� ������ ������
        if (meleeing || throwingGrenade)
            return false;
        
        //������ ������ ����������
        if (reloading || aiming)
            return false;

        //�߻� ��ư�� ������ �ְ� �Ѿ��� ���� �ִٸ�
        if (holdingButtonFire && equippedWeapon.HasAmmunition())
            return false;
        
        //�ڷ� �޸��ų� ������ ������ �����̴� ���� �����մϴ�.
        if(axisMovement.y <= 0 || Math.Abs(Math.Abs(axisMovement.x)-1) < 0.01f)
        {
            return false;
        }

        return true;

    }

    #endregion

    #region INPUT

    //�߻��ϱ� Input
    public void OnTryFire(InputAction.CallbackContext context)
    {
        //Ŀ���� ������� �ʴٸ�
        if (!cursorLocked)
            return;

        switch(context)
        {
            //���� ���� �� ȣ��
            case { phase: InputActionPhase.Started }:
                holdingButtonFire = true;
                shotsFired = 0;
                break;
            //������ ����� ȣ��
            case { phase: InputActionPhase.Performed }:
                if (!CanPlayAnimationFire())
                    break;

                if (equippedWeapon.HasAmmunition())
                {

                    if (equippedWeapon.IsAutomatic())
                    {
                        //�߻�� ���� �������մϴ�. ź���� �ٴڳ��� �� �ݵ�/Ȯ���� �ִ�� �������� �ʵ��� �մϴ�.
                        shotsFired = 0;

                        break;
                    }
                    if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
                        Fire();
                }
                //�Ѿ��� ���� ���
                else
                    FireEmpty();
                break;
            //���� �����
            case { phase: InputActionPhase.Canceled }:
               
                holdingButtonFire = false;

                shotsFired = 0;
                break;
        }
    }

    /// <summary>
    /// �������ϱ�
    /// </summary>
    public void OnTryPlayReload(InputAction.CallbackContext context)
    {
        //Ŀ���� ������� �ʴٸ� ����
        if (!cursorLocked)
            return;

        //�������� �� ���ٸ� 
        if (!CanPlayAnimationReload())
            return;
        
        switch(context)
        {
            case { phase: InputActionPhase.Performed }:

                PlayReloadAnimation();
                break;
        }    

    }

    //�˻��ϱ�
    public void OnTryInspect(InputAction.CallbackContext context)
    {
        //Ŀ���� ������� �ʴٸ�
        if (!cursorLocked)
            return;

        if (!CanPlayAnimationInspect())
            return;

        switch(context)
        {
            case { phase: InputActionPhase.Performed }:
                Inspect();
                break;
        }
    }

    /// <summary>
    /// �����ϱ�, 
    /// </summary>
    public void OnTryAiming(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
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
    /// ���� ����ֱ�
    /// </summary>
    public void OnTryHolster(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
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
    /// ����ź ������ 
    /// </summary>
    public void OnTryThrowGrenade(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
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
    /// ���������ϱ�
    /// </summary>
    public void OnTryMelee(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        switch(context.phase)
        {
            case InputActionPhase.Performed:
                //���������� �� �� �ִٸ�
                if (CanPlayAnimationMelee())
                    //���� ���� ����
                    PlayMelee();
                break;
        }
    }

    /// <summary>
    /// �޸���
    /// </summary>
    public void OnTryRun(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        switch(context.phase)
        {
            
            case InputActionPhase.Performed:
                //���� ����� ����� ��� ����մϴ�.
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
    /// �����ϱ�
    /// </summary>
    public void OnTryJump(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        switch(context.phase)
        {
            case InputActionPhase.Performed:

                movementBehaviour.Jump();
                break;
        }
    }

    /// <summary>
    /// ���� �κ��丮 ����
    /// </summary>
    public void OnTryInventoryNext(InputAction.CallbackContext context)
    {
        if (!cursorLocked)
            return;

        if (inventory == null)
            return;

        switch(context.phase)
        {
            case InputActionPhase.Performed:
                //��ũ�� �� ������ ����Ͽ� �κ��丮�� ���� �ε��� ���� ������ �����ɴϴ�.
                float scrollValue = context.valueType.IsEquivalentTo(typeof(Vector2)) ? Mathf.Sign(context.ReadValue<Vector2>().y) : 1.0f;
                //scrollValue�� ����̸� ����index �����̸� ��index
                int indexNext = scrollValue > 0 ? inventory.GetNextIndex() : inventory.GetLastIndex();
                //���� ������ ���� �ε���
                int indexCurrent = inventory.GetEquippedIndex();
                //���⸦ �ٲܼ� �ְ�, ���� �ε����� ���� �ε����� ���� �ʴٸ�
                if (CanChangeWeapon() && (indexCurrent != indexNext))
                    StartCoroutine(nameof(Equip), indexNext);
                break;              

        }
    }

    /// <summary>
    /// Ŀ�� �ᱸ��
    /// </summary>
    public void OnLockCursor(InputAction.CallbackContext context)
    {
        switch(context.phase)
        {
            case InputActionPhase.Performed:
                //Ŀ�� ��� ���� ����մϴ�.
                cursorLocked = !cursorLocked;
                //Ŀ�� ���� ������Ʈ
                UpdateCursorState();
                break;
        }
    }

    /// <summary>
    /// �����̱�
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        //�Է� �� �޾ƿ���
        axisMovement = cursorLocked ? context.ReadValue<Vector2>() : default;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //���콺 �Է� �޾ƿ���
        axisLook = cursorLocked ? context.ReadValue<Vector2>() : default;

        if (equippedWeapon == null)
            return;

        if (equippedWeaponScope == null)
            return;
        //�����ϰ� �ִٸ� ������ ������ �������� ���콺 ���� ������ ���մϴ�.
        axisLook *= aiming ? equippedWeaponScope.GetMutiplierMouseSensitivity() : 1.0f;

    }

    #endregion

    #region ANIMATION EVENTS

    /// <summary>
    /// ź�� ����
    /// </summary>
    public override void EjectCasing()
    {
        if (equippedWeapon != null)
            equippedWeapon.EjectCasing();

    }

    /// <summary>
    /// ź�� ä���
    /// </summary>
    /// <param name="amount">ź�� ��</param>
    public override void FillAmmunition(int amount)
    {
        //�縸ŭ ź���� ä�쵵�� ���⿡ �˸��ϴ�.
        if (equippedWeapon != null)
            equippedWeapon.FillAmmunition(amount);
    }

    /// <summary>
    /// ����ź ������
    /// </summary>
    public override void Grenade()
    {
        if (grenadePrefab == null)
            return;

        if (cameraWorld == null)
            return;

        if (!grenadesUnlimited)
            grenadeCount--;

        //ī�޶��� ��ġ�� �����ɴϴ�.
        Transform cTransform = cameraWorld.transform;
        //������ ��ġ�� ����մϴ�.

        Vector3 position = cTransform.position;
        position += cTransform.forward * grenadeSpawnOffset;
        //������
        Instantiate(grenadePrefab, position, cTransform.rotation);
    }

    /// <summary>
    /// źâ Ȱ��ȭ��Ű�� 
    /// </summary>
    public override void SetActiveMagzine(int active)
    {
        //źâ Ȱ��ȭ�ϱ�
        equipeedWeaponMagazine.gameObject.SetActive(active != 0);
    }

    /// <summary>
    /// ��Ʈ �׼� ������
    /// </summary>
    public override void AnimationEndedBolt()
    {
        UpdateBolt(false);
    }

    /// <summary>
    /// ������ �ִϸ��̼� ������
    /// </summary>
    public override void AnimationEndedReload()
    {
        reloading = false;
    }

    /// <summary>
    /// ����ź ������ �ִϸ��̼� ����
    /// </summary>
    public override void AnimationEndedGrenadeThrow()
    {
        throwingGrenade = false;
    }

    /// <summary>
    /// ���� ���� �ִϸ��̼� ����
    /// </summary>
    public override void AnimationEndedMelee()
    {
        meleeing = false;
    }

    /// <summary>
    /// �˻� �ִϸ��̼� ����
    /// </summary>
    public override void AnimationEndedInspect()
    {
        inspecting = false;
    }

    /// <summary>
    /// ���� �ִ� �ִϸ��̼� ����
    /// </summary>
    public override void AnimationEndedHolster()
    {
        holstering = false;
    }

    /// <summary>
    /// �����̵��
    /// </summary>
    public override void SetSlideBack(int back)
    {
        if(equippedWeapon != null)
            equippedWeapon.SetSlideBack(back);
    }

    /// <summary>
    /// Į Ȱ��ȭ�ϱ�
    /// </summary>
    public override void SetActiveKnife(int active)
    {
        //Ȱ��ȭ�ϱ�
        knife.SetActive(active != 0);
    }


    #endregion
}