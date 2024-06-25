using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerState : IState
{
    public PlayerMovementHandler MovementHandler;
    protected PlayerStateMachine _stateMachine;
    protected PlayerAnimeHandler _animeHandler;
    protected PlayerInputHandler _inputHandler;
    protected PlayerGrabHandler _grabHandler;
    protected PlayerPushHandler _pushHandler;

    protected PlayerState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;

        MovementHandler = new PlayerMovementHandler(_stateMachine);
        _animeHandler = new PlayerAnimeHandler(_stateMachine);
        _inputHandler = new PlayerInputHandler(_stateMachine);
        _grabHandler = new PlayerGrabHandler(_stateMachine);
        _pushHandler = new PlayerPushHandler(_stateMachine);
    }

    public virtual void Enter()
    {
        if (_stateMachine.Player.Input == null) return;
        _inputHandler.OnGrabPerformedEvent += OnGrabPerformed;
        _inputHandler.AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        if (_stateMachine.Player.Input == null) return;
        _inputHandler.OnGrabPerformedEvent -= OnGrabPerformed;
        _inputHandler.RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        MovementHandler.Movement(_stateMachine.MovementDir);
    }

    public virtual void Update() { }
    private void ReadMovementInput()
    {
        if (_stateMachine.Player.Input == null) return;
        if (!_stateMachine.Player.photonView.IsMine) return;
        _stateMachine.MovementDir = _stateMachine.Player.Input.PlayerActions.Move.ReadValue<Vector2>();
    }

    private void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if (_grabHandler.DetectAndGrab())
        {
            _stateMachine.ChangeState(_stateMachine.HoldState);
        }
    }

}
