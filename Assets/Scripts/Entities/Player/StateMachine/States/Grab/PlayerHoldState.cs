using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHoldState : PlayerGrabState
{
    public PlayerHoldState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _stateMachine.MovementSpeedModifier = _stateMachine.Player.Stat.CurrentStat.Condition.HoldSpeedModifier;
        _inputHandler.OnGrabCanceledEvent += OnGrabCanceled;
        _grabHandler.GrabObject();
        _animeHandler.StartAnimation(_stateMachine.Player.AnimeData.IsHoldParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        _inputHandler.OnGrabCanceledEvent -= OnGrabCanceled;
        _grabHandler.ReleaseObject();
        _animeHandler.StopAnimation(_stateMachine.Player.AnimeData.IsHoldParameterHash);
    }

    private void OnGrabCanceled(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

}
