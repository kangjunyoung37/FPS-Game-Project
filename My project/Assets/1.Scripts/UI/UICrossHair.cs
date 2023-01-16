using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICrossHair : UIElement
{
    #region Fields Serialzied

    [Title(label: "Serialized")]

    [SerializeField, NotNull]
    private CanvasGroup crosshairCanvasGroup;

    [SerializeField, NotNull]
    private CanvasGroup dotCanvasGroup;

    [SerializeField, NotNull]
    private RectTransform mainRectTransform;

    [Title(label: "Settings")]
    [SerializeField]
    private Vector2 minMaxScale = new Vector2(50.0f, 200.0f);

    [SerializeField]
    private float defaultScale = 50.0f;

    [Title(label: "Interpolation")]

    [SerializeField]
    private float interpolationSpeed = 7.0f;

    [SerializeField]
    private float interpolationSpeedDot = 50.0f;

    [SerializeField]
    private SpringSettings interpolationSizeDelta = SpringSettings.Default();

    [SerializeField]
    private float jumpingScaleAddtion = 50.0f;

    [SerializeField]
    private float crouchingScaleAddtion = -20.0f;

    [SerializeField]
    private float movementScaleAddtion = 25.0f;

    [Title(label: "Running")]

    [SerializeField]
    private float disableVisibility = 0.6f;

    [SerializeField]
    private float runningScaleAddtion = 15.0f;

    [Title(label: "Spread")]

    [SerializeField]
    private AnimationCurve spreadIncrease;
    #endregion

    #region Fields

    private MovementBehaviour movementBehaviour;

    private float crosshairLocalScale;

    private float crosshairVisibility;

    private float dotVisibility;

    private Spring springCrosshairSizeDelta;

    #endregion

    #region Unity


    protected override void Awake()
    {
        base.Awake();
        springCrosshairSizeDelta = new Spring();
        crosshairVisibility = 1.0f;
    }

    protected override void Tick()
    {
        if (crosshairCanvasGroup == null || dotCanvasGroup == null || mainRectTransform == null || characterBehaviour == null)
        {
            Debug.LogError("null Error");

            return;
        }

        movementBehaviour ??= characterBehaviour.GetComponent<MovementBehaviour>();
        if(movementBehaviour == null)
        {
            Debug.LogError("movementBehaviour null ");
            return;
        }

        int shotFired = characterBehaviour.GetShotsFired();

        float movementScale = characterBehaviour.GetInputMovement().sqrMagnitude * movementScaleAddtion;

        float sizeDeltaTarget = defaultScale + spreadIncrease.Evaluate(shotFired);

        var crosshairLocalScaleTarget = 1.0f;

        var crosshairVisibilityTarget = 1.0f;

        var dotVisibilityTarget = 1.0f;

        if (characterBehaviour.IsAiming())
            crosshairLocalScaleTarget = dotVisibilityTarget = crosshairVisibilityTarget = 0.0f;
        
   
        else
        {

            float fallingVelocity = (movementBehaviour.GetVelocity().y >= 0 ? Mathf.Clamp01(Mathf.Abs(movementBehaviour.GetVelocity().y)) : 1) * jumpingScaleAddtion;

            sizeDeltaTarget += characterBehaviour.IsCrouching() ? crouchingScaleAddtion : 0.0f;

            if(characterBehaviour.IsHolstered())
            {
                crosshairLocalScaleTarget = crosshairVisibilityTarget = 0.0f;

                dotVisibilityTarget = 1.0f;
            }
            else
            {

                if(characterBehaviour.IsRunning())
                {
                    sizeDeltaTarget += movementBehaviour.IsGrounded() ? default : fallingVelocity;

                    crosshairVisibilityTarget = disableVisibility;

                    crosshairLocalScaleTarget = 1.0f;

                    sizeDeltaTarget += runningScaleAddtion;
                }
                else
                {
                    sizeDeltaTarget += movementBehaviour.IsGrounded() ? movementScale : fallingVelocity;

                    crosshairLocalScaleTarget = dotVisibilityTarget = 1.0f;

                    bool isPerfomingDiablingaction = characterBehaviour.IsInspecting() || characterBehaviour.IsMeleeing() || characterBehaviour.IsReloading() || characterBehaviour.IsTrowingGrenade();

                    crosshairVisibilityTarget = isPerfomingDiablingaction ? disableVisibility : 1.0f;

                }
            }
           
        }
        dotVisibility = Mathf.Lerp(dotVisibility, Mathf.Clamp01(dotVisibilityTarget), Time.deltaTime * interpolationSpeedDot);

        crosshairLocalScale = Mathf.Lerp(crosshairLocalScale, Mathf.Clamp01(crosshairLocalScaleTarget), Time.deltaTime * interpolationSpeed);

        crosshairVisibility = Mathf.Lerp(crosshairVisibility, Mathf.Clamp01(crosshairVisibilityTarget), Time.deltaTime * interpolationSpeed);

        sizeDeltaTarget = Mathf.Clamp(sizeDeltaTarget, minMaxScale.x, minMaxScale.y);

        springCrosshairSizeDelta.UpdateEndValue(sizeDeltaTarget * Vector3.one);

        mainRectTransform.sizeDelta = springCrosshairSizeDelta.Evaluate(interpolationSizeDelta);
        mainRectTransform.localScale = crosshairLocalScale * Vector2.one;


        crosshairCanvasGroup.alpha = crosshairVisibility;
        dotCanvasGroup.alpha = dotVisibility;

    }
    #endregion
}
