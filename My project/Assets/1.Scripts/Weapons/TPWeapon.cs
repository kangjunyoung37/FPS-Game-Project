using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TPWeapon : MonoBehaviour
{

    private Animator WeaponAnimator;

    [Title("Change Transform")]
    
    [SerializeField]
    private bool ChangeTransform = false;

    [SerializeField , ShowIf(nameof(ChangeTransform), true)]
    private Transform ParentTransform;

    [Title("RuntimeAnimator")]
    
    [SerializeField]
    public RuntimeAnimatorController Controller;

    private CharacterBehaviour Character;

    private InventoryBehaviour FPInventory;

    private WeaponBehaviour EquipWeapon;

    private WeaponAttachmentManagerBehaviour EquipWeaponAttachmentManager;

    private PhotonView PV;

    private int ScopeIndex = -1;
    private int LaserIndex = -1;
    private int MuzzleIndex = 0;
    private int GripIndex = -1;

    private int TPScopeIndex = -1;
    private int TPLaserIndex = -1;
    private int TPMuzzleIndex = 0;
    private int TPGripIndex = -1;

    [Title("Weapon AttachMent")]

    [SerializeField]
    private Transform Scopes;

    [SerializeField]
    private Transform Lasers;

    [SerializeField]
    private Transform Muzzles;

    [SerializeField]
    private Transform Grips;

    [Title(label:"Renderer")]
    [SerializeField]
    private List<Renderer> renderers = new List<Renderer>();
   
    private GameObject[] ScopeGameObjects = new GameObject[8];

    private GameObject[] LaserGameObjects = new GameObject[2];

    private GameObject[] MuzzleGameObjects = new GameObject[4];

    private GameObject[] GripGameObjects = new GameObject[3];

    private bool isgo = false;

    private MuzzleBehaviour muzzle;

    #region UNITY METHODS
    private void Awake()
    {

        Init();
        if (ChangeTransform)
        {
            transform.SetParent(ParentTransform, false);
        }
        
    }

    private void Update()
    {
        if(!isgo&&EquipWeaponAttachmentManager.Getreceive()&&!PV.IsMine)
        {
            TPScopeIndex = EquipWeaponAttachmentManager.GetEquippedScopePVIndex();
            TPLaserIndex = EquipWeaponAttachmentManager.GetEquippedLaserPVIndex();
            TPMuzzleIndex = EquipWeaponAttachmentManager.GetEquippedMuzzlePVIndex();
            TPGripIndex = EquipWeaponAttachmentManager.GetEquippedGripPVIndex();

            SetActiveIndex(ScopeGameObjects, TPScopeIndex);
            SetActiveIndex(GripGameObjects, TPGripIndex);
            SetActiveIndex(LaserGameObjects, TPLaserIndex);
            SetActiveIndex(MuzzleGameObjects, TPMuzzleIndex);
            isgo = true;
        }

    }

    #endregion

    #region METHODS

    private void Init()
    {
        WeaponAnimator = transform.GetComponent<Animator>();
        Character = transform.parent.GetComponent<TPInventory>().GetCharacterBehaviour();
        PV = Character.transform.GetComponent<PhotonView>();
        FPInventory = Character.GetInventory();
        EquipWeapon = FPInventory.GetEquipped();
        EquipWeaponAttachmentManager = EquipWeapon.GetAttachmentManager();
        ScopeIndex = EquipWeaponAttachmentManager.GetEquippedScopeIndex();
        LaserIndex = EquipWeaponAttachmentManager.GetEquippedLaserIndex();
        MuzzleIndex = EquipWeaponAttachmentManager.GetEquippedMuzzleIndex();
        GripIndex = EquipWeaponAttachmentManager.GetEquippedGripIndex();

        SetActive(ScopeGameObjects, Scopes);
        SetActive(LaserGameObjects, Lasers);
        SetActive(MuzzleGameObjects,Muzzles);
        SetActive(GripGameObjects, Grips);

        muzzle = MuzzleGameObjects[MuzzleIndex].GetComponent<MuzzleBehaviour>();


        SetActiveIndex(ScopeGameObjects, TPScopeIndex);
        SetActiveIndex(GripGameObjects, TPGripIndex);
        SetActiveIndex(LaserGameObjects, TPLaserIndex);
        SetActiveIndex(MuzzleGameObjects, TPMuzzleIndex);

       
    }

    public void Reload(string stateName)
    {
        WeaponAnimator.SetBool("Reloading", true);

        WeaponAnimator.Play(stateName, 0, 0.0f);
    }

    public void Fire()
    {
        const string stateName = "Fire";
        WeaponAnimator.Play(stateName, 0, 0.0f);

    }

    public void SetSlideBack(int back)
    {
        const string boolName = "Slide Back";
        WeaponAnimator.SetBool(boolName, back != 0);
    }

    public void TPWeaponOff()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    public void MuzzleFire()
    {
        muzzle.Effect();
        
    }

    #endregion

    #region GETTERS

    public Animator GetAnimator() => WeaponAnimator;

    #endregion

    #region HELP METHODS

    /// <summary>
    /// 배열에 게임오브젝트를 넣고, 게임오브젝트 비활성화하기
    /// </summary>
    /// <param name="array"></param>
    /// <param name="gametransform"></param>
    private void SetActive(GameObject[] array, Transform gametransform)
    {
        for (int i = 0; i < gametransform.childCount; i++)
        {
            array[i] = gametransform.GetChild(i).gameObject;
            array[i].SetActive(false);
            renderers.Add(array[i].GetComponent<Renderer>());
        }
    }
    
    private void SetActiveIndex(GameObject[] array, int index)
    {
        if (index < 0)
            return;
        for(int i = 0; i < array.Length;i++)
        {
            array[i].SetActive(false);
        }
        array[index].SetActive(true);

    }

    #endregion

}
