using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class UIElementText : UIElement
{
    #region Fields

    protected TextMeshProUGUI textMesh;

    #endregion

    #region Unity

    protected override void Awake()
    {
        base.Awake();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    #endregion



}
