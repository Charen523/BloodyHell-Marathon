using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : ItemPickUp
{
    public float ReduceSpeed = 0.1f; 
    public float Duration = 5.0f; // 슬로우 효과 지속 시간

    public override void PickUp(Collider2D collision)
    {
        StatHandler stat = collision.gameObject.GetComponent<StatHandler>();
        CoroutineRunner.Instance.RunCoroutine(ApplySlowEffect(stat));
    }

    private IEnumerator ApplySlowEffect(StatHandler stat)
    {
        var statData = new CharacterStatSO { Type = StatType.Multiple, Condition = new StatData {MoveSpeed = ReduceSpeed } };
        stat.AddStat(statData);
        stat.UpdateStat();

        yield return new WaitForSeconds(Duration);

        stat.RemoveStat(statData);
        stat.UpdateStat();
    }
}
