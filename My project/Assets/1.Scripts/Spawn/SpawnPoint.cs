using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class SpawnPoint : MonoBehaviour
{
    #region FIEDLS
    
    private Renderer spawnRenderer;
    private bool isSpawning = false;

    #endregion

    #region Properites

    public bool IsSpawing
    {
        get { return isSpawning; }

        set { isSpawning = value; }
    }

    #endregion

    #region Unity Methods
    private void Awake()
    {
        spawnRenderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        spawnRenderer.enabled = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isSpawning);
        }
        else
        {
            isSpawning = (bool)stream.ReceiveNext();
        }
    }
    #endregion


}
