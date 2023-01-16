
using UnityEngine;

public class UIElement : MonoBehaviour
{
    #region Fields

    protected CharacterBehaviour characterBehaviour;

    protected InventoryBehaviour inventoryBehaviour;

    protected WeaponBehaviour equippedWeapon;

    #endregion

    #region Unity


    protected virtual void Awake()
    {
        characterBehaviour = transform.root.GetComponent<CharacterBehaviour>();
        inventoryBehaviour = characterBehaviour.GetInventory();
    }

    private void Update()
    {
        if (inventoryBehaviour == null)
            return;

        equippedWeapon = inventoryBehaviour.GetEquipped();
        Tick();
    }

    #endregion

    #region Methods

    protected virtual void Tick() { }

    #endregion

}
