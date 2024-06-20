using System;
using UnityEngine.InputSystem;

public class PlayerHoldState : PlayerGrapState
{
    public PlayerHoldState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _stateMachine.MovementSpeedModifier = _stateMachine.Player.Stat.CurrentStat.Condition.HoldSpeedModifier;
        _inputHandler.OnGrapCanceledEvent += OnGrapCanceled;
        _animeHandler.StartAnimation(_stateMachine.Player.AnimeData.IsHoldParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        _inputHandler.OnGrapCanceledEvent -= OnGrapCanceled;
        _animeHandler.StopAnimation(_stateMachine.Player.AnimeData.IsHoldParameterHash);
    }

    private void OnGrapCanceled(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

}
