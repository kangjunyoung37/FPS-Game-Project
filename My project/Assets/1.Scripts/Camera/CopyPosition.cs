using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    private bool x, y, z;

    private Transform target;

    private void Update()
    {
        if (!target) return;
        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y), 
            (z ? target.position.z : transform.position.z));
        Quaternion rot = target.rotation;

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0.0f, target.eulerAngles.y);
    }

    public Transform Target { 
        get { return target; } 
        set { target = value; }
    }
}
