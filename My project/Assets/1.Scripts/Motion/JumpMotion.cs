
using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMotion : Motion
{
    #region FIELDS SERIALIZED

    [Tooltip("ĳ������ Feel Manager")]
    [SerializeField, NotNull]
    private FeelManager feelManager;

    [Tooltip("ĳ������ MovementBehaviour ������Ʈ")]
    [SerializeField, NotNull]
    private MovementBehaviour movementBehaviour;

    [Tooltip("ĳ������ �ִϸ��̼� ������Ʈ")]
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
    /// Rotation Spring
    /// </summary>
    private readonly Spring springRotation = new Spring();

    /// <summary>
    /// ���� ��� ���� �
    /// </summary>
    private ACurves playedCurves;


    #endregion

    #region METHODS

    public override void Tick()
    {
        if(feelManager == null || movementBehaviour == null)
        { 
            Debug.LogError($"{this.gameObject}�� feelManager�� movementBehaviour �� Null �Դϴ�");
            return;
        }

        Feel feel = feelManager.Preset.GetFeel(motionType);
        if(feel == null)
        {
            Debug.LogError($"{this.gameObject}�� feel�� {feel}�Դϴ�");

            return;
        }
        //��ġ��
        Vector3 location = default;
        //�����̼ǰ�
        Vector3 rotation = default;

        FeelState state = feel.GetState(characterAnimator);

        //���� �����Ǿ����� ���� ���
        if(!movementBehaviour.IsGrounded())
        {
            //���߿� �� �ִ� �ð��� ���
            float airTime = Time.time - movementBehaviour.GetLastJumpTime();

            //���߿� �ִ°� ������ ���� ������ üũ
            if(movementBehaviour.IsJumping())
            {
                //�� ���� ���� ��� �Ϸ�Ǵ� ���� �� ���̸� ��Ÿ���ϴ�.
                var maxCurveLength = 0.0f;

                //state���� ���� � ��������
                ACurves jumpingCurves = state.JumpingCurves;

                //�ִϸ��̼� ��� �ϳ��� ���ϴ�.
                jumpingCurves.LocationCurves.ForEach(curve =>
                {
                    //���� �� ��� ��ġ�ϵ��� maxCurveLength ������Ʈ
                    if (curve.length > maxCurveLength)
                        maxCurveLength = curve.length;
                });

                jumpingCurves.RotationsCurves.ForEach(curve =>
                {
                    if (curve.length > maxCurveLength)
                        maxCurveLength = curve.length;

                });

                //���� Ŀ�갡 ���ݱ��� ����� �Ϸ��ߴ��� Ȯ���ϱ� , �������� �ִ� ����
                if (Time.time - movementBehaviour.GetLastJumpTime() >= maxCurveLength)
                {
                    //�������� �ִ� �ð�
                    airTime -= maxCurveLength;
                    //�������� ��� ���
                    playedCurves = state.FallingCurves;
                }
                else
                    playedCurves = state.JumpingCurves;
            }
            //�÷��̾ �������� �ʾ����Ƿ� �������� ��� ���
            else
            {
                playedCurves = state.FallingCurves;
            }

            //��ġ � ����
            location += playedCurves.LocationCurves.EvaluateCurves(airTime);
            //ȸ�� � ����
            rotation += playedCurves.RotationsCurves.EvaluateCurves(airTime);
        }

        //Spring ��ġ �� ������Ʈ
        springLocation.UpdateEndValue(location);
        //Spring ȸ�� �� ������Ʈ
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

    //���Ϸ� �ޱ� ��������
    public override Vector3 GetEulerAngles()
    {
        if (playedCurves == null)
            return default;

        return springRotation.Evaluate(playedCurves.RotationSpring);
    }

    #endregion
}
