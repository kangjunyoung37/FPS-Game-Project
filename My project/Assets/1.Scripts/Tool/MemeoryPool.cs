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
    /// ������Ʈ Ǯ��
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
    /// ������Ʈ Ǯ������ ��� ������Ʈ ���� �� �ʱ�ȭ
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
    /// Ȱ��ȭ
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
    /// ���ϴ� ������Ʈ Ǯ�� ��Ȱ��ȭ
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
