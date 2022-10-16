using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MagazineBehaviour
{
    #region FIELDS SERIALIZED
    [Title(label: "Settings")]

    [Tooltip("ÃÑ ÅºÃ¢¼ö")]
    [SerializeField]
    private int ammunitionTotal = 10;

    [Title(label: "Interface")]
    [SerializeField]
    private Sprite sprite;
    #endregion
    #region GETTERS
    //ÃÑÅºÃ¢¼ö ¸®ÅÏ
    public override int GetAmmunitionTotal() => ammunitionTotal;

    //ÇØ´ç sprite ¸®ÅÏ
    public override Sprite GetSprite() => sprite;

    #endregion
}
