using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : ItemPickUp
{
    public float AddSpeed = 3.0f;
    public float Duration = 3.0f;
    public override void PickUp(Collider other)
    {
        StartCoroutine(SpeedUp(other));
    }

    private IEnumerator SpeedUp(Collider other)
    {
        StatHandler stats = other.GetComponent<StatHandler>();
        if (stats != null)
        {
            stats.CurrentStat.Condition.MoveSpeed += AddSpeed;
            yield return new WaitForSeconds(Duration);
            stats.CurrentStat.Condition.MoveSpeed -= AddSpeed;
        }
    }
}
