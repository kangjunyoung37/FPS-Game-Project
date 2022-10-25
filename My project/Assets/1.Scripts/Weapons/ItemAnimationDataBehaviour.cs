using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

/// <summary>
/// Recoil 클래스에 대한 모든 정의와 LowerData,LeaningData,ItemOffset등을 포함하는 추상클래스입니다.
/// </summary>
public abstract class ItemAnimationDataBehaviour : MonoBehaviour
{
    #region GETTERS

    /// <summary>
    /// 카메라에 사용되는 RecoilData를 리턴합니다.
    /// </summary>
    public abstract RecoilData GetCameraRecoilData();

    /// <summary>
    /// 무기에 사용되는 RecoilData를 리턴합니다.
    /// </summary>
    public abstract RecoilData GetWeaponRecoilData();

    /// <summary>
    /// MotionType에 따라 RecoilData 값을 반환합니다.
    /// </summary>
    /// <param name="motionType">모션타입 카메라,아이템</param>
    public abstract RecoilData GetRecoilData(MotionType motionType);

    /// <summary>
    /// 무기의 낮은 자세를 설정하는 데 필요한 모든 데이터를 반환합니다. 
    /// </summary>
    public abstract LowerData GetLowerData();

    /// <summary>
    /// 장착 무기에 적용하는 데 필요한 LeaningData를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public abstract LeaningData GetLeaningData();

    /// <summary>
    /// 모든 아이템에 적절한 오프셋을 적용하는 데 필요한 
    /// ItemOffsets을 반환합니다.
    /// </summary>
    public abstract ItemOffsets GetItemOffsets();

    #endregion
}
