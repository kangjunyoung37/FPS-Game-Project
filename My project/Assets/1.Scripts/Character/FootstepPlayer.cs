using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("캐릭터의 움직임 컴포넌트")]
    [SerializeField, NotNull]
    private MovementBehaviour movementBehaviour;

    [Tooltip("캐릭터의 애니메이터 컴포넌트")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Tooltip("캐릭터의 발소리 전용 오디오 소스")]
    [SerializeField, NotNull]
    private AudioSource audioSource;

    [Title(label: "Settings")]

    [Tooltip("오디오 클립이 재생되는 최소 이동 속도")]
    [SerializeField]
    private float minVelocityMagnitude = 1.0f;

    [Title(label: "Audio Clips")]

    [Tooltip("걷는 동안 재생되는 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipWalking;

    [Tooltip("뛰는 동안 재생되는 오디오 클립")]
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
            Debug.LogError($"{this.gameObject}에 characterAnimator = {characterAnimator} ,movementBehaviour = {movementBehaviour},audioSource = {audioSource}입니다");

            return;
        }

        //땅에서 움직이는 체크
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
