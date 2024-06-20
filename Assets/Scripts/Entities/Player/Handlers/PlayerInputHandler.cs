using System;
using UnityEngine.InputSystem;

public class PlayerInputHandler
{
    private PlayerStateMachine _stateMachine;
    public event Action<InputAction.CallbackContext> OnMoveCanceledEvent;
    public event Action<InputAction.CallbackContext> OnRunPerformedEvent;
    public event Action<InputAction.CallbackContext> OnRunCanceledEvent;

    public event Action<InputAction.CallbackContext> OnGrapPerformedEvent;
    public event Action<InputAction.CallbackContext> OnGrapCanceledEvent;

    public PlayerInputHandler(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void AddInputActionsCallbacks()
    {
        PlayerInput input = _stateMachine.Player.Input;
        input.PlayerActions.Move.canceled += OnMoveCanceled;

        input.PlayerActions.Run.performed += OnRunPerformed;
        input.PlayerActions.Run.canceled += OnRunCanceled;

        input.PlayerActions.Grap.performed += OnGrapPerformed;
        input.PlayerActions.Grap.canceled += OnGrapCanceled;
    }

    public void RemoveInputActionsCallbacks()
    {
        PlayerInput input = _stateMachine.Player.Input;
        input.PlayerActions.Move.canceled -= OnMoveCanceled;

        input.PlayerActions.Run.started -= OnRunPerformed;
        input.PlayerActions.Run.canceled -= OnRunCanceled;

        input.PlayerActions.Grap.performed -= OnGrapPerformed;
        input.PlayerActions.Grap.canceled -= OnGrapCanceled;
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

    private void OnGrapPerformed(InputAction.CallbackContext context)
    {
        OnGrapPerformedEvent?.Invoke(context);
    }
    private void OnGrapCanceled(InputAction.CallbackContext context)
    {
        OnGrapCanceledEvent?.Invoke(context);
    }
}
