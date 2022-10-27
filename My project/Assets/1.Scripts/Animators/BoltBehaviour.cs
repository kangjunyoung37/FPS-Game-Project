using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltBehaviour : StateMachineBehaviour
{
    #region FIELDS

    /// <summary>
    /// 플레이어 캐릭터Behaviour
    /// </summary>
    private CharacterBehaviour playerCharacter;

    /// <summary>
    /// 플레이어 인벤토리
    /// </summary>
    private InventoryBehaviour playerInventoryBehaviour;

    #endregion

    #region UNITY

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //필요한 캐릭터 컴포넌트를 받아옵니다.
        playerCharacter ??= ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        //인벤토리 가져오기
        playerInventoryBehaviour = playerCharacter.GetInventory();

        if (!(playerInventoryBehaviour.GetEquipped() is { } weaponBehaviour))
            return;
        //무기 애니메이터 가져오기
        var weaponAnimator = weaponBehaviour.gameObject.GetComponent<Animator>();
        //볼트액션 애니메이션 재생
        weaponAnimator.Play("Bolt Action");
    }


    #endregion
}
