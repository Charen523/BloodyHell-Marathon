using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _inputHandler.OnRunCanceledEvent += OnRunCanceled;
        _stateMachine.MovementSpeedModifier = _stateMachine.Player.Stat.CurrentStat.Condition.RunSpeedModifier;
        _animeHandler.StartAnimation(_stateMachine.Player.AnimeData.IsRunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        _inputHandler.OnRunCanceledEvent -= OnRunCanceled;
        _animeHandler.StopAnimation(_stateMachine.Player.AnimeData.IsRunParameterHash);
    }

    private void OnRunCanceled(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
