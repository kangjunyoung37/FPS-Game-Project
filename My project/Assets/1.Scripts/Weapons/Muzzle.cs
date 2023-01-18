using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MuzzleBehaviour
{
    #region FIELDS SERIALIZE
    [Title(label: "Settings")]

    [Tooltip("총구의 위치")]
    [SerializeField]
    private Transform socket;

    [Tooltip("인터페이스에 쓰일 Sprite")]
    [SerializeField]
    private Sprite sprite;

    [Tooltip("발사할때 플레이되는 오디오 클립")]
    [SerializeField]
    private AudioClip audioClipFire;

    [SerializeField]
    private float shotSoundRange;

    [Title(label: "Particles")]

    [Tooltip("발사 파티클")]
    [SerializeField]
    private GameObject prefabFlashParticles;

    [Tooltip("발사시 방출할 파티클 수")]
    [SerializeField]
    private int flashParticlesCount = 5;

    [Title(label: "Flash Light")]
    [Tooltip("발사할때 사용되는 Flash Light")]
    [SerializeField]
    private GameObject prefabFlashLight;

    [Tooltip("플래시 라이트가 켜져있는 시간")]
    [SerializeField]
    private float flashLightDuration;

    [Tooltip("라이트에 적용되는 로컬 오프셋값")]
    [SerializeField]
    private Vector3 flashLightOffset;

    [Title(label: "Renderer")]
    
    [Tooltip("총구의 Renderer")]
    [SerializeField]
    private Renderer MuzzleRender;

    #endregion
    #region FIELDS

    private ParticleSystem particles;

    private Light flashlight;

  

    #endregion

    #region UNITY FUNCTIONS
    private void Awake()
    {
        MuzzleRender = GetComponent<Renderer>();
        if(prefabFlashParticles != null)
        {

            //파티클 생성
            GameObject spawnedParticlesPrefab = Instantiate(prefabFlashParticles, socket);
            //포지션 초기화
            spawnedParticlesPrefab.transform.localPosition = default;
            //로테이션 초기화
            spawnedParticlesPrefab.transform.localEulerAngles = default;
            //파티클 시스템 가져오기
            particles = spawnedParticlesPrefab.GetComponent<ParticleSystem>();

        }
        if(prefabFlashLight)
        {
            //플래쉬 라이트 생성
            GameObject spawnedFlashLightPrefab = Instantiate(prefabFlashLight, socket);

            //불빛 위치 초기화
            spawnedFlashLightPrefab.transform.localPosition = flashLightOffset;

            //불빛 로테이션 초기화
            spawnedFlashLightPrefab.transform.localEulerAngles = default;

            flashlight = spawnedFlashLightPrefab.GetComponent<Light>();

            flashlight.enabled = false;
        }

    }
    #endregion

    #region GETTERS
    public override void Effect()
    {
        if (particles != null)
            particles.Emit(flashParticlesCount);
        if(flashlight != null)
        {
            flashlight.enabled = true;
            StartCoroutine(nameof(DisableLight));
        }
    }

    public override Transform GetSocket() => socket;
    public override Sprite GetSprite() => sprite;
    public override AudioClip GetAudioClipFire() => audioClipFire;
    public override ParticleSystem GetParticleFire() => particles;
    public override int GetParticleFireCount() => flashParticlesCount;
    public override Light GetFlashLight() => flashlight;
    public override float GetFlashLightDuration() => flashLightDuration;
    public override float GetShotSoundRange() => shotSoundRange;
    #endregion

    #region METHODS

    private IEnumerator DisableLight()
    {
        yield return new WaitForSeconds(flashLightDuration);
        flashlight.enabled = false;
    }

    public override void FPMuzzleOff()
    {
        MuzzleRender.enabled = false;
    }

    #endregion
}
