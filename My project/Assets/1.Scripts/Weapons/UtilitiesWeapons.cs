using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilitiesWeapons 
{
    /// <summary>
    /// 한 오브젝트를 활성화하고 다른 오브젝트를 다 비활성화합니다.
    /// </summary>
    public static T SelectAndSetActive<T>(this T[] array , int index) where T : MonoBehaviour
    {
        //배열에 객체가 있는지 확인하세여!
        if(!array.IsValid())
        {
            return null;
        }
        //모든 오브젝트 비활성화
        array.ForrEach(obj => obj.gameObject.SetActive(false));

        if (!array.IsValidIndex(index))
            return null;
        
        //원하는 오브젝트 활성화
        T behaviour = array[index];

        if(behaviour != null)
            behaviour.gameObject.SetActive(true);

        return behaviour;

    }
}
