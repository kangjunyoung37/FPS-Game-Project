using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPRenController : MonoBehaviour
{
    [SerializeField]
    private Renderer[] FPRenderer;

    public void FPRenOff()
    {
        for(int i = 0; i < FPRenderer.Length; i++)
        {
            FPRenderer[i].enabled = false;
        }

    }

    public void FPChangePlayerMaterial(Material playerMaterial)
    {
        FPRenderer[0].material = playerMaterial;
    }
}
