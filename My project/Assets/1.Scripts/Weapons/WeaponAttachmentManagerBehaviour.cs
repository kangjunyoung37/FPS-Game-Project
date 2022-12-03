using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAttachmentManagerBehaviour : MonoBehaviour
{

    #region UNITY FUNCTIONS
    protected virtual void Awake() { }
   
    protected virtual void Start() { }
   
    protected virtual void Update() { }

    protected virtual void LateUpdate() { }

    #endregion

    #region GETTERS

    /// <summary>
    /// ������ �������� �����մϴ�.
    /// </summary>
    public abstract ScopeBehaviour GetEquippedScope();

    /// <summary>
    /// ������ ������ default���� �����մϴ�.
    /// </summary>
    public abstract ScopeBehaviour GetEquippedScopeDefault();

    /// <summary>
    /// ������ źâ�� �����մϴ�.
    /// </summary>
    public abstract MagazineBehaviour GetEquippedMagazine();

    /// <summary>
    /// ������ �ѱ��� �����մϴ�.
    /// </summary>
    public abstract MuzzleBehaviour GetEquippedMuzzle();

    /// <summary>
    /// ������ �������� �����մϴ�.
    /// </summary>
    public abstract LaserBehaviour GetEquippedLaser();
    
    /// <summary>
    /// ������ �׸��� �����մϴ�.
    /// </summary>
    public abstract GripBehaviour GetEquippedGrip();

    /// <summary>
    /// �ѱ� �ε����� �����մϴ�.
    /// </summary>
    public abstract int GetEquippedMuzzleIndex();
    
    /// <summary>
    /// �� ������ �ε����� �����մϴ�.
    /// </summary>
    public abstract int GetEquippedScopeIndex();

    /// <summary>
    /// ������ �ε����� �����մϴ�.
    /// </summary>
    public abstract int GetEquippedLaserIndex();
    
    /// <summary>
    /// �׸� �ε����� �����մϴ�.
    /// </summary>
    public abstract int GetEquippedGripIndex();

    /// <summary>
    /// ��� �׸� Renderer�� ���ϴ�.
    /// </summary>
    public abstract void FPGripsOff();

    /// <summary>
    /// ��� ������ Renderer�� ���ϴ�.
    /// </summary>
    public abstract void FPScopesOff();

    /// <summary>
    /// ��� �ѱ� Renderer�� ���ϴ�.
    /// </summary>
    public abstract void FPMuzzlesOff();

    /// <summary>
    /// ��� ������ Renderer�� ���ϴ�.
    /// </summary>
    public abstract void FPLasersOff();

    /// <summary>
    /// ��� źâ Renderer�� ���ϴ�.
    /// </summary>
    public abstract void FPMagazinesOff();

    #endregion
}

