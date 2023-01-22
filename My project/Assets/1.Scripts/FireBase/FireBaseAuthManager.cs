using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro;
using System;

public class FireBaseAuthManager
{
    private static FireBaseAuthManager instance = null;

    public static FireBaseAuthManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new FireBaseAuthManager();
            }
            return instance;
        }
        
    }
    private FirebaseAuth auth;
    private FirebaseUser user;

    private Uri photoUrI;

    public Action<bool> LoginState;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;

        if(auth.CurrentUser != null)
        {
            LogOut();
        }
        auth.StateChanged += OnChaged;
    }

    public void OnChaged(object sender, EventArgs e)
    {
        if(auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if(!signed && user != null)
            {
                Debug.Log("�α׾ƿ�");
                LoginState?.Invoke(false);
            }
            user = auth.CurrentUser;

            if(signed)
            {
                Debug.Log("�α���");
                LoginState?.Invoke(true);
            }
        }
    }

    public void Create(string email,string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if(task.IsCanceled)
            {
                Debug.LogError("ȸ������ ���");
                return;
            }
            if(task.IsFaulted)
            {
                Debug.LogError("ȸ������ ����");
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.Log("ȸ������ �Ϸ�");

        });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("�α��� ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("�α��� ����");
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.Log("�α��� �Ϸ�");

        });
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("�α׾ƿ�");

    }

    public string UserId => user?.UserId ?? string.Empty;

    public Uri PhotonURI => photoUrI;
}
