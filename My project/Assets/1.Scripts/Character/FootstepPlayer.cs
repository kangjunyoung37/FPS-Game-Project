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

    #region UNITY

    private void Awake()
    {
        if(audioSource != null)
        {
            audioSource.clip = audioClipWalking;
            audioSource.loop = true;
        }
    }

    private void Update()
    {
        if(characterAnimator == null || movementBehaviour == null || audioSource == null)
        {
            Debug.LogError($"{this.gameObject}�� characterAnimator = {characterAnimator} ,movementBehaviour = {movementBehaviour},audioSource = {audioSource}�Դϴ�");

            return;
        }

        //������ �����̴� üũ
        if (movementBehaviour.IsGrounded() && movementBehaviour.GetVelocity().sqrMagnitude > minVelocityMagnitude)
        {
            audioSource.clip = characterAnimator.GetBool(AHashes.Running) ? audioClipWalking : audioClipRunning;
            if (!audioSource.isPlaying)
                audioSource.Play();

        }
        else if (audioSource.isPlaying)
            audioSource.Pause();


    }




    #endregion
}
