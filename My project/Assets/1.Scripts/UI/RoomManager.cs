using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private static RoomManager instance;

    public static RoomManager Instance
    {
        get { return instance; }
    }

    private PhotonView PV;

    private void Awake()
    {
        instance = this;
        //PV = GetComponent<PhotonView>();

        DontDestroyOnLoad(gameObject);
              
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
       
        if(scene.buildIndex == 1)//게임 Scene 번호
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }


}
