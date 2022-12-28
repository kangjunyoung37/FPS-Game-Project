using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    #endregion

    #region FIELDS

    private PhotonView PV;
    //나중에 포톤룸 설정에서 받아올거
    [SerializeField]
    private int winPoint = 0;
    [SerializeField]
    private float totalTime = 600.0f;
    [SerializeField]
    private float rountTime;
    private float curTime;
    private float startTime;
    private int redTeamPoint = 0;
    private int blueTeamPoint = 0;
    private bool bombinstallation = false;

    [SerializeField]
    private bool gameStart = false;
    private bool gameEnd = false;
    private bool firstStart = false;
    #endregion

    #region PROPERTIES

    public int RedTeamPoint
    {
        get
        {
            return redTeamPoint;
        }
        set
        {
            redTeamPoint = value;
        }
    }

    public int BlueTeamPoint
    {
        get
        {
            return blueTeamPoint;
        }
        set
        {
            blueTeamPoint = value;
        }
    }

    #endregion

    private static InGame instance;
    
    public static InGame Instance
    {
        get {
            if (instance == null)
                return null;
            else
                return instance; 
            }
    }

    private void Awake()
    {
        instance = this;
        PV = GetComponent<PhotonView>();
        scoreText.text = $" Blue :{blueTeamPoint} , Red : {RedTeamPoint}";

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(redTeamPoint);
            stream.SendNext(blueTeamPoint);
            stream.SendNext(curTime);
            stream.SendNext(gameStart);
 
        }
        else
        {
            redTeamPoint = (int)stream.ReceiveNext();
            blueTeamPoint = (int)stream.ReceiveNext();
            curTime = (float)stream.ReceiveNext();
            gameStart = (bool)stream.ReceiveNext();
            scoreText.text = "Blue :" + blueTeamPoint.ToString() + "Red :" + redTeamPoint.ToString();
        }
    }

    private void Update()
    {
        if (gameStart && !firstStart)
        {
            firstStart = true;
            startTime = (float)System.DateTime.Now.TimeOfDay.TotalSeconds;
        }
        CaculateTime();



    }

    #region METHODS

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

    private void CaculateTime()
    {
        if (PhotonNetwork.IsMasterClient && gameStart)
        {
            curTime = (float)System.DateTime.Now.TimeOfDay.TotalSeconds - startTime;
                  
        }
        int minute = (int)(totalTime - curTime) / 60;
        int second = (int)(totalTime - curTime) % 60;

        timeText.text = minute.ToString("00") +":"+ second.ToString("00");
    }

    private void StartGame()
    {

    }

    #endregion
}
