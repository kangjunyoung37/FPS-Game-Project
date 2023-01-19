using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public enum MapMode
{
    DeathMatch,
    Explosion
}

public class InGame : MonoBehaviourPunCallbacks, IPunObservable
{
    
    private class PoolItem
    {
        public bool isActive;
        public GameObject gameObject;
    }

    #region SERIALZIED FIELDS

    [Title(label:"InGame Settings")]

    [Header("Map Mode")]
    
    [SerializeField]
    private MapMode mapMode = MapMode.DeathMatch;

    [Header("Score")]
    
    [SerializeField]
    private TMP_Text blueScoreText;
    
    [SerializeField]
    private TMP_Text redScoreText;
    
    [SerializeField]
    private TMP_Text toalScoreText;


    [Header("Mini Map")]
    
    [SerializeField]
    private TMP_Text timeText;

    public CopyPosition miniMapCameraCopyTransform;

    [Header("Spawn Point")]
    [SerializeField]
    private SpawnPoint[] redSpawnPoints;

    [SerializeField]
    private SpawnPoint[] blueSpawnPoints;

    [Header("Game End Text")]
    
    [SerializeField]
    private TMP_Text gameEndText;

    [SerializeField]
    private TMP_Text winOrLose;

    [Header("Post Proecessing")]

    [SerializeField , NotNull]
    private PostProcessVolume PPV;

    [SerializeField, NotNull]
    private PostProcessVolume weaponPPV;

    [Header("Blood Frame")]
    [SerializeField]
    private Image bloodFrame;

    [Header("DashBoard")]

    [SerializeField]
    private GameObject leaderBoard;


    [Header("Kill Log")]

    [SerializeField]
    private Transform killPeedTrnasform;

    [SerializeField]
    GameObject killLogGameObject;

    [Header("Setting Menu")]

    [SerializeField]
    private SettingMenu settingMenu;

    [Header("Memory Pool")]

    [SerializeField]
    private Transform projectileParent;

    [SerializeField]
    private GameObject poolObject;
    
    private DamageIndicatorSystem indicatorSystem;

    public float lerpTime = 10.0f;
    float currentTime = 0;
    bool stop = true;
    #endregion

    #region FIELDS

    private static InGame instance;
    private PhotonView PV;

    private int increaseCount = 5;
    private int maxCount;
    private int activeCount;

    private List<PoolItem> poolItemList = new List<PoolItem>();

    //나중에 포톤룸 설정에서 받아올거   
    [SerializeField]
    private int winPoint = 0;
    [SerializeField]
    private float totalTime = 600.0f;
    private float curTime;
    private float startTime;
    private float colorAlpha = 1.0f;
    private int redTeamPoint = 0;
    private int blueTeamPoint = 0;
    private bool redTeamEnter = false;
    private bool blueTeamEnter = false;
    private bool bloodIsRunning = false;
    //[SerializeField]
    //private float rountTime = 180.0f;
    //private bool bombinstallation = false;
    int roomPlayerCnt;
    private bool gameStart = false;
    [SerializeField]
    private bool gameEnd = false;
    Room currentRoom;

    private Dictionary<int,Transform> userTransform = new Dictionary<int,Transform>();
    private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
   
    private DepthOfField DOF;
    private DepthOfField weaponDOF;

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
    
    public bool Stop
    {
        set { stop = value; }
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
        activeCount = 0;
        InstantiateObjects(30);
        indicatorSystem = GetComponentInChildren<DamageIndicatorSystem>();
        PPV.profile.TryGetSettings(out DOF);
        weaponPPV.profile.TryGetSettings(out weaponDOF);
        roomPlayerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
        currentRoom = PhotonNetwork.CurrentRoom;
        instance = this;
        PV = GetComponent<PhotonView>();
        toalScoreText.text = winPoint.ToString();
        blueScoreText.text = blueTeamPoint.ToString();
        redScoreText.text = redTeamPoint.ToString();

    }

    private void Start()
    {
        StartCoroutine(GameStart());
    }

    #endregion

    #region METHODS

    /// <summary>
    /// 오브젝트 풀링
    /// </summary>
    private void InstantiateObjects(int createCount)
    {
        maxCount += createCount;

        for(int i = 0; i< createCount; ++i)
        {
            PoolItem poolItem = new PoolItem();
            poolItem.isActive = false;
            poolItem.gameObject = Instantiate(poolObject, projectileParent);
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);

        }
    }

    /// <summary>
    /// 오브젝트 풀링중인 모든 오브젝트 삭제 후 초기화
    /// </summary>
    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    /// <summary>
    /// 활성화
    /// </summary>
    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;

        if(maxCount == activeCount)
            InstantiateObjects(increaseCount);

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];
            if(poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                //poolItem.gameObject.SetActive(true);
                return poolItem.gameObject;
            }
        }
        return null;
    }
    
    /// <summary>
    /// 원하는 오브젝트 풀링 비활성화
    /// </summary>
    /// <param name="removeObject"></param>
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;
        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];
            if(poolItem.gameObject == removeObject)
            {
                activeCount--;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
                poolItem.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                return;
            }
        }
    }

    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }
        activeCount = 0;

    }

    public void CreateKillPeed(string killPlayer,string deadPlayer,int killerTeam, int deadTeam , int index)
    {

        int cnt = killPeedTrnasform.childCount;
        if (cnt == 4)
        {
            KillLog killLog = transform.GetChild(0).GetComponent<KillLog>();
            transform.GetChild(0).transform.SetAsLastSibling();
            killLog.Setup(killPlayer, deadPlayer,killerTeam,deadTeam , index);

            return;
        }
        GameObject killlog = Instantiate(killLogGameObject, killPeedTrnasform);
        killlog.GetComponent<KillLog>().Setup(killPlayer, deadPlayer,killerTeam,deadTeam , index);

    }

    IEnumerator GameStart()
    {
        while(!gameEnd)
        {

            if (PhotonNetwork.IsMasterClient)
            {
                if (!gameStart && blueTeamEnter && redTeamEnter)
                {
                    startTime = (float)System.DateTime.Now.TimeOfDay.TotalSeconds;
                    gameStart = true;
                }
            }
            TimeUpdate();
            RoomCheckAndUpdate();
            if (blueTeamPoint == winPoint || redTeamPoint == winPoint)
                gameEnd = true;
            
            yield return null;

        }

        leaderBoard.SetActive(true);
        CalculateScore();
        yield return new WaitForSeconds(5f);

        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.DestroyAll();
        PhotonNetwork.LeaveRoom();


    }

    private void CalculateScore()
    {
        if (blueTeamPoint == redTeamPoint)
        {
            //Draw
            winOrLose.text = "Draw";
            winOrLose.color = Color.gray;
        }
        else if (blueTeamPoint > redTeamPoint)
        {
            //Blue Team win
            ChangeWinOrLoseText(0);
        }
        else
        {
            //Red Team win
            ChangeWinOrLoseText(1);
        }
    }

    private void ChangeWinOrLoseText(int winTeam)
    {
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == winTeam)
        {
            winOrLose.text = "WIN";
            winOrLose.color = Color.blue;
        }
        else
        {
            winOrLose.text = "Lose";
            winOrLose.color = Color.red;
        }
    }

    public override void OnLeftRoom()
    {
        LoadNextScene();
    }

    /// <summary>
    /// 딕션너리 업데이트
    /// </summary>
    /// <param name="chracterTransform"></param>
    /// <param name="ViewID"></param>
    public void UpdateDictionary(Transform chracterTransform, int ViewID)
    {
        if (userTransform.ContainsKey(ViewID))
        {
            userTransform[ViewID] = chracterTransform;
        }
        else
            userTransform.Add(ViewID, chracterTransform);
    }

    private void RoomCheckAndUpdate()
    {
        if(currentRoom.PlayerCount != roomPlayerCnt && !gameEnd)
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
            if (redCnt == 0 || blueCnt == 0)
                gameEnd = true;
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
        if (PhotonNetwork.IsMasterClient && !gameEnd)
        {
            //점수 올리기
            if (team == 0)
            {
                blueTeamPoint += 1;
                blueScoreText.text = blueTeamPoint.ToString();
            }
                          
            else
            {
                redTeamPoint += 1;
                redScoreText.text = redTeamPoint.ToString();
            }                  
        }
    }

    private void TimeUpdate()
    {
        if (gameStart && !gameEnd)
        {
            curTime = (float)System.DateTime.Now.TimeOfDay.TotalSeconds - startTime;
        }
        if((int)totalTime == (int)curTime)
        {
            gameEnd = true;
            return;
        }
        int minute = (int)(totalTime - curTime) / 60;
        int second = (int)(totalTime - curTime) % 60;
        
        timeText.text = minute.ToString("00") +":"+ second.ToString("00");
    }

    public void ExitGame()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();       
    }

    public void LoadNextScene()
    {
        // 비동기적으로 Scene을 불러오기 위해 Coroutine을 사용한다.
        StartCoroutine(LoadMyAsyncScene());
    }

    public SpawnPoint GetSpawnPoint(int team)
    {
        spawnPoints.Clear();
        //블루팀
        if (team == 0)
        {
            foreach(SpawnPoint spawn in blueSpawnPoints)
            {
                if(!spawn.IsSpawing)
                    spawnPoints.Add(spawn);

            }
        }
        else
        {
            foreach(SpawnPoint spawn in redSpawnPoints)
            {
                if(!spawn.IsSpawing)
                    spawnPoints.Add(spawn);

            }

        }

        SpawnPoint point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
 
        return point;
    }

    IEnumerator LoadMyAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator BloodFrame()
    {

        Color color = bloodFrame.color;
        color.a = colorAlpha;
        while(color.a > 0f)
        {
            color.a = colorAlpha;
            bloodFrame.color = color;
            colorAlpha -= Time.deltaTime;
            yield return null;
        }
        bloodIsRunning = false;
    }

    public void BloodFrameOn()
    {
        colorAlpha = 1.0f;
        if (bloodIsRunning)
            return;
        
        bloodIsRunning = true;
        StartCoroutine(BloodFrame());
    }
    
    public void BlurCamera()
    {
        currentTime = 0f;
        stop = true;
        StartCoroutine(BlurEffect(0.0f, 40.0f));
    }

    IEnumerator BlurEffect(float start, float end)
    {
        weaponDOF.active = true;
        int cnt = 0;
        while (stop)
        {
            if (cnt == 4)
                stop = false;

            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime)
            {
                cnt += 1;
                currentTime = 0.0f;
                continue;
            }
            float t = currentTime / lerpTime;

            t = t * t * t * (t * (6f * t - 15f) + 10f);
            if (cnt % 2 == 0)
                DOF.focalLength.value = Mathf.Lerp(start, end, t);
            else
                DOF.focalLength.value = Mathf.Lerp(end, start,t);

            yield return null;
        }
        weaponDOF.active = false;
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
            blueScoreText.text = blueTeamPoint.ToString();
            redScoreText.text = redTeamPoint.ToString();
        }
        foreach(SpawnPoint spawnPoint in redSpawnPoints)
        {
            spawnPoint.OnPhotonSerializeView(stream, info);
        }
        foreach (SpawnPoint spawnPoint in blueSpawnPoints)
        {
            spawnPoint.OnPhotonSerializeView(stream, info);
        }

    }

    public void SetActiveSettingMenu()
    {
        if (Cursor.lockState == CursorLockMode.None)
            InGame.Instance.GetSettingMenu().OpenSettings();
        else
            InGame.Instance.GetSettingMenu().CloseSettings();
    }

    #endregion

    #region Getters

    public SpawnPoint[] GetRedSpawnPoint() => redSpawnPoints;

    public SpawnPoint[] GetBluSpawnPoint() => blueSpawnPoints;

    public Dictionary<int , Transform> GetDictionary() => userTransform;

    public MapMode GetMapMode() => mapMode;

    public DamageIndicatorSystem GetdamageIndicator() => indicatorSystem;

    public SettingMenu GetSettingMenu() => settingMenu;

    #endregion
}
