using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum WeaponType
{
    AR,
    SMG,
    SG,
    HG,
    SN
}

public class UIWeaponButton : MonoBehaviour , IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField]
    private WeaponType weaponType = WeaponType.AR;
    
    [SerializeField]
    private string weaponName = "";
    
    [SerializeField]
    private StorageManager storageManager;

    [SerializeField]
    private UIWeaponData uiWeaponData; 

    private Image backGroundImage;
    private Sprite defaultSprite;
    private Color defalutColor;

 
    private int index;

    private void Awake()
    {
        backGroundImage = GetComponent<Image>();
        defaultSprite = GetComponent<Sprite>();
        defalutColor = backGroundImage.color;
        transform.root.GetComponent<Launcher>();

    }
    #region Getters

    public WeaponType GetWeaponType() => weaponType;

    public UIWeaponData GetWeaponData() => uiWeaponData;

    public string GetWeaponName() => weaponName;

    #endregion

    #region Properties

    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    #endregion

    public void OnPointerEnter(PointerEventData eventData)
    {
        storageManager.ButtonClickImageActive(true);
        backGroundImage.sprite = storageManager.GetGradationSprite();
        backGroundImage.color = Color.blue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        storageManager.ButtonClickImageActive(false);
        backGroundImage.sprite = defaultSprite;
        backGroundImage.color = defalutColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            storageManager.SelectAndActive(storageManager.GetUIWeapons(), index);
        }

        if(eventData.button == PointerEventData.InputButton.Right)
        {
            storageManager.ClickUIWeaponButton(this);
            storageManager.GetAudioSorce().Play();
        }
        storageManager.UpdateData(uiWeaponData);
    }
}
