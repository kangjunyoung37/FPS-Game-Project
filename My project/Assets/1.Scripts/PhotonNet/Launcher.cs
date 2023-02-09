using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using WebSocketSharp;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region SerializeField

    [Header("Player Data")]
    
    public PlayerStorageData playerStorageData;

    public PlayerData playerData;

    [Header("Room ")]
    
    [SerializeField]
    Transform roomListContent;

    [SerializeField]
    TMP_Text roomNameText;

    [SerializeField]
    GameObject roomListItemPrefab;

    [SerializeField] 
    TMP_InputField roomNameInputField;

    [SerializeField]
    TMP_InputField roomSizeInputField;

    [SerializeField]
    TMP_Dropdown mapdropdown;

    [SerializeField]
    TMP_Dropdown timeDropDown;

    [SerializeField]
    TMP_Dropdown pointDropDown;

    [Header("PlayerList")]
    [SerializeField]
    Transform playerRedListContent;

    [SerializeField]
    Transform playerBlueListContent;

    [SerializeField]
    GameObject PlayerListItemPrefab;

    [Header("Start Button")]

    [SerializeField]
    GameObject startGameButton;

    [Header("Warning Alert")]
    
    [SerializeField]
    GameObject WaringAlert;
    
    [SerializeField]
    TMP_Text errorText;

    [Header("Player ID Set")]

    [SerializeField]
    TMP_InputField playerIDInputField;
    
    [Header("Debug Mode")]
    
    [SerializeField]
    bool practiceMode = false;

    #endregion

    #region Field
    public static Launcher Instance;

    private List<RoomInfo> myroomlist = new List<RoomInfo>();
    private int redTeamCnt = 0;
    private int blueTeamCnt = 0;
    private MainCamManager mainCamManager;
    private int roomTime;
    private int winPoint;
    #endregion

    #region UNITYMETHODS
    void Awake()
    {
        Instance = this;
        PhotonNetwork.UseRpcMonoBehaviourCache = true;
        mainCamManager = GetComponent<MainCamManager>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        roomTime = 5;
        winPoint = 5;
    }
    void Start()
    {
 
        if(FireBaseAuthManager.Instance.Auth == null)
        {
            MenuManager.Instance.OpenMenu("Login");
            return;
        }
        if (PhotonNetwork.CurrentLobby != null)
        {
            if(PhotonNetwork.NetworkClientState != ClientState.ConnectingToMasterServer)
                PhotonNetwork.JoinLobby();
        }
           
                 
    }

    #endregion

    #region PhotonNetwork
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

    // 방에 플레이어가 들어갔을 때
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

    // 사용자가 방에서 나갈 때 호출
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Title");
    }

    // 성공적으로 방에 들어갔을 때
    public override void OnJoinedRoom()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if ((bool)room.CustomProperties["practice"] == true)
        {
            PhotonNetwork.LoadLevel("LoadingScene");
            return;
        }
        Hashtable hashtable = room.CustomProperties;
        int temp;
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 1)
        {
            temp = (int)hashtable["red"];
            hashtable["red"] = temp + 1;
        }
        else
        {
            temp = (int)hashtable["blue"];
            hashtable["blue"] = temp + 1;
        }
        room.SetCustomProperties(hashtable);

        MenuManager.Instance.OpenMenu("Room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerRedListContent)
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
            else if ((int)players[i].CustomProperties["Team"] == 1)
            {
                Instantiate(PlayerListItemPrefab, playerRedListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    // 방에 들어가는걸 실패했을 때
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed :" + message;
        MenuManager.Instance.OpenMenu("Error");
    }

    public override void OnConnectedToMaster()
    {
        //Debug.Log("Connected to Master");
        PhotonNetwork.AutomaticallySyncScene = true;
        //처음 들어왔을때만 프로퍼티를 정의
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"] == null)
        {
            int mainWeapon = playerData.MainWeapon;
            int subWeapon = playerData.SubWeapon;

            PlayerWeaponData mainWeaponData = playerStorageData.playerStorage.playerWeaponData[mainWeapon];
            PlayerWeaponData subWEaponData = playerStorageData.playerStorage.playerWeaponData[subWeapon];

            PhotonNetwork.LocalPlayer.CustomProperties = new Hashtable {
            {"Team" ,0},
            {"IsDead", false },
            {"MainWeapon" , mainWeapon},
            {"MainScope" ,  mainWeaponData.scopeIndex},
            {"MainMuzzle" , mainWeaponData.muzzleIndex },
            {"MainLaser" , mainWeaponData.laserIndex },
            {"MainGrip" , mainWeaponData.gripIndex },
            {"SubWeapon" , subWeapon},
            {"SubScope" , subWEaponData.scopeIndex },
            {"SubMuzzle" ,subWEaponData.muzzleIndex },
            {"SubLaser" , subWEaponData.laserIndex },
            {"Kill" , 0 },
            {"Death" , 0 }
        };
        }
        if(playerData.userName == string.Empty)
            MenuManager.Instance.OpenMenu("SetPlayerName");
        
        else
        {
             PhotonNetwork.JoinLobby();
        }
            
        


    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.LocalPlayer.NickName = playerData.userName;
        MenuManager.Instance.OpenMenu("Title");
        Hashtable playerHash = PhotonNetwork.LocalPlayer.CustomProperties;
        playerHash["Team"] = 0;
        playerHash["IsDead"] = false;
        playerHash["Kill"] = 0;
        playerHash["Death"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerHash);
 
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if ((int)changedProps["Team"] == 0)
        {
            Transform trans = FindPlayer(targetPlayer, playerRedListContent);
            if(trans != null)
                trans.SetParent(playerBlueListContent);
        }
        else
        {
            Transform trans = FindPlayer(targetPlayer, playerBlueListContent);
            if(trans != null)
                trans.SetParent(playerRedListContent);
        }
    }

    #endregion

    #region METHODS

    private Transform FindPlayer(Player player, Transform ListContent)
    {
        foreach (Transform child in ListContent)
        {
            if (child.GetComponent<PlayerListItem>().player == player)
            {
                return child;
            }
        }

        return null;
    }

    private void roomListInstantiate()
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < myroomlist.Count; i++)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(myroomlist[i]);
        }
    }



    /// <summary>
    /// 값이 바뀔 때마다 호출
    /// </summary>
    public void OnValueChange()
    {
        if (roomSizeInputField.text.IsNullOrEmpty())
            return;
        int roomsize = int.Parse(roomSizeInputField.text);
        if (roomsize <= 0 || roomsize > 10)
        {
            roomSizeInputField.text = "10";
        }
        else
        {
            if (roomsize % 2 == 1)
            {
                roomsize += 1;
            }
            roomSizeInputField.text = roomsize.ToString();
        }
    }

    #region ButtonEvent

    /// <summary>
    /// 경고창 키고 끄기
    /// </summary>
    public void WarningAlertExit(bool want)
    {
        WaringAlert.SetActive(want);
    }

    /// <summary>
    /// 방 들어가기
    /// </summary>
    public void JoinRoom(RoomInfo info)
    {
        if (info.MaxPlayers == info.PlayerCount)
        {
            WarningAlertExit(true);
            return;
        }

        int redCnt = (int)info.CustomProperties["red"];
        int blueCnt = (int)info.CustomProperties["blue"];
        Hashtable playerHash = PhotonNetwork.LocalPlayer.CustomProperties;

        if (redCnt >= blueCnt)
            playerHash["Team"] = 0;
        else
            playerHash["Team"] = 1;

        PhotonNetwork.SetPlayerCustomProperties(playerHash);
        if (!PhotonNetwork.JoinRoom(info.Name))
            return;
        MenuManager.Instance.OpenMenu("Loading");

    }

    /// <summary>
    /// 방 떠나기
    /// </summary>
    public void LeaveRoom()
    {
        Hashtable roomHash = PhotonNetwork.CurrentRoom.CustomProperties;

        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 1)
        {
            int redCnt = (int)roomHash["red"];
            roomHash["red"] = redCnt - 1;

        }
        else
        {
            int blueCnt = (int)roomHash["blue"];
            roomHash["blue"] = blueCnt - 1;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame()
    {
        if (practiceMode)           
        {
            PhotonNetwork.LoadLevel("LoadingScene");
            return;
        }
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["red"] > 0 && (int)PhotonNetwork.CurrentRoom.CustomProperties["blue"] > 0)
            PhotonNetwork.LoadLevel("LoadingScene");
        else
        {
            //경고창 띄우기
        }
            
    }

    public void OnClickTeambutton(int Team)
    {
        Room curRoom = PhotonNetwork.CurrentRoom;
        Hashtable roomTable = curRoom.CustomProperties;
        Hashtable hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
        int teamMaxPlayer = (int)PhotonNetwork.CurrentRoom.MaxPlayers / 2;

        //자기가 속해 있는 팀을 선택했을 경우 그냥 리턴
        if ((int)hashtable["Team"] == Team)
            return;
        redTeamCnt = (int)roomTable["red"];
        blueTeamCnt = (int)roomTable["blue"];

        //Team이 red일경우
        if (Team == 1)
        {
            if (redTeamCnt >= teamMaxPlayer) return;
            //팀바꾸기
            hashtable["Team"] = 1;
            roomTable["red"] = redTeamCnt + 1;
            roomTable["blue"] = blueTeamCnt - 1;
            PhotonNetwork.SetPlayerCustomProperties(hashtable);

        }
        //Team이 blue일 경우
        else
        {
            if (blueTeamCnt >= teamMaxPlayer) return;
            hashtable["Team"] = 0;
            roomTable["blue"] = blueTeamCnt + 1;
            roomTable["red"] = redTeamCnt - 1;
            PhotonNetwork.SetPlayerCustomProperties(hashtable);
        }
        curRoom.SetCustomProperties(roomTable);

    }

    /// <summary>
    /// 방만들기
    /// </summary>
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text) || string.IsNullOrEmpty(roomSizeInputField.text))
            return;
        RoomOptions roomoptions = new RoomOptions();
        roomoptions.MaxPlayers = byte.Parse(roomSizeInputField.text);
        roomoptions.CustomRoomProperties = new Hashtable
        {
            { "map", mapdropdown.value },
            {"red",  0},
            {"blue", 0},
            {"time" ,roomTime},
            {"point",winPoint },
            {"practice" ,false }
        };

        string[] customProperties = new string[6];
        customProperties[0] = "map";
        customProperties[1] = "red";
        customProperties[2] = "blue";
        customProperties[3] = "time";
        customProperties[4] = "point";
        customProperties[5] = "practice";
        roomoptions.CustomRoomPropertiesForLobby = customProperties;
        PhotonNetwork.CreateRoom(roomNameInputField.text, roomoptions);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public void EnterWeaponCustom()
    {
        MenuManager.Instance.OpenMenu("CustomMenu");
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }


    IEnumerator WaitAndJoinLobby()
    {
        bool check = false;
        while (check)
        {
            yield return new WaitForSeconds(2.0f);
            check = true;
            

        }
        PhotonNetwork.JoinLobby();
    }

    public void ChangeValueDropDown()
    {
        roomTime = int.Parse(timeDropDown.captionText.text);
        winPoint = int.Parse(pointDropDown.captionText.text);

    }

    public void EnterPracticeRoom()
    {
        MenuManager.Instance.OpenMenu("Loading");
        RoomOptions roomoptions = new RoomOptions();
        roomoptions.MaxPlayers = 1;
        roomoptions.IsOpen = false;
        roomoptions.IsVisible = false;
        roomoptions.CustomRoomProperties = new Hashtable
        {
            {"map", 2 },
            {"red",  0},
            {"blue", 0},
            {"time" ,10},
            {"point",10},
            {"practice",true }
        };

        string[] customProperties = new string[6];
        customProperties[0] = "map";
        customProperties[1] = "red";
        customProperties[2] = "blue";
        customProperties[3] = "time";
        customProperties[4] = "point";
        customProperties[5] = "practice";
        roomoptions.CustomRoomPropertiesForLobby = customProperties;
        PhotonNetwork.CreateRoom("", roomoptions);
       

    }

    IEnumerator DisconnectedServerAndOffline()
    {
        while(true)
        {
            if(PhotonNetwork.NetworkClientState == ClientState.Disconnected)
            {
                PhotonNetwork.OfflineMode = true;
                break;
            }

            yield return null;
        }

    }


    #endregion

    #endregion



}
