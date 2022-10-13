using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 캐릭터가 웅크리고 있든 없든 카메라가 올바른 위치에 있을 수 있게 함
/// </summary>
public class CameraHeight : MonoBehaviour
{
    #region FIELDS SERIALIZED
    [Title(label: "References")]
    [Tooltip("캐릭터 콘트롤러")]
    [SerializeField]
    private CharacterController characterController;
    [Title(label: "Settings")]
    [Tooltip("보간 속도입니다. 카메라가 새 위치로 얼마나 부드럽게 전환되는지 결정합니다.")]
    [SerializeField]
    private float interpolationSpeed = 12.0f;
    #endregion

    #region FIELDS


    private float height = 1.8f;
    #endregion

    #region UNITY

    private void Update()
    {
        if(characterController == null)
        {
            Debug.LogError($" {gameObject.name} 게임오브젝트에 {this.name}컴포넌트가 없습니다");
            return;
        }
        float heightTarget = characterController.height * 0.9f;

        height = Mathf.Lerp(height,heightTarget,interpolationSpeed * Time.deltaTime);
        transform.localPosition = Vector3.up * height;
    }

    #endregion
}
