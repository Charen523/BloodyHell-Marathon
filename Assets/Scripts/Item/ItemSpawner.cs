using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemSpawner : MonoBehaviour
{
    public Tilemap SpawnRange;

    public float SpawnTime = 1f;
    public int MaxItemsOnMap = 10;
    private int CurItemsOnMap = 0;

    private void Start()
    {
        if (SpawnRange == null) Debug.LogWarning("Need an area (Tilemap) to spawn items.");
        else StartCoroutine(SpawnItem());
    }

    private Vector2 ReturnRandPos()
    {
        BoundsInt bounds = SpawnRange.cellBounds;
        Vector3Int min = bounds.min;
        Vector3Int max = bounds.max;

        int randomX = Random.Range(min.x, max.x);
        int randomY = Random.Range(min.y, max.y);
        Vector3 worldPos = SpawnRange.CellToWorld(new Vector3Int(randomX, randomY, 0));
        Vector2 randomPos = new Vector2(worldPos.x, worldPos.y);
        
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
        string randomRcode = pools[Random.Range(0, pools.Count)].Prefab.name;
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
