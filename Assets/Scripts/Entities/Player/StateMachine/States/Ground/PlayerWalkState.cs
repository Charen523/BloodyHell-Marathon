using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _animeHandler.StartAnimation(_stateMachine.Player.AnimeData.IsWalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        _animeHandler.StopAnimation(_stateMachine.Player.AnimeData.IsWalkParameterHash);
    }

    public override void Update()
    {
        if (_stateMachine.MovementDir == Vector2.zero)
        {

        }
    }
}
