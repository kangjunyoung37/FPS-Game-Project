using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// CameraFeel,Item Feel�� ���ԵǾ� �ֽ��ϴ�.
/// </summary>
[CreateAssetMenu(fileName ="SO_Feel_Preset",menuName ="FPS Game/Shooter Pack/Feel Preset",order = 0)]
public class FeelPreset : ScriptableObject
{
    #region FIELDS SERIALIZED

    [Title(label: "Camera Feel")]
    
    [Tooltip("ī�޶� �󸶳� ���������� ���� ���� �����ϰ� �ֽ��ϴ�.")]
    [SerializeField, InLineEditor]
    private Feel cameraFeel;

    [Title(label: "Item Feel")]

    [Tooltip("�������� �󸶳� ���������� ���� ���� �����ϰ� �ֽ��ϴ�")]
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
