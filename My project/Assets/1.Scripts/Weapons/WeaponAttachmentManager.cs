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
    [Tooltip("���� �𵨿� ���̾����Ʈ�� ǥ������ ���θ� �����մϴ�")]
    [SerializeField]
    private bool scopeDefaultShow = true;

    [Tooltip("�⺻ ������")]
    [SerializeField]
    private ScopeBehaviour scopeDefaultBehaviour;

    [Tooltip("������ �ε��� ���࿡ ������� ���̾����Ʈ�� ����")]
    [SerializeField]
    private int scopeIndex = -1;

    [Tooltip("���� �������� ����� �� ù��° ������ ���ؽ�")]
    [SerializeField]
    private int scopeIndexFirst = -1;

    [Tooltip("������ ���۵Ǹ� ���� �������� ����� ������")]
    [SerializeField]
    private bool scopeIndexRandom;

    [Tooltip("�� ���Ⱑ ����� �� �ִ� ��������")]
    [SerializeField]
    private ScopeBehaviour[] scopeArray;

    [Title(label: "Muzzle")]

    [Tooltip("���õ� �ѱ� �ε���")]
    [SerializeField]
    private int muzzleIndex;

    [Tooltip("������ ���۵Ǹ� ���� �ѱ��� ����� ������")]
    [SerializeField]
    private bool muzzleIndexRandom = true;

    [Tooltip("�� ���Ⱑ ����� �� �ִ� ��� �ѱ� ��������")]
    [SerializeField]
    private MuzzleBehaviour[] muzzleArray;

    [Title(label: "Laser")]

    [Tooltip("���õ� ������ �ε���")]
    [SerializeField]
    private int laserIndex = -1;

    [Tooltip("������ ���۵Ǹ� ���� �������� ����� ������")]
    [SerializeField]
    private bool laserIndexRandom = true;

    [Tooltip("�� ���⿡ ����� �� �ִ� ������ �迭")]
    [SerializeField]
    private LaserBehaviour[] laserArray;

    [Title(label: "Grip")]

    [Tooltip("���õ� �׸�")]
    [SerializeField]
    private int gripIndex = -1;

    [Tooltip("������ ���۵Ǹ� ���� �׸��� ����� ������")]
    [SerializeField]
    private bool gripIndexRandom = true;

    [Tooltip("�� ���⿡ ����� �� �ִ� �׸� �迭")]
    [SerializeField]
    private GripBehaviour[] gripArray;

    [Title(label: "Magazine")]

    [Tooltip("���õ� źâ ���ؽ�")]
    [SerializeField]
    private int magazineIndex;

    [Tooltip("������ ���۵Ǹ� ���� źâ�� ����� ������")]
    [SerializeField]
    private bool magazineIndexRandom = true;

    [Tooltip("�� ���⿡ ����� �� �ִ� źâ �迭")]
    [SerializeField]
    private Magazine[] magazineArray;
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
                 
        //���� ���������
        if (scopeIndexRandom)
            scopeIndex = Random.Range(scopeIndexFirst, scopeArray.Length);

        //������ ����
        scopeBehaviour = scopeArray.SelectAndSetActive(scopeIndex);

        //�������� ���ٸ�
        if(scopeBehaviour == null)
        {
            scopeBehaviour = scopeDefaultBehaviour;
            scopeBehaviour.gameObject.SetActive(scopeDefaultShow);
        }
        //���� �ѱ�
        if (muzzleIndexRandom)
            muzzleIndex = Random.Range(0, muzzleArray.Length);
        //�ѱ� ����
        muzzleBehaviour = muzzleArray.SelectAndSetActive(muzzleIndex);
        //���� ���������
        if (laserIndexRandom)
            laserIndex = Random.Range(0, laserArray.Length);
        //������ ����
        laserBehaviour = laserArray.SelectAndSetActive(laserIndex);

        //���� �׸��̶��
        if(gripIndexRandom)
            gripIndex = Random.Range(0, gripArray.Length);
        //�׸� ����
        gripBehaviour = gripArray.SelectAndSetActive(gripIndex);
        //���� źâ�̶��
        if(magazineIndexRandom)
            magazineIndex = Random.Range(0, magazineArray.Length);
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

