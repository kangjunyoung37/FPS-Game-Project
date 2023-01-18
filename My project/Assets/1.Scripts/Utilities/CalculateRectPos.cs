using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateRectPos : MonoBehaviour
{

    [SerializeField]
    private AttachmentOffset attachmentOffset;

    [SerializeField]
    Transform muzzleTransform;

    [SerializeField]
    Transform laserTransform;

    [SerializeField]
    Transform gripTransform;

    [SerializeField]
    Transform scopeTransform;


    [SerializeField]
    private RectTransform whiteRect;
    [SerializeField]
    private RectTransform rectTransform;
    
    public void Click()
    {

        Vector3 a = whiteRect.anchoredPosition - rectTransform.anchoredPosition;
        if(rectTransform == muzzleTransform)
        {
            attachmentOffset.muzzleOffset = a;
        }
        else if (rectTransform == laserTransform)
        {
            attachmentOffset.laserOffset = a;

        }
        else if (rectTransform == scopeTransform)
        {
            attachmentOffset.scopeOffset = a;
        }
        else if (rectTransform == gripTransform)
        {
            attachmentOffset.gripOffset = a;
        }

    }

}
