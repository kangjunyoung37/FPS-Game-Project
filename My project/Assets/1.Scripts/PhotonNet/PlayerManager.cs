using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class PlayerManager : MonoBehaviour
{
    #region FIELDS

    PhotonView PV;
    GameObject playerGameObject;
    SpawnPoint mySpawnPoint;
    int team;
    ExitGames.Client.Photon.Hashtable playerHashTable;

    #endregion

    #region Unity Methods
    private void Awake()
    {

        PV = GetComponent<PhotonView>();
        if ((int)PV.Owner.CustomProperties["Team"] == 1)
            InGame.Instance.RedTeamEnter = true;
        else
            InGame.Instance.BlueTeamEnter = true;
        team = (int)PV.Owner.CustomProperties["Team"];
        playerHashTable = PV.Owner.CustomProperties;


    }

    void Start()
    {
       if(PV.IsMine)
        {
            mySpawnPoint = InGame.Instance.GetSpawnPoint(team);
            CreateController(mySpawnPoint);
        }
    }

    #endregion

    #region Methods

    private void CreateController(SpawnPoint spawn)
    {
        spawn.IsSpawing = false;
        playerGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerCharacter Main"), spawn.transform.position, spawn.transform.rotation);
        playerGameObject.GetComponent<CharacterBehaviour>().OnCharacterDie += CharacterDie;
      
    }
    
    IEnumerator ReSpawn()
    {
        mySpawnPoint = InGame.Instance.GetSpawnPoint(team);
        mySpawnPoint.IsSpawing = true;
        yield return new WaitForSecondsRealtime(5.0f);
        PhotonNetwork.Destroy(playerGameObject);
        playerHashTable["IsDead"] = false;
        PV.Owner.SetCustomProperties(playerHashTable);
        CreateController(mySpawnPoint);

    }

    private void CharacterDie()
    {      
        if(InGame.Instance.GetMapMode() == MapMode.DeathMatch)
            StartCoroutine(nameof(ReSpawn));
    }

    #endregion


}
