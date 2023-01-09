using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;




public class UIWeapon : MonoBehaviour, IDragHandler
{
    [Title(label:"WeaponAttach Transform")]
    [SerializeField]
    private Transform muzzleTransform;
    [SerializeField]
    private Transform gripTransform;
    [SerializeField]
    private Transform scopeTransform;
    [SerializeField]
    private Transform laserTransform;

    [Title(label:"Rotate Speed")]
    public float rotateSpeed = 10;

    [Title(label:"Offset")]
    [SerializeField]
    private Vector3 muzzleOffset;
    [SerializeField]
    private Vector3 gripOffset;
    [SerializeField]
    private Vector3 scopeOffset;
    [SerializeField]
    private Vector3 laserOffset;

    Vector3 muzzlePoint;
    Vector3 gripPoint;
    Vector3 scopePoint;
    Vector3 laserPoint;

    private Quaternion weaponRot;

    private void Awake()
    {
        weaponRot = transform.localRotation;
        muzzlePoint = Camera.main.WorldToScreenPoint(muzzleTransform.position);
        gripPoint = Camera.main.WorldToScreenPoint(gripTransform.position);
        scopePoint = Camera.main.WorldToScreenPoint(scopeTransform.position);
        laserPoint = Camera.main.WorldToScreenPoint(laserTransform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        float x = eventData.delta.x * Time.deltaTime * rotateSpeed;
        float y = eventData.delta.y * Time.deltaTime * rotateSpeed;
        Quaternion rot = Quaternion.Euler(0, -x, 0);
        weaponRot *= rot;
        transform.localRotation = Quaternion.Slerp(rot, weaponRot, rotateSpeed);

    }

    #region Properties

    public Vector3 MuzzlePoint
    {
        get { return muzzlePoint + muzzleOffset; }
    }
    public Vector3 GripPoint
    { 
        get { return gripPoint + gripOffset; }
    }
    public Vector3 ScopePoint
    {
        get { return scopePoint + scopeOffset; }
    }
    public Vector3 LaserPoint
    {
        get { return laserPoint + laserOffset; }
    }
    public Transform GetMuzzleTransform => muzzleTransform;
    public Transform GetGripTransform => gripTransform;
    public Transform GetScopeTransform => scopeTransform;
    public Transform GetLaserTransform => laserTransform;

    #endregion

}
