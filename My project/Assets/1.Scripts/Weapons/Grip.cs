using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grip : GripBehaviour
{

    #region FIELDS SERIALIZED
    [Title(label: "Settings")]

    [Tooltip("����� �������̽��� ���� Sprite")]
    [SerializeField]
    private Sprite sprite;

    [Title(label:"Renderer")]

    [Tooltip("�׸��� Renderer")]
    [SerializeField]
    private Renderer GripRender;
    #endregion

    #region GETTERS

    public override Sprite GetSprite() => sprite;

    #endregion

    #region METHODS

    public override void FPRenderOff()
    {
        GripRender.enabled = false;
    }
    #endregion
}
