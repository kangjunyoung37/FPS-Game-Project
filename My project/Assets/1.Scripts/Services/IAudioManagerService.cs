using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioManagerService : IGameService
{
   
    /// <summary>
    /// 오디오 클립을 한번 재생합니다.
    /// </summary>
    /// <param name="clip">오디오 클립</param>
    /// <param name="settings">오디오 세팅</param>
    void PlayOneShot(AudioClip clip, AudioSettings settings = default);


    /// <summary>
    /// 지연시간을 가지고 오디오 클립을 재생한다.
    /// </summary>
    /// <param name="clip">오디오 클립</param>
    /// <param name="settings">오디오 세팅</param>
    /// <param name="delay">오디오 딜레이</param>
    void PlayOneShotDelayed(AudioClip clip, AudioSettings settings = default,float delay = 1.0f);


    /// <summary>
    /// 원하는 위치에서 오디오 클립을 재생합니다.
    /// </summary>
    /// <param name="clip">오디오 클립</param>
    /// <param name="position">위치</param>
    /// <param name="settings">오디오 세팅</param>
    void PlayOneShotPosition(AudioClip clip, Vector3 position, AudioSettings settings = default);

}
