using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManagerService : MonoBehaviour , IAudioManagerService
{
    /// <summary>
    /// OneShot ����� �÷��̿� ������ �����͸� �����ϰ� ����
    /// </summary>
    private readonly struct OneShotCoroutine
    {
        public AudioClip Clip { get; }

        public AudioSettings Settings { get; }

        public float Delay { get; }
        
        public OneShotCoroutine(AudioClip clip, AudioSettings settings,float delay)
        {
            Clip = clip;
            Settings = settings;
            Delay = delay;
        }
    }
    /// <summary>
    /// ����� �ҽ��� ����ǰ� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="source">����� �ҽ�</param>
    private bool IsPlayingSource(AudioSource source)
    {
        if(source == null)
            return false;
        
        return source.isPlaying;
    }

    /// <summary>
    /// ����� ������ ����� �ҽ��� �����մϴ�.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private IEnumerator DestroySourceWhenFinished(AudioSource source)
    {
        yield return new WaitWhile(() => IsPlayingSource(source));

        if(source != null)
        {
            DestroyImmediate(source.gameObject);
        }
    }

    private IEnumerator PlayOneShotAfterDelay(OneShotCoroutine value)
    {
        yield return new WaitForSeconds(value.Delay);

        PlayOneShot_Internal(value.Clip, value.Settings);
    }

    private void PlayOneShot_Internal(AudioClip clip, AudioSettings settings)
    {
        if (clip == null)
            return;
        var newSourceObject = new GameObject($"Audio Source -> {clip.name}");
        var newAudioSource = newSourceObject.AddComponent<AudioSource>();

        newAudioSource.volume = settings.Volume;

        newAudioSource.spatialBlend = settings.SpatialBlend;

        newAudioSource.PlayOneShot(clip);

        if (settings.AutomaticCleanup)
            StartCoroutine(nameof(DestroySourceWhenFinished), newAudioSource);
    }

    #region Audio Manager Service Interface

    public void PlayOneShot(AudioClip clip, AudioSettings settings)
    {
        PlayOneShot_Internal(clip, settings);
    }

    public void PlayOneShotDelayed(AudioClip clip, AudioSettings settings = default, float delay = 1.0f)
    {
        StartCoroutine(nameof(PlayOneShotAfterDelay), new OneShotCoroutine(clip, settings, delay));
    }
    #endregion
}
