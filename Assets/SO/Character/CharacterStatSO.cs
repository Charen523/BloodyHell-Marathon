using System;
using UnityEngine;

public enum StatType
{
    Add,
    Multiple,
    Override
}

[Serializable]
public class StatData
{
    [Header("Condition")]
    public int MaxHP;
    public int HPRegen;
    public int MaxMP;
    public int MPRegen;
    [Header("Movement")]
    public float MoveSpeed;
    [Header("Attack")]
    public float Attack;
    public float AttackSpeed;
    [Header("Defense")]
    public float Defense;
    public float MagicDefense;
    [Header("Critical")]
    public float CriticalRate;
    public float CriticalDamage;
    [Header("Other")]
    public float EvasionRate;
    public float HitRate;
}

[CreateAssetMenu(fileName = "CharacterStat", menuName = "Character/new Stat")]
public class CharacterStatSO : ScriptableObject
{
    public StatType Type;
    public StatData Condition;

    public CharacterStatSO DeepCopy()
    {
        return Instantiate(this);
    }

    public void Add(CharacterStatSO other)
    {
        Condition.MaxHP += other.Condition.MaxHP;
        Condition.HPRegen += other.Condition.HPRegen;
        Condition.MaxMP += other.Condition.MaxMP;
        Condition.MPRegen += other.Condition.MPRegen;
        Condition.MoveSpeed += other.Condition.MoveSpeed;
        Condition.Attack += other.Condition.Attack;
        Condition.AttackSpeed += other.Condition.AttackSpeed;
        Condition.Defense += other.Condition.Defense;
        Condition.MagicDefense += other.Condition.MagicDefense;
        Condition.CriticalRate += other.Condition.CriticalRate;
        Condition.CriticalDamage += other.Condition.CriticalDamage;
        Condition.EvasionRate += other.Condition.EvasionRate;
        Condition.HitRate += other.Condition.HitRate;
    }

    public void Multiply(CharacterStatSO other)
    {
        Condition.MaxHP *= other.Condition.MaxHP;
        Condition.HPRegen *= other.Condition.HPRegen;
        Condition.MaxMP *= other.Condition.MaxMP;
        Condition.MPRegen *= other.Condition.MPRegen;
        Condition.MoveSpeed *= other.Condition.MoveSpeed;
        Condition.Attack *= other.Condition.Attack;
        Condition.AttackSpeed *= other.Condition.AttackSpeed;
        Condition.Defense *= other.Condition.Defense;
        Condition.MagicDefense *= other.Condition.MagicDefense;
        Condition.CriticalRate *= other.Condition.CriticalRate;
        Condition.CriticalDamage *= other.Condition.CriticalDamage;
        Condition.EvasionRate *= other.Condition.EvasionRate;
        Condition.HitRate *= other.Condition.HitRate;
    }
}
