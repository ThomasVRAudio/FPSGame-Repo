using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;

    private List<Pool> PoolList;

    public class Pool
    {
        public GameObject Prefab;
        public List<GameObject> Objects;        
    }

    private void Awake()
    {
        SharedInstance = this;
        PoolList = new List<Pool>();
    }

    public void AddObjectPool(GameObject prefab, int amount)
    {
        GameObject obj;

        foreach (var pool in PoolList)
        {
            if (prefab == pool.Prefab)
            {
                for (int i = 0; i < amount; i++)
                {
                    obj = Instantiate(prefab, gameObject.transform);
                    obj.SetActive(false);
                    pool.Objects.Add(obj);
                }

                return;
            }
        }

        PoolList.Add(new Pool());
        PoolList[PoolList.Count - 1].Objects = new List<GameObject>();
        PoolList[PoolList.Count - 1].Prefab = prefab; 


        for (int i = 0; i < amount; i++)
        {
            obj = Instantiate(prefab, gameObject.transform);
            obj.SetActive(false);
            PoolList[PoolList.Count-1].Objects.Add(obj);
        }
    }

    public int? GetObjectPoolNumber(GameObject prefab)
    {
        foreach (var pool in PoolList)
        {
            if (prefab == pool.Prefab)
            {
                return PoolList.IndexOf(pool);
            }
        }

        return null;
    }

    public GameObject GetPooledObject(int? list)
    {
        if (list == null) return null;

        for (int i = 0; i < PoolList[(int)list].Objects.Count; i++)
        {
            if (!PoolList[(int)list].Objects[i].activeInHierarchy)
            {
                return PoolList[(int)list].Objects[i];
            }  
        }

        return null;
    }
}
