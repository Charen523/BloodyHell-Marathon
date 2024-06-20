using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject SpawnRange;
    private BoxCollider2D rangeCollider;

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
}
