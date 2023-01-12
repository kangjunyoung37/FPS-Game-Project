using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class KillLog : MonoBehaviour
{
    [SerializeField]
    private TMP_Text killID;
    [SerializeField]
    private TMP_Text deadID;
    [SerializeField]
    private Image weaponImage;

    private float destroyTimer = 3.0f;

    public void Setup(string killID,string deadID)
    {
        this.killID.text = killID;
        this.deadID.text = deadID;
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
        Color color = killID.color;
        color.a = alpha;
        killID.color = color;
        deadID.color = color;
        weaponImage.color = color;
    }


}
