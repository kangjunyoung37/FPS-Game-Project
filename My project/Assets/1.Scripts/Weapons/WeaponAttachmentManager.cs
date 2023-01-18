using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class WeaponAttachmentManager : WeaponAttachmentManagerBehaviour
{
    #region FIELDS SERIALZED

    [Title(label: "Scope")]
    [Tooltip("무기 모델에 아이언사이트를 표시할지 여부를 결정합니다")]
    [SerializeField]
    private bool scopeDefaultShow = true;

    [Tooltip("기본 스코프")]
    [SerializeField]
    private ScopeBehaviour scopeDefaultBehaviour;

    [Tooltip("스코프 인덱스 만약에 음수라면 아이언사이트를 장착")]
    [SerializeField]
    private int scopeIndex = -1;


    [Tooltip("이 무기가 사용할 수 있는 스코프들")]
    [SerializeField]
    private ScopeBehaviour[] scopeArray;

    [Title(label: "Muzzle")]

    [Tooltip("선택된 총구 인덱스")]
    [SerializeField]
    private int muzzleIndex;

    [Tooltip("이 무기가 사용할 수 있는 모든 총구 부착물들")]
    [SerializeField]
    private MuzzleBehaviour[] muzzleArray;

    [Title(label: "Laser")]

    [Tooltip("선택된 레이저 인덱스")]
    [SerializeField]
    private int laserIndex = -1;

    [Tooltip("이 무기에 사용할 수 있는 레이저 배열")]
    [SerializeField]
    private LaserBehaviour[] laserArray;

    [Title(label: "Grip")]

    [Tooltip("선택된 그립")]
    [SerializeField]
    private int gripIndex = -1;

    [Tooltip("이 무기에 사용할 수 있는 그립 배열")]
    [SerializeField]
    private GripBehaviour[] gripArray;

    [Title(label: "Magazine")]

    [Tooltip("선택된 탄창 인텍스")]
    [SerializeField]
    private int magazineIndex;

    [Tooltip("이 무기에 사용할 수 있는 탄창 배열")]
    [SerializeField]
    private Magazine[] magazineArray;

    [Title(label: "External Attachment")]
    [SerializeField]
    private Renderer[] externalAttachment;

    #endregion
    #region FIELDS

    /// <summary>
    /// 장착된 스코프
    /// </summary>
    private ScopeBehaviour scopeBehaviour ;

    /// <summary>
    /// 장착된 총구
    /// </summary>
    private MuzzleBehaviour muzzleBehaviour;

    /// <summary>
    /// 장착된 레이저
    /// </summary>
    private LaserBehaviour laserBehaviour;

    /// <summary>
    /// 장착된 그립
    /// </summary>
    private GripBehaviour gripBehaviour;

    /// <summary>
    /// 장착된 탄창
    /// </summary>
    private MagazineBehaviour magazineBehaviour;

    /// <summary>
    /// CharacterBehaviour
    /// </summary>
    private CharacterBehaviour characterBehaviour;

    /// <summary>
    /// 부착물이 장착될 무기
    /// </summary>
    private Weapon weapon;

    private HashTable playerHashTable;

    private PhotonView PV;

    private int PVScopeIndex = -1;
    private int PVLaserIndex = -1;
    private int PVGripIndex = -1;
    private int PVMuzzleIndex = 0;

    //주무기인지
    private bool isMainWeapon;

    /// <summary>
    /// 데이터를 받았는지 
    /// </summary>
    private bool isreceive = false;

    #endregion

    #region UNITY FUNCTIONS

    protected override void Awake()
    {
        //캐싱
        characterBehaviour = transform.root.GetComponent<CharacterBehaviour>();
        PV = characterBehaviour.GetPhotonView();
        playerHashTable = PV.Owner.CustomProperties;
        weapon = transform.GetComponent<Weapon>();
        
        if(weapon.weaponType != WeaponType.HG)
        {
            isMainWeapon = true;
        }

        if(isMainWeapon)
        {
            scopeIndex = (int)playerHashTable["MainScope"];
            gripIndex = (int)playerHashTable["MainGrip"];
            laserIndex = (int)playerHashTable["MainLaser"];
            muzzleIndex = (int)playerHashTable["MainMuzzle"] + 1;
        }

        else
        {
            scopeIndex = (int)playerHashTable["SubScope"];
            laserIndex = (int)playerHashTable["SubLaser"];
            muzzleIndex = (int)playerHashTable["SubMuzzle"] + 1;
        }

  

        //스코프 선택
        scopeBehaviour = scopeArray.SelectAndSetActive(scopeIndex);

        //스코프가 없다면
        if(scopeBehaviour == null)
        {
            scopeBehaviour = scopeDefaultBehaviour;
            scopeBehaviour.gameObject.SetActive(scopeDefaultShow);
        }

        //총구가 없다면
        muzzleBehaviour = muzzleArray.SelectAndSetActive(muzzleIndex);

        //레이저 선택
        laserBehaviour = laserArray.SelectAndSetActive(laserIndex);

        //그립 선택
        gripBehaviour = gripArray.SelectAndSetActive(gripIndex);

        //탄창 선택
        magazineBehaviour = magazineArray.SelectAndSetActive(magazineIndex);

    }

    #endregion

    #region GETTERS

    public override ScopeBehaviour GetEquippedScope() => scopeBehaviour;
    public override ScopeBehaviour GetEquippedScopeDefault() => scopeDefaultBehaviour;
    public override MagazineBehaviour GetEquippedMagazine() => magazineBehaviour;
    public override MuzzleBehaviour GetEquippedMuzzle() => muzzleBehaviour;
    public override LaserBehaviour GetEquippedLaser() => laserBehaviour;
    public override GripBehaviour GetEquippedGrip() => gripBehaviour;
    
    public override int GetEquippedMuzzleIndex() => muzzleIndex;
    public override int GetEquippedLaserIndex() => laserIndex;
    public override int GetEquippedGripIndex() => gripIndex;
    public override int GetEquippedScopeIndex() => scopeIndex;

    public override int GetEquippedMuzzlePVIndex() => PVMuzzleIndex;
    public override int GetEquippedLaserPVIndex() => PVLaserIndex;
    public override int GetEquippedGripPVIndex() => PVGripIndex;
    public override int GetEquippedScopePVIndex() => PVScopeIndex;


    public override bool Getreceive() => isreceive;
    
    #endregion

    #region METHODS

    public override void FPGripsOff()
    {
        for(int i = 0; i < gripArray.Length; i++)
        {
            gripArray[i].FPRenderOff();
        }
    }

    public override void FPScopesOff()
    {
        for(int i = 0; i < scopeArray.Length;i++)
        {
            scopeArray[i].FPScopeRenOff();
            scopeArray[i].RenDisable();
        }
        scopeDefaultBehaviour.FPScopeRenOff();
    }

    public override void FPMuzzlesOff()
    {
        for(int i = 0; i < muzzleArray.Length; i++)
        {
            muzzleArray[i].FPMuzzleOff();
        }
    }

    public override void FPLasersOff()
    {
        for(int i = 0; i < laserArray.Length; i++)
        {
            laserArray[i].FPLaserOff();
        }
        
    }

    public override void FPMagazinesOff()
    {
        for(int i = 0; i < magazineArray.Length; i++)
        {
            magazineArray[i].FPMagazineOff();

        }
    }

    public override void FPexternalAttachmentOff()
    {
        if (externalAttachment.Length == 0)
            return;
        for (int i = 0; i < externalAttachment.Length; i++)
            externalAttachment[i].enabled = false;
    }

    public override void OnPhotonSerializeView(PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(scopeIndex);
            stream.SendNext(laserIndex);
            stream.SendNext(muzzleIndex);
            stream.SendNext(gripIndex);
            stream.SendNext(magazineIndex);
            stream.SendNext(true);
        }
        else
        {

            PVScopeIndex = (int)stream.ReceiveNext();
            PVLaserIndex = (int)stream.ReceiveNext();
            PVMuzzleIndex = (int)stream.ReceiveNext();
            PVGripIndex = (int)stream.ReceiveNext();
            magazineIndex = (int)stream.ReceiveNext();
            isreceive = (bool)stream.ReceiveNext();
      
        }
    }
    #endregion
}

