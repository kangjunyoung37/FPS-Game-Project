using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilitiesWeapons 
{
    /// <summary>
    /// �� ������Ʈ�� Ȱ��ȭ�ϰ� �ٸ� ������Ʈ�� �� ��Ȱ��ȭ�մϴ�.
    /// </summary>
    public static T SelectAndSetActive<T>(this T[] array , int index) where T : MonoBehaviour
    {
        //�迭�� ��ü�� �ִ��� Ȯ���ϼ���!
        if(!array.IsValid())
        {
            return null;
        }
        //��� ������Ʈ ��Ȱ��ȭ
        array.ForrEach(obj => obj.gameObject.SetActive(false));

        if (!array.IsValidIndex(index))
            return null;
        
        //���ϴ� ������Ʈ Ȱ��ȭ
        T behaviour = array[index];

        if(behaviour != null)
            behaviour.gameObject.SetActive(true);

        return behaviour;

    }
}
