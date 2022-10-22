using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기별 절차 데이터와 관련된 모든 정보를 저장합니다.
/// Item Offsets,Lowerd Data,Leaning Data,Camera RecoilData,Item RecoilData
/// </summary>
public class ItemAnimationData : ItemAnimationDataBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "Item Offsets")]

    [Tooltip("오프셋 데이터를 포함하고 있는 ItemOffsets")]
    [SerializeField, InLineEditor]
    private ItemOffsets itemOffsets;

    [Title(label: "Lowerd Data")]

    [Tooltip("무기의 낮은 자세를 설정하는 데 필요한 모든 데이터가 포함되어 있습니다")]
    [SerializeField, InLineEditor]
    private LowerData lowerData;

    [Title(label: "Leaning Data")]

    [Tooltip("캐릭터가 기울고 있는 동안 무기가 얼마나 기울어져야하는지 대한 모든 정보가 포함되어 있습니다.")]
    [SerializeField, InLineEditor]
    private LeaningData leaningData;

    [Title(label: "Camera Recoil Data")]

    [Tooltip("무기 반동 데이터. 무기에 대한 카메라 반동 값을 얻는 데 사용됩니다.")]
    [SerializeField, InLineEditor]
    private RecoilData cameraRecoilData;

    [Title(label: "Weapon Recoil Data")]

    [Tooltip("무기 반동 데이터. 무기에 대한 반동 값을 얻는 데 사용됩니다. ")]
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
