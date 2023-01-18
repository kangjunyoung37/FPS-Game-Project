using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField]
    public TMP_Text text;

    [SerializeField]
    public TMP_Text roomsetting;

    [SerializeField]
    public TMP_Text mapname;

    RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        text.text = info.Name;
        roomsetting.text = info.PlayerCount + "/" + info.MaxPlayers;
   
        mapname.text = "Map_"+ info.CustomProperties["map"].ToString();
    }


    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
