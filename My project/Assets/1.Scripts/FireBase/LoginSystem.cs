using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LoginSystem : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;
    private PlayerDataHandler playerDataHandler;

    private void Start()
    {
        playerDataHandler = transform.GetComponent<PlayerDataHandler>();
        FireBaseAuthManager.Instance.Init();
        FireBaseAuthManager.Instance.LoginState = OnChangedState;
    }

    public void OnChangedState(bool sign)
    {
        if (sign)
        {
            playerDataHandler.LoadData();
            MenuManager.Instance.OpenMenu("Title");
            PhotonNetwork.ConnectUsingSettings();
        }
            
        else
            MenuManager.Instance.OpenMenu("Login");
    }

    public void Create()
    {
        string e= email.text;
        string p = password.text;
        FireBaseAuthManager.Instance.Create(e, p);
    }
    public void LogIn()
    {
        string e = email.text;
        string p = password.text;
        FireBaseAuthManager.Instance.Login(e, p);
    }
    public void LogOut()
    {
        FireBaseAuthManager.Instance.LogOut();
    }
}
