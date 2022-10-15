using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grip : GripBehaviour
{

    #region FIELDS SERIALIZED
    [Title(label: "Settings")]

    [Tooltip("사용자 인터페이스에 쓰일 Sprite")]
    [SerializeField]
    private Sprite sprite;

    #endregion

    #region GETTERS
    public override Sprite GetSprite() => sprite;

    #endregion
}
