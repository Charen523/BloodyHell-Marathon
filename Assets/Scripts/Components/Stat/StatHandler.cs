using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class StatHandler : MonoBehaviour
{
    [HideInInspector] public CharacterStatSO CurrentStat { get; private set; }
    public event Action OnStatChangedEvent;
    [SerializeField] CharacterStatSO _baseStat;
    List<CharacterStatSO> _statModifiers = new List<CharacterStatSO>();

    void Awake()
    {
        UpdateStat();
    }

    public void UpdateStat()
    {
        CurrentStat = _baseStat.DeepCopy();

        _statModifiers.OrderBy(stat => stat.Type);

        foreach (CharacterStatSO stat in _statModifiers)
        {
            switch (stat.Type)
            {
                case StatType.Add:
                    CurrentStat.Add(stat);
                    break;
                case StatType.Multiple:
                    CurrentStat.Add(stat);
                    break;
                case StatType.Override:
                    CurrentStat = stat.DeepCopy();
                    break;

            }
        }
        OnStatChangedEvent?.Invoke();
    }

    public void AddStat(CharacterStatSO stat)
    {
        _statModifiers.Add(stat);
    }
}
