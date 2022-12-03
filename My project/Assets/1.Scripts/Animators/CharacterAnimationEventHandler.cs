using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ĳ���Ϳ��� ������ ��� �ִϸ��̼��� �����մϴ�.
/// </summary>
public class CharacterAnimationEventHandler : MonoBehaviour
{

    #region FIELDS

    /// <summary>
    /// ĳ���� ������Ʈ
    /// </summary>
    private CharacterBehaviour playerCharacter;
    private Character Character;

    #endregion

    #region UNITY

    private void Awake()
    {
        //ĳ���� Behaviour ������Ʈ ��������
        //playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        playerCharacter = transform.gameObject.GetComponentInParent<CharacterBehaviour>();
        
        Character = playerCharacter.GetComponent<Character>();
        
    }

    #endregion

    #region ANIMATION

    /// <summary>
    /// ĳ������ ���� ���⿡�� ź�Ǹ� �����ϴ�.
    /// </summary>
    private void OnEjectCasing()
    {
        if (playerCharacter != null)
            playerCharacter.EjectCasing();
    }

    /// <summary>
    /// ĳ���Ϳ� ������ ������ ź���� ������ ä��ų� 0���� ������ ��� ������ ä��ϴ�.
    /// </summary>
    private void OnAmmunitionFill(int amount = 0)
    {
        if (playerCharacter != null)
            playerCharacter.FillAmmunition(amount);
    }

    /// <summary>
    /// ĳ������ �������� Ȱ��ȭ�մϴ�.
    /// </summary>
    private void OnSetActiveKnife(int active)
    {
        if (playerCharacter != null)
            playerCharacter.SetActiveKnife(active);
    }

    /// <summary>
    /// ��Ȯ�� ��ġ���� ����ź�� �����մϴ�.
    /// </summary>
    private void OnGrenade()
    {
        if (playerCharacter != null)
            playerCharacter.Grenade();
    }

    /// <summary>
    /// ������ ������ źâ�� Ȱ��ȭ�մϴ�.
    /// </summary>
    private void OnSetActiveMagazine(int active)
    {
        if (playerCharacter != null)
            playerCharacter.SetActiveMagzine(active);
    }

    /// <summary>
    /// ��Ʈ �׼� �ִϸ��̼��� �����ϴ�.
    /// </summary>
    private void OnAnimationEndedBolt()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedBolt();
    }

    /// <summary>
    /// ������ �ִϸ��̼��� �����ϴ�.
    /// </summary>
    private void OnAnimationEndedReload()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedReload();
    }

    /// <summary>
    /// ����ź ������ �ִϸ��̼��� �����ϴ�.
    /// </summary>
    private void OnAnimationEndedGrenadeThrow()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedGrenadeThrow();
    }

    /// <summary>
    /// ���� ���� �ִϸ��̼��� �����ϴ�.
    /// </summary>
    private void OnAnimationEndedMelee()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedMelee();
    }

    /// <summary>
    /// �˻� �ִϸ��̼��� �����ϴ�.
    /// </summary>
    private void OnAnimationEndedInspect()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedInspect();
    }

    /// <summary>
    /// ���� �ִ� �ִϸ��̼��� �����ϴ�.
    /// </summary>
    private void OnAnimationEndedHolster()
    {
        if (playerCharacter != null)
            playerCharacter.AnimationEndedHolster();
    }

    /// <summary>
    /// ĳ���Ͱ� ����� ������ �����̵� �� ��� �����մϴ�.
    /// </summary>
    private void OnSlideBack(int back)
    {
        if (playerCharacter != null)
            playerCharacter.SetSlideBack(back);
    }

    private void OnDropMagazine(int drop = 0)
    {
        if (playerCharacter != null)
            playerCharacter.DropMagazine(drop == 0);
    }

    private void isgofalse()
    {
        Character.ishostering = false;
    }

    #endregion
}
