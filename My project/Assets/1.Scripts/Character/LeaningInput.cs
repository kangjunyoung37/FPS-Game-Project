using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeaningInput : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [Title(label: "References")]

    [Tooltip("캐릭터의 characterBehaviour 컴포넌트")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Tooltip("캐릭터의 애니메이터 컴포넌트")]
    [SerializeField, NotNull]
    private Animator characterAnimater;

    [SerializeField]
    private Transform spineTransform;

    [SerializeField]
    private Transform LenaingTransform;
    #endregion

    #region FIELDS

    /// <summary>
    /// 현재 기울기 값
    /// </summary>
    private float leaningInput;

    /// <summary>
    /// 기울이고 있는 중이라면 True
    /// </summary>
    private bool isLeaning;

    private float leaningnum = 0.0f;

    private PhotonView PV;

    #endregion

    #region METHODS
    
    private void Awake()
    {
        PV = characterBehaviour.transform.GetComponent<PhotonView>();   
    }

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
        //기울기 상태 업데이트
        isLeaning = (leaningInput != 0.0f);
        
        characterAnimater.SetFloat(AHashes.LeaningInput, leaningInput);
        characterAnimater.SetBool(AHashes.Leaning, isLeaning);


    }


    public void Lean(InputAction.CallbackContext context)
    {

        if (!PV.IsMine || characterBehaviour.GetPlayerDead())
            return;
        //커서가 잠겨있지 않다면
        if(!characterBehaviour.isCursorLocked())
        {
            //기울기를 0으로 유지
            leaningInput = 0.0f;
            return;
        }

        leaningInput = context.ReadValue<float>();



    }

    public void OnPhotonSerializeView(PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(leaningInput);
        }
        else
        {
            leaningInput = (float)stream.ReceiveNext();

        }
    }


    #endregion
}
