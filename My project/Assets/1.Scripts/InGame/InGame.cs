using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using Hashtable = ExitGames.Client.Photon.Hashtable;
public enum MapMode
{
    DeathMatch,
    Explosion
}

public class InGame : MonoBehaviourPunCallbacks, IPunObservable
{

    #region SERIALZIED FIELDS

    [SerializeField]
    private MapMode mapMode = MapMode.DeathMatch;
    
    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text timeText;

    [SerializeField]
    private Transform[] redTransform;

    [SerializeField]
    private Transform[] blueTransform;

    [SerializeField]
    private TMP_Text gameEndText;
    #endregion

    #region FIELDS
    
    private static InGame instance;
    private PhotonView PV;
    
    //나중에 포톤룸 설정에서 받아올거   
    [SerializeField]
    private int winPoint = 0;
    [SerializeField]
    private float totalTime = 600.0f;
    [SerializeField]
    private float rountTime = 180.0f;
    private float curTime;
    private float startTime;
    private int redTeamPoint = 0;
    private int blueTeamPoint = 0;
    private bool redTeamEnter = false;
    private bool blueTeamEnter = false;
    private bool bombinstallation = false;
    int roomPlayerCnt;
    private bool gameStart = false;
    private bool gameEnd = false;
    Room currentRoom;

    #endregion

    #region PROPERTIES

    public int RedTeamPoint
    {
        get { return redTeamPoint; } 
        set { redTeamPoint = value; }
        
    }

    public int BlueTeamPoint
    {
        get { return blueTeamPoint; }      
        set { blueTeamPoint = value; }
        
    }

    public bool RedTeamEnter
    {
        set { redTeamEnter = value; }
    }

    public bool BlueTeamEnter
    {
        set { blueTeamEnter = value; }
    }

    public static InGame Instance
    {
        get
        {
            if (instance == null)
                return null;
            else
                return instance;
        }
    }

    #endregion

    #region UnityMethods
    
    private void Awake()
    {
        roomPlayerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
        currentRoom = PhotonNetwork.CurrentRoom;
        instance = this;
        PV = GetComponent<PhotonView>();
        scoreText.text = $" Blue :{blueTeamPoint} , Red : {RedTeamPoint}";
        
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (!gameStart && blueTeamEnter && redTeamEnter)
            {
                startTime = (float)System.DateTime.Now.TimeOfDay.TotalSeconds;
                gameStart = true;
            }
        }
        if (gameEnd)
            gameEndText.text = "end";
        RoomCheckAndUpdate();
        //데스매치일 경우
        TimeUpdate();
    }


    #endregion

    #region METHODS

    private void RoomCheckAndUpdate()
    {
        if(currentRoom.PlayerCount != roomPlayerCnt)
        {
            roomPlayerCnt = currentRoom.PlayerCount;
            Hashtable roomHash = currentRoom.CustomProperties;
            Player[] players = PhotonNetwork.PlayerList;
            int redCnt = 0;
            int blueCnt = 0;
            foreach (Player player in players)
            {
                if ((int)player.CustomProperties["Team"] == 1)
                    redCnt++;
                else
                    blueCnt++;
            }
            roomHash["red"] = redCnt;
            roomHash["blue"] = blueCnt;
            currentRoom.SetCustomProperties(roomHash);
        }
    }

    public void GameCheck(int team)
    {
        switch(mapMode)
        {
            case MapMode.DeathMatch:
                 ScoreUP(team);

                 break;
            case MapMode.Explosion:

                break;
        }        
    }

    private void ScoreUP(int team)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //점수 올리기
            if (team == 0)
                blueTeamPoint += 1;
            else
                redTeamPoint += 1;
        }

        scoreText.text = "Blue :" + blueTeamPoint.ToString() + "Red :" + redTeamPoint.ToString();
    }

    private void TimeUpdate()
    {
        if (gameStart)
        {
            curTime = (float)System.DateTime.Now.TimeOfDay.TotalSeconds - startTime;
        }
        if(totalTime == curTime)
        {
            gameEnd = true;
        }
        int minute = (int)(totalTime - curTime) / 60;
        int second = (int)(totalTime - curTime) % 60;
        
        timeText.text = minute.ToString("00") +":"+ second.ToString("00");
    }

    private void StartGame()
    {
        
    }

    private void GameReset()
    {

    }

    public void ExitGame()
    {
        Destroy(RoomManager.Instance.gameObject);
        PhotonNetwork.Disconnect();
        LoadNextScene();    
    }

    public void LoadNextScene()
    {
        // 비동기적으로 Scene을 불러오기 위해 Coroutine을 사용한다.
        StartCoroutine(LoadMyAsyncScene());
    }

    IEnumerator LoadMyAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    #endregion

    #region PUNCallBack

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(redTeamPoint);
            stream.SendNext(blueTeamPoint);
            stream.SendNext(curTime);
            stream.SendNext(gameStart);
            stream.SendNext(gameEnd);
            stream.SendNext(startTime);

        }
        else
        {
            redTeamPoint = (int)stream.ReceiveNext();
            blueTeamPoint = (int)stream.ReceiveNext();
            curTime = (float)stream.ReceiveNext();
            gameStart = (bool)stream.ReceiveNext();
            gameEnd = (bool)stream.ReceiveNext();
            startTime = (float)stream.ReceiveNext();
            scoreText.text = "Blue :" + blueTeamPoint.ToString() + "Red :" + redTeamPoint.ToString();
        }
    }

    #endregion
}
