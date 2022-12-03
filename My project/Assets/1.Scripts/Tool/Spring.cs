using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보간 처리를 위해 사용
/// </summary>
public class Spring 
{
    //세팅
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
    /// 현재 보유 프레임
    /// </summary>
    private int hFrames;

    /// <summary>
    /// 현재 보유 힘
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
    /// 상태 리셋시키기
    /// </summary>
    private void Reset()
    {
        currentValue = start;
        currentVelocity = initialVelocity;
        currentAcceleration = Vector3.zero;
    }
    /// <summary>
    /// 설정을 조정합니다
    /// </summary>
    public void Adjust(SpringSettings newSettings) => settings = newSettings;

    /// <summary>
    /// 현재 속도를 재사용하고 나중에 부드럽게 값을 보간합니다
    /// </summary>
    /// <param name="value"></param>
    public void UpdateEndValue(Vector3 value) => UpdateEndValue(value, currentVelocity);

    /// <summary>
    /// 사용할 힘을 설정합니다.
    /// </summary>
    /// <param name="force">가할 힘</param>
    public void SetHeldForce(HeldForce force) => heldForce = force;

    public void UpdateEndValue(Vector3 value, Vector3 velocity)
    {
        end = value;
        currentVelocity = velocity;
    }

    /// <summary>
    /// 초단위로 진행
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
