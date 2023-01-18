using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScopeBehaviour : MonoBehaviour
{

    #region GETTERS

    /// <summary>
    /// ���콺 ���� �������� �����մϴ�.
    /// </summary>
    public abstract float GetMutiplierMouseSensitivity();

    /// <summary>
    /// ����Spread���� �����մϴ�.
    /// </summary>
    public abstract float GetMultiplierSpread();

    /// <summary>
    /// ���� ��ġ ������ ���� �����մϴ�.
    /// </summary>
    public abstract Vector3 GetOffsetAimingLocation();

    /// <summary>
    /// ���� �����̼� ������ ���� �����մϴ�.
    /// </summary>
    public abstract Vector3 GetOffsetAimingRotation();

    /// <summary>
    /// FOV ���� �������� �����մϴ�
    /// </summary>
    public abstract float GetFieldOfViewMutiplierAim();
    /// <summary>
    /// ���� ���� FOV�������� �����մϴ�.
    /// </summary>
    public abstract float GetFieldOfViewMutiplierAimWeapon();

    /// <summary>
    /// �������̽��� ���� Sprite���� �����մϴ�.
    /// </summary>
    public abstract Sprite GetSprite();

    /// <summary>
    /// �� �������� �����ϴ� ���� ���� ��鸲�� ���� ���� �����մϴ�.
    /// </summary>
    public abstract float GetSwayMutiplier();

    #endregion

    #region METHODS
    /// <summary>
    /// �������� ����ϴ� ĳ���Ͱ� �������� ������ �� ȣ��˴ϴ�.
    /// </summary>
    public abstract void OnAim();

    /// <summary>
    /// �������� ����ϴ� ĳ���Ͱ� ������ ������ ������ �� ȣ��˴ϴ�.
    /// </summary>
    public abstract void OnAimStop();

    /// <summary>
    /// ������ �������� ���ϴ�.
    /// </summary>
    public abstract void FPScopeRenOff();

    /// <summary>
    /// ������ ī�޶� ���ϴ�.
    /// </summary>
    public abstract void RenDisable();
    #endregion
}
