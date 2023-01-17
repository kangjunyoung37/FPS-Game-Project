using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHitEffect : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup hitEffectGroup;

    [SerializeField]
    private float interpolationSpeed = 8.0f;

    private bool isRunning = false;
    Coroutine coroutine = null;
    IEnumerator HitEffect()
    {
        isRunning = true;
        hitEffectGroup.alpha = 1.0f;
        yield return new WaitForSeconds(0.7f);
        while(hitEffectGroup.alpha > 0)
        {
            hitEffectGroup.alpha -= Time.deltaTime * interpolationSpeed;
            yield return null;  
        }
        isRunning = false;
    }

    public void CreateHitEffect()
    {
        if(isRunning)
        {
            isRunning = false;
            StopCoroutine(coroutine);
            coroutine = null;
        }

        coroutine =  StartCoroutine(HitEffect());

    }


}
