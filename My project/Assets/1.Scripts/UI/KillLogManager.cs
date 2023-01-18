using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class KillLogManager : MonoBehaviour
{



    [SerializeField]
    GameObject killLogGameObject;

    int index = 0;

    private void Awake()
    {

    }


    
    public void ChangeParent()
    {

        int cnt = transform.childCount;
        if(cnt == 4)
        {
            KillLog killLog = transform.GetChild(0).GetComponent<KillLog>();
            transform.GetChild(0).transform.SetAsLastSibling();
            //killLog.Setup(index.ToString(), "asdsad0");
            index += 1;
            return;
        }
        GameObject killlog =  Instantiate(killLogGameObject,this.transform);
        //killlog.GetComponent<KillLog>().Setup(index.ToString(), "Player20");

        index += 1;
    }

    
}
