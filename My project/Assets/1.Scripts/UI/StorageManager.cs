using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class StorageManager : MonoBehaviour
{

    // contentÆ®·£½ºÆû
    [SerializeField]
    private Transform contentTrnasform;

    //Ã¢°í¹«±âÀÇ Æ®·£½ºÆû
    [SerializeField]
    private Transform storageWeapon;

    [SerializeField]
    private Transform buttonClickTransform;

    //MyWeapon MainWeaponUI Sprite
    [SerializeField]
    private Transform mainWeaponUITransform;

    //MyWeapon SubWeaponUI Transform
    [SerializeField]
    private Transform subWeaponUITransform;

    [SerializeField]
    private Image mainWeaponUIImage;
    [SerializeField]
    private Image subWeaponUIImage;

    [SerializeField]
    private Sprite gradationSprite;
    
    // Weapon Spirtes
    [SerializeField]
    private Sprite[] weaponSprites;

    [SerializeField]
    private Transform weaponTypeButton;

    [SerializeField]
    private Image DamageBarGage;

    [SerializeField]
    private Image RPMBarGage;
    
    [SerializeField]
    private Image RecoilBarGage;
    
    [SerializeField]
    private Image MobilityBarGage;

    [SerializeField]
    private TMP_Text magazine;

    [SerializeField]
    private Transform explaintion;

    private List<UIWeaponButton> mainWeaponList = new();
    private List<UIWeaponButton> subWeaponList = new();
    private List<Transform> uIWeapons = new();
    private UIWeapon equipMainWeapon;
    private UIWeapon equipSubWeapon;
    private Hashtable playerHashTable;
    private AudioSource audioSource;
    private TMP_Text mainWeaponText;
    private TMP_Text subWeaponText;
    private bool isMainWeapon = false;
    private bool isSubWeapon = false;

    #region Unity Methods

    private void Awake()
    {
        //Ä³½Ì
        mainWeaponText = mainWeaponUITransform.GetComponentInChildren<TMP_Text>();
        subWeaponText = subWeaponUITransform.GetComponentInChildren<TMP_Text>();
        audioSource = GetComponent<AudioSource>();  
        foreach(Transform child in storageWeapon)
        {
            uIWeapons.Add(child);  
        }
        for (int i = 0; i < contentTrnasform.childCount; i++)
        {
            UIWeaponButton uiWeapon;
            uiWeapon = contentTrnasform.GetChild(i).GetComponent<UIWeaponButton>();
            uiWeapon.Index = i;
            if(uiWeapon.GetWeaponType() != WeaponType.HG)
               mainWeaponList.Add(uiWeapon);
            else
                subWeaponList.Add(uiWeapon);
        }
        SetActiveButton(mainWeaponList);    
    }

    private void Start()
    {
        playerHashTable = PhotonNetwork.LocalPlayer.CustomProperties;
        SetActiveMainWeapon();
        UpdateMyWeapon(true);
        UpdateMyWeapon(false);
    }

    #endregion

    public void UpdateData(UIWeaponData uIWeaponData)
    {
        DamageBarGage.fillAmount = uIWeaponData.Damage;
        RPMBarGage.fillAmount = uIWeaponData.RPM;
        RecoilBarGage.fillAmount = uIWeaponData.Recoil;
        MobilityBarGage.fillAmount = uIWeaponData.Mobility;
        magazine.text = uIWeaponData.Magazine.ToString();
    }

    private void UpdateMyWeapon(bool mainWeapon)
    {
        
        if (mainWeapon)
        {
            int index = (int)playerHashTable["MainWeapon"];
            mainWeaponUIImage.sprite = weaponSprites[index];
            mainWeaponUIImage.SetNativeSize();
            mainWeaponText.text = mainWeaponList[index].GetWeaponName();
        }
        else
        {
            int index = (int)playerHashTable["SubWeapon"];
            subWeaponUIImage.sprite = weaponSprites[index];
            subWeaponUIImage.SetNativeSize();
            index -= (mainWeaponList.Count);
            subWeaponText.text = subWeaponList[index].GetWeaponName();
        }
    }

    private void SetActiveButton(List<UIWeaponButton> weaponList)
    {
        foreach(Transform buttonTransform in contentTrnasform)
        {
            buttonTransform.gameObject.SetActive(false);
        }

        foreach(UIWeaponButton ui in weaponList)
        {
            ui.gameObject.SetActive(true);
        }
    }

    private void SetActiveButton(List<UIWeaponButton> weaponList, WeaponType weaponType)
    {
        foreach (Transform buttonTransform in contentTrnasform)
        {
            buttonTransform.gameObject.SetActive(false);
        }

        foreach (UIWeaponButton ui in weaponList)
        {
            if(ui.GetWeaponType() == weaponType)
                ui.gameObject.SetActive(true);
        }
    }

    public void SetActiveWeaponTpye(string weapontype) 
    {
        if (weapontype == "AR")
            SetActiveButton(mainWeaponList, WeaponType.AR);
        else if (weapontype == "SMG")
            SetActiveButton(mainWeaponList, WeaponType.SMG);
        else if (weapontype == "SN")
            SetActiveButton(mainWeaponList, WeaponType.SN);
        else
            SetActiveButton(mainWeaponList, WeaponType.SG);
    }
    
    public void SetActiveMainWeapon()
    {
        if (isMainWeapon) return; 
        SetActiveButton(mainWeaponList);
        if (playerHashTable != null)
        {
            SelectAndActive(uIWeapons,0);
            UpdateData(mainWeaponList[0].GetWeaponData());
        }

        weaponTypeButton.gameObject.SetActive(true);
        isSubWeapon = false;
        isMainWeapon = true;
    }

    public void SetActiveSubWeapon()
    {
        if(isSubWeapon) return;
        SetActiveButton(subWeaponList);
        if (playerHashTable != null)
        {
            SelectAndActive(uIWeapons,12);
            UpdateData(subWeaponList[0].GetWeaponData());
        }

        weaponTypeButton.gameObject.SetActive(false);
        isMainWeapon = false;
        isSubWeapon = true;
    }

    public void SelectAndActive<T>(List<T> list , int active) where T : Transform
    {
        foreach(T t in list)
        {
            t.gameObject.SetActive(false);
        }
        list[active].gameObject.SetActive(true);
    }

    public void ClickUIWeaponButton(UIWeaponButton uIWeaponButton)
    {
        SelectAndActive(uIWeapons, uIWeaponButton.Index);
        if (uIWeaponButton.GetWeaponType() != WeaponType.HG)
        {
            playerHashTable["MainWeapon"] = uIWeaponButton.Index;
            UpdateMyWeapon(true);
        }
        else
        {
            playerHashTable["SubWeapon"] = uIWeaponButton.Index;
            UpdateMyWeapon(false);
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerHashTable);
    }

    public void ButtonClickImageActive(bool active)
    {
        buttonClickTransform.gameObject.SetActive(active);
    }
   
    #region Getters

    public List<Transform> GetUIWeapons() => uIWeapons;

    public Sprite GetGradationSprite() => gradationSprite;

    public AudioSource GetAudioSorce() => audioSource;
    #endregion
}
