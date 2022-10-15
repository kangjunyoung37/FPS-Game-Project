using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GripBehaviour : MonoBehaviour
{
    #region GETTERS

    /// <summary>
    /// 사용자 인터페이스에 사용될 Sprite를 리턴합니다.
    /// </summary>
    public abstract Sprite GetSprite();
    #endregion
}
