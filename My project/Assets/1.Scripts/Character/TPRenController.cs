using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPRenController : MonoBehaviour
{
    /// <summary>
    /// TP캐릭터 렌더러
    /// </summary>
    [SerializeField]
    public Renderer[] TPRenderer;

    public void TPRenderOff()
    {
        for(int i = 0; i < TPRenderer.Length; i++)
        {
            TPRenderer[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }


}
