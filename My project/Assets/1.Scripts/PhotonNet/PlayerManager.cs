using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
       if(PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        int x = Random.Range(0, 3);
       
        int z = Random.Range(0, 3);

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerCharacter Main"), new Vector3(x,0.0f,z), Quaternion.identity);
    }
}
