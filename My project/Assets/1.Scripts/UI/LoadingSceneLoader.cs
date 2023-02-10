using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneLoader : MonoBehaviour
{

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private TMP_Text persentText;

    private void Awake()
    {
        progressBar.fillAmount = 0f;
    }
    void Start()
    { 
        StartCoroutine(nameof(LoadSceneProcess), (int)PhotonNetwork.CurrentRoom.CustomProperties["map"]+1);     
    }


    IEnumerator LoadSceneProcess(int SceneNum)
    {
        PhotonNetwork.LoadLevel(SceneNum);

        while (PhotonNetwork.LevelLoadingProgress < 1)
        {
            progressBar.fillAmount = PhotonNetwork.LevelLoadingProgress;
            persentText.text = (PhotonNetwork.LevelLoadingProgress * 100.0f).ToString() + "%";
            yield return null;
        }

    }

}
