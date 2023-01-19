using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : WeaponBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "Settings")]
    
    [Tooltip("�����, �Ⱦ������� ����� ����")]
    [SerializeField]
    private string weaponName;

    [Tooltip("���� ����")]
    [SerializeField]
    public WeaponType weaponType = WeaponType.AR;

    [Tooltip("��������")]
    [SerializeField]
    private bool isShotGun = false;

    [Tooltip("������ ������")]
    [SerializeField]
    private int damage;

    [Tooltip("�� ���⸦ ����� �� �� ĳ������ �̵� �ӵ��� �� �� �����ϴ���")]
    [SerializeField]
    private float multiplierMovementSpeed = 1.0f;

    [Title(label: "Firing")]

    [Tooltip("�� ���Ⱑ �߻��尡 �ڵ��̸� �߻��ư�� ��� ������ ������ ��� �߻簡 �˴ϴ�")]
    [SerializeField]
    private bool automatic;

    [Tooltip("��Ʈ�׼� �����̸� ��Ʈ �׼� �ִϸ��̼��� ����˴ϴ�")]
    [SerializeField]
    private bool boltAction;

    [Tooltip("�� ���� �߻�Ǵ� �Ѿ��� ��, ��ź�Ѱ� ���� �������� �߻�ü�� ������ ��� ���Դϴ�")]
    [SerializeField]
    private int shotCount = 1;

    [Tooltip("ȭ�� �߾ӿ��� ���⸦ �߻��� �� �ִ� �Ÿ�")]
    [SerializeField]
    private float spread = 0.25f;

    [Tooltip("�߻�ü�� �ӵ�")]
    [SerializeField]
    private float projectileImpulse = 400.0f;

    [Tooltip("�� ���Ⱑ 1�п� �� �� �ִ� �Ѿ��� ��, ������ �߻� �ӵ��� �����մϴ�")]
    [SerializeField]
    private int roundsPerMinutes = 200;

    [Title(label: "Reloading")]

    [Tooltip(" �� ���� �ϳ��� �Ѿ��� �������� ���θ� �ǹ��մϴ�.")]
    [SerializeField]
    private bool cycledReload;

    [Tooltip("�� ���⿡ ź���� ���� �� ���� �� �������� �� �ִ���")]
    [SerializeField]
    private bool canReloadWhenFull = true;

    [Tooltip("������ �߻� �� �ڵ����� �������� �Ǵ���")]
    [SerializeField]
    private bool automaticReloadOnEmpty;

    [Tooltip("�������� �ڵ����� ���۵Ǵ� ������ �� ������ �ð��Դϴ�.")]
    [SerializeField]
    private float automaticReloadOnEmptyDelay = 0.25f;

    [Title(label: "Animation")]

    [Tooltip("ź�ǰ� ����Ǵ� ���� �ǹ��մϴ�")]
    [SerializeField]
    private Transform socketEjection;

    [Tooltip("�����ϴ� ���� �������� �� �� �ִ���")]
    [SerializeField]
    private bool canReloadAimed = true;

    [Title(label: "Resources")]

    [Tooltip("ź�� Prefab")]
    [SerializeField]
    private GameObject prefabCasing;

    [Tooltip("�߻�ü Prefab")]
    [SerializeField]
    private GameObject prefabProjectile;

    [Tooltip("�÷��̾� ĳ���Ͱ� ���⸦ ����� �� ����ؾ��ϴ� �ִϸ����� ��Ʈ�ѷ�")]
    [SerializeField]
    public RuntimeAnimatorController controller;

    [Tooltip("���� ��ü �ؽ���")]
    [SerializeField]
    private Sprite spriteBody;

    [Title(label: "Audio Clips Holster")]

    [Tooltip("���� ����ִ� ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipHolster;

    [Tooltip("���� ���� ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipUnHolster;

    [Title(label: "Audio Clips Reloads")]

    [Tooltip("������ ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipReload;

    [Tooltip("�� ������ ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipReloadEmpty;

    [Title(label: "����� Ŭ�� ����Ŭ")]

    [Tooltip("������ ���� ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipReloadOpen;

    [Tooltip("������ ���� ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipReloadInsert;

    [Tooltip("������ ���� ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipReloadClose;

    [Title(label: "Audio Clips Other")]

    [Tooltip("�Ѿ��� ���µ� �߻��Ϸ��� �� �� ���� ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipFireEmpty;

    [Tooltip("��Ʈ �׼� ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipBoltAction;

    [Title(label:"Weapon Renderer")]

    [Tooltip("���� Renderer")]
    [SerializeField]
    private Renderer WeaponRenderer;

    #endregion

    #region FIELDS

    /// <summary>
    /// ���� �ִϸ�����
    /// </summary>
    private Animator animator;

    /// <summary>
    /// ������ �Ŵ���
    /// </summary>
    private WeaponAttachmentManagerBehaviour attachMentManager;
    
    /// <summary>
    /// ���� ź�෮
    /// </summary>
    private int ammunitionCurrent;

    /// <summary>
    /// �� ź�෮
    /// </summary>
    private int ammunitionTotal;

    #region Attachment Behaviours

    /// <summary>
    /// ������ ������ ����
    /// </summary>
    private ScopeBehaviour scopeBehaviour;

    /// <summary>
    /// ������ źâ ����
    /// </summary>
    private MagazineBehaviour magazineBehaviour;

    /// <summary>
    /// ������ �ѱ� ����
    /// </summary>
    private MuzzleBehaviour muzzleBehaviour;

    /// <summary>
    /// ������ ������ ����
    /// </summary>
    private LaserBehaviour laserBehaviour;

    /// <summary>
    /// ������ �׸� ����
    /// </summary>
    private GripBehaviour gripBehaviour;



    #endregion


    private IGameModeService gameModeService;

    /// <summary>
    /// ���� �÷��̾� �ൿ ������Ʈ
    /// </summary>
    private CharacterBehaviour characterBehaviour;

    /// <summary>
    /// �÷��̾� ī�޶�
    /// </summary>
    private Transform playerCamera;

    /// <summary>
    /// �÷��̾��� PhotonView
    /// </summary>
    private PhotonView PV;

    /// <summary>
    /// TPRenderer ��Ʈ�ѷ�
    /// </summary>
    private TPRenController tPRenController;

    /// <summary>
    /// �ڱ� �ڽ��� collider
    /// </summary>
    private Collider[] colliders;

    /// <summary>
    /// ������ ����
    /// </summary>
    private int index;

    #endregion

    #region UNITY


    protected override void Awake()
    {
      
        animator = GetComponent<Animator>();
        attachMentManager = GetComponent<WeaponAttachmentManagerBehaviour>();
        gameModeService = ServiceLocator.Current.Get<IGameModeService>();
        characterBehaviour = transform.parent.GetComponent<Inventory>().GetCharacterBehaviour();
        PV = characterBehaviour.GetPhotonView();
        tPRenController = characterBehaviour.GetTPRenController();
        playerCamera = characterBehaviour.GetCameraWold().transform;
        index = transform.GetSiblingIndex();
    }

    protected override void Start()
    {
        colliders = tPRenController.GetColliders();
        scopeBehaviour = attachMentManager.GetEquippedScope();
        magazineBehaviour = attachMentManager.GetEquippedMagazine();
        muzzleBehaviour = attachMentManager.GetEquippedMuzzle();
        laserBehaviour = attachMentManager.GetEquippedLaser();
        gripBehaviour = attachMentManager.GetEquippedGrip();
        ammunitionCurrent = magazineBehaviour.GetAmmunitionTotal();
        ammunitionTotal = magazineBehaviour.GetAmmunitionTotal() * 3;

    }
    
    #endregion

    #region GETTERS

    /// <summary>
    /// �������� ���� ����FOV�� ��������
    /// </summary>
    public override float GetFieldOfViewMutiplierAim()
    {
        if (scopeBehaviour != null)
            return scopeBehaviour.GetFieldOfViewMutiplierAim();

        //Debug.LogError("Weapon has no scope equipped");
        return 1.0f;
    }

    /// <summary>
    /// �������� ���� ���⿡�� FOV�� ��������
    /// </summary>
    /// <returns></returns>
    public override float GetFieldOfViewMutiplierAimWeapon()
    {
        if (scopeBehaviour != null)
            return scopeBehaviour.GetFieldOfViewMutiplierAimWeapon();

        //Debug.LogError("Weapon has no scope equipped");

        return 1.0f;
    }

    public override Animator GetAnimator() => animator;

    public override bool CanReloadAimed() => canReloadAimed;

    public override Sprite GetSpriteBody() => spriteBody;

    public override float GetMultipleierMovementSpeed() => multiplierMovementSpeed;
    
    public override AudioClip GetAudioClipHolster() => audioClipHolster;
    
    public override AudioClip GetAudioClipUnHolster() => audioClipUnHolster;
    
    public override AudioClip GetAudioClipReload() => audioClipReload;
    
    public override AudioClip GetAudioClipReloadEmpty() => audioClipReloadEmpty;
    
    public override AudioClip GetAudioClipReloadOpen() => audioClipReloadOpen;
    
    public override AudioClip GetAudioClipReloadInsert() => audioClipReloadInsert;
    
    public override AudioClip GetAudioClipReloadClose() => audioClipReloadClose;
    
    public override AudioClip GetAudioClipFireEmpty() => audioClipFireEmpty;
    
    public override AudioClip GetAudioClipBoltAction() => audioClipBoltAction;
    
    public override AudioClip GetAudioClipFire() => muzzleBehaviour.GetAudioClipFire();

    public override int GetAmmunitionWeaponTotal() => ammunitionTotal;

    public override int GetAmmunitionCurrent() => ammunitionCurrent;

    public override int GetAmmunitionTotal() => magazineBehaviour.GetAmmunitionTotal();

    public override bool HasCycledReload() => cycledReload;

    public override bool IsAutomatic() => automatic;

    public override bool IsBoltAction() => boltAction;

    public override bool GetAutomaticallyReloadOnEmpty() => automaticReloadOnEmpty;

    public override float GetAutomaticallyReloadOnEmptyDelay() => automaticReloadOnEmptyDelay;

    public override bool CanReloadWhenFull() => canReloadWhenFull;

    public override float GetRateOfFire() => roundsPerMinutes;

    public override bool IsFull() => ammunitionCurrent == magazineBehaviour.GetAmmunitionTotal();

    public override bool HasAmmunition() => ammunitionCurrent > 0;

    public override RuntimeAnimatorController GetAnimatorController() => controller;

    public override WeaponAttachmentManagerBehaviour GetAttachmentManager() => attachMentManager;

    public override float ProjectileSpeed() => projectileImpulse;

    #endregion

    #region METHODS

    /// <summary>
    /// ������
    /// </summary>
    public override void Reload()
    {
        
        const string boolName = "Reloading";
        animator.SetBool(boolName, true);
        
        //������ ���� Ŭ�� ���
        ServiceLocator.Current.Get<IAudioManagerService>().PlayOneShot(HasAmmunition() ? audioClipReload : audioClipReloadEmpty, new AudioSettings(1.0f, 0.0f, true));
        
        //������ �ִϸ��̼� ���
        animator.Play(cycledReload ? "Reload Open" : (HasAmmunition() ? "Reload" : "Reload Empty"), 0, 0.0f);

    }

    /// <summary>
    /// �߻�
    /// </summary>
    public override void Fire(float spreadMutiplier = 1)
    {
        //�߻縦 �ϱ� ���ؼ��� �ѱ��� �ʿ��մϴ�.
        if (muzzleBehaviour == null)
            return;
        //ī�޶� ĳ�õǾ����� Ȯ���ؾ��մϴ�. �׷��� ������ ������ trace�� ������ �� �����ϴ�.
        if (playerCamera == null)
            return;

        const string stateName = "Fire";
        animator.Play(stateName, 0, 0.0f);
        //���� ź�� �ϳ� ���̱�
        ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, magazineBehaviour.GetAmmunitionTotal());

        if (ammunitionCurrent == 0)
        {
            SetSlideBack(1);
        }
        muzzleBehaviour.Effect();

        //���� �߻�ü�� �߻��ؾ��ϴ� ���
        for(var i = 0; i < shotCount; i++)
        {            
            
            //������ �������� �� 
            Vector3 spreadValue = Random.insideUnitSphere * ( isShotGun ? spread : spread * spreadMutiplier);
            spreadValue.z = 0;
            //���� ��ǥ�� ��ȯ�ϱ�
            spreadValue = playerCamera.TransformDirection(spreadValue);
            //�߻�ü ��ȯ
            GameObject projectile = InGame.Instance.ActivatePoolItem();
            projectile.transform.position = playerCamera.position + playerCamera.forward * 0.3f;
            projectile.transform.localRotation = Quaternion.Euler(playerCamera.eulerAngles + spreadValue);
            Physics.IgnoreCollision(characterBehaviour.transform.GetComponent<Collider>(), projectile.GetComponent<Collider>(), true);
            if (colliders == null)
                return;
            foreach (Collider col in colliders)
            {
                Physics.IgnoreCollision(col, projectile.GetComponent<Collider>(), true);
            }

            projectile.GetComponent<Projectile>().Setup(characterBehaviour, damage, index);
            projectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
            projectile.SetActive(true);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;


        }
    }

    public override void InstateProjectile(float spreadMutiplier = 1)
    {
        for (var i = 0; i < shotCount; i++)
        {
            //������ �������� �� 
            Vector3 spreadValue = Random.insideUnitSphere * (isShotGun ? spread : spread * spreadMutiplier);

            spreadValue.z = 0;
            //���� ��ǥ�� ��ȯ�ϱ�
            spreadValue = playerCamera.TransformDirection(spreadValue);
            //�߻�ü ��ȯ
            GameObject projectile = InGame.Instance.ActivatePoolItem();
            projectile.transform.position = playerCamera.position;
            projectile.transform.localRotation = Quaternion.Euler(playerCamera.eulerAngles + spreadValue);
            Physics.IgnoreCollision(characterBehaviour.transform.GetComponent<Collider>(), projectile.GetComponent<Collider>(), true);
            if (colliders == null)
                return;
            foreach (Collider col in colliders)
            {
                Physics.IgnoreCollision(col, projectile.GetComponent<Collider>(), true);
            }

            projectile.GetComponent<Projectile>().Setup(characterBehaviour, damage, index);
            projectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
            projectile.SetActive(true);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;

        }
    }

    //private Vector3 CalculateDirection()
    //{
    //    Ray ray = new Ray();
    //    RaycastHit[] raycastHit;
    //    Vector3 targetPoint = Vector3.zero;

    //    ray.origin = playerCamera.position;
    //    ray.direction = playerCamera.forward;
    //    raycastHit = Physics.RaycastAll(playerCamera.position, playerCamera.forward, 300);
        
    //    for(int i = 0; i < raycastHit.Length; i++)
    //    {
    //        RaycastHit hit = raycastHit[i];
    //        if (hit.collider.transform.root == characterBehaviour.transform)
    //            continue;
    //        else
    //        {
    //            targetPoint = hit.point;
    //            Debug.DrawRay(playerCamera.position, playerCamera.forward, Color.blue, 3.0f);
    //            break;
    //        }
        
    //    }
    //    if(targetPoint == Vector3.zero)
    //        targetPoint = playerCamera.position + playerCamera.forward * 300;

    //    Vector3 test = muzzleBehaviour.GetSocket().position - targetPoint;
    //    Debug.Log(test.normalized + " " + test.magnitude);

    //    Debug.DrawRay(muzzleBehaviour.GetSocket().position, (targetPoint - muzzleBehaviour.GetSocket().position).normalized, Color.red, 5f);
    //    return (targetPoint - muzzleBehaviour.GetSocket().position).normalized;

    //}

    //ź�� ä���
    public override void FillAmmunition(int amount)
    {   if (ammunitionTotal == 0)
            return;

        if(amount == 0)
        {
            int ammoToFill = magazineBehaviour.GetAmmunitionTotal() - ammunitionCurrent;
            if(ammoToFill >= ammunitionTotal)
            {
                ammunitionCurrent += ammunitionTotal; 
                ammunitionTotal = 0;
            }
            else
            {
                ammunitionCurrent = magazineBehaviour.GetAmmunitionTotal();
                ammunitionTotal -= ammoToFill;
            }
        }
        else
        {
            ammunitionCurrent = Mathf.Clamp(ammunitionCurrent+ amount,0,GetAmmunitionTotal());
            ammunitionTotal -= amount;
        }
            
    }

    //ź���� ���� ��� 
    public override void SetSlideBack(int back)
    {
        const string boolName = "Slide Back";
        animator.SetBool(boolName, back != 0);
    } 

    //ź�� ����
    public override void EjectCasing()
    {        

        if(prefabCasing != null && socketEjection != null)
            Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
    }

    public override void FPWPOff()
    {
        attachMentManager.FPGripsOff();
        attachMentManager.FPScopesOff();
        attachMentManager.FPMuzzlesOff();
        attachMentManager.FPLasersOff();
        attachMentManager.FPMagazinesOff();
        attachMentManager.FPexternalAttachmentOff();
        WeaponRenderer.enabled = false;
        
    }


    public override void OnPhotonSerializeView(PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ammunitionCurrent);
        }
        else
        {
            ammunitionCurrent = (int)stream.ReceiveNext();
  
        }
    }



    #endregion
}
