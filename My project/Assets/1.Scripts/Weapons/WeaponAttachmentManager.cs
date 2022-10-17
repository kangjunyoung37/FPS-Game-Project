using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WeaponAttachmentManager : WeaponAttachmentManagerBehaviour
{
    #region FIELDS SERIALZED

    [Title(label: "Scope")]
    [Tooltip("���� �𵨿� ���̾����Ʈ�� ǥ������ ���θ� �����մϴ�")]
    [SerializeField]
    private bool scopeDefaultShow = true;

    [Tooltip("�⺻ ������")]
    [SerializeField]
    private ScopeBehaviour scopeDefaultBehaviour;

    [Tooltip("������ �ε��� ���࿡ ������� ���̾����Ʈ�� ����")]
    [SerializeField]
    private int scopeIndex = -1;

    [Tooltip("���� �������� ����� �� ù��° ������ ���ؽ�")]
    [SerializeField]
    private int scopeIndexFirst = -1;

    [Tooltip("������ ���۵Ǹ� ���� �������� ����� ������")]
    [SerializeField]
    private bool scopeIndexRandom;

    [Tooltip("�� ���Ⱑ ����� �� �ִ� ��������")]
    [SerializeField]
    private ScopeBehaviour[] scopeArray;

    [Title(label: "Muzzle")]

    [Tooltip("���õ� �ѱ� �ε���")]
    [SerializeField]
    private int muzzleIndex;

    [Tooltip("������ ���۵Ǹ� ���� �ѱ��� ����� ������")]
    [SerializeField]
    private bool muzzleIndexRandom = true;

    [Tooltip("�� ���Ⱑ ����� �� �ִ� ��� �ѱ� ��������")]
    [SerializeField]
    private MuzzleBehaviour[] muzzleArray;

    [Title(label: "Laser")]

    [Tooltip("���õ� ������ �ε���")]
    [SerializeField]
    private int laserIndex = -1;

    [Tooltip("������ ���۵Ǹ� ���� �������� ����� ������")]
    [SerializeField]
    private bool laserIndexRandom = true;

    [Tooltip("�� ���⿡ ����� �� �ִ� ������ �迭")]
    [SerializeField]
    private LaserBehaviour[] laserArray;

    [Title(label: "Grip")]

    [Tooltip("���õ� �׸�")]
    [SerializeField]
    private int gripIndex = -1;

    [Tooltip("������ ���۵Ǹ� ���� �׸��� ����� ������")]
    [SerializeField]
    private bool gripIndexRandom = true;

    [Tooltip("�� ���⿡ ����� �� �ִ� �׸� �迭")]
    [SerializeField]
    private GripBehaviour[] gripArray;

    [Title(label: "Magazine")]

    [Tooltip("���õ� źâ ���ؽ�")]
    [SerializeField]
    private int magazineIndex;

    [Tooltip("������ ���۵Ǹ� ���� źâ�� ����� ������")]
    [SerializeField]
    private bool magazineIndexRandom = true;

    [Tooltip("�� ���⿡ ����� �� �ִ� źâ �迭")]
    [SerializeField]
    private Magazine[] magazineArray;
    #endregion
    #region FIELDS

    /// <summary>
    /// ������ ������
    /// </summary>
    private ScopeBehaviour scopeBehaviour;

    /// <summary>
    /// ������ �ѱ�
    /// </summary>
    private MuzzleBehaviour muzzleBehaviour;

    /// <summary>
    /// ������ ������
    /// </summary>
    private LaserBehaviour laserBehaviour;

    /// <summary>
    /// ������ �׸�
    /// </summary>
    private GripBehaviour gripBehaviour;

    /// <summary>
    /// ������ źâ
    /// </summary>
    private MagazineBehaviour magazineBehaviour;
    #endregion

    #region UNITY FUNCTIONS

    protected override void Awake()
    {
        //���� ���������
        if (scopeIndexRandom)
            scopeIndex = Random.Range(scopeIndexFirst, scopeArray.Length);
        //������ ����
        scopeBehaviour = scopeArray.SelectAndSetActive(scopeIndex);

        //�������� ���ٸ�
        if(scopeBehaviour == null)
        {
            scopeBehaviour = scopeDefaultBehaviour;

            scopeBehaviour.gameObject.SetActive(scopeDefaultShow);
        }
        //���� �ѱ�
        if (muzzleIndexRandom)
            muzzleIndex = Random.Range(0, muzzleArray.Length);
        //�ѱ� ����
        muzzleBehaviour = muzzleArray.SelectAndSetActive(muzzleIndex);
        //���� ���������
        if (laserIndexRandom)
            laserIndex = Random.Range(0, laserArray.Length);
        //������ ����
        laserBehaviour = laserArray.SelectAndSetActive(laserIndex);

        //���� �׸��̶��
        if(gripIndexRandom)
            gripIndex = Random.Range(0, gripArray.Length);
        //�׸� ����
        gripBehaviour = gripArray.SelectAndSetActive(gripIndex);
        //���� źâ�̶��
        if(magazineIndexRandom)
            magazineIndex = Random.Range(0, magazineArray.Length);
        //źâ ����
        magazineBehaviour = magazineArray.SelectAndSetActive(magazineIndex);

    }
    #endregion

    #region GETTERS

    public override ScopeBehaviour GetEquippedScope() => scopeBehaviour;
    public override ScopeBehaviour GetEquippedScopeDefault() => scopeDefaultBehaviour;

    public override MagazineBehaviour GetEquippedMagazine() => magazineBehaviour;
    public override MuzzleBehaviour GetEquippedMuzzle() => muzzleBehaviour;

    public override LaserBehaviour GetEquippedLaser() => laserBehaviour;
    public override GripBehaviour GetEquippedGrip() => gripBehaviour;

    #endregion
}

