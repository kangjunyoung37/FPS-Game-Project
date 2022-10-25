using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ӿ��� ����Ǵ� FeelPreset�� �����ϰ� �ٸ������� ������ �� �ֵ��� �մϴ�.
/// </summary>
public class FeelManager : MonoBehaviour
{
    #region PROPERTIES

    public FeelPreset Preset
    {
        get => preset;

        set => preset = value;
    }

    #endregion

    #region FIELDS SERIALIZED

    [Tooltip("Feel Preset")]
    [SerializeField]
    private FeelPreset preset;

    #endregion
}
