using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int minSize;
    public int maxSize;
}

public class ObjectPool : MonoBehaviour
{
    public List<Pool> pools;
    private Dictionary<string, ObjectPool<GameObject>> poolDic;

    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();

        foreach (var pool in pools)
        {
            ObjectPool<GameObject> objectPool = new ObjectPool<GameObject>(
                createFunc: () => {
                    var newObj = Instantiate(pool.prefab);
                    newObj.SetActive(false);
                    return newObj;
                },
                actionOnGet: (obj) => {
                    obj.SetActive(true);
                },
                actionOnRelease: (obj) => {
                    obj.SetActive(false);
                },
                actionOnDestroy: (obj) => {
                    Destroy(obj);
                },
                collectionCheck: false,
                defaultCapacity: pool.minSize,
                maxSize: pool.maxSize
            );

            poolDic.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetObject(string tag)
    {
        if (poolDic.ContainsKey(tag))
        {
            return poolDic[tag].Get();
        }
        Debug.LogWarning("No pool with tag: " + tag);
        return null;
    }

    public void ReleaseObject(string tag, GameObject obj)
    {
        if (poolDic.ContainsKey(tag))
        {
            poolDic[tag].Release(obj);
        }
        else
        {
            Debug.LogWarning("No pool with tag: " + tag);
        }
    }
}
