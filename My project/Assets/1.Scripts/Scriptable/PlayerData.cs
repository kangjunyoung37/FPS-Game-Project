using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

[Serializable]
public class PlayerInformationData
{
    public int kill;
    public int death;
    public int mainWeapon = 0;
    public int subWeapon = 12;
}

[CreateAssetMenu(fileName = "New_Player_Data", menuName = "FPS Game/Shooter Pack/Player Data")]
[JsonObject(MemberSerialization.OptIn)]
public class PlayerData : ScriptableObject
{

    [JsonProperty]
    public PlayerInformationData playerImformationData = new PlayerInformationData();

    public int Kill
    {
        get  => playerImformationData.kill;
        set
        {
            playerImformationData.kill = value;
        }
    }

    public int Death
    {
        get => playerImformationData.death;
        set => playerImformationData.death = value;
    }

    public int MainWeapon
    {
        get => playerImformationData.mainWeapon;
        set => playerImformationData.mainWeapon = value;
    }

    public int SubWeapon
    {
        get => playerImformationData.subWeapon;
        set => playerImformationData.subWeapon = value;                   
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(playerImformationData, Formatting.Indented);
    }

    public void FromJson(string jsonString)
    {
        PlayerInformationData newPlayerImforamtionData = JsonConvert.DeserializeObject<PlayerInformationData>(jsonString);
        playerImformationData.kill = newPlayerImforamtionData.kill;
        playerImformationData.death = newPlayerImforamtionData.death;
        playerImformationData.mainWeapon = newPlayerImforamtionData.mainWeapon;
        playerImformationData.subWeapon = newPlayerImforamtionData.subWeapon;
    }

    public void Init()
    {
        playerImformationData = new PlayerInformationData();
       
    }

}
