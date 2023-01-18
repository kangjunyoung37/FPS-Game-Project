using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class UITextAmmunitionTotal : UIElementText
{
    #region Methods


    protected override void Tick()
    {
        float ammunitionTotatl = equippedWeapon.GetAmmunitionWeaponTotal();

        textMesh.text = ammunitionTotatl.ToString(CultureInfo.InvariantCulture);
    }

    #endregion

}
