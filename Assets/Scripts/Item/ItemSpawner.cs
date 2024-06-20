using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject SpawnRange;
    private BoxCollider2D rangeCollider;

    public ObjectPool ObjectPool;
    public string[] Rcodes;

    private void Awake()
    {
        rangeCollider = SpawnRange.GetComponent<BoxCollider2D>();
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

    private void SpawnItem(string rcode)
    {
        Vector2 spawnPosition = ReturnRandPos();
        GameObject spawnedObject = ObjectPool.GetObject(rcode); // Ç®¿¡¼­ °´Ã¼¸¦ ²¨³¿

        if (spawnedObject != null)
        {
            spawnedObject.transform.position = spawnPosition; // ·£´ý À§Ä¡¿¡ °´Ã¼ À§Ä¡ ¼³Á¤
        }
        else
        {
            Debug.LogWarning("Failed to get object from pool: " + rcode);
        }
    }

    public void SpawnRandomItem()
    {
        if (Rcodes.Length == 0)
        {
            Debug.LogError("ItemTags array is empty.");
            return;
        }
        string randomRcode = Rcodes[Random.Range(0, Rcodes.Length)];
        SpawnItem(randomRcode);
    }
}
