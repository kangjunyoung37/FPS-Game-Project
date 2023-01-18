using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TPWeapon : MonoBehaviour
{

    private Animator WeaponAnimator;

    [Title("Change Transform")]
    
    [SerializeField]
    private bool ChangeTransform = false;

    [SerializeField , ShowIf(nameof(ChangeTransform), true)]
    private Transform ParentTransform;

    [SerializeField, ShowIf(nameof(ChangeTransform), true)]
    private AimIK aimik;

    [SerializeField, ShowIf(nameof(ChangeTransform), true)]
    private LeftHandOnGun leftHandOnGun;


    [Title("RuntimeAnimator")]
    
    [SerializeField]
    public RuntimeAnimatorController Controller;

    private CharacterBehaviour Character;

    private InventoryBehaviour FPInventory;

    private WeaponBehaviour EquipWeapon;

    private WeaponAttachmentManagerBehaviour EquipWeaponAttachmentManager;

    private PhotonView PV;

    private AudioSource audioSource;

    private AudioSource weaponAudioSource;

    private AudioClip audioClip;

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

    private MuzzleBehaviour muzzle;

    private IAudioManagerService audioManagerService;

    private AudioSettings audioSettings = new AudioSettings(1.0f, 1.0f, true);

    #region UNITY METHODS
    private void Awake()
    {
  
        Init();
        if (ChangeTransform)
        {
            transform.SetParent(ParentTransform, false);
            aimik.enabled = true;
            leftHandOnGun.enabled = false;
        }
        //Character.OnCharacterDie += CharacterDie;

    }

    private void CharacterDie()
    {
        transform.parent = null;
    }

    private void OnEnable()
    {
        if(ChangeTransform)
        {
            aimik.enabled = true;
            leftHandOnGun.enabled = false;
            for (int i = 3; i < 7; i++)
            {
                aimik.solver.bones[i].weight = 0.0f;
            }

        }
    }

    private void OnDisable()
    {
        if (ChangeTransform)
        {
            aimik.enabled = false;
            leftHandOnGun.enabled = true;
            for (int i = 3; i < 7; i++)
            {
                aimik.solver.bones[i].weight = 1.0f;
            }
        }

    }


    #endregion

    #region METHODS

    private void Init()
    {
        
        audioManagerService = ServiceLocator.Current.Get<IAudioManagerService>();
        weaponAudioSource = GetComponent<AudioSource>();
        audioSource = transform.parent.GetComponent<AudioSource>();
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
        muzzle = EquipWeaponAttachmentManager.GetEquippedMuzzle();
        audioClip = muzzle.GetAudioClipFire();

        SetActive(ScopeGameObjects, Scopes);
        SetActive(LaserGameObjects, Lasers);
        SetActive(MuzzleGameObjects,Muzzles);
        SetActive(GripGameObjects, Grips);


        SetActiveIndex(ScopeGameObjects, ScopeIndex);
        SetActiveIndex(GripGameObjects, GripIndex);
        SetActiveIndex(LaserGameObjects, LaserIndex);
        SetActiveIndex(MuzzleGameObjects, MuzzleIndex);

       
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

    public void TPWPRendererControl(ShadowCastingMode shadowCastingMode)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.shadowCastingMode = shadowCastingMode;
        }
    }

    public void MuzzleFire()
    {
        if (muzzle == null)
            return;
        muzzle.Effect();
        PlaySound(audioClip, muzzle.GetShotSoundRange());
    }

    public void SoundPlay(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
    
    public void PlaySound(AudioClip audioClip,float soundRange)
    {

        weaponAudioSource.maxDistance = soundRange;
        weaponAudioSource.PlayOneShot(audioClip);

    }
    

    #endregion

    #region GETTERS

    public Animator GetAnimator() => WeaponAnimator;

    public PhotonView GetPhotonView() => PV;

    public AudioSource GetTPAudiosorce() => audioSource;

    public WeaponBehaviour GetEquippedWeaponBehaviour() => EquipWeapon;

    public List<Renderer> GetTPWeaponRenderer() => renderers;

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
