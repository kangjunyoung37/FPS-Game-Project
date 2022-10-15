using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserType { LaserSight, Flashlight }

public class Laser : LaserBehaviour
{
    #region FIELDS SERIALIZED
    [Title(label: "Settings")]

    [Tooltip("����� �������̽��� ���� Sprite")]
    [SerializeField]
    private Sprite sprite;

    [Tooltip("������ Ÿ��")]
    [SerializeField]
    private LaserType laserType;

    [Tooltip("������ ����Ʈ�� ų �� �ִٸ� ���� ��ȯ")]
    [SerializeField]
    private bool active = true;

    [Tooltip("True�� ĳ���Ͱ� �޸��� ���� �������� �ڵ����� �����ϴ�")]
    [SerializeField]
    private bool turnOffWhileRunning = true;

    [Tooltip("True�� ĳ���Ͱ� ������ �ϴ� ���� �������� �ڵ����� �����ϴ�")]
    [SerializeField]
    private bool turnOffWhileAiming = true;

    [Title(label: "Audio")]
    [Tooltip("�������� ����ϴ� ���� �÷��̵Ǵ� AudioClip")]
    [SerializeField]
    private AudioClip toggleClip;

    [Tooltip("���Ŭ���� ���Ǵ� ����� ����")]
    [SerializeField]
    private InfimaGames.LowPolyShooterPack.AudioSettings toggleAudioSettings;
    [Title(label: "Expandes Settings")]

    [Tooltip("�������� Transform")]
    [SerializeField]
    private Transform laserTransform;

    [ShowIf("laserType", LaserType.LaserSight)]
    [Tooltip("������ ���� �β��� �����մϴ�")]
    [SerializeField]
    private float beamThickness = 1.2f;
    [ShowIf("laserType", LaserType.LaserSight)]
    [Tooltip("������ ���� �ִ�� �� �� �ִ� �Ÿ�")]
    [SerializeField]
    private float beamMaxDistance = 500.0f;

    #endregion
    #region FIELDS

    /// <summary>
    /// ���� �θ� ������Ʈ
    /// </summary>
    private Transform beamParent;
    #endregion

    #region GETTERS

    public override Sprite GetSprite() => sprite;

    public override bool GetTurnOffWhileRunning() => turnOffWhileRunning;

    public override bool GetTurnOffWhileAiming() => turnOffWhileAiming;

    #endregion

    #region METHODS

    public override void Toggle()
    {
        active = !active;
        //������
        Reapply();

        if(toggleClip != null)
        {

        }
    }
    public override void Reapply()
    {
        if(laserTransform != null)
        {
            laserTransform.gameObject.SetActive(active);
        }
    }
    public override void Hide()
    {
        if(laserTransform != null)
        {
            laserTransform.gameObject.SetActive(false);
        }
    }
    #endregion

    #region UNITY
    private void Awake()
    {
        if (laserTransform == null)
            return;
        beamParent = laserTransform.parent;
    }
    private void Update()
    {
        if (laserTransform == null)
            return;
        float targetScale = beamMaxDistance;
        if (Physics.Raycast(new Ray(laserTransform.position,beamParent.forward),out RaycastHit hit,beamMaxDistance))
        {
            targetScale = hit.distance * 5.0f;
        }
        beamParent.localScale = new Vector3(beamThickness, beamThickness, targetScale);
    }
    #endregion



}
