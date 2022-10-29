using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchingInput : MonoBehaviour
{
    #region FIELDS SERIALIZED


    [Title(label: "References")]

    [Tooltip("캐릭터의 characterBehaviour 컴포넌트")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Tooltip("캐릭터의 움직임 컴포넌트")]
    [SerializeField, NotNull]
    private MovementBehaviour movementBehaviour;

    [Title(label: "Settings")]

    [Tooltip("true라면 웅크리기를 유지하려면 계속 누르고 있어야 합니다")]
    [SerializeField]
    private bool holdToCrouch;

    #endregion

    #region FIELDS

    /// <summary>
    /// 플레이어가 웅크리기 버튼을 계속 누르고 있는지
    /// </summary>
    private bool holding;

    #endregion

    #region UNITY

    private void Update()
    {
        if (holdToCrouch)
            movementBehaviour.TryCrouch(holding);
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if(characterBehaviour == null || movementBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}에 characterBehaviour = {characterBehaviour}, movementBehaviour = {movementBehaviour}입니다");
            
            return;
        }
        //커서가 잠겨있지 않다면
        if (!characterBehaviour.isCursorLocked())
            return;

        switch(context.phase)
        {
            case InputActionPhase.Started:
                holding = true;
                break;

            case InputActionPhase.Performed:
                if (!holdToCrouch)
                    movementBehaviour.TryToggleCrouch();
                break;

            case InputActionPhase.Canceled:
                holding = false;
                break;
        }
    }

    #endregion
}
