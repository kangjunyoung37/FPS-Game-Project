
using UnityEngine;
using UnityEngine.UI;

public class UIImageWeapon : UIElement
{
    #region Fields Serialzed

    [Title(label: "Color")]

    [SerializeField]
    private Color imageColor = Color.white;


    [SerializeField]
    private Image imageWeaponBody;

    [SerializeField]
    private Image imageWeaponGrip;

    [SerializeField]
    private Image imageWeaponLaser;

    [SerializeField]
    private Image imageWeaponMuzzle;

    [SerializeField]
    private Image imageWeaponMagazine;

    [SerializeField]
    private Image imageWeaponScope;

    [SerializeField]
    private Image imageWEaponScopeDefault;

    #endregion

    #region Fields

    private WeaponAttachmentManagerBehaviour attachmentManagerBehaviour;

    #endregion

    protected override void Tick()
    {
        Color attachmentColor = imageColor;
        
        foreach(Image image in GetComponents<Image>())
        {
            image.color = attachmentColor;
        }
        attachmentManagerBehaviour = equippedWeapon.GetAttachmentManager();

        imageWeaponBody.sprite = equippedWeapon.GetSpriteBody();

        Sprite sprite = default;

        ScopeBehaviour scopeDefaultBehaviour = attachmentManagerBehaviour.GetEquippedScopeDefault();

        if (scopeDefaultBehaviour != null)
            sprite = scopeDefaultBehaviour.GetSprite();

        UpdateSprite(imageWEaponScopeDefault, sprite, scopeDefaultBehaviour == null);

        ScopeBehaviour scopeBehaviour = attachmentManagerBehaviour.GetEquippedScope();

        if (scopeBehaviour != null)
            sprite = scopeBehaviour.GetSprite();

        UpdateSprite(imageWeaponScope, sprite, scopeBehaviour == null || scopeBehaviour == scopeDefaultBehaviour);

        MagazineBehaviour magazineBehaviour = attachmentManagerBehaviour.GetEquippedMagazine();
        if (magazineBehaviour != null)
            sprite = magazineBehaviour.GetSprite();
        UpdateSprite(imageWeaponMagazine, sprite, magazineBehaviour == null);

        LaserBehaviour laserBehaviour = attachmentManagerBehaviour.GetEquippedLaser();
        if (laserBehaviour != null)
            sprite = laserBehaviour.GetSprite();

        UpdateSprite(imageWeaponLaser, sprite, laserBehaviour == null);

        GripBehaviour gripBehaviour = attachmentManagerBehaviour.GetEquippedGrip();
        if (gripBehaviour != null)
            sprite = gripBehaviour.GetSprite();

        UpdateSprite(imageWeaponGrip, sprite, gripBehaviour == null);

        MuzzleBehaviour muzzleBehaviour = attachmentManagerBehaviour.GetEquippedMuzzle();

        if (muzzleBehaviour != null)
            sprite = muzzleBehaviour.GetSprite();

        UpdateSprite(imageWeaponMuzzle, sprite, muzzleBehaviour == null);
    }
    

    private static void UpdateSprite(Image image,Sprite sprite,bool forceHide = false)
    {
        image.sprite = sprite;
        image.enabled = sprite != null && !forceHide;


    }
}
