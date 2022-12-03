using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMotion : Motion
{
    #region FIELDS SERIALIZED

    [Tooltip("ĳ������ FeelManager ������Ʈ")]
    [SerializeField, NotNull]
    private FeelManager feelManager;

    [Tooltip("ĳ������ movementBehaviour ������Ʈ")]
    [SerializeField, NotNull]
    private MovementBehaviour movementBehaviour;

    [Tooltip("ĳ������ Animator")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Title(label: "Settings")]

    [Tooltip("����� Ÿ��")]
    [SerializeField]
    private MotionType motionType;

    #endregion

    #region FIELDS
    
    /// <summary>
    /// ��ġ Spring
    /// </summary>
    private readonly Spring springLocation = new Spring();
    
    /// <summary>
    /// ȸ�� Spring
    /// </summary>
    private readonly Spring springRotation = new Spring();
    
    /// <summary>
    /// ���� ����ǰ� �ִ� �ִϸ��̼� �
    /// </summary>
    private ACurves playedCurves;

    /// <summary>
    /// ĳ���Ͱ� ���������� ���� �����ߴ� �ð�
    /// </summary>
    private float landingTime;

    #endregion

    #region METHODS

    public override void Tick()
    {
        if(feelManager == null || movementBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}�� feelManager�� {feelManager}�̰�,movementBehaviour�� {movementBehaviour}�Դϴ�");

            return;
        }

        Feel feel = feelManager.Preset.GetFeel(motionType);
        if(feel == null)
        {
            Debug.LogError($"{this.gameObject}�� feel �� Null �Դϴ�");

            return;
        }
        //��ġ
        Vector3 location = default;
        //ȸ��
        Vector3 rotation = default;

        //landing Time ����
        if (movementBehaviour.IsGrounded() && !movementBehaviour.WasGrounded())
            landingTime = Time.time;

        //���� ������� ����
        playedCurves = feel.GetState(characterAnimator).LandingCurves;

        float evaluateTime = Time.time - landingTime;
        
        //��ġ � �����ϱ�
        location += playedCurves.LocationCurves.EvaluateCurves(evaluateTime);
        //ȸ�� � �����ϱ�
        rotation += playedCurves.RotationCurves.EvaluateCurves(evaluateTime);

        //�������� ��ġ �����ϱ�
        springLocation.UpdateEndValue(location);
        //�������� ȸ�� �����ϱ�
        springRotation.UpdateEndValue(rotation);
        
    }

    #endregion

    #region FUNCTIONS

    //��ġ ��������
    public override Vector3 GetLocation()
    {
        if (playedCurves == null)
            return default;

        return springLocation.Evaluate(playedCurves.LocationSpring);
    }

    public override Vector3 GetEulerAngles()
    {
        if (playedCurves == null)
            return default;

        return springRotation.Evaluate(playedCurves.RotationSpring);
    }

    #endregion

}
