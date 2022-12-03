using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPBoltBehaviour : StateMachineBehaviour
{
    #region FIELDS

    /// <summary>
    /// �÷��̾� ĳ����Behaviour
    /// </summary>
    private CharacterBehaviour playerCharacter;

    /// <summary>
    /// �÷��̾� �κ��丮
    /// </summary>
    private InventoryBehaviour playerInventoryBehaviour;

    #endregion

    #region UNITY

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //�ʿ��� ĳ���� ������Ʈ�� �޾ƿɴϴ�.
        //playerCharacter ??= ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        playerCharacter = animator.GetComponentInParent<CharacterBehaviour>();
        //�κ��丮 ��������
        playerInventoryBehaviour = playerCharacter.GetInventory();

        if (!(playerInventoryBehaviour.EquipTPWeapon() is { } weaponBehaviour))
            return;
        //���� �ִϸ����� ��������
        var weaponAnimator = weaponBehaviour.gameObject.GetComponent<Animator>();

        //��Ʈ�׼� �ִϸ��̼� ���
        weaponAnimator.Play("Bolt Action");
    }


    #endregion
}
