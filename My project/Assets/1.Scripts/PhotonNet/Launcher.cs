using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher Instance;

    [SerializeField] 
    TMP_InputField roomNameInputField;
    
    [SerializeField]
    TMP_Text errorText;
    
    [SerializeField]
    TMP_Text roomNameText;

    [SerializeField]
    Transform roomListContent;

    [SerializeField]
    GameObject roomListItemPrefab;

    [SerializeField]
    Transform playerListContent;

    [SerializeField]
    GameObject PlayerListItemPrefab;

    [SerializeField]
    GameObject startGameButton;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
            return;
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("Loading");

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Title");
    }

    /// <summary>
    /// 성공적으로 방에 들어갔을 때
    /// </summary>
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;


        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < players.Count(); i++)
        {
            
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    /// <summary>
    /// 방에 들어가는걸 실패했을 때
    /// </summary>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed :" + message;
        MenuManager.Instance.OpenMenu("Error");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("Title");
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

}
