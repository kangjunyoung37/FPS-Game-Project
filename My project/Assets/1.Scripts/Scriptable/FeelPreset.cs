using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// CameraFeel,Item Feel이 포함되어 있습니다.
/// </summary>
[CreateAssetMenu(fileName ="SO_Feel_Preset",menuName ="FPS Game/Shooter Pack/Feel Preset",order = 0)]
public class FeelPreset : ScriptableObject
{
    #region FIELDS SERIALIZED

    [Title(label: "Camera Feel")]
    
    [Tooltip("카메라가 얼마나 느끼는지에 대한 값을 포함하고 있습니다.")]
    [SerializeField, InLineEditor]
    private Feel cameraFeel;

    [Title(label: "Item Feel")]

    [Tooltip("아이템이 얼마나 느끼는지에 대한 값을 포함하고 있습니다")]
    [SerializeField, InLineEditor]
    private Feel itemFeel;

    #endregion

    #region FUNCTIONS
    
    public Feel GetFeel(MotionType motionType)
    {
        return motionType switch
        {
            MotionType.Camera => cameraFeel,
            MotionType.Item => itemFeel,
        };
    }

    #endregion
}
