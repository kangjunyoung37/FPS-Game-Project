using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerWeaponData
{
    public int muzzleIndex = -1;
    public int laserIndex = -1;
    public int gripIndex = -1;
    public int scopeIndex = -1;
}

[Serializable]
public class PlayerStorage
{
    public PlayerWeaponData[] playerWeaponData = new PlayerWeaponData[16];
}

[CreateAssetMenu(fileName = "New PlayerStorageData", menuName = "FPS Game/Shooter Pack/PlayerStorageData")]
[JsonObject(MemberSerialization.OptIn)]
public class PlayerStorageData : ScriptableObject
{

    [JsonProperty]
    public PlayerStorage playerStorage = new PlayerStorage();


    public string ToJson()
    {
        return JsonConvert.SerializeObject(playerStorage, Formatting.Indented);
    }

    public void FromJson(string jsonString)
    {
        PlayerStorage newPlayerStorage = JsonConvert.DeserializeObject<PlayerStorage>(jsonString);
        playerStorage = newPlayerStorage;
    }


    public void Init()
    {
        foreach (PlayerWeaponData playerWeaponData in playerStorage.playerWeaponData)
        {
            playerWeaponData.muzzleIndex = -1;
            playerWeaponData.laserIndex = -1;
            playerWeaponData.gripIndex = -1;
            playerWeaponData.scopeIndex = -1;

        }
    }

}
