using System;
using UnityEngine;

[Serializable]
public class PlayerAnimeData
{
    [SerializeField] string _groundParameterName = "@Ground";
    [SerializeField] string _isIdleParameterName = "IsIdle";
    [SerializeField] string _isWalkParameterName = "IsWalk";

    public int GroundParameterHash { get; private set; }
    public int IsIdleParameterHash { get; private set; }
    public int IsWalkParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(_groundParameterName);
        IsIdleParameterHash = Animator.StringToHash(_isIdleParameterName);
        IsWalkParameterHash = Animator.StringToHash(_isWalkParameterName);
    }
}
