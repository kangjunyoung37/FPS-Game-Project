using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// FeelState�� ���ԵǾ� �ֽ��ϴ�.
/// ���ִ� ����, ��ũ���� �ִ� ����, ���� ����,�޸��� �ִ� ����
/// </summary>
[CreateAssetMenu(fileName = "SO_Feel",menuName = "FPS Game/Shooter Pack/Feel",order = 0)]
public class Feel : ScriptableObject
{

    #region PROPERTIES

    /// <summary>
    /// ���ִ� ����
    /// </summary>
    public FeelState Standing => standing;

    /// <summary>
    /// ��ũ���� �ִ� ����
    /// </summary>
    public FeelState Crouching => crouching;

    /// <summary>
    /// ���� ����
    /// </summary>    
    public FeelState Aiming => aiming;

    /// <summary>
    /// �޸��� �ִ� ����
    /// </summary>
    public FeelState Running => running;

    #endregion


    #region FIEDLS SERIALIZED

    [Title(label: "Standing State")]

    [Tooltip("�� ���� �� ���Ǵ� FeelState")]
    [SerializeField]
    private FeelState standing;

    [Title(label: "Crouching State")]

    [Tooltip("��ũ�� �� ���Ǵ� FeelState")]
    [SerializeField]
    private FeelState crouching;

    [Title(label: "Aiming State")]

    [Tooltip("�����ϴ� ���� ���Ǵ� FeelState")]
    [SerializeField]
    private FeelState aiming;

    [Title(label: "Running State")]

    [Tooltip("�޸��� ���� ���Ǵ� FeelState")]
    [SerializeField]
    private FeelState running;


    #endregion

    #region FUNCTIONS

    public FeelState GetState(Animator characterAnimator)
    {
        //�ٴ� ��
        if(characterAnimator.GetBool(AHashes.Running))
        {
            return Running;
        }
        else
        {
            //���� ��
            if (characterAnimator.GetBool(AHashes.Aim))
                return Aiming;

            else
            {
                //��ũ���� ��
                if (characterAnimator.GetBool(AHashes.Crouching))
                    return Crouching;
                
                //�� �ִ� ��
                else
                    return Standing;
            }
        }
        return Standing;
    }


    #endregion
}
