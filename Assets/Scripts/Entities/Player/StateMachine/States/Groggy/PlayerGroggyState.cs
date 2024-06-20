public class PlayerGroggyState : PlayerState
{
    public PlayerGroggyState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _stateMachine.MovementSpeedModifier = 0;
        _animeHandler.StartAnimation(_stateMachine.Player.AnimeData.IsGroggyParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        _animeHandler.StopAnimation(_stateMachine.Player.AnimeData.IsGroggyParameterHash);
    }
}
