using Firebase.Database;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class UserData
{
    public string deviceID;
}

public class UserName
{
    public string userName;
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
    public TMP_InputField nickNameInputField;
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
        SaveUserNickName("", userId);
        SavePlayerData(userId);
        SavePlayerStorageData(userId);
        
    }

    public void LoadData()
    {

        bool hassnapshot = false;
        var userId = FireBaseAuthManager.Instance.UserId;
        if(userId == string.Empty)
            return;
        SaveUserDevicedata();
        
        databaseRef.Child(UserDataPath).Child(userId).Child(PlayerUserDataPath).ValueChanged += LogoutFunction;
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

        databaseRef.Child(UserDataPath).Child(userId).Child("username").GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot snapshot = task.Result;

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
            playerData.userName = JsonConvert.DeserializeObject<string>(snapshot.GetRawJsonValue());

        });

    }

    public void SaveUserDevicedata()
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


    public void SavePlayerData(string userId)
    {
        string playerJson = playerData.ToJson();

        databaseRef.Child(UserDataPath).Child(userId).Child(PlayerDataPath).SetRawJsonValueAsync(playerJson).ContinueWith(task =>
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
            Debug.LogFormat("Save Data Sucessfully");
        });
    }

    public void SavePlayerStorageData(string userId)
    {
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

    public void SaveUserNickName(string userNickName, string userID)
    {
   
        Dictionary<string,object> childUpdates =new Dictionary<string,object>();
        childUpdates["/users/" +userID + "/" +"username"] = userNickName;

        databaseRef.UpdateChildrenAsync(childUpdates).ContinueWith(task =>
        {
            Debug.Log("Save");
        });

    }

    public void NickNameCheck()
    {
        string userNickName = nickNameInputField.text;
        if (userNickName == string.Empty)
        {
            Debug.Log("���̵� �������");
        }    
        databaseRef.Child(UserDataPath).OrderByChild("username").EqualTo(userNickName).GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot snapshot = task.Result;

            if(task.Result.ChildrenCount == 0)
            {
                SaveUserNickName(userNickName, FireBaseAuthManager.Instance.UserId);
                MenuManager.Instance.OpenMenu("Title");
                return;
            }
            else
            {
                Debug.Log("�ߺ� ���̵�");
                return;
            }
            
        });

    }

    private void LogoutFunction(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        UserData newUserData = JsonConvert.DeserializeObject<UserData>(args.Snapshot.GetRawJsonValue());
        //device id�� ���� ���� �ٸ� ���
        if (SystemInfo.deviceUniqueIdentifier != newUserData.deviceID)
        {
            Debug.Log("�ߺ� �α���");
            FireBaseAuthManager.Instance.LogOut();

        }
    }

    private void OnDisable()
    {
        databaseRef.Child(UserDataPath).Child(PlayerUserDataPath).ValueChanged -= LogoutFunction;
    }

}

