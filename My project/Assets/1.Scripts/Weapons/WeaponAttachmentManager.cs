using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Tooltip("랜덤 스코프를 사용할 때 첫번째 스코프 인텍스")]
    [SerializeField]
    private int scopeIndexFirst = -1;

    [Tooltip("게임이 시작되면 랜덤 스코프를 사용할 것인지")]
    [SerializeField]
    private bool scopeIndexRandom;

    [Tooltip("이 무기가 사용할 수 있는 스코프들")]
    [SerializeField]
    private ScopeBehaviour[] scopeArray;

    [Title(label: "Muzzle")]

    [Tooltip("선택된 총구 인덱스")]
    [SerializeField]
    private int muzzleIndex;

    [Tooltip("게임이 시작되면 랜덤 총구를 사용할 것인지")]
    [SerializeField]
    private bool muzzleIndexRandom = true;

    [Tooltip("이 무기가 사용할 수 있는 모든 총구 부착물들")]
    [SerializeField]
    private MuzzleBehaviour[] muzzleArray;

    [Title(label: "Laser")]

    [Tooltip("선택된 레이저 인덱스")]
    [SerializeField]
    private int laserIndex = -1;

    [Tooltip("게임이 시작되면 랜덤 레이저를 사용할 것인지")]
    [SerializeField]
    private bool laserIndexRandom = true;

    [Tooltip("이 무기에 사용할 수 있는 레이저 배열")]
    [SerializeField]
    private LaserBehaviour[] laserArray;

    [Title(label: "Grip")]

    [Tooltip("선택된 그립")]
    [SerializeField]
    private int gripIndex = -1;

    [Tooltip("게임이 시작되면 랜덤 그립을 사용할 것인지")]
    [SerializeField]
    private bool gripIndexRandom = true;

    [Tooltip("이 무기에 사용할 수 있는 그립 배열")]
    [SerializeField]
    private GripBehaviour[] gripArray;

    [Title(label: "Magazine")]

    [Tooltip("선택된 탄창 인텍스")]
    [SerializeField]
    private int magazineIndex;

    [Tooltip("게임이 시작되면 랜덤 탄창을 사용할 것인지")]
    [SerializeField]
    private bool magazineIndexRandom = true;

    [Tooltip("이 무기에 사용할 수 있는 탄창 배열")]
    [SerializeField]
    private Magazine[] magazineArray;
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

    [SerializeField]
    private PhotonView PV;

    private int PVScopeIndex = -1;
    private int PVLaserIndex = -1;
    private int PVGripIndex = -1;
    private int PVMuzzleIndex = 0;

    [SerializeField]
    private bool isreceive = false;

    #endregion

    #region UNITY FUNCTIONS

    protected override void Awake()
    {
                 
        //랜덤 스코프라면
        if (scopeIndexRandom)
            scopeIndex = Random.Range(scopeIndexFirst, scopeArray.Length);

        //스코프 선택
        scopeBehaviour = scopeArray.SelectAndSetActive(scopeIndex);

        //스코프가 없다면
        if(scopeBehaviour == null)
        {
            scopeBehaviour = scopeDefaultBehaviour;
            scopeBehaviour.gameObject.SetActive(scopeDefaultShow);
        }
        //랜덤 총구
        if (muzzleIndexRandom)
            muzzleIndex = Random.Range(0, muzzleArray.Length);
        //총구 선택
        muzzleBehaviour = muzzleArray.SelectAndSetActive(muzzleIndex);
        //랜덤 레이저라면
        if (laserIndexRandom)
            laserIndex = Random.Range(0, laserArray.Length);
        //레이저 선택
        laserBehaviour = laserArray.SelectAndSetActive(laserIndex);

        //랜덤 그립이라면
        if(gripIndexRandom)
            gripIndex = Random.Range(0, gripArray.Length);
        //그립 선택
        gripBehaviour = gripArray.SelectAndSetActive(gripIndex);
        //랜덤 탄창이라면
        if(magazineIndexRandom)
            magazineIndex = Random.Range(0, magazineArray.Length);
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

