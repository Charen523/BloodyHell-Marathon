using UnityEngine;

public class EntityMovementHandler
{
    protected EntityStateMachine _stateMachine;
    private bool _isFacingRight = true;

    public EntityMovementHandler(EntityStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Movement(Vector2 dir)
    {
        Move(dir);
        UpdateSpriteDirection(dir);
    }

    protected virtual void Move(Vector2 dir)
    {
        float movementSpeed = GetMovementSpeed();
        _stateMachine.Entity.Rigid.velocity = dir * movementSpeed;
    }
    private float GetMovementSpeed()
    {
        float moveSpeed = _stateMachine.Entity.Stat.CurrentStat.Condition.MoveSpeed * _stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    protected virtual void UpdateSpriteDirection(Vector2 direction)
    {
        if (direction.x > 0.2 && !_isFacingRight)
        {
            FlipSprite(true);
        }
        else if (direction.x < -0.2 && _isFacingRight)
        {
            FlipSprite(false);
        }
    }

    protected virtual void FlipSprite(bool faceRight)
    {
        _isFacingRight = faceRight;
        Vector3 scale = _stateMachine.Entity.MainSprite.localScale;
        scale.x = _isFacingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        _stateMachine.Entity.MainSprite.localScale = scale;

        if (_stateMachine.Entity.HandPoint != null)
        {
            Vector3 handPointPosition = _stateMachine.Entity.HandPoint.localPosition;
            handPointPosition.x = _isFacingRight ? Mathf.Abs(handPointPosition.x) : -Mathf.Abs(handPointPosition.x);
            _stateMachine.Entity.HandPoint.localPosition = handPointPosition;
        }
    }
}
