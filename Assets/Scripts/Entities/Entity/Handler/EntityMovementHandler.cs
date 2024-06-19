using UnityEngine;

public class EntityMovementHandler
{
    protected StateMachine _stateMachine;
    private bool _isFacingRight = true;

    public EntityMovementHandler(StateMachine stateMachine)
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
        float movementSpeed = _stateMachine.Entity.Stat.CurrentStat.Condition.MoveSpeed;
        _stateMachine.Entity.Rigid.velocity = dir * movementSpeed;
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
    }
}
