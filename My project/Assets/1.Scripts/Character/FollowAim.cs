using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowAim : MonoBehaviour
{

    #region SERIALZED

    [Title(label:"FP Aim Target")]
    [SerializeField]
    private Transform FPAimTransform;

    [Title(label: "Settings")]
    [Tooltip("���� ���� ���ǵ�")]
    [SerializeField]
    private float smoothSpeed;

    [SerializeField]
    [Tooltip("�޸��� ������ ĳ���Ͱ� ���� ��ġ")]
    private Vector3 normal;

    #endregion

    #region FIELDS

    private CharacterBehaviour characterBehaviour;

    private bool running;

    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        characterBehaviour = transform.root.GetComponent<CharacterBehaviour>();
    }

    private void Update()
    {
        running = characterBehaviour.GetRunning();
    }

    private void FixedUpdate()
    {
        //���� ����
        Vector3 desiredPositon = FPAimTransform.position;
        Vector3 smoothPostion = Vector3.Lerp(transform.position, desiredPositon, smoothSpeed * Time.deltaTime);
        //���� ����
        Vector3 runningSmoothPostion = Vector3.Lerp(transform.localPosition,normal,smoothSpeed * Time.deltaTime);
        
        //�޸��� ���̶��
        if (running || characterBehaviour.IsReloading())
        {
            transform.localPosition = runningSmoothPostion;
        }
        else
        {
            transform.position = smoothPostion;
        }
    }
    #endregion

}
