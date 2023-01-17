using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : UIElement
{
    #region Fields Serialized

    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private Image healthGage;

    [SerializeField]
    private bool updateColor = true;

    [SerializeField]
    private float lowHPSpeed = 1.5f;

    [SerializeField]
    private Color dieHPColor = Color.red;

    #endregion

    #region Fields

    private float health = -1.0f;

    #endregion
    protected override void Awake()
    {
        base.Awake();
        
    }

    protected override void Tick()
    {
        health = characterBehaviour.GetPlayerHP();
        healthText.text = health.ToString();

        if(updateColor)
        {
            float colorAlpha = (health / 100) * lowHPSpeed;
            healthText.color = Color.Lerp(dieHPColor, Color.white,colorAlpha);
        }

        healthGage.fillAmount = (health / 100);


    }



}
