using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchingInput : MonoBehaviour
{
    #region FIELDS SERIALIZED


    [Title(label: "References")]

    [Tooltip("ĳ������ characterBehaviour ������Ʈ")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Tooltip("ĳ������ ������ ������Ʈ")]
    [SerializeField, NotNull]
    private MovementBehaviour movementBehaviour;

    [Title(label: "Settings")]

    [Tooltip("true��� ��ũ���⸦ �����Ϸ��� ��� ������ �־�� �մϴ�")]
    [SerializeField]
    private bool holdToCrouch;

    #endregion

    #region FIELDS

    /// <summary>
    /// �÷��̾ ��ũ���� ��ư�� ��� ������ �ִ���
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
            Debug.LogError($"{this.gameObject}�� characterBehaviour = {characterBehaviour}, movementBehaviour = {movementBehaviour}�Դϴ�");
            
            return;
        }
        //Ŀ���� ������� �ʴٸ�
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
