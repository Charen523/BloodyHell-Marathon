using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject SpawnRange;
    private BoxCollider2D rangeCollider;

    public float SpawnTime = 1f;
    public int MaxItemsOnMap = 10;
    private int CurItemsOnMap = 0;

    private void Awake()
    {
        rangeCollider = SpawnRange.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        StartCoroutine(SpawnItem());
    }

    private Vector2 ReturnRandPos()
    {
        Vector2 min = rangeCollider.bounds.min;
        Vector2 max = rangeCollider.bounds.max;

        float randomX = Random.Range(min.x, max.x);
        float randomY = Random.Range(min.y, max.y);
        Vector2 randomPos = new Vector2(randomX, randomY);

        return randomPos;
    }

    private string SpawnRandomItem()
    {
        var pools = ObjectPoolManager.Instance.Pools;
        if (pools.Count == 0)
        {
            Debug.LogError("Item pools is empty.");
            return null;
        }
        string randomRcode = pools[Random.Range(0, pools.Count)].Rcode;
        return randomRcode;
    }

    private IEnumerator SpawnItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnTime);

            if(CurItemsOnMap < MaxItemsOnMap)
            {
                Vector2 spawnPos = ReturnRandPos();
                string spawnRcode = SpawnRandomItem();
                
                GameObject spawnedObject = ObjectPoolManager.Instance.GetObject(spawnRcode);
                if (spawnedObject != null)
                {
                    spawnedObject.transform.position = spawnPos;
                    CurItemsOnMap++;
                    spawnedObject.GetComponent<ItemPickUp>().OnPickUp += () => CurItemsOnMap--;
                }
                else
                {
                    Debug.LogWarning("Failed to get object from pool: " + spawnRcode);
                }
            }
        }
    }
}
