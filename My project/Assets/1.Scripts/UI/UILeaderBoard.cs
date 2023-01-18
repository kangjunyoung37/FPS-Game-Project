using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class UILeaderBoard : MonoBehaviourPunCallbacks
{
    #region Serialize Field
    [SerializeField]
    private GameObject leaderBoardPlayerPrefab;

    [SerializeField]
    private Transform playerRedListTransform;

    [SerializeField]
    private Transform playerBlueListTransform;

    #endregion

    #region PunCallBack

    private void Start()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            OnPlayerEnteredRoom(player);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if ((int)newPlayer.CustomProperties["Team"] == 0)
        {
            Instantiate(leaderBoardPlayerPrefab, playerBlueListTransform).GetComponent<UIReaderBoardPlayerItem>().SetUp(newPlayer);
        }
        else if ((int)newPlayer.CustomProperties["Team"] == 1)
        {
            Instantiate(leaderBoardPlayerPrefab, playerRedListTransform).GetComponent<UIReaderBoardPlayerItem>().SetUp(newPlayer);

        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        UIReaderBoardPlayerItem uiReaderBoardPlayerItem;
        if ((int)changedProps["Team"] == 0)
        {
            uiReaderBoardPlayerItem = FindPlayer(targetPlayer, playerBlueListTransform);
            uiReaderBoardPlayerItem.ChangePlayerKillDeath();
        }

        if ((int)changedProps["Team"] == 1)
        {
            uiReaderBoardPlayerItem = FindPlayer(targetPlayer, playerRedListTransform);
            uiReaderBoardPlayerItem.ChangePlayerKillDeath();
        }

    }

    #endregion

    #region Methods

    private UIReaderBoardPlayerItem FindPlayer(Player player,Transform listTransform)
    {
        foreach(Transform child in listTransform)
        {
            if (child.GetComponent<UIReaderBoardPlayerItem>().player == player)
            {
                return child.GetComponent<UIReaderBoardPlayerItem>();
            }
        }
        return null;

    }


    #endregion
}
