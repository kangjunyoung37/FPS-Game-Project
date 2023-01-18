using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MuzzleBehaviour
{
    #region FIELDS SERIALIZE
    [Title(label: "Settings")]

    [Tooltip("�ѱ��� ��ġ")]
    [SerializeField]
    private Transform socket;

    [Tooltip("�������̽��� ���� Sprite")]
    [SerializeField]
    private Sprite sprite;

    [Tooltip("�߻��Ҷ� �÷��̵Ǵ� ����� Ŭ��")]
    [SerializeField]
    private AudioClip audioClipFire;

    [SerializeField]
    private float shotSoundRange;

    [Title(label: "Particles")]

    [Tooltip("�߻� ��ƼŬ")]
    [SerializeField]
    private GameObject prefabFlashParticles;

    [Tooltip("�߻�� ������ ��ƼŬ ��")]
    [SerializeField]
    private int flashParticlesCount = 5;

    [Title(label: "Flash Light")]
    [Tooltip("�߻��Ҷ� ���Ǵ� Flash Light")]
    [SerializeField]
    private GameObject prefabFlashLight;

    [Tooltip("�÷��� ����Ʈ�� �����ִ� �ð�")]
    [SerializeField]
    private float flashLightDuration;

    [Tooltip("����Ʈ�� ����Ǵ� ���� �����°�")]
    [SerializeField]
    private Vector3 flashLightOffset;

    [Title(label: "Renderer")]
    
    [Tooltip("�ѱ��� Renderer")]
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

            //��ƼŬ ����
            GameObject spawnedParticlesPrefab = Instantiate(prefabFlashParticles, socket);
            //������ �ʱ�ȭ
            spawnedParticlesPrefab.transform.localPosition = default;
            //�����̼� �ʱ�ȭ
            spawnedParticlesPrefab.transform.localEulerAngles = default;
            //��ƼŬ �ý��� ��������
            particles = spawnedParticlesPrefab.GetComponent<ParticleSystem>();

        }
        if(prefabFlashLight)
        {
            //�÷��� ����Ʈ ����
            GameObject spawnedFlashLightPrefab = Instantiate(prefabFlashLight, socket);

            //�Һ� ��ġ �ʱ�ȭ
            spawnedFlashLightPrefab.transform.localPosition = flashLightOffset;

            //�Һ� �����̼� �ʱ�ȭ
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
