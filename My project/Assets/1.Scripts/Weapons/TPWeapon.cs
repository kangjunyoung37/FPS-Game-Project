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


    private int ScopeIndex = -1;
    private int LaserIndex = -1;
    private int MuzzleIndex = 0;
    private int GripIndex = -1;

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


    private void Awake()
    {

        Init();
        WeaponAnimator = transform.GetComponent<Animator>();

        if (ChangeTransform)
        {
            transform.SetParent(ParentTransform, false);
        }
        
    }

    #region METHODS

    private void Init()
    {
        //Character = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        Character = transform.parent.GetComponent<TPInventory>().GetCharacterBehaviour();
        FPInventory = Character.GetInventory();
        EquipWeapon = FPInventory.GetEquipped();
        EquipWeaponAttachmentManager = EquipWeapon.GetAttachmentManager();
        
        ScopeIndex = EquipWeaponAttachmentManager.GetEquippedScopeIndex();
        LaserIndex = EquipWeaponAttachmentManager.GetEquippedLaserIndex();
        MuzzleIndex = EquipWeaponAttachmentManager.GetEquippedMuzzleIndex();
        GripIndex = EquipWeaponAttachmentManager.GetEquippedGripIndex();

        for (int i = 0; i < Scopes.childCount; i++)
        {
            ScopeGameObjects[i] = Scopes.GetChild(i).gameObject;
            ScopeGameObjects[i].SetActive(false);
            renderers.Add(ScopeGameObjects[i].GetComponent<Renderer>());
        }
        
        for(int i = 0; i < Lasers.childCount; i++)
        {
            LaserGameObjects[i] = Lasers.GetChild(i).gameObject;
            LaserGameObjects[i].SetActive(false);
            renderers.Add(LaserGameObjects[i].GetComponent<Renderer>());
        }
        
        for(int i = 0; i < Muzzles.childCount; i++)
        {
            MuzzleGameObjects[i] = Muzzles.GetChild(i).gameObject;
            MuzzleGameObjects[i].SetActive(false);
            renderers.Add(MuzzleGameObjects[i].GetComponent<Renderer>());
        }
        
        for(int i = 0; i < Grips.childCount; i++)
        {
            GripGameObjects[i] = Grips.GetChild(i).gameObject;
            GripGameObjects[i].SetActive(false);
            renderers.Add(GripGameObjects[i].GetComponent<Renderer>());
        }
        EquipAttachment();

    }

    private void EquipAttachment()
    {
        if(ScopeIndex != -1)
        {
            ScopeGameObjects[ScopeIndex].SetActive(true);
        }
        if(LaserIndex != -1)
        {
            LaserGameObjects[LaserIndex].SetActive(true);
        }
        if(GripIndex != -1)
        {
            GripGameObjects[GripIndex].SetActive(true);
        }
        if(MuzzleIndex != 0)
        {
            MuzzleGameObjects[MuzzleIndex-1].SetActive(true);
        }
  
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
    #endregion

    #region GETTERS

    public Animator GetAnimator() => WeaponAnimator;

    #endregion
}
