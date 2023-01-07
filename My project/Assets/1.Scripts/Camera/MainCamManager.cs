using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamManager : MonoBehaviour
{
    private static MainCamManager instance ;

    public static MainCamManager Instance
    {
        get { 
                if (instance == null)
                    return null;
                return instance;
            }
    }

    public Camera mainCam;
    
    [SerializeField]
    private Transform storageTransform;
    [SerializeField]
    private Transform mainTransform;
    private Vector3 defaultPos;
    private Quaternion defaultRot;
    private float currentTime = 0f;
    public float lerpTime = 5f;

    private void Awake()
    {
        instance = this;
    }

    public void CamMoveWihtMenu(Transform wantTransform,string menuname)
    {
        currentTime = 0f;
        Vector3 wantPos = wantTransform.position;
        Quaternion wantRot = wantTransform.rotation;
        defaultPos = mainCam.transform.position;
        defaultRot = mainCam.transform.rotation;
        StartCoroutine(CamMove(wantRot, wantPos, menuname));
    }

    public void OnlyCamMove(Transform wantTransform)
    {
        currentTime = 0f;
        Vector3 wantPos = wantTransform.position;
        Quaternion wantRot = wantTransform.rotation;
        defaultPos = mainCam.transform.position;
        defaultRot = mainCam.transform.rotation;
        StartCoroutine(CamMove(wantRot, wantPos));
    }

    public void CamMoveMain()
    {
        CamMoveWihtMenu(mainTransform,"Title");
    }

    public void CamMoveStroage()
    {
        CamMoveWihtMenu(storageTransform, "StorageMenu");  
    }

    IEnumerator CamMove(Quaternion wantRot, Vector3 wantPos, string menuname = null)
    {
        

        defaultPos = mainCam.transform.localPosition;
        defaultRot = mainCam.transform.localRotation;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= lerpTime)
                currentTime = lerpTime;
            float t = currentTime / lerpTime;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            mainCam.transform.rotation = Quaternion.Lerp(defaultRot, wantRot, t);
            mainCam.transform.position = Vector3.Lerp(defaultPos, wantPos, t);

            yield return null;
        }
        if(menuname != null)
            MenuManager.Instance.OpenMenu(menuname);
    }

}
