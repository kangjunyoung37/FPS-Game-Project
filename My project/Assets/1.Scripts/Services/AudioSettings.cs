using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오디오매니저서비스에 사용되는 오디오 세팅
/// </summary>
[System.Serializable]
public struct AudioSettings 
{

    public bool AutomaticCleanup => automaticCleanup;

    public float Volume => volume;

    public float SpatialBlend => spatialBlend;

    [Header("Settings")]
    [Tooltip("True인 경우 생성된 모든 AudioSource는 재생이 완료된 후 제거됩니다.")]
    [SerializeField]
    private bool automaticCleanup;

    [Tooltip("볼륨")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float volume;

    [Tooltip("Spatial Blend")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float spatialBlend;

    public AudioSettings(float volume = 1.0f, float spatialBlend = 0.0f,bool automaticCleanUp = true)
    {
        this.volume = volume;
        this.spatialBlend = spatialBlend;
        this.automaticCleanup = automaticCleanUp;
    }





}
