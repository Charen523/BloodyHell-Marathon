using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    private Coroutine _staminaCoroutine;
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _inputHandler.OnRunCanceledEvent += OnRunCanceled;
        _stateMachine.MovementSpeedModifier = _stateMachine.Player.Stat.CurrentStat.Condition.RunSpeedModifier;
        _animeHandler.StartAnimation(_stateMachine.Player.AnimeData.IsRunParameterHash);

        _staminaCoroutine = _stateMachine.Player.StartCoroutine(ConsumeStamina());
    }

    public override void Exit()
    {
        base.Exit();
        _inputHandler.OnRunCanceledEvent -= OnRunCanceled;
        _animeHandler.StopAnimation(_stateMachine.Player.AnimeData.IsRunParameterHash);

        if (_staminaCoroutine != null)
        {
            _stateMachine.Player.StopCoroutine(_staminaCoroutine);
            _staminaCoroutine = null;
        }
    }

    private void OnRunCanceled(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    private IEnumerator ConsumeStamina()
    {
        while (true)
        {
            bool success = _stateMachine.Player.Stamina.Modify(-7);
            if (!success)
            {
                _stateMachine.ChangeState(_stateMachine.IdleState);
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
