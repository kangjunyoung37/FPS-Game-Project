using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attachment Offset",menuName = "FPS Game/Shooter Pack/AttachmentOffset")]
public class AttachmentOffset : ScriptableObject
{
    public Vector3 muzzleOffset;

    public Vector3 gripOffset;

    public Vector3 laserOffset;

    public Vector3 scopeOffset;

    public void SetUP(Vector3 a)
    {
        muzzleOffset = a;
    }

}
