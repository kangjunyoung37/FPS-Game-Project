using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ŵ������񽺿� ���Ǵ� ����� ����
/// </summary>
[System.Serializable]
public struct AudioSettings 
{

    public bool AutomaticCleanup => automaticCleanup;

    public float Volume => volume;

    public float SpatialBlend => spatialBlend;

    [Header("Settings")]
    [Tooltip("True�� ��� ������ ��� AudioSource�� ����� �Ϸ�� �� ���ŵ˴ϴ�.")]
    [SerializeField]
    private bool automaticCleanup;

    [Tooltip("����")]
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
