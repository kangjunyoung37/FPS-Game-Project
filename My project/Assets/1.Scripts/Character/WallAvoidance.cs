using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//벽회피. 이 구성 요소는 벽 근처에 있을 때
//캐릭터의 장착 아이템을 낮추는 것을 처리합니다.
public class WallAvoidance : MonoBehaviour
{
    #region PROPERTIES

    public bool HasWall => hasWall;

    #endregion

    #region FIELDS SERIALZIED

    [Title(label: "References")]

    [Tooltip("캐릭터의 카메라 Trnasform")]
    [SerializeField, NotNull]
    private Transform playerCamera;

    [Title(label: "Settings")]

    [Tooltip("벽을 확인할 최대 거리")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float distance = 1.0f;

    [Tooltip("sphere check 반지름")]
    [Range(0.0f, 2.0f)]
    [SerializeField]
    private float radius = 0.5f;

    [Tooltip("벽 레이어로 계산할 레이어")]
    [SerializeField]
    private LayerMask layerMask;

    #endregion

    #region FIELDS
    
    //캐릭터가 보고 있는 벽이 있으면 참입니다.
    private bool hasWall;

    #endregion

    #region METHODS

    private void Update()
    {
        if (playerCamera == null)
        {
            
            Debug.LogError($" playerCamera가 {playerCamera} 입니다");

            return;
        }

        //Trace Ray
        var ray = new Ray(playerCamera.position, playerCamera.forward);

        hasWall = Physics.SphereCast(ray, radius, distance, layerMask);


    }

    #endregion
}
