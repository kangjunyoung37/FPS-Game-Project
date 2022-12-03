using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾ ���� �� �Ǵ� �ʿ��� Ư�� ��Ȳ���� 
/// ĳ������ ���⸦ ���ߴ� ���� ó���մϴ�
/// </summary>
public class LowerWeapon : MonoBehaviour
{
    #region FIELDS SERIALZIED

    [Title(label: "Reference")]
    [Tooltip("ĳ���� �ִϸ����� ������Ʈ")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Tooltip("ĳ���Ͱ� ���� ���ϰ� �ִ��� Ȯ���ϰ� �ڵ����� ���⸦ �������� WallAvoidance ���� ��Ұ� �ʿ��մϴ�.")]
    private WallAvoidance wallAvoidance;

    [Tooltip("ĳ���� �κ��丮 ������Ʈ")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;

    [Tooltip("ĳ������ CharacterBehaviour ������Ʈ")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Title(label: "Settings")]

    [Tooltip("���࿡ True��� ĳ���Ͱ� �߻縦 �� �� lowered state�� ����ϴ�")]
    [SerializeField]
    private bool stopWhileFiring = true;

    #endregion

    #region FIELDS

    /// <summary>
    /// True�� ĳ���Ͱ� ���⸦ ���߰� ������ �� �ִ� �ൿ�� ���� ���� �����Դϴ�.
    /// </summary>
    private bool lowerd;
    /// <summary>
    /// �÷��̾ ���ߵ��� ��û�� �� True�� �˴ϴ�.
    /// �ٸ� ���¿� ���� ���⸦ ���� ������ ���� ���� �ֽ��ϴ�.
    /// </summary>
    private bool lowerdPressed;

    #endregion

    #region UNITY

    private void Update()
    {
        if(characterAnimator == null || characterBehaviour == null || inventoryBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}�� characterAnimator = {characterAnimator}, characterBehaviour ={characterBehaviour} ,inventoryBehaviour ={inventoryBehaviour} ");

            return;
            
        }
        //ĳ���Ͱ� ���� ���� �ƴϰų� ,�ٴ� ���� �ƴϰų�, �˻� ���� �ƴϰų� ���⸦ ���� ���� �ʾ�����
        lowerd = (lowerdPressed || wallAvoidance != null && wallAvoidance.HasWall) && !characterBehaviour.IsAiming() && !characterBehaviour.IsRunning()
            && !characterBehaviour.IsInspecting() && !characterBehaviour.IsHolstered();
        
        //�߻��ϴ� ���ȿ��� lowerd ���¸� �����մϴ�.
        if(stopWhileFiring && characterBehaviour.isHoldingButtonFire())
        {
            lowerd = false;
        }
        //���� ���⿡ ItemAnimationDataBehaviour�� �ִ��� Ȯ��
        var animationData = inventoryBehaviour.GetEquipped().GetComponent<ItemAnimationDataBehaviour>();
        if (animationData == null)
            lowerd = false;

        else
        {
            //animationData�� LowerData�� �ִ��� Ȯ��
            if (animationData.GetLowerData() == null)
                lowerd = false;
        }
        
        characterAnimator.SetBool(AHashes.Lowered, lowerd);

    }

    #endregion

    #region GETTERS

    /// <summary>
    /// ĳ������ ���Ⱑ lowerd �����̰� ĳ���Ͱ� ���� ���� �� �� ���� �����̸� 
    /// true�� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public bool IsLowered() => lowerd;

    #endregion

    #region METHODS
    /// <summary>
    /// ���� ���߱�, ĳ������ PlayerInput��ҿ� ���ؼ� ȣ��˴ϴ�.
    /// </summary>
    /// <param name="context"></param>
    public void Lower(InputAction.CallbackContext context)
    {
        //Ŀ���� ��� �����Ǿ� �ִ� ���� �����մϴ�.
        if (!characterBehaviour.isCursorLocked())
            return;
        //����,�˻�,�ٱ�,���⸦ �ִ� ���¶�� �������� �ʽ��ϴ�.
        if(characterBehaviour.IsAiming() || characterBehaviour.IsInspecting()||characterBehaviour.IsRunning()||characterBehaviour.IsHolstered())
        {
            return;
        }

        switch(context)
        {
            case { phase: InputActionPhase.Performed }:

                lowerdPressed = !lowerdPressed;
                break;
        }


    }
    #endregion
}
