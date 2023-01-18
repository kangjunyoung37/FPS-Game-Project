using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MuzzleBehaviour : MonoBehaviour
{
    #region GETTERS

    /// <summary>
    /// �ѼҸ� ����
    /// </summary>
    public abstract float GetShotSoundRange();

    /// <summary>
    /// �ѱ��� ��ġ�� �����մϴ�.
    /// </summary>
    public abstract Transform GetSocket();

    /// <summary>
    /// �������̽� ���� �ѱ� Sprite�� �����մϴ�.
    /// </summary>
    public abstract Sprite GetSprite();

    /// <summary>
    /// �߻��Ҷ� ���� ���带 �����մϴ�.
    /// </summary>
    public abstract AudioClip GetAudioClipFire();

    /// <summary>
    /// �߻��Ҷ� ����ϱ� ���� Particle�� �����մϴ�.
    /// </summary>
    public abstract ParticleSystem GetParticleFire();

    /// <summary>
    /// �߻��� �� ������ ������ ���� �����մϴ�.
    /// </summary>
    public abstract int GetParticleFireCount();

    /// <summary>
    /// �߻��Ҷ� ���Ǵ� light ������Ʈ�� �����մϴ�.
    /// </summary>
    public abstract Light GetFlashLight();

    /// <summary>
    /// �÷����� ������µ� �ɸ��� �ð��� �����մϴ�.
    /// </summary>
    public abstract float GetFlashLightDuration();
    #endregion

    #region METHODS
    /// <summary>
    /// ��� �ѱ� ȿ���� �����ŵ�ϴ�.
    /// </summary>
    public abstract void Effect();

    /// <summary>
    /// �ѱ� �������� ���ϴ�.
    /// </summary>
    public abstract void FPMuzzleOff();
    #endregion
}
