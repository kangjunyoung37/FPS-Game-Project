using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public class SettingMenu : MonoBehaviour
{


    FullScreenMode screenMode;

    [Title(label: "UI Settings")]

    public bool isMenu = true;

    public AudioMixer audioMixer;
    
    public TMP_Dropdown resolutionDropDown;

    public Toggle toggle;

    public TMP_InputField sensitivityInputField;

    public Slider sensitivitySilder;

    [ShowIf(nameof(isMenu),true)]
    public PostProcessVolume PPV;

    private DepthOfField DOF;

    private List<Resolution> resolutions = new List<Resolution>();

    int resolutionNum = 0;

    private void Awake()
    {
        if(isMenu)
            PPV.profile.TryGetSettings(out DOF);
    }

    private void Start()
    {        
        InitUI();
    }
    private void OnEnable()
    {
        if (isMenu)
            DOF.active = true;
    }
    private void InitUI()
    {
        if(PlayerPrefs.HasKey("MouseSensitivity"))
        {       
            sensitivitySilder.value = PlayerPrefs.GetFloat("MouseSensitivity");
            sensitivityInputField.text = sensitivitySilder.value.ToString();
        }
        else
        {
            sensitivitySilder.value = 1.0f;
            sensitivityInputField.text = "1.0";
        }
            
        for(int i = 0; i <Screen.resolutions.Length; i++)
        {
     
            if (Screen.resolutions[i].refreshRate == 60)
            {
                Debug.Log(Screen.resolutions[i]);
                resolutions.Add(Screen.resolutions[i]);          
            }               
        }

        resolutionDropDown.options.Clear();
        int optionNum = 0;
        foreach (Resolution res in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = res.width + " x " + res.height;
            resolutionDropDown.options.Add(option);

            if (res.width == Screen.width && res.height == Screen.height)
                resolutionDropDown.value = optionNum;
            optionNum++;
        }
        resolutionDropDown.RefreshShownValue();
        toggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume",volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OKBtnClick()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySilder.value);
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }

    public void ChangeMouseValue(float value)
    {
        
        sensitivityInputField.text = value.ToString();
        
    }

    public void ChangeMouseValueInputField(string value)
    {
        sensitivitySilder.value = float.Parse(value);
    }

    public void OpenSettings()
    { 
        if(isMenu)
            MenuManager.Instance.OpenMenu("Setting");
        else
            gameObject.SetActive(true);
    }


    public void CloseSettings()
    {
        if (isMenu)
        {
            DOF.active = false;
            MenuManager.Instance.OpenMenu("Title");
        }
        else
        {
            gameObject.SetActive(false);
        }
    }



}
