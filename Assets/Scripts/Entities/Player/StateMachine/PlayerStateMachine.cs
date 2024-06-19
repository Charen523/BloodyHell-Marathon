using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; private set; }
    public Vector2 MovementDir { get; set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }

    public PlayerStateMachine(Player player)
    {
        Entity = player;
        Player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
    }
}