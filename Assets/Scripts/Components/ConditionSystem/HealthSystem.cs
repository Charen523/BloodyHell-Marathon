using System;
using UnityEngine.Events;

public class HealthSystem : BaseCondition
{
  public override ConditionType Type => ConditionType.Health;
  public override int Max { get => _stat.CurrentStat.Condition.MaxHP; }

  public event Action<int> OnDamageEvent;
  public event Action<int> OnHealEvent;
  public UnityEvent OnDeathEvent;

  private float _regenRate = 0.5f;

  protected override void Start()
  {
    base.Start();
    if (_stat.CurrentStat.Condition.HPRegen != 0)
    {
      InvokeRepeating(nameof(Regen), _regenRate, _regenRate);
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

  private void Regen()
  {
    Modify(_stat.CurrentStat.Condition.HPRegen);
  }
}