using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ĳ���Ͱ� ��ũ���� �ֵ� ���� ī�޶� �ùٸ� ��ġ�� ���� �� �ְ� ��
/// </summary>
public class CameraHeight : MonoBehaviour
{
    #region FIELDS SERIALIZED
    [Title(label: "References")]
    [Tooltip("ĳ���� ��Ʈ�ѷ�")]
    [SerializeField]
    private CharacterController characterController;
    [Title(label: "Settings")]
    [Tooltip("���� �ӵ��Դϴ�. ī�޶� �� ��ġ�� �󸶳� �ε巴�� ��ȯ�Ǵ��� �����մϴ�.")]
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
            Debug.LogError($" {gameObject.name} ���ӿ�����Ʈ�� {this.name}������Ʈ�� �����ϴ�");
            return;
        }
        float heightTarget = characterController.height * 0.9f;

        height = Mathf.Lerp(height,heightTarget,interpolationSpeed * Time.deltaTime);
        transform.localPosition = Vector3.up * height;
    }

    #endregion
}
