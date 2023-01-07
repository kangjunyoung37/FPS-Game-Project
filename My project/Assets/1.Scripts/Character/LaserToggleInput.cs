using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LaserToggleInput : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("ĳ���� �ִϸ����� ������Ʈ")]
    [SerializeField, NotNull]
    private Animator animator;

    [Tooltip("ĳ������ �κ��丮 ������Ʈ")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;



    [Tooltip("����� Ŭ��")]
    [SerializeField, NotNull]
    private AudioClip audioClip;
    #endregion

    #region FIEDLS

    /// <summary>
    /// ������ ������
    /// </summary>
    private LaserBehaviour laserBehaviour;

    /// <summary>
    /// ������ �����ӿ� �������̿��°�
    /// </summary>
    private bool wasAiming;

    /// <summary>
    /// ������ �����ӿ� �޸��� ���̿��°�
    /// </summary>
    private bool wasRunning;
    
    /// <summary>
    /// ����� �ҽ�
    /// </summary>
    private AudioSource audioSorce;

    #endregion

    #region UNITY

    private void Awake()
    {
        audioSorce = inventoryBehaviour.GetComponent<AudioSource>();
        audioSorce.clip = audioClip;
        
    }

    private void Update()
    {
        
        if(animator == null || inventoryBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}�� animator = {animator} inventoryBehaviour = {inventoryBehaviour}�Դϴ�");
            return;
        }
        //�κ��丮���� ���� ������ ���⸦ �����ɴϴ�.
        WeaponBehaviour weaponBehaviour = inventoryBehaviour.GetEquipped();
        if (weaponBehaviour == null)
            return;

        //���� ������ ���⿡�� �������� �����ɴϴ�.
        laserBehaviour = weaponBehaviour.GetAttachmentManager().GetEquippedLaser();
        if (laserBehaviour == null)
            return;

        //���� ����������
        bool aiming = animator.GetBool(AHashes.Aim);

        //�޸��� �ִ� ������
        bool running = animator.GetBool(AHashes.Running);

        //���� ������ �����ϰ� �ִٸ�
        if(aiming && !wasAiming)
        {
            if (laserBehaviour.GetTurnOffWhileAiming())
                laserBehaviour.Hide();
        }

        //ĳ���Ͱ� ������ ���߰� �ִٸ�
        else if(!aiming && wasAiming)
        {
            if (laserBehaviour.GetTurnOffWhileAiming())
                laserBehaviour.Reapply();
        }
        //ĳ���Ͱ� �޸��� �����ϴ� ���̶��
        if(running && !wasRunning)
        {
            if (laserBehaviour.GetTurnOffWhileRunning())
                laserBehaviour.Hide();
        }

        //�޸��⸦ ���ߴ� ���̶��
        else if(!running && wasRunning)
        {
            if (laserBehaviour.GetTurnOffWhileRunning())
                laserBehaviour.Reapply();
        }

        wasAiming = aiming;
        wasRunning = running;
    }

    /// <summary>
    /// ��۹��
    /// </summary>
    [PunRPC]
    private void Toggle()
    {
        laserBehaviour.Toggle();
        audioSorce.Play();
    }
    #endregion
}
