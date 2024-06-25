using UnityEngine;

public class PlayerStateMachine : EntityStateMachine
{
    public Player Player { get; private set; }
    public Vector2 MovementDir { get; set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }

    public PlayerHoldState HoldState { get; private set; }

    public PlayerGroggyState GroggyState { get; private set; }

    public PlayerPushState PushState { get; private set; }

    public PlayerStateMachine(Player player) : base(player)
    {
        Player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        HoldState = new PlayerHoldState(this);
        GroggyState = new PlayerGroggyState(this);
        PushState = new PlayerPushState(this);
    }
}