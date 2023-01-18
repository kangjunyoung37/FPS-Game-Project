using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;




public class UIWeapon : MonoBehaviour, IDragHandler
{

    [Title(label:"Weapon Type")]
    public WeaponType weaponType = WeaponType.AR;

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

    [Title(label: "Offset")]
    [SerializeField]
    AttachmentOffset attachmentOffset;

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
        get { return muzzlePoint + attachmentOffset.muzzleOffset; }
    }
    public Vector3 GripPoint
    { 
        get { return gripPoint + attachmentOffset.gripOffset; }
    }
    public Vector3 ScopePoint
    {
        get { return scopePoint + attachmentOffset.scopeOffset; }
    }
    public Vector3 LaserPoint
    {
        get { return laserPoint + attachmentOffset.laserOffset; }
    }
    public Transform GetMuzzleTransform => muzzleTransform;
    public Transform GetGripTransform => gripTransform;
    public Transform GetScopeTransform => scopeTransform;
    public Transform GetLaserTransform => laserTransform;

    #endregion

}
