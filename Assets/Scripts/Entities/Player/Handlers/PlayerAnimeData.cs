using System;
using UnityEngine;

[Serializable]
public class PlayerAnimeData
{
    [SerializeField] string _groundParameterName = "@Ground";
    [SerializeField] string _isIdleParameterName = "IsIdle";
    [SerializeField] string _isWalkParameterName = "IsWalk";
    [SerializeField] string _isRunParameterName = "IsRun";

    [SerializeField] string _grabParameterName = "@Grab";
    [SerializeField] string _isHoldParameterName = "IsHold";

    [SerializeField] string _isGroggyParameterName = "IsGroggy";

    [SerializeField] string _pushParameterName = "Push";

    public int GroundParameterHash { get; private set; }
    public int IsIdleParameterHash { get; private set; }
    public int IsWalkParameterHash { get; private set; }
    public int IsRunParameterHash { get; private set; }

    public int GrabParameterHash { get; private set; }
    public int IsHoldParameterHash { get; private set; }

    public int IsGroggyParameterHash { get; private set; }
    public int PushParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(_groundParameterName);
        IsIdleParameterHash = Animator.StringToHash(_isIdleParameterName);
        IsWalkParameterHash = Animator.StringToHash(_isWalkParameterName);
        IsRunParameterHash = Animator.StringToHash(_isRunParameterName);

        GrabParameterHash = Animator.StringToHash(_grabParameterName);
        IsHoldParameterHash = Animator.StringToHash(_isHoldParameterName);

        IsGroggyParameterHash = Animator.StringToHash(_isGroggyParameterName);

        PushParameterHash = Animator.StringToHash(_pushParameterName);
    }
}
