using Firebase.Database;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public string deviceID;


}

public class PlayerDataHandler : MonoBehaviour
{

    private DatabaseReference databaseRef;
    
    private string UserDataPath => "users"; // -> /users/

    private string PlayerDataPath => "imformation";

    private string PlayerStoragePath => "storage";

    private string PlayerUserDataPath => "userdata";

    public PlayerData playerData;
    public PlayerStorageData playerStorageData;
    private UserData userData = new UserData();
    
    private void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        userData.deviceID = SystemInfo.deviceUniqueIdentifier;
    }

    public void SaveData()
    {
        var userId = FireBaseAuthManager.Instance.UserId;
        if (userId == string.Empty)
            return;

        string playerJson = playerData.ToJson();
        databaseRef.Child(UserDataPath).Child(userId).Child(PlayerDataPath).SetRawJsonValueAsync(playerJson).ContinueWith(task =>
        {
            if(task.IsCanceled)
            {
                Debug.Log("Save User data was canceled");
                return;
            }
            if(task.IsFaulted)
            {
                Debug.LogError("Save user data encountered an error" + task.Exception);
                return;
            }
            Debug.LogFormat("Save user data in successfully {0} {1}", userId, playerJson);
        });
        string playerStorageJson = playerStorageData.ToJson();

        databaseRef.Child(UserDataPath).Child(userId).Child(PlayerStoragePath).SetRawJsonValueAsync(playerStorageJson).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Save User data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Save user data encountered an error" + task.Exception);
                return;
            }
            Debug.LogFormat("Save user data in successfully {0} {1}", userId, playerStorageData);
        });
    }

    public void LoadData()
    {

        bool hassnapshot = false;
        var userId = FireBaseAuthManager.Instance.UserId;
        if(userId == string.Empty)
            return;

        databaseRef.Child(UserDataPath).Child(userId).Child(PlayerUserDataPath).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Load User data was canceled");
                return;
            }
            if (task.IsFaulted)
            {

                Debug.LogError("Load user data encountered an error" + task.Exception);
                return;
            }
            DataSnapshot snapshot = task.Result;
            hassnapshot = snapshot.Exists;
            if (!hassnapshot)
            {
                SaveUserdata();
                return;
            }
            else
            {
                UserData newUserdata = JsonConvert.DeserializeObject<UserData>(snapshot.GetRawJsonValue());
                if(newUserdata.deviceID != SystemInfo.deviceUniqueIdentifier)
                {
                    Debug.Log(newUserdata.deviceID + " ");
                    SaveUserdata();
                }
            }
           
            Debug.LogFormat("Load user Data in successfully {0} {1}", userId, snapshot.GetRawJsonValue());
        });
        
        databaseRef.Child(UserDataPath).Child(PlayerUserDataPath).ValueChanged += LogoutFunction;
        databaseRef.Child(UserDataPath).Child(userId).Child(PlayerDataPath).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Load User data was canceled");
                return;
            }
            if (task.IsFaulted)
            {

                Debug.LogError("Load user data encountered an error" + task.Exception);
                return;
            }
            DataSnapshot snapshot = task.Result;
            hassnapshot = snapshot.Exists;
            if(!hassnapshot)
            {
                playerData.Init();
                playerStorageData.Init();
                SaveData();
                return;
            }
            playerData.FromJson(snapshot.GetRawJsonValue());
            
            Debug.LogFormat("Load user Data in successfully {0} {1}", userId, snapshot.GetRawJsonValue());
        });

        databaseRef.Child(UserDataPath).Child(userId).Child(PlayerStoragePath).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Load User data was canceled");
                return;
            }
            if (task.IsFaulted)
            {

                Debug.LogError("Load user data encountered an error" + task.Exception);
                return;
            }
            DataSnapshot snapshot = task.Result;
            playerStorageData.FromJson(snapshot.GetRawJsonValue());
                 
            Debug.LogFormat("Load user Data in successfully {0} {1}", userId, snapshot.GetRawJsonValue());
        });


    }

    public void SaveUserdata()
    {
        var userId = FireBaseAuthManager.Instance.UserId;

        string playerUserDataJson = JsonConvert.SerializeObject(userData, Formatting.Indented);

        databaseRef.Child(UserDataPath).Child(userId).Child(PlayerUserDataPath).SetRawJsonValueAsync(playerUserDataJson).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Save User data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Save user data encountered an error" + task.Exception);
                return;
            }
            Debug.LogFormat("Save user data in successfully {0} {1}", userId, playerUserDataJson);
        });


    }

    private void LogoutFunction(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        //device id와 받은 값이 다른 경우
        if (SystemInfo.deviceUniqueIdentifier != (string)args.Snapshot.Value)
        {
            Debug.Log((string)args.Snapshot.Value);
            //로그아웃 처리
            Debug.Log("dd");
        }
    }

    private void OnDisable()
    {
        databaseRef.Child(UserDataPath).Child(PlayerUserDataPath).ValueChanged -= LogoutFunction;
    }

}

