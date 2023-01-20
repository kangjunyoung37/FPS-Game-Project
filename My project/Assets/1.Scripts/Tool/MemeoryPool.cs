using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemeoryPool
{
    private class PoolItem
    {
        public bool isActive;
        public GameObject gameObject;
    }
    private int increaseCount = 5;
    private int maxCount;
    private int activeCount;

    private GameObject poolObject;

    private List<PoolItem> poolItemList = new List<PoolItem>();

    public MemeoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();


        InstantiateObjects(increaseCount);
    }

    /// <summary>
    /// 오브젝트 풀링
    /// </summary>
    private void InstantiateObjects(int createCount)
    {
        maxCount += createCount;

        for (int i = 0; i < createCount; ++i)
        {
            PoolItem poolItem = new PoolItem();
            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);

        }
    }

    /// <summary>
    /// 오브젝트 풀링중인 모든 오브젝트 삭제 후 초기화
    /// </summary>
    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    /// <summary>
    /// 활성화
    /// </summary>
    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;

        if (maxCount == activeCount)
            InstantiateObjects(increaseCount);

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];
            if (poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                //poolItem.gameObject.SetActive(true);
                return poolItem.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// 원하는 오브젝트 풀링 비활성화
    /// </summary>
    /// <param name="removeObject"></param>
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;
        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];
            if (poolItem.gameObject == removeObject)
            {
                activeCount--;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
                poolItem.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                return;
            }
        }
    }

    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }
        activeCount = 0;

    }
}
