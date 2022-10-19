using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���� ������
/// �÷��̾ �������鼭 ī�޶�� ���� ���� �������� �۵��ϰ� �ϴ� �Ͱ� ���õ� �����͵�
/// </summary>
[CreateAssetMenu(fileName ="SO_Leaning_Name",menuName ="FPS Game/Shooter Pack/Leaning Data",order = 0)]
public class LeaningData : ScriptableObject
{
    #region FIELDS SERIALZED
    [Title(label: "Item Curves")]
    [Tooltip("ĳ���Ͱ� �����ϴ� ���� ����� �� �����ۿ��� ����Ǵ� �ִϸ��̼� �")]
    [SerializeField, InLineEditor]
    private ACurves itemAiming;

    [Tooltip("ĳ���Ͱ� �� �ִ� ���� ����� �� �����ۿ��� ����Ǵ� �ִϸ��̼� �")]
    [SerializeField, InLineEditor]
    private ACurves itemStading;

    [Title(label: "Camera Curves")]

    [Tooltip("ĳ���Ͱ� �����ϴ� ���� ����� �� ī�޶󿡼� ����Ǵ� �ִϸ��̼� �")]
    [SerializeField, InLineEditor]
    private ACurves cameraAiming;

    [Tooltip("ĳ���Ͱ� �� �ִ� ���� ����� �� ī�޶󿡼� ����Ǵ� �ִϸ��̼� �")]
    [SerializeField, InLineEditor]
    private ACurves cameraStanding;

    #endregion

    #region FUNCTIONS

    public ACurves GetCurves(MotionType motionType, bool aiming = false)
    {
        return motionType switch
        {
            MotionType.Camera => aiming ? cameraAiming : cameraStanding,
            MotionType.Item => aiming ? itemAiming : itemStading,
            _ => itemStading
        };
    }

    #endregion

}
