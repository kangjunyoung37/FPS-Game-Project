using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LoginSystem : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;

    public TMP_Text outputText;

    private void Start()
    {
        FireBaseAuthManager.Instance.Init();
        FireBaseAuthManager.Instance.LoginState = OnChangedState;
    }

    public void OnChangedState(bool sign)
    {
        outputText.text = sign ? "로그인 : " : "로그아웃 : ";
        outputText.text += FireBaseAuthManager.Instance.UserId;
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
