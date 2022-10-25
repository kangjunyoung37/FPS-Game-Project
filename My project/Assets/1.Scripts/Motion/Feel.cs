using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// FeelState가 포함되어 있습니다.
/// 서있는 상태, 웅크리고 있는 상태, 조준 상태,달리고 있는 상태
/// </summary>
[CreateAssetMenu(fileName = "SO_Feel",menuName = "FPS Game/Shooter Pack/Feel",order = 0)]
public class Feel : ScriptableObject
{

    #region PROPERTIES

    /// <summary>
    /// 서있는 상태
    /// </summary>
    public FeelState Standing => standing;

    /// <summary>
    /// 웅크리고 있는 상태
    /// </summary>
    public FeelState Crouching => crouching;

    /// <summary>
    /// 조준 상태
    /// </summary>    
    public FeelState Aiming => aiming;

    /// <summary>
    /// 달리고 있는 상태
    /// </summary>
    public FeelState Running => running;

    #endregion


    #region FIEDLS SERIALIZED

    [Title(label: "Standing State")]

    [Tooltip("서 있을 때 사용되는 FeelState")]
    [SerializeField]
    private FeelState standing;

    [Title(label: "Crouching State")]

    [Tooltip("웅크릴 때 사용되는 FeelState")]
    [SerializeField]
    private FeelState crouching;

    [Title(label: "Aiming State")]

    [Tooltip("조준하는 동안 사용되는 FeelState")]
    [SerializeField]
    private FeelState aiming;

    [Title(label: "Running State")]

    [Tooltip("달리는 동안 사용되는 FeelState")]
    [SerializeField]
    private FeelState running;


    #endregion

    #region FUNCTIONS

    public FeelState GetState(Animator characterAnimator)
    {
        //뛰는 중
        if(characterAnimator.GetBool(AHashes.Running))
        {
            return Running;
        }
        else
        {
            //조준 중
            if (characterAnimator.GetBool(AHashes.Aim))
                return Aiming;

            else
            {
                //웅크리는 중
                if (characterAnimator.GetBool(AHashes.Crouching))
                    return Crouching;
                
                //서 있는 중
                else
                    return Standing;
            }
        }
        return Standing;
    }


    #endregion
}
