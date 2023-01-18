using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextAmmunitionCurrent : UIElementText
{


    #region Fields Serialzed

    [Title(label: "Color")]

    [SerializeField]
    private bool updateColor = true;

    [SerializeField]
    private float emptySpeed = 1.5f;

    [SerializeField]
    private Color emptyColor = Color.red;

    #endregion

    protected override void Tick()
    {
        float current = equippedWeapon.GetAmmunitionCurrent();

        float total = equippedWeapon.GetAmmunitionTotal();

        textMesh.text = current.ToString();

        if(updateColor)
        {
            float colorAlpha = (current/total) * emptySpeed;
            textMesh.color = Color.Lerp(emptyColor, Color.white, colorAlpha);
        }

    }
}
