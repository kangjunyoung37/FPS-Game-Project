using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using WebSocketSharp;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher Instance;

    [SerializeField] 
    TMP_InputField roomNameInputField;

    [SerializeField]
    TMP_InputField roomSizeInputField;

    [SerializeField]
    TMP_Text errorText;
    
    [SerializeField]
    TMP_Text roomNameText;

    [SerializeField]
    Transform roomListContent;

    [SerializeField]
    GameObject roomListItemPrefab;

    [SerializeField]
    Transform playerRedListContent;

    [SerializeField]
    Transform playerBlueListContent;

    [SerializeField]
    GameObject PlayerListItemPrefab;

    [SerializeField]
    GameObject startGameButton;

    [SerializeField]
    GameObject WaringAlert;

    [SerializeField]
    TMP_Dropdown dropdown;

    private List<RoomInfo> myroomlist = new List<RoomInfo>();

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }
    /// <summary>
    /// ���� ����
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
    
    /// <summary>
    /// ���â Ű�� ����
    /// </summary>
    public void WarningAlertExit(bool want)
    {
        WaringAlert.SetActive(want);
    }

    /// <summary>
    /// �游���
    /// </summary>
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text) || string.IsNullOrEmpty(roomSizeInputField.text))
            return;
        RoomOptions roomoptions = new RoomOptions();
        int MaxPlayer = int.Parse(roomSizeInputField.text) / 2;
        roomoptions.MaxPlayers = byte.Parse(roomSizeInputField.text);

        roomoptions.CustomRoomProperties = new Hashtable
        {
            { "map", dropdown.value },
            { "red", MaxPlayer },
            {"blue", MaxPlayer }
        };

        string[] customProperties = new string[3];
        customProperties[0] = "map";
        customProperties[1] = "red";
        customProperties[2] = "blue";
        roomoptions.CustomRoomPropertiesForLobby = customProperties;

        PhotonNetwork.CreateRoom(roomNameInputField.text,roomoptions);
        MenuManager.Instance.OpenMenu("Loading");
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void StartGame()
    {
        PhotonNetwork.LoadLevel((int)PhotonNetwork.CurrentRoom.CustomProperties["map"] + 1);
    }

    /// <summary>
    /// �� ������
    /// </summary>
    public void LeaveRoom()
    {
        Room room = PhotonNetwork.CurrentRoom;
        Hashtable roomhash = room.CustomProperties;
        Hashtable playerhash = PhotonNetwork.LocalPlayer.CustomProperties;
      
        //�÷��̾��� ���� Blue�� ���
        if ((int)playerhash["Team"] == 0)
        {
            int bluecnt = (int)roomhash["blue"];
            roomhash["blue"] = bluecnt + 1;
            room.SetCustomProperties(roomhash);
        }
        else
        {
            int redcnt = (int)roomhash["red"];
            roomhash["red"] = redcnt + 1;
            room.SetCustomProperties(roomhash);

        }

        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    /// <summary>
    /// �� ����
    /// </summary>
    public void JoinRoom(RoomInfo info)
    {
        if (info.MaxPlayers == info.PlayerCount)
        {
            WarningAlertExit(true);
            return;
        }

        Hashtable roomProperties = info.CustomProperties;
        int redcnt = (int)roomProperties["red"];
        int bluecnt = (int)roomProperties["blue"];

        if(redcnt > bluecnt)
        {
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = 1;
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = 0;
        }
        

        if (!PhotonNetwork.JoinRoom(info.Name))       
            return;

        MenuManager.Instance.OpenMenu("Loading");

    }

    /// <summary>
    /// ���� �ٲ� ������ ȣ��
    /// </summary>
    public void OnValueChange()
    {
        if (roomSizeInputField.text.IsNullOrEmpty())
            return;
        int roomsize = int.Parse(roomSizeInputField.text);
        if(roomsize <= 0 || roomsize > 10) 
        {
            roomSizeInputField.text = "10";
        }
        else
        {
            if(roomsize % 2 == 1)
            {
                roomsize += 1;
            }
            roomSizeInputField.text = roomsize.ToString();
        }
    }   
   
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {


        for (int i = 0; i < roomList.Count; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myroomlist.Contains(roomList[i]))
                    myroomlist.Add(roomList[i]);
                else
                    myroomlist[myroomlist.IndexOf(roomList[i])] = roomList[i];
                
            }
            else if (myroomlist.IndexOf(roomList[i]) != -1)
            {
                myroomlist.RemoveAt(myroomlist.IndexOf(roomList[i]));
            }
            
        }
        roomListInstantiate();
    }

    private void roomListInstantiate()
    { 
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for(int i = 0; i < myroomlist.Count; i++)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(myroomlist[i]);
        }
    }

    // �濡 �÷��̾ ���� ��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        if ((int)newPlayer.CustomProperties["Team"] == 0)
        {
            Instantiate(PlayerListItemPrefab, playerBlueListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
            
        }
            
        else if ((int)newPlayer.CustomProperties["Team"] == 1)
        {
            Instantiate(PlayerListItemPrefab, playerRedListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        }
    
    }

    // ����ڰ� �濡�� ���� �� ȣ��
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Title");
    }

    // ���������� �濡 ���� ��
    public override void OnJoinedRoom()
    {

        Room room = PhotonNetwork.CurrentRoom;
        Hashtable hashtable = room.CustomProperties;
        int temp;
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 1)
        { 
            temp = (int)hashtable["red"];
            hashtable["red"] = temp - 1;          
        }
        else
        {
            temp = (int)hashtable["blue"];
            hashtable["blue"] = temp - 1;
        }
        room.SetCustomProperties(hashtable);

        MenuManager.Instance.OpenMenu("Room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;    
        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerRedListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in playerBlueListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            if ((int)players[i].CustomProperties["Team"] == 0)
            {
                Instantiate(PlayerListItemPrefab, playerBlueListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
            else if((int)players[i].CustomProperties["Team"] == 1)
            {
                Instantiate(PlayerListItemPrefab, playerRedListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    // �濡 ���°� �������� ��
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
        PhotonNetwork.LocalPlayer.CustomProperties["Team"] = 0;
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void OnClickTeambutton(int Team)
    {
        Room room = PhotonNetwork.CurrentRoom;
        Hashtable hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
        Hashtable roomhashtable = room.CustomProperties;
        
        //�ڱⰡ ���� �ִ� ���� �������� ��� �׳� ����
        if ((int)hashtable["Team"] == Team)
            return;

        int redcnt = (int)roomhashtable["red"];
        int bluecnt = (int)roomhashtable["blue"];

        //Team�� red�ϰ��
        if (Team == 1)
        {
            if ((int)roomhashtable["red"] == 0) return;
            //���ٲٱ�
            hashtable["Team"] = 1;
            PhotonNetwork.SetPlayerCustomProperties(hashtable);
            roomhashtable["red"] = redcnt -= 1;
            roomhashtable["blue"] = bluecnt += 1;
            room.SetCustomProperties(roomhashtable);

        }
        //Team�� blue�� ���
        else
        {
            if ((int)roomhashtable["blue"] == 0) return;
            hashtable["Team"] = 0;
            PhotonNetwork.SetPlayerCustomProperties(hashtable);
            roomhashtable["blue"] = bluecnt -= 1;
            roomhashtable["red"] = redcnt += 1;
            room.SetCustomProperties(roomhashtable);
        }     

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if ((int)changedProps["Team"] == 0)
        {
            Transform trans = FindPlayer(targetPlayer, playerRedListContent);
            trans.SetParent(playerBlueListContent);
        }
        else
        {
            Transform trans = FindPlayer(targetPlayer, playerBlueListContent);
            trans.SetParent(playerRedListContent);
        }
    }

    private Transform FindPlayer(Player player, Transform ListContent)
    {
        foreach(Transform child in ListContent)
        {
            if(child.GetComponent<PlayerListItem>().player == player)
            {
                return child;
            }
        }

        return null;
    }


}
