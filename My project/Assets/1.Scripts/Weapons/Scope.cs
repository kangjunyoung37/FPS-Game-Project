using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : ScopeBehaviour
{
    #region FIELDS SERIALZED
    [Title(label: "Mutipliers")]

    [Tooltip("�� �������� ���ص��� ���콺 ������ ���� ��")]
    [SerializeField]
    private float multiplierMouseSensitivity = 0.8f;

    [Tooltip("�� �������� �����ϴ� ���� ������ ������ ���� ��")]
    [SerializeField]
    private float multiplierSpread = 0.1f;

    [Title(label: "Interface")]
    [Tooltip("�������̽� Sprite")]
    [SerializeField]
    private Sprite sprite;

    [Title(label: "Sway")]

    [Tooltip("�� �������� �����ϸ鼭 ������ ��鸲�� ���ϴ� ��")]
    [SerializeField]
    private float swayMultiplier = 1.0f;

    [Title(label: "Aiming Offset")]

    [Tooltip("���� �� ���� �� ��ġ ������ ��")]
    [SerializeField]
    private Vector3 offsetAimingLocation;

    [Tooltip("���� �� ���� �� �����̼� ������ ��")]
    [SerializeField]
    private Vector3 offsetAimingRotation;

    [Title(label: "Field Of View")]

    [Tooltip("���� FOV ���� ��")]
    [SerializeField]
    private float fieldOfViewMutiplierAim = 0.9f;

    [Tooltip("���� ���� FOV ���� ��")]
    [SerializeField]
    private float fieldOfViewMultiplierAimWeapon = 0.7f;

    [Title(label: "Material")]

    [Tooltip("�������� ���� �� �������� ������ material�� �ε���")]
    [SerializeField]
    private int materialIndex = 3;

    [Tooltip("�������� ������ �������� ���� Material")]
    [SerializeField]
    private Material materialHiddnen;
    #endregion

    #region FIELDS

    //Mesh Renderer
    private MeshRenderer meshRenderer;

    /// <summary>
    /// �⺻ ������ material�Դϴ�. �����ӿ� ����Ǳ� ������ �ٽ� ������ �� �ֵ��� �����մϴ�.
    /// </summary>
    private Material materialDefault;
    #endregion

    #region UNITY
    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (!HasMaterialIndex())
            return;

        materialDefault = meshRenderer.materials[materialIndex];
    }

    private void Start()
    {
        OnAimStop();
    }
    #endregion

    #region GETTERS
    public override float GetMutiplierMouseSensitivity() => multiplierMouseSensitivity;
    public override float GetMultiplierSpread() => multiplierSpread;
    public override Vector3 GetOffsetAimingLocation() => offsetAimingLocation;
    public override Vector3 GetOffsetAimingRotation() => offsetAimingRotation;
    public override float GetFieldOfViewMutiplierAim() => fieldOfViewMutiplierAim;
    public override float GetFieldOfViewMutiplierAimWeapon() => fieldOfViewMultiplierAimWeapon;
    public override Sprite GetSprite() => sprite;
    public override float GetSwayMutiplier() => swayMultiplier;

    /// <summary>
    /// ������ �޽� �������� material index�� ���� �� ������ true�� �����մϴ�.
    /// </summary>
    private bool HasMaterialIndex()
    {
        if(meshRenderer == null)
            return false;
        return materialIndex < meshRenderer.materials.Length && materialIndex >= 0;
    }

    #endregion
    #region METHODS
    public override void OnAim()
    {
        if(!HasMaterialIndex())
        {
            return;
        }
        //Material�� �����´�
        Material[] materials = meshRenderer.materials;
        //default material�� �����մϴ�.
        materials[materialIndex] = materialDefault;
        //������Ʈ
        meshRenderer.materials = materials;
    }
    public override void OnAimStop()
    {
        if (!HasMaterialIndex())
            return;
        Material[] materials = meshRenderer.materials;
        //material �����
        materials[materialIndex] = materialHiddnen;
        meshRenderer.materials = materials;

    }
    #endregion
}
