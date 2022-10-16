using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MagazineBehaviour
{
    #region FIELDS SERIALIZED
    [Title(label: "Settings")]

    [Tooltip("�� źâ��")]
    [SerializeField]
    private int ammunitionTotal = 10;

    [Title(label: "Interface")]
    [SerializeField]
    private Sprite sprite;
    #endregion
    #region GETTERS
    //��źâ�� ����
    public override int GetAmmunitionTotal() => ammunitionTotal;

    //�ش� sprite ����
    public override Sprite GetSprite() => sprite;

    #endregion
}
