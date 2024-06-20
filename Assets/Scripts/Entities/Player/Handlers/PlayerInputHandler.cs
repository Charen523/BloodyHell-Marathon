using System;
using UnityEngine.InputSystem;

public class PlayerInputHandler
{
    private PlayerStateMachine _stateMachine;
    public event Action<InputAction.CallbackContext> OnMoveCanceledEvent;
    public event Action<InputAction.CallbackContext> OnRunPerformedEvent;
    public event Action<InputAction.CallbackContext> OnRunCanceledEvent;

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
    }

    public void RemoveInputActionsCallbacks()
    {
        PlayerInput input = _stateMachine.Player.Input;
        input.PlayerActions.Move.canceled -= OnMoveCanceled;

        input.PlayerActions.Run.started -= OnRunPerformed;
        input.PlayerActions.Run.canceled -= OnRunCanceled;
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
}
