using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _inputHandler.OnRunPerformedEvent += OnRunPerformed;
        _stateMachine.MovementSpeedModifier = _stateMachine.Player.Stat.CurrentStat.Condition.WalkSpeedModifier;
        _animeHandler.StartAnimation(_stateMachine.Player.AnimeData.IsWalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        _inputHandler.OnRunPerformedEvent -= OnRunPerformed;
        _animeHandler.StopAnimation(_stateMachine.Player.AnimeData.IsWalkParameterHash);
    }

    private void OnRunPerformed(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.RunState);
    }
}
