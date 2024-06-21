using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : BaseCondition
{
  public override ConditionType Type => ConditionType.Health;
  public override int Max { get => _stat.CurrentStat.Condition.MaxHP; }

  public event Action<int> OnDamageEvent;
  public event Action<int> OnHealEvent;
  public UnityEvent OnDeathEvent;

  private float _regenRate = 0.5f;
  private Coroutine _regenCoroutine;

  protected override void Start()
  {
    base.Start();
    if (_stat.CurrentStat.Condition.HPRegen != 0)
    {
      _regenCoroutine = StartCoroutine(Regen());
    }
  }

  public override bool Modify(int amount)
  {
    if (!base.Modify(amount)) return false;

    if (Current == 0)
    {
      OnDeathEvent?.Invoke();
    }

    if (amount < 0)
    {
      OnDamageEvent?.Invoke(amount);
    }
    else
    {
      OnHealEvent?.Invoke(amount);
    }

    return true;
  }

  private IEnumerator Regen()
  {
    while (true)
    {
      Modify(_stat.CurrentStat.Condition.HPRegen);
      yield return new WaitForSeconds(_regenRate);
    }
  }
}