using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TPRenController : MonoBehaviour
{
    /// <summary>
    /// TP캐릭터 렌더러
    /// </summary>
    [SerializeField]
    public Renderer[] TPRenderer;

    /// <summary>
    /// 캐릭터 콜리더
    /// </summary>
    private Collider[] colliders;
    private PhotonView PV;
    private CharacterBehaviour characterBehaviour;
    private Rigidbody[] rigidbodies;
    private Animator TPAnimator;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        PV = transform.root.GetComponent<PhotonView>();
        TPAnimator = GetComponent<Animator>();
        characterBehaviour = transform.root.GetComponent<CharacterBehaviour>();
        ChageLayer(7);
        characterBehaviour.OnCharacterDie += RagDollOn;

    }
    private void Start()
    {
        StartCoroutine(nameof(ActiveCollider));
        
    }

    IEnumerator ActiveCollider()
    {
        foreach(Collider collider in colliders)
        {
            collider.enabled = false;
        }
        yield return null;
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    private void ChageLayer(int layer)
    {
        if(!PV.IsMine)
        {
            foreach (Collider collider in colliders)
            {
                collider.gameObject.layer = layer;
            }
        }
    }

    private void RagDollOn()
    {
        TPAnimator.enabled = false;
        ChageLayer(6);    
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = false;
        }
    
    }
    
    
    public void TPRenderControl(ShadowCastingMode shadowCastingMode)
    {

        for(int i = 0; i < TPRenderer.Length; i++)
        {
            TPRenderer[i].shadowCastingMode = shadowCastingMode;
        }
    }

    public void TPChangePlayerMaterial(Material material)
    {
        Material[] mat = TPRenderer[0].materials;
        for(int i = 0; i < 5;i++)
        {
            mat[i] = material;
        }
        TPRenderer[0].materials = mat;

    }
    #region GETTERS

    public Collider[] GetColliders() => colliders;

    #endregion


}
