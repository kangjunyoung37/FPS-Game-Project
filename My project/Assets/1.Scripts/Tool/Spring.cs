using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ó���� ���� ���
/// </summary>
public class Spring 
{
    //����
    private SpringSettings settings;

    private Vector3 initialVelocity;

    private Vector3 start;
    private Vector3 end;

    private Vector3 currentValue;
    private Vector3 currentVelocity;
    private Vector3 currentAcceleration;

    private float stepSize = 1f / 61f;
    private bool isFirstEvaluete = true;

    /// <summary>
    /// ���� ���� ������
    /// </summary>
    private int hFrames;

    /// <summary>
    /// ���� ���� ��
    /// </summary>
    private HeldForce heldForce;

    public Spring()
    {
        settings = new SpringSettings()
        {
            damping = 16.0f,
            mass = 1.0f,
            speed = 1.0f,
            stiffness = 169.0f
        };
    }
    public Spring(SpringSettings newSettings) => settings = newSettings;

    /// <summary>
    /// ���� ���½�Ű��
    /// </summary>
    private void Reset()
    {
        currentValue = start;
        currentVelocity = initialVelocity;
        currentAcceleration = Vector3.zero;
    }
    /// <summary>
    /// ������ �����մϴ�
    /// </summary>
    public void Adjust(SpringSettings newSettings) => settings = newSettings;

    /// <summary>
    /// ���� �ӵ��� �����ϰ� ���߿� �ε巴�� ���� �����մϴ�
    /// </summary>
    /// <param name="value"></param>
    public void UpdateEndValue(Vector3 value) => UpdateEndValue(value, currentVelocity);

    /// <summary>
    /// ����� ���� �����մϴ�.
    /// </summary>
    /// <param name="force">���� ��</param>
    public void SetHeldForce(HeldForce force) => heldForce = force;

    public void UpdateEndValue(Vector3 value, Vector3 velocity)
    {
        end = value;
        currentVelocity = velocity;
    }

    /// <summary>
    /// �ʴ����� ����
    /// </summary>
    public Vector3 Evaluate()
    {
        if(heldForce.Frames >0)
        {
            hFrames++;
            if(hFrames >= heldForce.Frames)
            {
                hFrames = 0;
                heldForce = default;
            }
        }
        if(isFirstEvaluete)
        {
            Reset();
            isFirstEvaluete = false;
        }
        float deltaTime = Time.deltaTime * settings.speed;

        float c = settings.damping;
        float m = settings.mass;
        float k = settings.stiffness;

        Vector3 x = currentValue;
        Vector3 v = currentVelocity;
        Vector3 a = currentAcceleration;

        float _stepSize = deltaTime > stepSize ? stepSize : deltaTime - 0.001f;
        float steps = Mathf.Ceil(deltaTime / _stepSize);

        for(var i = 0; i < steps; i++)
        {
            float dt = Mathf.Abs(i - (steps - 1)) < 0.01f ? deltaTime - i * _stepSize : _stepSize;
            
            x += v * dt + a * (dt * dt * 0.5f);
            var _a = (-k * (x - (end + heldForce.Force)) + -c * v) / m;
            v += (a + _a) * (dt * 0.5f);
            a = _a;
        }
        currentValue = x;
        currentVelocity = v;
        currentAcceleration = a;

        return currentValue;
    }

    public Vector3 Evaluate(SpringSettings newSettings)
    {
        Adjust(newSettings);

        return Evaluate();
    }
}
