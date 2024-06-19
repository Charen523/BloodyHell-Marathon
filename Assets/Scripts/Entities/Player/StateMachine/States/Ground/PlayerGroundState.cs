using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _inputHandler.OnMoveCanceledEvent += OnMovementCanceled;
        _animeHandler.StartAnimation(_stateMachine.Player.AnimeData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        _inputHandler.OnMoveCanceledEvent -= OnMovementCanceled;
        _animeHandler.StopAnimation(_stateMachine.Player.AnimeData.GroundParameterHash);
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (_stateMachine.MovementDir == Vector2.zero) return;

        _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
