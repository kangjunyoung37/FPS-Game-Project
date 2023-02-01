using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;



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

        if (auth.CurrentUser != null)
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
                //Debug.Log("LogOut");
                LoginState?.Invoke(false);
            }
            user = auth.CurrentUser;

            if(signed)
            {
                //Debug.Log("Login");
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
                Debug.LogError("Register Cancle");
                return;
            }
            if(task.IsFaulted)
            {
                Debug.LogError("Register Failed");
                return;
            }

            FirebaseUser newUser = task.Result;
            //Debug.Log("Register Success");
            
        });

        

    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Login Cancle");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Login Failed");
                return;
            }

            FirebaseUser newUser = task.Result;
            //Debug.Log("Login Success");
           

        });
        
    }

    public void LogOut()
    {
        auth.SignOut();
        //Debug.Log("LogOut");

    }

    public string UserId => user?.UserId ?? string.Empty;

    public Uri PhotonURI => photoUrI;

    public FirebaseAuth Auth => auth;
}
