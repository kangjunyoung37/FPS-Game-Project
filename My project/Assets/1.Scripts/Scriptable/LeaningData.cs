using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 기울기 데이터
/// 플레이어가 기울어지면서 카메라와 무기 또한 기울어지게 작동하게 하는 것과 관련된 데이터들
/// </summary>
[CreateAssetMenu(fileName ="SO_Leaning_Name",menuName ="FPS Game/Shooter Pack/Leaning Data",order = 0)]
public class LeaningData : ScriptableObject
{
    #region FIELDS SERIALZED
    [Title(label: "Item Curves")]
    [Tooltip("캐릭터가 조준하는 동안 기울일 때 아이템에서 재생되는 애니메이션 곡선")]
    [SerializeField, InLineEditor]
    private ACurves itemAiming;

    [Tooltip("캐릭터가 서 있는 동안 기울일 때 아이템에서 재생되는 애니메이션 곡선")]
    [SerializeField, InLineEditor]
    private ACurves itemStading;

    [Title(label: "Camera Curves")]

    [Tooltip("캐릭터가 조준하는 동안 기울일 때 카메라에서 재생되는 애니메이션 곡선")]
    [SerializeField, InLineEditor]
    private ACurves cameraAiming;

    [Tooltip("캐릭터가 서 있는 동안 기울일 때 카메라에서 재생되는 애니메이션 곡선")]
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
