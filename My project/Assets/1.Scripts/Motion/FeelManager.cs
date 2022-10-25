using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임에서 실행되는 FeelPreset을 포함하고 다른데에서 접근할 수 있도록 합니다.
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
