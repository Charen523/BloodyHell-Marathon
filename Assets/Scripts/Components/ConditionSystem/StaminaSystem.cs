using UnityEngine;
using System.Collections;

public class StaminaSystem : BaseCondition
{
  public override ConditionType Type => ConditionType.Stamina;
  public override int Max => _stat.CurrentStat.Condition.MaxStamina;

  private float _regenRate = 0.5f;
  private Coroutine _regenCoroutine;

  protected override void Start()
  {
    base.Start();
    if (_stat.CurrentStat.Condition.StaminaRegen != 0)
    {
      _regenCoroutine = StartCoroutine(Regen());
    }
  }

  private IEnumerator Regen()
  {
    while (true)
    {
      Modify(_stat.CurrentStat.Condition.StaminaRegen);
      yield return new WaitForSeconds(_regenRate);
    }
  }

  private void OnDisable()
  {
    if (_regenCoroutine != null)
    {
      StopCoroutine(_regenCoroutine);
    }
  }
}
