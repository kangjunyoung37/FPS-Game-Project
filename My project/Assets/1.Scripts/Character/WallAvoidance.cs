using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ȸ��. �� ���� ��Ҵ� �� ��ó�� ���� ��
//ĳ������ ���� �������� ���ߴ� ���� ó���մϴ�.
public class WallAvoidance : MonoBehaviour
{
    #region PROPERTIES

    public bool HasWall => hasWall;

    #endregion

    #region FIELDS SERIALZIED

    [Title(label: "References")]

    [Tooltip("ĳ������ ī�޶� Trnasform")]
    [SerializeField, NotNull]
    private Transform playerCamera;

    [Title(label: "Settings")]

    [Tooltip("���� Ȯ���� �ִ� �Ÿ�")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float distance = 1.0f;

    [Tooltip("sphere check ������")]
    [Range(0.0f, 2.0f)]
    [SerializeField]
    private float radius = 0.5f;

    [Tooltip("�� ���̾�� ����� ���̾�")]
    [SerializeField]
    private LayerMask layerMask;

    #endregion

    #region FIELDS
    
    //ĳ���Ͱ� ���� �ִ� ���� ������ ���Դϴ�.
    private bool hasWall;

    #endregion

    #region METHODS

    private void Update()
    {
        if (playerCamera == null)
        {
            
            Debug.LogError($" playerCamera�� {playerCamera} �Դϴ�");

            return;
        }

        //Trace Ray
        var ray = new Ray(playerCamera.position, playerCamera.forward);

        hasWall = Physics.SphereCast(ray, radius, distance, layerMask);


    }

    #endregion
}
