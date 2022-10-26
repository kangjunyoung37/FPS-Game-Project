using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeaningInput : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("ĳ������ characterBehaviour ������Ʈ")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Tooltip("ĳ������ �ִϸ����� ������Ʈ")]
    [SerializeField, NotNull]
    private Animator characterAnimater;

    #endregion

    #region FIELDS

    /// <summary>
    /// ���� ���� ��
    /// </summary>
    private float leaningInput;

    /// <summary>
    /// ����̰� �ִ� ���̶�� True
    /// </summary>
    private bool isLeaning;

    #endregion

    #region METHODS

    private void Update()
    {
        //���� ���� ������Ʈ
        isLeaning = (leaningInput != 0.0f);
        
        characterAnimater.SetFloat(AHashes.LeaningInput, leaningInput);
        characterAnimater.SetBool(AHashes.Leaning, isLeaning);
    }

    public void Lean(InputAction.CallbackContext context)
    {

        //Ŀ���� ������� �ʴٸ�
        if(!characterBehaviour.isCursorLocked())
        {
            //���⸦ 0���� ����
            leaningInput = 0.0f;
            
            return;
        }

        leaningInput = context.ReadValue<float>();

    }


    #endregion
}
