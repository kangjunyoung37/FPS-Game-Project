using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어가 원할 때 또는 필요한 특정 상황에서 
/// 캐릭터의 무기를 낮추는 것을 처리합니다
/// </summary>
public class LowerWeapon : MonoBehaviour
{
    #region FIELDS SERIALZIED

    [Title(label: "Reference")]
    [Tooltip("캐릭터 애니메이터 컴포넌트")]
    [SerializeField, NotNull]
    private Animator characterAnimator;

    [Tooltip("캐릭터가 벽을 향하고 있는지 확인하고 자동으로 무기를 내리려면 WallAvoidance 구성 요소가 필요합니다.")]
    private WallAvoidance wallAvoidance;

    [Tooltip("캐릭터 인벤토리 컴포넌트")]
    [SerializeField, NotNull]
    private InventoryBehaviour inventoryBehaviour;

    [Tooltip("캐릭터의 CharacterBehaviour 컴포넌트")]
    [SerializeField, NotNull]
    private CharacterBehaviour characterBehaviour;

    [Title(label: "Settings")]

    [Tooltip("만약에 True라면 캐릭터가 발사를 할 때 lowered state를 멈춥니다")]
    [SerializeField]
    private bool stopWhileFiring = true;

    #endregion

    #region FIELDS

    /// <summary>
    /// True면 캐릭터가 무기를 낮추고 수행할 수 있는 행동이 많지 않은 상태입니다.
    /// </summary>
    private bool lowerd;
    /// <summary>
    /// 플레이어가 낮추도록 요청할 때 True가 됩니다.
    /// 다른 상태에 따라서 무기를 직접 내리지 않을 수도 있습니다.
    /// </summary>
    private bool lowerdPressed;

    #endregion

    #region UNITY

    private void Update()
    {
        if(characterAnimator == null || characterBehaviour == null || inventoryBehaviour == null)
        {
            Debug.LogError($"{this.gameObject}에 characterAnimator = {characterAnimator}, characterBehaviour ={characterBehaviour} ,inventoryBehaviour ={inventoryBehaviour} ");

            return;
            
        }
        //캐릭터가 에임 중이 아니거나 ,뛰는 중이 아니거나, 검사 중이 아니거나 무기를 집어 넣지 않았으면
        lowerd = (lowerdPressed || wallAvoidance != null && wallAvoidance.HasWall) && !characterBehaviour.IsAiming() && !characterBehaviour.IsRunning()
            && !characterBehaviour.IsInspecting() && !characterBehaviour.IsHolstered();
        
        //발사하는 동안에는 lowerd 상태를 중지합니다.
        if(stopWhileFiring && characterBehaviour.isHoldingButtonFire())
        {
            lowerd = false;
        }
        //장착 무기에 ItemAnimationDataBehaviour가 있는지 확인
        var animationData = inventoryBehaviour.GetEquipped().GetComponent<ItemAnimationDataBehaviour>();
        if (animationData == null)
            lowerd = false;

        else
        {
            //animationData에 LowerData가 있는지 확인
            if (animationData.GetLowerData() == null)
                lowerd = false;
        }
        
        characterAnimator.SetBool(AHashes.Lowered, lowerd);

    }

    #endregion

    #region GETTERS

    /// <summary>
    /// 캐릭터의 무기가 lowerd 상태이고 캐릭터가 많은 일을 할 수 없는 상태이면 
    /// true를 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public bool IsLowered() => lowerd;

    #endregion

    #region METHODS
    /// <summary>
    /// 무기 낮추기, 캐릭터의 PlayerInput요소에 의해서 호출됩니다.
    /// </summary>
    /// <param name="context"></param>
    public void Lower(InputAction.CallbackContext context)
    {
        //커서가 잠금 해제되어 있는 동안 차단합니다.
        if (!characterBehaviour.isCursorLocked())
            return;
        //에임,검사,뛰기,무기를 넣는 상태라면 실행하지 않습니다.
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
