using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaserBehaviour : MonoBehaviour
{
    #region GETTHERS

    /// <summary>
    /// ����� �������̽��� ���� ��������Ʈ�� �����մϴ�.
    /// </summary>
    public abstract Sprite GetSprite();

    /// <summary>
    /// ĳ���Ͱ� �޸��� ���� �������� �����ϴ� ��� ���� �����մϴ�.
    /// </summary>
    public abstract bool GetTurnOffWhileRunning();

    /// <summary>
    /// ĳ���Ͱ� �����ϴ� ���� �������� �����ϴ� ��� ���� �����մϴ�.
    /// </summary>
    public abstract bool GetTurnOffWhileAiming();

    /// <summary>
    /// ������ ����ϱ�
    /// </summary>
    public abstract void Toggle();

    /// <summary>
    /// ������ �������ϱ�
    /// </summary>
    public abstract void Reapply();

    /// <summary>
    /// ������ �����
    /// </summary>
    public abstract void Hide();

    /// <summary>
    /// ������ Renderer����
    /// </summary>
    public abstract void FPLaserOff();
    #endregion
}
