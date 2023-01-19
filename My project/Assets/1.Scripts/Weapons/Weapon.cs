using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : WeaponBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "Settings")]
    
    [Tooltip("무기명, 픽업용으로 사용할 예정")]
    [SerializeField]
    private string weaponName;

    [Tooltip("무기 종류")]
    [SerializeField]
    public WeaponType weaponType = WeaponType.AR;

    [Tooltip("샷건인지")]
    [SerializeField]
    private bool isShotGun = false;

    [Tooltip("무기의 데미지")]
    [SerializeField]
    private int damage;

    [Tooltip("이 무기를 사용할 때 이 캐릭터의 이동 속도가 몇 배 증가하는지")]
    [SerializeField]
    private float multiplierMovementSpeed = 1.0f;

    [Title(label: "Firing")]

    [Tooltip("이 무기가 발사모드가 자동이면 발사버튼을 계속 누르고 있으면 계속 발사가 됩니다")]
    [SerializeField]
    private bool automatic;

    [Tooltip("볼트액션 무기이면 볼트 액션 애니메이션이 재생됩니다")]
    [SerializeField]
    private bool boltAction;

    [Tooltip("한 번에 발사되는 총알의 수, 산탄총과 같은 여러발의 발사체가 나가는 경우 쓰입니다")]
    [SerializeField]
    private int shotCount = 1;

    [Tooltip("화면 중앙에서 무기를 발사할 수 있는 거리")]
    [SerializeField]
    private float spread = 0.25f;

    [Tooltip("발사체의 속도")]
    [SerializeField]
    private float projectileImpulse = 400.0f;

    [Tooltip("이 무기가 1분에 쏠 수 있는 총알의 수, 무기의 발사 속도를 결정합니다")]
    [SerializeField]
    private int roundsPerMinutes = 200;

    [Title(label: "Reloading")]

    [Tooltip(" 한 번에 하나의 총알을 삽입할지 여부를 의미합니다.")]
    [SerializeField]
    private bool cycledReload;

    [Tooltip("이 무기에 탄약이 가득 차 있을 때 재장전할 수 있는지")]
    [SerializeField]
    private bool canReloadWhenFull = true;

    [Tooltip("마지막 발사 후 자동으로 재장전이 되는지")]
    [SerializeField]
    private bool automaticReloadOnEmpty;

    [Tooltip("재장전이 자동으로 시작되는 마지막 샷 이후의 시간입니다.")]
    [SerializeField]
    private float automaticReloadOnEmptyDelay = 0.25f;

    [Title(label: "Animation")]

    [Tooltip("탄피가 배출되는 곳을 의미합니다")]
    [SerializeField]
    private Transform socketEjection;

    [Tooltip("조준하는 동안 재장전을 할 수 있는지")]
    [SerializeField]
    private bool canReloadAimed = true;

    [Title(label: "Resources")]

    [Tooltip("탄피 Prefab")]
    [SerializeField]
    private GameObject prefabCasing;

    [Tooltip("발사체 Prefab")]
    [SerializeField]
    private GameObject prefabProjectile;

    [Tooltip("플레이어 캐릭터가 무기를 사용할 때 사용해야하는 애니메이터 컨트롤러")]
    [SerializeField]
    public RuntimeAnimatorController controller;

    [Tooltip("무기 본체 텍스쳐")]
    [SerializeField]
    private Sprite spriteBody;

    [Title(label: "Audio Clips Holster")]

    [Tooltip("총을 집어넣는 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipHolster;

    [Tooltip("총을 빼는 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipUnHolster;

    [Title(label: "Audio Clips Reloads")]

    [Tooltip("재장전 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipReload;

    [Tooltip("빈 재장전 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipReloadEmpty;

    [Title(label: "오디오 클립 사이클")]

    [Tooltip("재장전 오픈 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipReloadOpen;

    [Tooltip("재장전 삽입 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipReloadInsert;

    [Tooltip("재장전 닫힘 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipReloadClose;

    [Title(label: "Audio Clips Other")]

    [Tooltip("총알이 없는데 발사하려고 할 때 나는 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipFireEmpty;

    [Tooltip("볼트 액션 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipBoltAction;

    [Title(label:"Weapon Renderer")]

    [Tooltip("무기 Renderer")]
    [SerializeField]
    private Renderer WeaponRenderer;

    #endregion

    #region FIELDS

    /// <summary>
    /// 무기 애니메이터
    /// </summary>
    private Animator animator;

    /// <summary>
    /// 부착물 매니저
    /// </summary>
    private WeaponAttachmentManagerBehaviour attachMentManager;
    
    /// <summary>
    /// 남은 탄약량
    /// </summary>
    private int ammunitionCurrent;

    /// <summary>
    /// 총 탄약량
    /// </summary>
    private int ammunitionTotal;

    #region Attachment Behaviours

    /// <summary>
    /// 장착된 스코프 참조
    /// </summary>
    private ScopeBehaviour scopeBehaviour;

    /// <summary>
    /// 장착된 탄창 참조
    /// </summary>
    private MagazineBehaviour magazineBehaviour;

    /// <summary>
    /// 장착된 총구 참조
    /// </summary>
    private MuzzleBehaviour muzzleBehaviour;

    /// <summary>
    /// 장착된 레이저 참조
    /// </summary>
    private LaserBehaviour laserBehaviour;

    /// <summary>
    /// 장착된 그립 참조
    /// </summary>
    private GripBehaviour gripBehaviour;



    #endregion


    private IGameModeService gameModeService;

    /// <summary>
    /// 메인 플레이어 행동 컴포넌트
    /// </summary>
    private CharacterBehaviour characterBehaviour;

    /// <summary>
    /// 플레이어 카메라
    /// </summary>
    private Transform playerCamera;

    /// <summary>
    /// 플레이어의 PhotonView
    /// </summary>
    private PhotonView PV;

    /// <summary>
    /// TPRenderer 컨트롤러
    /// </summary>
    private TPRenController tPRenController;

    /// <summary>
    /// 자기 자신의 collider
    /// </summary>
    private Collider[] colliders;

    /// <summary>
    /// 무기의 순서
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
    /// 스코프에 따른 에임FOV값 가져오기
    /// </summary>
    public override float GetFieldOfViewMutiplierAim()
    {
        if (scopeBehaviour != null)
            return scopeBehaviour.GetFieldOfViewMutiplierAim();

        //Debug.LogError("Weapon has no scope equipped");
        return 1.0f;
    }

    /// <summary>
    /// 스코프에 따른 무기에임 FOV값 가져오기
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
    /// 재장전
    /// </summary>
    public override void Reload()
    {
        
        const string boolName = "Reloading";
        animator.SetBool(boolName, true);
        
        //재장전 사운드 클립 재생
        ServiceLocator.Current.Get<IAudioManagerService>().PlayOneShot(HasAmmunition() ? audioClipReload : audioClipReloadEmpty, new AudioSettings(1.0f, 0.0f, true));
        
        //재장전 애니메이션 재생
        animator.Play(cycledReload ? "Reload Open" : (HasAmmunition() ? "Reload" : "Reload Empty"), 0, 0.0f);

    }

    /// <summary>
    /// 발사
    /// </summary>
    public override void Fire(float spreadMutiplier = 1)
    {
        //발사를 하기 위해서는 총구가 필요합니다.
        if (muzzleBehaviour == null)
            return;
        //카메라가 캐시되었는지 확인해야합니다. 그렇지 않으면 실제로 trace를 실행할 수 없습니다.
        if (playerCamera == null)
            return;

        const string stateName = "Fire";
        animator.Play(stateName, 0, 0.0f);
        //현재 탄약 하나 줄이기
        ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, magazineBehaviour.GetAmmunitionTotal());

        if (ammunitionCurrent == 0)
        {
            SetSlideBack(1);
        }
        muzzleBehaviour.Effect();

        //많은 발사체를 발사해야하는 경우
        for(var i = 0; i < shotCount; i++)
        {            
            
            //무작위 스프레드 값 
            Vector3 spreadValue = Random.insideUnitSphere * ( isShotGun ? spread : spread * spreadMutiplier);
            spreadValue.z = 0;
            //월드 좌표로 변환하기
            spreadValue = playerCamera.TransformDirection(spreadValue);
            //발사체 소환
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
            //무작위 스프레드 값 
            Vector3 spreadValue = Random.insideUnitSphere * (isShotGun ? spread : spread * spreadMutiplier);

            spreadValue.z = 0;
            //월드 좌표로 변환하기
            spreadValue = playerCamera.TransformDirection(spreadValue);
            //발사체 소환
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

    //탄약 채우기
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

    //탄약이 없을 경우 
    public override void SetSlideBack(int back)
    {
        const string boolName = "Slide Back";
        animator.SetBool(boolName, back != 0);
    } 

    //탄피 생성
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
