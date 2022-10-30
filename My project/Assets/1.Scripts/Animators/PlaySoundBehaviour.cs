using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ ����մϴ�. ����� ���񽺸� ����Ͽ� ����� Ŭ���� ����մϴ�.
/// </summary>
public class PlaySoundBehaviour : StateMachineBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "Setup")]

    [Tooltip("����� Ŭ��")]
    [SerializeField]
    private AudioClip clip;

    [Title(label: "Settings")]

    [Tooltip("����� ����")]
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
