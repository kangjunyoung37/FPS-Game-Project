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

    [Tooltip("캐리터 애니메이터 컴포넌트")]
    [SerializeField, NotNull]
    private Animator animator;

    [Tooltip("캐릭터의 인벤토리 컴포넌트")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;



    [Tooltip("오디오 클립")]
    [SerializeField, NotNull]
    private AudioClip audioClip;
    #endregion

    #region FIEDLS

    /// <summary>
    /// 장착된 레이저
    /// </summary>
    private LaserBehaviour laserBehaviour;

    /// <summary>
    /// 마지막 프레임에 조준중이였는가
    /// </summary>
    private bool wasAiming;

    /// <summary>
    /// 마지막 프레임에 달리는 중이였는가
    /// </summary>
    private bool wasRunning;
    
    /// <summary>
    /// 오디오 소스
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
            Debug.LogError($"{this.gameObject}에 animator = {animator} inventoryBehaviour = {inventoryBehaviour}입니다");
            return;
        }
        //인벤토리에서 현재 장착된 무기를 가져옵니다.
        WeaponBehaviour weaponBehaviour = inventoryBehaviour.GetEquipped();
        if (weaponBehaviour == null)
            return;

        //현재 장착된 무기에서 레이저를 가져옵니다.
        laserBehaviour = weaponBehaviour.GetAttachmentManager().GetEquippedLaser();
        if (laserBehaviour == null)
            return;

        //지금 조준중인지
        bool aiming = animator.GetBool(AHashes.Aim);

        //달리고 있는 중인지
        bool running = animator.GetBool(AHashes.Running);

        //만약 조준을 시작하고 있다면
        if(aiming && !wasAiming)
        {
            if (laserBehaviour.GetTurnOffWhileAiming())
                laserBehaviour.Hide();
        }

        //캐릭터가 조준을 멈추고 있다면
        else if(!aiming && wasAiming)
        {
            if (laserBehaviour.GetTurnOffWhileAiming())
                laserBehaviour.Reapply();
        }
        //캐릭터가 달리기 시작하는 중이라면
        if(running && !wasRunning)
        {
            if (laserBehaviour.GetTurnOffWhileRunning())
                laserBehaviour.Hide();
        }

        //달리기를 멈추는 중이라면
        else if(!running && wasRunning)
        {
            if (laserBehaviour.GetTurnOffWhileRunning())
                laserBehaviour.Reapply();
        }

        wasAiming = aiming;
        wasRunning = running;
    }

    /// <summary>
    /// 토글방식
    /// </summary>
    [PunRPC]
    private void Toggle()
    {
        laserBehaviour.Toggle();
        audioSorce.Play();
    }
    #endregion
}
