using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : ItemPickUp
{
    public float AddSpeed = 3.0f;
    public float Duration = 3.0f;
    public override void PickUp(Collider2D collision)
    {
        StartCoroutine(SpeedUp(collision));
    }

    private IEnumerator SpeedUp(Collider2D collision)
    {
        StatHandler stats = collision.GetComponent<StatHandler>();
        if (stats != null)
        {
            stats.CurrentStat.Condition.MoveSpeed += AddSpeed;
            yield return new WaitForSeconds(Duration);
            stats.CurrentStat.Condition.MoveSpeed -= AddSpeed;
        }
    }
}
