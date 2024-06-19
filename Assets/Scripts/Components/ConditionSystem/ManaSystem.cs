using System;

public class ManaSystem : BaseCondition
{
  public override ConditionType Type => ConditionType.Mana;
  public override int Max { get => _stat.CurrentStat.Condition.MaxMP; }

  public event Action OnUsed;
  public event Action OnFilled;

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

    if (amount <= 0f)
    {
      Use();
    }
    else
    {
      Fill();
    }

    return true;
  }
  private void Fill()
  {
    OnFilled?.Invoke();
  }

  private void Use()
  {
    OnUsed?.Invoke();
  }

  private void Regen()
  {
    Modify(_stat.CurrentStat.Condition.MPRegen);
  }
}