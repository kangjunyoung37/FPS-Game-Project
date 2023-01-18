using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioManagerService : IGameService
{
   
    /// <summary>
    /// ����� Ŭ���� �ѹ� ����մϴ�.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="settings">����� ����</param>
    void PlayOneShot(AudioClip clip, AudioSettings settings = default);


    /// <summary>
    /// �����ð��� ������ ����� Ŭ���� ����Ѵ�.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="settings">����� ����</param>
    /// <param name="delay">����� ������</param>
    void PlayOneShotDelayed(AudioClip clip, AudioSettings settings = default,float delay = 1.0f);


    /// <summary>
    /// ���ϴ� ��ġ���� ����� Ŭ���� ����մϴ�.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="position">��ġ</param>
    /// <param name="settings">����� ����</param>
    void PlayOneShotPosition(AudioClip clip, Vector3 position, AudioSettings settings = default);

}
