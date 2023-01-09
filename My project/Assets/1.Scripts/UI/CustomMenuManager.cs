using Photon.Pun;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using HashTable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
public class CustomMenuManager : MonoBehaviour
{
    [Title(label:"PostProcessing")]
    [SerializeField]
    private PostProcessVolume PPV;

    [Title(label:"UIWeapon Parent Transform")]
    [SerializeField]
    private Transform UIWeaponTransform;

    [Title(label:"Attachment Button")]
    [SerializeField]
    private RectTransform muzzleButton;

    [SerializeField]
    private RectTransform gripButton;

    [SerializeField]
    private RectTransform laserButton;

    [SerializeField]
    private RectTransform scopeButton;

    [Title(label:"Sprites Settings")]
    [SerializeField]
    private Sprite[] gripSprites;
    
    [SerializeField]
    private Sprite[] scopeSprites;
    
    [SerializeField]
    private Sprite[] laserSprites;
    
    [SerializeField]
    private Sprite[] muzzleSprites;

    private List<UIWeapon> uiWeapons = new List<UIWeapon>();

    private HashTable playerHashTable = new HashTable();

    private UIWeapon equipMainWeapon = null;

    private UIWeapon equipSubWeapon = null;

    private DepthOfField DOF;

    private Image gripImage;
    private Image laserImage;
    private Image scopeImage;
    private Image muzzleImage;

    private bool isMainWeapon = false;
    private bool isSubWeapon = false;

    #region Unity Methods
    private void Awake()
    {
        gripImage = gripButton.GetChild(0).GetComponent<Image>();
        laserImage = laserButton.GetChild(0).GetComponent<Image>();
        scopeImage = scopeButton.GetChild(0).GetComponent<Image>();
        muzzleImage = muzzleButton.GetChild(0).GetComponent<Image>();

        foreach (Transform tf in UIWeaponTransform)
        {
            uiWeapons.Add(tf.GetComponent<UIWeapon>());
        }

        PPV.profile.TryGetSettings(out DOF);
        
    }
    
    private void Start()
    {
        equipMainWeapon = uiWeapons[(int)playerHashTable["MainWeapon"]];
        equipSubWeapon = uiWeapons[(int)playerHashTable["SubWeapon"]];
        equipMainWeapon.gameObject.SetActive(true);
        isMainWeapon = true;
    }

    private void OnEnable()
    {
        DOF.active = true;
        playerHashTable = PhotonNetwork.LocalPlayer.CustomProperties;
        equipSubWeapon = uiWeapons[(int)playerHashTable["SubWeapon"]];
        if (equipMainWeapon != uiWeapons[(int)playerHashTable["MainWeapon"]])
        {
            equipMainWeapon = uiWeapons[(int)playerHashTable["MainWeapon"]];
            equipMainWeapon.gameObject.SetActive(true);
        }
        else
            equipMainWeapon.gameObject.SetActive(true);
        CreateAttachmentButton(equipMainWeapon);
        UpdateAttachMentAllButtons(true);
    }

    #endregion

    #region Methods
    
    /// <summary>
    /// 부착물 버튼을 화면상에 원하는 위치로 옮기기 
    /// </summary>
    private void CreateAttachmentButton(UIWeapon equipWeapon)
    {
        muzzleButton.anchoredPosition = equipWeapon.MuzzlePoint;
        gripButton.anchoredPosition = equipWeapon.GripPoint;
        laserButton.anchoredPosition = equipWeapon.LaserPoint;
        scopeButton.anchoredPosition = equipWeapon.ScopePoint;
    }

    /// <summary>
    /// 모든 버튼 스프라이트 업데이트 시키기
    /// </summary>
    private void UpdateAttachMentAllButtons(bool isMainWepon)
    {
        if(isMainWepon)
        {
            gripButton.gameObject.SetActive(true);
            UpdateAttachMentButton(scopeImage, (int)playerHashTable["MainScope"], scopeSprites);
            UpdateAttachMentButton(gripImage, (int)playerHashTable["MainGrip"], gripSprites);
            UpdateAttachMentButton(muzzleImage, (int)playerHashTable["MainMuzzle"], muzzleSprites);
            UpdateAttachMentButton(laserImage, (int)playerHashTable["MainLaser"], laserSprites);

        }
        else
        {
            gripButton.gameObject.SetActive(false);
            UpdateAttachMentButton(scopeImage, (int)playerHashTable["SubScope"], scopeSprites);
            UpdateAttachMentButton(muzzleImage, (int)playerHashTable["SubMuzzle"], muzzleSprites);
            UpdateAttachMentButton(laserImage, (int)playerHashTable["SubLaser"], laserSprites);
            
        }

    }

    /// <summary>
    /// 부착물 버튼 업데이트 시키기
    /// </summary>
    /// <param name="attachMentImage">부착물 이미지</param>
    /// <param name="index">인덱스</param>
    /// <param name="attchmentsprites">부착물 스프라이트 그룹</param>
    private void UpdateAttachMentButton(Image attachMentImage , int index, Sprite[] attchmentsprites)
    {
        if (index < 0)
        {
            attachMentImage.enabled = false;
            return;
        }
        attachMentImage.enabled = true;
        attachMentImage.sprite = attchmentsprites[index];
        attachMentImage.SetNativeSize();
        
    }

    private void UpdateAllAttachMentWepaon(bool isMainWeapon)
    {
        if (isMainWeapon)
        {
            UpdateAttachMentWeapon(equipMainWeapon.GetGripTransform, (int)playerHashTable["MainGrip"]);
            UpdateAttachMentWeapon(equipMainWeapon.GetScopeTransform, (int)playerHashTable["MainScope"]);
            UpdateAttachMentWeapon(equipMainWeapon.GetLaserTransform, (int)playerHashTable["MainLaser"]);
            UpdateAttachMentWeapon(equipMainWeapon.GetMuzzleTransform, (int)playerHashTable["MainMuzzle"]);
        }
        else
        {
            UpdateAttachMentWeapon(equipSubWeapon.GetScopeTransform, (int)playerHashTable["SubScope"]);
            UpdateAttachMentWeapon(equipSubWeapon.GetLaserTransform, (int)playerHashTable["SubLaser"]);
            UpdateAttachMentWeapon(equipSubWeapon.GetMuzzleTransform, (int)playerHashTable["SubMuzzle"]);
        }
        
    }

    private void UpdateAttachMentWeapon(Transform attchMentTransform , int index)
    {
        if (index < 0)
            return;
        foreach(Transform child in attchMentTransform)
        {
            child.gameObject.SetActive(false);
        }
        attchMentTransform.GetChild(index).gameObject.SetActive(true);

    }

    #endregion

    #region Button Event

    public void ExitMenu()
    {
        DOF.active = false;
        equipMainWeapon.gameObject.SetActive(false);
        equipSubWeapon.gameObject.SetActive(false);
        MenuManager.Instance.OpenMenu("Title");
    }

    public void MainWeaponButton()
    {
        if (isMainWeapon)
            return;
        isMainWeapon = true;
        isSubWeapon = false;
        equipSubWeapon.gameObject.SetActive(false);
        equipMainWeapon.gameObject.SetActive(true);
        CreateAttachmentButton(equipMainWeapon);
        UpdateAttachMentAllButtons(true);
    }

    public void SubWeaponButton()
    {
        if (isSubWeapon)
            return;
        isMainWeapon = false;
        isSubWeapon = true;
        equipMainWeapon.gameObject.SetActive(false);
        equipSubWeapon.gameObject.SetActive(true);
        CreateAttachmentButton(equipSubWeapon);
        UpdateAttachMentAllButtons(false);
    }

    #endregion



}
