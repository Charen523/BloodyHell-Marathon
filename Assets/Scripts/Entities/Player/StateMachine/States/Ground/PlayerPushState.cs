using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPushState : PlayerGroundState
{
    public PlayerPushState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        _animeHandler.SetTriggerAnimation(_stateMachine.Player.AnimeData.PushParameterHash);
        _pushHandler.Push();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        _stateMachine.MovementDir = Vector2.zero;
        _stateMachine.Player.Rigid.velocity = Vector2.zero;
        _stateMachine.MovementSpeedModifier = 0f;

        float normalizedTime = _animeHandler.GetNormalizedTime(_stateMachine.Player.Animator, "Push");
        if (normalizedTime >= 0.83f)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
    protected override void OnPushStarted(InputAction.CallbackContext context) { }
}
