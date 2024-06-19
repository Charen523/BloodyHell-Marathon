public class PlayerMovementHandler : EntityMovementHandler
{
    public PlayerMovementHandler(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        _stateMachine = stateMachine;
    }
}
