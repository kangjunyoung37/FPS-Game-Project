using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Title(label: "Text")]
    public TMP_Text playerNickName;
    public TMP_Text playerKillDeath;
    [Title(label: "Player Data")]
    public PlayerData playerData;


    private void UpdatePlayerData()
    {
        playerNickName.text = playerData.userName;
        float killDeath = (float)playerData.Kill / (playerData.Death + playerData.Kill);
        if(playerData.Death == 0)
            playerKillDeath.text = $"Kill/Death : {playerData.Kill} / {playerData.Death} (0%)";
        else
            playerKillDeath.text = $"Kill/Death : {playerData.Kill} / {playerData.Death} ({(int)(killDeath * 100)}%)";
    }
    private void OnEnable()
    {
        UpdatePlayerData();
    }

}
