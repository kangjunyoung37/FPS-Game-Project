using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PracticeRoomManager : MonoBehaviour
{

    [SerializeField]
    private GameObject PlayerCharacter;

    private GameObject playerGameObject;

    void Start()
    {
        playerGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerCharacter Main"), new Vector3(0,0,0),Quaternion.identity);
        //playerGameObject.GetComponent<CharacterBehaviour>().OnCharacterDie += CharacterDie;
    }

 
}
