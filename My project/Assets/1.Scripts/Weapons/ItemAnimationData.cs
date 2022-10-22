using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���⺰ ���� �����Ϳ� ���õ� ��� ������ �����մϴ�.
/// Item Offsets,Lowerd Data,Leaning Data,Camera RecoilData,Item RecoilData
/// </summary>
public class ItemAnimationData : ItemAnimationDataBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "Item Offsets")]

    [Tooltip("������ �����͸� �����ϰ� �ִ� ItemOffsets")]
    [SerializeField, InLineEditor]
    private ItemOffsets itemOffsets;

    [Title(label: "Lowerd Data")]

    [Tooltip("������ ���� �ڼ��� �����ϴ� �� �ʿ��� ��� �����Ͱ� ���ԵǾ� �ֽ��ϴ�")]
    [SerializeField, InLineEditor]
    private LowerData lowerData;

    [Title(label: "Leaning Data")]

    [Tooltip("ĳ���Ͱ� ���� �ִ� ���� ���Ⱑ �󸶳� ���������ϴ��� ���� ��� ������ ���ԵǾ� �ֽ��ϴ�.")]
    [SerializeField, InLineEditor]
    private LeaningData leaningData;

    [Title(label: "Camera Recoil Data")]

    [Tooltip("���� �ݵ� ������. ���⿡ ���� ī�޶� �ݵ� ���� ��� �� ���˴ϴ�.")]
    [SerializeField, InLineEditor]
    private RecoilData cameraRecoilData;

    [Title(label: "Weapon Recoil Data")]

    [Tooltip("���� �ݵ� ������. ���⿡ ���� �ݵ� ���� ��� �� ���˴ϴ�. ")]
    [SerializeField, InLineEditor]
    private RecoilData waeponRecoilData;

    #endregion

    #region GETTERS

   
    public override RecoilData GetCameraRecoilData() => cameraRecoilData;

    public override RecoilData GetWeaponRecoilData() => waeponRecoilData;

    public override RecoilData GetRecoilData(MotionType motionType) => motionType == MotionType.Item ? GetWeaponRecoilData() : GetCameraRecoilData();

    public override LowerData GetLowerData() => lowerData;

    public override LeaningData GetLeaningData() => leaningData;

    public override ItemOffsets GetItemOffsets() => itemOffsets;

    #endregion

}
