using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class Pool
{
    public GameObject Prefab;
    public int MinSize;
    public int MaxSize;
}

[RequireComponent(typeof(PhotonView))]
public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public List<Pool> Pools;
    private Dictionary<string, ObjectPool<GameObject>> poolDic;

    protected override void Awake()
    {
        base.Awake();
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
        foreach (var pool in Pools)
        {
            ObjectPool<GameObject> objectPool = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    GameObject newObj = PhotonNetwork.Instantiate("Items/"+pool.Prefab.name, Vector3.zero, Quaternion.identity);
                    newObj.SetActive(false);
                    return newObj;
                },
                actionOnGet: (obj) => obj.SetActive(true),
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => PhotonNetwork.Destroy(obj),
                collectionCheck: false,
                defaultCapacity: pool.MinSize,
                maxSize: pool.MaxSize
            );
            poolDic.Add(pool.Prefab.name, objectPool);
        }
    }

    public GameObject GetObject(string rcode)
    {
        if (poolDic.ContainsKey(rcode))
        {
            return poolDic[rcode].Get();
        }
        Debug.LogWarning("No pool with prefab name: " + rcode);
        return null;
    }

    public void ReleaseObject(string rcode, GameObject obj)
    {
        if (poolDic.ContainsKey(rcode) && obj != null)
        {
            poolDic[rcode].Release(obj);
        }
        else
        {
            Debug.LogWarning("No pool with prefab name: " + rcode);
        }
    }
}