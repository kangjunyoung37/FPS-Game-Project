using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GripBehaviour : MonoBehaviour
{
    #region GETTERS

    /// <summary>
    /// ����� �������̽��� ���� Sprite�� �����մϴ�.
    /// </summary>
    public abstract Sprite GetSprite();

    /// <summary>
    /// ������(�׸�)�� �������� ���ϴ�.
    /// </summary>
    public abstract void FPRenderOff();

    #endregion
}
