using Firebase.Database;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetPlayerIDManager : MonoBehaviour
{

    [Title(label:"Settings")]
    
    [SerializeField]
    private TMP_InputField playerIDInputField;

    [SerializeField]
    private GameObject warningGameObject;

    [SerializeField]
    private TMP_Text waringText;

    [SerializeField]
    private PlayerData playerData;

    private DatabaseReference databaseRef;

    private string warning;
    private bool isRunning = false;
    private bool goTitle = false;
    string userNickName;
    private void Awake()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Update()
    {
        waringText.text = warning;
    } 

    public void NickNameCheck()
    {
        if (isRunning)
            return;

        userNickName = playerIDInputField.text;
        if (userNickName.Length < 6 ) 
        {
            warning = "ID must be at least 6 characters long";
            WarningPanelSetActive(true, warning);
            return;
        }
       
        isRunning = true;
        databaseRef.Child("users").OrderByChild("username").EqualTo(userNickName).GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot snapshot = task.Result;
            Debug.Log(task.Result.ChildrenCount);
            if (task.Result.ChildrenCount == 0)
            {
                goTitle = true;
            }
            else
            {
                goTitle = false;
            }
            isRunning = false;
        });
        StartCoroutine(GoTitle());
    }

    public void SaveUserNickName(string userNickName, string userID)
    {

        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["/users/" + userID + "/" + "username"] = userNickName;
        playerData.userName = userNickName;
        databaseRef.UpdateChildrenAsync(childUpdates).ContinueWith(task =>
        {
            Debug.Log("Save");
        });

    }

    private void WarningPanelSetActive(bool setActive, string waring = null)
    {
        warningGameObject.SetActive(setActive);       
        waringText.text = waring;
    }

    IEnumerator GoTitle()
    {
        yield return new WaitUntil(() => isRunning == false);
        if(goTitle)
        {
            PhotonNetwork.JoinLobby();
            SaveUserNickName(userNickName, FireBaseAuthManager.Instance.UserId);
            
        }
        else
        {
            warning = "Duplicate ID";
            WarningPanelSetActive(true, warning);
        }

    }

    public void DisableWarningPaenl()
    {
        WarningPanelSetActive(false);
    }





}
