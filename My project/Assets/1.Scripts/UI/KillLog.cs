using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class KillLog : MonoBehaviour
{

    [Title(label:"Settings")]
    [SerializeField]
    private TMP_Text killID;
    [SerializeField]
    private TMP_Text deadID;
    [SerializeField]
    private Image weaponImage;


    [Title(label:"Sprites")]
    [SerializeField]
    private Sprite grenadeImage;

    [SerializeField]
    private Sprite knifeImage;

    [SerializeField]
    private Sprite[] weaponImagegroup;

    private float destroyTimer = 3.0f;

    int killerTeam = 0;
    int deadTeam = 0;

    public void Setup(string killID,string deadID,int killerTeam,int deadTeam , int index)
    {
        this.killID.text = killID;
        this.deadID.text = deadID;
        this.killerTeam = killerTeam;
        this.deadTeam = deadTeam;
        TextColorChange(killerTeam, this.killID);
        TextColorChange(deadTeam, this.deadID);
        if (index == -1)
            weaponImage.sprite = grenadeImage;
        else if (index == -2)
            weaponImage.sprite = knifeImage;
        else
            weaponImage.sprite = weaponImagegroup[index];

        destroyTimer = 3.0f;

    }

    IEnumerator StartAndDestroy()
    {
        yield return new WaitForSeconds(2.0f);

        while(destroyTimer >0f)
        {
            destroyTimer -= Time.deltaTime;

            SetAlpha(destroyTimer/3.0f);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void Awake()
    {
        //SetActive(false);
        StartCoroutine(StartAndDestroy());
    }

    private void SetActive(bool active)
    {
        killID.enabled = false;

        deadID.enabled = false;

        weaponImage.enabled = false;

    }

    private void SetAlpha(float alpha)
    {
        Color killcolor = killID.color;
        killcolor.a = alpha;
        killID.color = killcolor;
        Color deadcolor = deadID.color;
        deadcolor.a = alpha;
        deadID.color = deadcolor;
        Color weaponColor = weaponImage.color;
        weaponColor.a = alpha;  
        weaponImage.color = weaponColor;
    }

    private void TextColorChange(int team,TMP_Text playerName)
    {
        if(team == 0)
        {
            playerName.color = Color.blue;
        }
        else
        {
            playerName.color = Color.red;
        }

    }

}
