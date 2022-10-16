using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : ScopeBehaviour
{
    #region FIELDS SERIALZED
    [Title(label: "Mutipliers")]

    [Tooltip("이 스코프로 조준동안 마우스 감도에 곱할 값")]
    [SerializeField]
    private float multiplierMouseSensitivity = 0.8f;

    [Tooltip("이 스코프를 조준하는 동안 무기의 퍼짐에 곱할 값")]
    [SerializeField]
    private float multiplierSpread = 0.1f;

    [Title(label: "Interface")]
    [Tooltip("인터페이스 Sprite")]
    [SerializeField]
    private Sprite sprite;

    [Title(label: "Sway")]

    [Tooltip("이 스코프를 조준하면서 무기의 흔들림에 곱하는 값")]
    [SerializeField]
    private float swayMultiplier = 1.0f;

    [Title(label: "Aiming Offset")]

    [Tooltip("조준 중 무기 뼈 위치 오프셋 값")]
    [SerializeField]
    private Vector3 offsetAimingLocation;

    [Tooltip("조준 중 무기 뼈 로케이션 오프셋 값")]
    [SerializeField]
    private Vector3 offsetAimingRotation;

    [Title(label: "Field Of View")]

    [Tooltip("에임 FOV 곱셈 값")]
    [SerializeField]
    private float fieldOfViewMutiplierAim = 0.9f;

    [Tooltip("무기 에임 FOV 곱셈 값")]
    [SerializeField]
    private float fieldOfViewMultiplierAimWeapon = 0.7f;

    [Title(label: "Material")]

    [Tooltip("조준하지 않을 때 숨겨지는 스코프 material의 인덱스")]
    [SerializeField]
    private int materialIndex = 3;

    [Tooltip("조준하지 않을때 스코프를 막는 Material")]
    [SerializeField]
    private Material materialHiddnen;
    #endregion

    #region FIELDS

    //Mesh Renderer
    private MeshRenderer meshRenderer;

    /// <summary>
    /// 기본 스코프 material입니다. 런터임에 변경되기 때문에 다시 적용할 수 있도록 저장합니다.
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
    /// 스코프 메쉬 렌더러에 material index가 있을 수 있으면 true를 리턴합니다.
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
        //Material를 가져온다
        Material[] materials = meshRenderer.materials;
        //default material을 복원합니다.
        materials[materialIndex] = materialDefault;
        //업데이트
        meshRenderer.materials = materials;
    }
    public override void OnAimStop()
    {
        if (!HasMaterialIndex())
            return;
        Material[] materials = meshRenderer.materials;
        //material 숨기기
        materials[materialIndex] = materialHiddnen;
        meshRenderer.materials = materials;

    }
    #endregion
}
