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
    [Tooltip("���� �𵨿� ���̾����Ʈ�� ǥ������ ���θ� �����մϴ�")]
    [SerializeField]
    private bool scopeDefaultShow = true;

    [Tooltip("�⺻ ������")]
    [SerializeField]
    private ScopeBehaviour scopeDefaultBehaviour;

    [Tooltip("������ �ε��� ���࿡ ������� ���̾����Ʈ�� ����")]
    [SerializeField]
    private int scopeIndex = -1;


    [Tooltip("�� ���Ⱑ ����� �� �ִ� ��������")]
    [SerializeField]
    private ScopeBehaviour[] scopeArray;

    [Title(label: "Muzzle")]

    [Tooltip("���õ� �ѱ� �ε���")]
    [SerializeField]
    private int muzzleIndex;

    [Tooltip("�� ���Ⱑ ����� �� �ִ� ��� �ѱ� ��������")]
    [SerializeField]
    private MuzzleBehaviour[] muzzleArray;

    [Title(label: "Laser")]

    [Tooltip("���õ� ������ �ε���")]
    [SerializeField]
    private int laserIndex = -1;

    [Tooltip("�� ���⿡ ����� �� �ִ� ������ �迭")]
    [SerializeField]
    private LaserBehaviour[] laserArray;

    [Title(label: "Grip")]

    [Tooltip("���õ� �׸�")]
    [SerializeField]
    private int gripIndex = -1;

    [Tooltip("�� ���⿡ ����� �� �ִ� �׸� �迭")]
    [SerializeField]
    private GripBehaviour[] gripArray;

    [Title(label: "Magazine")]

    [Tooltip("���õ� źâ ���ؽ�")]
    [SerializeField]
    private int magazineIndex;

    [Tooltip("�� ���⿡ ����� �� �ִ� źâ �迭")]
    [SerializeField]
    private Magazine[] magazineArray;

    [Title(label: "External Attachment")]
    [SerializeField]
    private Renderer[] externalAttachment;

    #endregion
    #region FIELDS

    /// <summary>
    /// ������ ������
    /// </summary>
    private ScopeBehaviour scopeBehaviour ;

    /// <summary>
    /// ������ �ѱ�
    /// </summary>
    private MuzzleBehaviour muzzleBehaviour;

    /// <summary>
    /// ������ ������
    /// </summary>
    private LaserBehaviour laserBehaviour;

    /// <summary>
    /// ������ �׸�
    /// </summary>
    private GripBehaviour gripBehaviour;

    /// <summary>
    /// ������ źâ
    /// </summary>
    private MagazineBehaviour magazineBehaviour;

    /// <summary>
    /// CharacterBehaviour
    /// </summary>
    private CharacterBehaviour characterBehaviour;

    /// <summary>
    /// �������� ������ ����
    /// </summary>
    private Weapon weapon;

    private HashTable playerHashTable;

    private PhotonView PV;

    private int PVScopeIndex = -1;
    private int PVLaserIndex = -1;
    private int PVGripIndex = -1;
    private int PVMuzzleIndex = 0;

    //�ֹ�������
    private bool isMainWeapon;

    /// <summary>
    /// �����͸� �޾Ҵ��� 
    /// </summary>
    private bool isreceive = false;

    #endregion

    #region UNITY FUNCTIONS

    protected override void Awake()
    {
        //ĳ��
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

  

        //������ ����
        scopeBehaviour = scopeArray.SelectAndSetActive(scopeIndex);

        //�������� ���ٸ�
        if(scopeBehaviour == null)
        {
            scopeBehaviour = scopeDefaultBehaviour;
            scopeBehaviour.gameObject.SetActive(scopeDefaultShow);
        }

        //�ѱ��� ���ٸ�
        muzzleBehaviour = muzzleArray.SelectAndSetActive(muzzleIndex);

        //������ ����
        laserBehaviour = laserArray.SelectAndSetActive(laserIndex);

        //�׸� ����
        gripBehaviour = gripArray.SelectAndSetActive(gripIndex);

        //źâ ����
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

