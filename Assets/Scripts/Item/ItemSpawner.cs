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

    private List<Vector3Int> validTilePositions = new List<Vector3Int>();

    private void Start()
    {
        if (SpawnRange == null) Debug.LogWarning("Need an area (Tilemap) to spawn items.");
        else
        {
            FindValidTilePositions();
            StartCoroutine(SpawnItem());
        }
    }

    private void FindValidTilePositions()
    {
        BoundsInt bounds = SpawnRange.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (SpawnRange.HasTile(tilePosition))
                {
                    validTilePositions.Add(tilePosition);
                }
            }
        }
    }

    private Vector3 ReturnRandPos()
    {
        Vector3 randomPos = Vector3.zero;

        if (validTilePositions.Count > 0)
        {
            Vector3Int randomTilePos = validTilePositions[Random.Range(0, validTilePositions.Count)];
            randomPos = SpawnRange.CellToWorld(randomTilePos);
        }

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
                Vector3 spawnPos = ReturnRandPos();
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
