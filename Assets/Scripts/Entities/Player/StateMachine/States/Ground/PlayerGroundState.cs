using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        if (_inputHandler != null)
        {
            _inputHandler.OnMoveCanceledEvent += OnMovementCanceled;
            _inputHandler.OnPushStartedEvent += OnPushStarted;
        }
        _animeHandler.StartAnimation(_stateMachine.Player.AnimeData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        if (_inputHandler != null)
        {
            _inputHandler.OnMoveCanceledEvent -= OnMovementCanceled;
            _inputHandler.OnPushStartedEvent -= OnPushStarted;
        }
        _animeHandler.StopAnimation(_stateMachine.Player.AnimeData.GroundParameterHash);
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (_stateMachine.MovementDir == Vector2.zero) return;

        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    protected virtual void OnPushStarted(InputAction.CallbackContext context)
    {
        if (_stateMachine.Player.Stamina.Modify(-20))
        {
            _stateMachine.ChangeState(_stateMachine.PushState);
        }
    }
}
