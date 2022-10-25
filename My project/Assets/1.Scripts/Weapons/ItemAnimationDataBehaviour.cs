using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

/// <summary>
/// Recoil Ŭ������ ���� ��� ���ǿ� LowerData,LeaningData,ItemOffset���� �����ϴ� �߻�Ŭ�����Դϴ�.
/// </summary>
public abstract class ItemAnimationDataBehaviour : MonoBehaviour
{
    #region GETTERS

    /// <summary>
    /// ī�޶� ���Ǵ� RecoilData�� �����մϴ�.
    /// </summary>
    public abstract RecoilData GetCameraRecoilData();

    /// <summary>
    /// ���⿡ ���Ǵ� RecoilData�� �����մϴ�.
    /// </summary>
    public abstract RecoilData GetWeaponRecoilData();

    /// <summary>
    /// MotionType�� ���� RecoilData ���� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="motionType">���Ÿ�� ī�޶�,������</param>
    public abstract RecoilData GetRecoilData(MotionType motionType);

    /// <summary>
    /// ������ ���� �ڼ��� �����ϴ� �� �ʿ��� ��� �����͸� ��ȯ�մϴ�. 
    /// </summary>
    public abstract LowerData GetLowerData();

    /// <summary>
    /// ���� ���⿡ �����ϴ� �� �ʿ��� LeaningData�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public abstract LeaningData GetLeaningData();

    /// <summary>
    /// ��� �����ۿ� ������ �������� �����ϴ� �� �ʿ��� 
    /// ItemOffsets�� ��ȯ�մϴ�.
    /// </summary>
    public abstract ItemOffsets GetItemOffsets();

    #endregion
}
