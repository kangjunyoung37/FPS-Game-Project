using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이션 해쉬
/// </summary>
public static class AHashes
{
    /// <summary>
    /// 기울기
    /// </summary>
    public static readonly int Leaning = Animator.StringToHash("Leaning");

    /// <summary>
    /// 조준 
    /// </summary>
    public static readonly int Aim = Animator.StringToHash("Aim");

    /// <summary>
    /// 웅크리기
    /// </summary>
    public static readonly int Crouching = Animator.StringToHash("Crouching");

    /// <summary>
    /// 기울기 인풋
    /// </summary>
    public static readonly int LeaningInput = Animator.StringToHash("Leaning Input");

    /// <summary>
    /// 정지
    /// </summary>
    public static readonly int Stop = Animator.StringToHash("Stop");

    /// <summary>
    /// 재장전
    /// </summary>
    public static readonly int Reloading = Animator.StringToHash("Reloading");
    /// <summary>
    /// 검사
    /// </summary>
    public static readonly int Inspecting = Animator.StringToHash("Inspecting");

    /// <summary>
    /// 근접공격
    /// </summary>
    public static readonly int Meleeing = Animator.StringToHash("Meleeing");
   
    /// <summary>
    /// 수류탄
    /// </summary>
    public static readonly int Grenading = Animator.StringToHash("Grenading");

    /// <summary>
    /// 볼트 액션
    /// </summary>
    public static readonly int Bolt = Animator.StringToHash("Bolt Action");

    /// <summary>
    /// 무기 넣기
    /// </summary>
    public static readonly int Holstering = Animator.StringToHash("Holstering");
    
    /// <summary>
    /// 무기 넣었는지
    /// </summary>
    public static readonly int Holstered = Animator.StringToHash("Holstered");

    /// <summary>
    /// 뛰는중
    /// </summary>
    public static readonly int Running = Animator.StringToHash("Running");
    
    /// <summary>
    /// 무기 내리기
    /// </summary>
    public static readonly int Lowered = Animator.StringToHash("Lowered");

    /// <summary>
    /// 알파 액션 오프셋
    /// </summary>
    public static readonly int AlphaActionOffset = Animator.StringToHash("Alpha Action Offset");

    /// <summary>
    /// AlphaIKHandLeft.
    /// </summary>
    public static readonly int AlphaIKHandLeft = Animator.StringToHash("Alpha IK Hand Left");
    
    /// <summary>
    /// AlphaIKHandRight.
    /// </summary>
    public static readonly int AlphaIKHandRight = Animator.StringToHash("Alpha IK Hand Right");

    /// <summary>
    /// Aiming Alpha Value.
    /// </summary>
    public static readonly int AimingAlpha = Animator.StringToHash("Aiming");

    /// <summary>
    /// Hashed "Movement".
    /// </summary>
    public static readonly int Movement = Animator.StringToHash("Movement");
   
    /// <summary>
    /// Hashed "Leaning".
    /// </summary>
    public static readonly int LeaningForward = Animator.StringToHash("Leaning Forward");

    /// <summary>
    /// Hashed "Aiming Speed Multiplier".
    /// </summary>
    public static readonly int AimingSpeedMultiplier = Animator.StringToHash("Aiming Speed Multiplier");
   
    /// <summary>
    /// Hashed "Turning".
    /// </summary>
    public static readonly int Turning = Animator.StringToHash("Turning");

    /// <summary>
    /// Hashed "Horizontal".
    /// </summary>
    public static readonly int Horizontal = Animator.StringToHash("Horizontal");
   
    /// <summary>
    /// Hashed "Vertical".
    /// </summary>
    public static readonly int Vertical = Animator.StringToHash("Vertical");

    /// <summary>
    /// Hashed "Play Rate Locomotion Forward".
    /// </summary>
    public static readonly int PlayRateLocomotionForward = Animator.StringToHash("Play Rate Locomotion Forward");
   
    /// <summary>
    /// Hashed "Play Rate Locomotion Sideways".
    /// </summary>
    public static readonly int PlayRateLocomotionSideways = Animator.StringToHash("Play Rate Locomotion Sideways");
   
    /// <summary>
    /// Hashed "Play Rate Locomotion Backwards".
    /// </summary>
    public static readonly int PlayRateLocomotionBackwards = Animator.StringToHash("Play Rate Locomotion Backwards");

    public static readonly int Test = Animator.StringToHash("Test");
}
