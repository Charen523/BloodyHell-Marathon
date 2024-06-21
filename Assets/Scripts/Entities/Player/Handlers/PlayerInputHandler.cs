using System;
using UnityEngine.InputSystem;

public class PlayerInputHandler
{
    private PlayerStateMachine _stateMachine;
    public event Action<InputAction.CallbackContext> OnMoveCanceledEvent;
    public event Action<InputAction.CallbackContext> OnRunPerformedEvent;
    public event Action<InputAction.CallbackContext> OnRunCanceledEvent;

    public event Action<InputAction.CallbackContext> OnGrabPerformedEvent;
    public event Action<InputAction.CallbackContext> OnGrabCanceledEvent;

    public PlayerInputHandler(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void AddInputActionsCallbacks()
    {
        if (!_stateMachine.Player.PhotonView.IsMine) return;
        PlayerInput input = _stateMachine.Player.Input;
        input.PlayerActions.Move.canceled += OnMoveCanceled;

        input.PlayerActions.Run.performed += OnRunPerformed;
        input.PlayerActions.Run.canceled += OnRunCanceled;

        input.PlayerActions.Grab.performed += OnGrabPerformed;
        input.PlayerActions.Grab.canceled += OnGrabCanceled;
    }

    public void RemoveInputActionsCallbacks()
    {
        if (!_stateMachine.Player.PhotonView.IsMine) return;
        PlayerInput input = _stateMachine.Player.Input;
        input.PlayerActions.Move.canceled -= OnMoveCanceled;

        input.PlayerActions.Run.started -= OnRunPerformed;
        input.PlayerActions.Run.canceled -= OnRunCanceled;

        input.PlayerActions.Grab.performed -= OnGrabPerformed;
        input.PlayerActions.Grab.canceled -= OnGrabCanceled;
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        OnMoveCanceledEvent?.Invoke(context);
    }
    private void OnRunPerformed(InputAction.CallbackContext context)
    {
        OnRunPerformedEvent?.Invoke(context);
    }
    private void OnRunCanceled(InputAction.CallbackContext context)
    {
        OnRunCanceledEvent?.Invoke(context);
    }

    private void OnGrabPerformed(InputAction.CallbackContext context)
    {
        OnGrabPerformedEvent?.Invoke(context);
    }
    private void OnGrabCanceled(InputAction.CallbackContext context)
    {
        OnGrabCanceledEvent?.Invoke(context);
    }
}
