using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("ĳ������ ������ ������Ʈ")]
    [SerializeField, NotNull]
    private MovementBehaviour movementBehaviour;

    [Tooltip("ĳ������ �ִϸ����� ������Ʈ")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Tooltip("ĳ������ �߼Ҹ� ���� ����� �ҽ�")]
    [SerializeField, NotNull]
    private AudioSource audioSource;

    [Title(label: "Settings")]

    [Tooltip("����� Ŭ���� ����Ǵ� �ּ� �̵� �ӵ�")]
    [SerializeField]
    private float minVelocityMagnitude = 1.0f;

    [Title(label: "Audio Clips")]

    [Tooltip("�ȴ� ���� ����Ǵ� ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipWalking;

    [Tooltip("�ٴ� ���� ����Ǵ� ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipRunning;

    #endregion

    #region FIELD

    private PhotonView PV;

    #endregion


    #region UNITY

    private void Awake()
    {
        if(audioSource != null)
        {
            audioSource.clip = audioClipWalking;
            audioSource.loop = true;
        }
        PV = gameObject.GetComponent<PhotonView>();
  
    }

    private void Update()
    {
        if(characterAnimator == null || movementBehaviour == null || audioSource == null)
        {
            Debug.LogError($"{this.gameObject}�� characterAnimator = {characterAnimator} ,movementBehaviour = {movementBehaviour},audioSource = {audioSource}�Դϴ�");

            return;
        }

        if(!PV.IsMine)
        {

            PlayWalkSound(movementBehaviour.GetPVIsGrounded() ,movementBehaviour.GetPVVelocity());
        }
        else
        {
            PlayWalkSound(movementBehaviour.IsGrounded(),movementBehaviour.GetVelocity());
        }

    }


    #endregion

    #region GETTERS

    public Transform GetTransform => movementBehaviour.transform;

    #endregion

    #region METHODS

    private void PlayWalkSound(bool IsGrounded ,Vector3 charactervelocity)
    {
        if (IsGrounded && charactervelocity.sqrMagnitude > minVelocityMagnitude)
        {
            audioSource.clip = characterAnimator.GetBool(AHashes.Running) ? audioClipRunning : audioClipWalking;
            if (!audioSource.isPlaying)
                audioSource.Play();

        }
        else if (audioSource.isPlaying)
            audioSource.Pause();

    }
    #endregion
}
