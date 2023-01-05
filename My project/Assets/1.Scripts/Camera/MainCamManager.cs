using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamManager : MonoBehaviour
{
    public Camera mainCam;
    
    [SerializeField]
    private Transform storageTransform;
    [SerializeField]
    private Transform mainTransform;
    private Vector3 defaultPos;
    private Quaternion defaultRot;
    private float currentTime = 0f;
    public float lerpTime = 5f;

    public void CamMove(Transform wantTransform,string menuname)
    {
        currentTime = 0f;
        Vector3 wantPos = wantTransform.position;
        Quaternion wantRot = wantTransform.rotation;
        defaultPos = mainCam.transform.position;
        defaultRot = mainCam.transform.rotation;
        StartCoroutine(CamMove(wantRot, wantPos, menuname));
    }

    public void CamMoveMain()
    {
        CamMove(mainTransform,"Title");
    }

    public void CamMoveStroage()
    {
        CamMove(storageTransform, "StorageMenu");  
    }

    IEnumerator CamMove(Quaternion wantRot, Vector3 wantPos, string menuname)
    {

        defaultPos = mainCam.transform.localPosition;
        defaultRot = mainCam.transform.localRotation;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= lerpTime)
                currentTime = lerpTime;
            mainCam.transform.rotation = Quaternion.Lerp(defaultRot, wantRot, currentTime / lerpTime);
            mainCam.transform.position = Vector3.Lerp(defaultPos, wantPos, currentTime / lerpTime);

            yield return null;
        }

        MenuManager.Instance.OpenMenu(menuname);

    }


}
