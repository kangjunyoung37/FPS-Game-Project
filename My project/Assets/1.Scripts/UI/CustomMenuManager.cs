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

    private Transform gripButtonGroup;
    private Transform muzzleButtonGroup;
    private Transform scopeButtonGroup;
    private Transform laserButtonGroup;
    private GameObject openButtonGroup;

    private List<AttachmentUIButton> attachMentButtonList = new List<AttachmentUIButton>();
    [SerializeField]
    private bool isMainWeapon = false;
    [SerializeField]
    private bool isSubWeapon = false;

    #region Unity Methods
    private void Awake()
    {
        gripButtonGroup = gripButton.GetChild(1);
        laserButtonGroup = laserButton.GetChild(1);
        muzzleButtonGroup = muzzleButton.GetChild(1);
        scopeButtonGroup = scopeButton.GetChild(1);
        GetAttachMentList(gripButtonGroup);
        GetAttachMentList(laserButtonGroup);
        GetAttachMentList(muzzleButtonGroup);
        GetAttachMentList(scopeButtonGroup);

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
    }

    private void OnEnable()
    {
        DOF.active = true;
        isMainWeapon = true;
        playerHashTable = PhotonNetwork.LocalPlayer.CustomProperties;
        equipSubWeapon = uiWeapons[(int)playerHashTable["SubWeapon"]];
        if (equipMainWeapon != uiWeapons[(int)playerHashTable["MainWeapon"]])
        {
            equipMainWeapon = uiWeapons[(int)playerHashTable["MainWeapon"]];
            equipMainWeapon.gameObject.SetActive(true);
        }
        else
            equipMainWeapon.gameObject.SetActive(true);
        
        //MainWeapon AttachMent Check
        AttachMentCheck(laserButtonGroup, equipMainWeapon, "MainLaser");
        AttachMentCheck(gripButtonGroup, equipMainWeapon, "MainGrip");
        AttachMentCheck(muzzleButtonGroup, equipMainWeapon, "MainMuzzle");
        AttachMentCheck(scopeButtonGroup, equipMainWeapon, "MainScope");
        
        CreateAttachmentButton(equipMainWeapon);
        EnableAttachMentButton(equipMainWeapon);
        
        UpdateAttachMentAllButtons(true);
        UpdateAllAttachMentWepaon(true);
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
        foreach (Transform child in attchMentTransform)
        {
            child.gameObject.SetActive(false);
        }
        if (index < 0)
            return;
        attchMentTransform.GetChild(index).gameObject.SetActive(true);

    }
    private void GetAttachMentList(Transform attchMentTransform)
    {
        foreach(Transform child in attchMentTransform)
        {
            attachMentButtonList.Add(child.GetComponent<AttachmentUIButton>());
        }
    }

    private void EnableAttachMentButton(UIWeapon uIWeapon)
    {
        foreach(AttachmentUIButton attachmentUIButton in attachMentButtonList)
        {
            attachmentUIButton.gameObject.SetActive(attachmentUIButton.CheckEquip(uIWeapon.weaponType));
        }
    }

    private void AttachMentCheck(Transform attachMentTransform,UIWeapon equipWeapon,string attachMenttype)
    {
        if ((int)playerHashTable[attachMenttype] == -1)
            return;
        //+1 Because of default Attachment 
        if (!attachMentTransform.GetChild((int)playerHashTable[attachMenttype]+1).GetComponent<AttachmentUIButton>().CheckEquip(equipWeapon.weaponType))
        {
            playerHashTable[attachMenttype] = -1;
        }
    }

    private void CloseButtonGrop()
    {
        if (openButtonGroup != null)
        {
            openButtonGroup.SetActive(false);
            openButtonGroup = null;
        }
    }

    #endregion

    #region Button Event

    public void ExitMenu()
    {

        DOF.active = false;
        equipMainWeapon.gameObject.SetActive(false);
        equipSubWeapon.gameObject.SetActive(false);
        isSubWeapon = false;
        isMainWeapon = false;
        CloseButtonGrop();
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
        CloseButtonGrop();
        EnableAttachMentButton(equipMainWeapon);

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
        UpdateAllAttachMentWepaon(false);
        CloseButtonGrop();
        EnableAttachMentButton(equipSubWeapon);
    }

    public void ButtonGropOpenOrClose(GameObject gameObject)
    {
        if (openButtonGroup == null)
        {
            openButtonGroup = gameObject;
            openButtonGroup.SetActive(true);
        }
        else
        {
            if(openButtonGroup == gameObject)
            {
                openButtonGroup.SetActive(false);
                openButtonGroup = null;
            }
            else
            {
                openButtonGroup.SetActive(false);
                openButtonGroup = gameObject;
                openButtonGroup.SetActive(true);
            }
        }
    }

    public void ClickAttachMentButton(AttachmentUIButton attachmentUIButton)
    {
        if(isMainWeapon)
        {
            switch(attachmentUIButton.type)
            {
                case AttachmentType.Scope:
                    playerHashTable["MainScope"] = attachmentUIButton.index;
                    UpdateAttachMentButton(scopeImage, attachmentUIButton.index, scopeSprites);
                    UpdateAttachMentWeapon(equipMainWeapon.GetScopeTransform, attachmentUIButton.index);
                    break;
                case AttachmentType.Grip:
                    playerHashTable["MainGrip"] = attachmentUIButton.index;
                    UpdateAttachMentButton(gripImage, attachmentUIButton.index, gripSprites);
                    UpdateAttachMentWeapon(equipMainWeapon.GetGripTransform, attachmentUIButton.index);
                    break;
                case AttachmentType.Muzzle:
                    playerHashTable["MainMuzzle"] = attachmentUIButton.index;
                    UpdateAttachMentButton(muzzleImage, attachmentUIButton.index, muzzleSprites);
                    UpdateAttachMentWeapon(equipMainWeapon.GetMuzzleTransform, attachmentUIButton.index);

                    break;
                case AttachmentType.LaserSight:
                    playerHashTable["MainLaser"] = attachmentUIButton.index;
                    UpdateAttachMentButton(laserImage, attachmentUIButton.index, laserSprites);
                    UpdateAttachMentWeapon(equipMainWeapon.GetLaserTransform, attachmentUIButton.index);
                    break;

            }
        }
        else
        {
            switch (attachmentUIButton.type)
            {
                case AttachmentType.Scope:
                    playerHashTable["SubScope"] = attachmentUIButton.index;
                    UpdateAttachMentButton(scopeImage, attachmentUIButton.index, scopeSprites);
                    UpdateAttachMentWeapon(equipSubWeapon.GetScopeTransform, attachmentUIButton.index);
                    break;

                case AttachmentType.Muzzle:
                    playerHashTable["SubMuzzle"] = attachmentUIButton.index;
                    UpdateAttachMentButton(muzzleImage, attachmentUIButton.index, muzzleSprites);
                    UpdateAttachMentWeapon(equipSubWeapon.GetMuzzleTransform, attachmentUIButton.index);

                    break;
                case AttachmentType.LaserSight:
                    playerHashTable["SubLaser"] = attachmentUIButton.index;
                    UpdateAttachMentButton(laserImage, attachmentUIButton.index, laserSprites);
                    UpdateAttachMentWeapon(equipSubWeapon.GetLaserTransform, attachmentUIButton.index);
                    break;

            }
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerHashTable);
    }

    #endregion



}
