using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사운드 동작을 재생합니다. 오디오 서비스를 사용하여 오디오 클립을 재생합니다.
/// </summary>
public class PlaySoundBehaviour : StateMachineBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "Setup")]

    [Tooltip("오디오 클립")]
    [SerializeField]
    private AudioClip clip;

    [Title(label: "Settings")]

    [Tooltip("오디오 세팅")]
    [SerializeField]
    private AudioSettings settings = new AudioSettings(1.0f, 0.0f, true);

    private IAudioManagerService audioManagerService;

    #endregion

    #region UNITY

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioManagerService ??= ServiceLocator.Current.Get<AudioManagerService>();
        audioManagerService?.PlayOneShot(clip, settings);
    }

    #endregion
}
