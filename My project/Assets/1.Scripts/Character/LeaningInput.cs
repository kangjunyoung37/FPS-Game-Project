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

    
    [SerializeField]
    private Transform spineTransform;

    [SerializeField]
    private Transform LenaingTransform;
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

    private float leaningnum = 0.0f;
    #endregion

    #region METHODS

    private void LateUpdate()
    {
       
        if(leaningInput == 1.0f)
        {
            leaningnum = Mathf.Lerp(leaningnum, 30.0f, Time.deltaTime * 20f);
        }
        else if(leaningInput == -1.0f)
        {
            leaningnum = Mathf.Lerp(leaningnum, -35.0f, Time.deltaTime * 20f);
        }
        else
        {
            leaningnum = Mathf.Lerp(leaningnum, 0.0f, Time.deltaTime * 20f);
        }

        Quaternion leaning = Quaternion.Euler(spineTransform.transform.localRotation.x, leaningnum, spineTransform.transform.localRotation.z);
        spineTransform.transform.localRotation = leaning;
        
   
    }

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
