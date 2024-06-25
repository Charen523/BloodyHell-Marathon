using UnityEngine;

public class PlayerPushHandler
{
    private PlayerStateMachine _stateMachine;

    private float _outerRadius = 3f;
    private float _innerRadius = 1f;

    private LayerMask _playerLayerMask;

    public PlayerPushHandler(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _playerLayerMask = LayerMask.GetMask("Player");
    }

    public void Push()
    {
        Vector3 playerPosition = _stateMachine.Player.transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(playerPosition, _outerRadius, _playerLayerMask);

        foreach (Collider2D hit in hits)
        {
            float distance = Vector2.Distance(playerPosition, hit.transform.position);
            if (distance >= _innerRadius && hit != null)
            {
                ForceReceiver temp = hit.GetComponent<ForceReceiver>();
                if (temp != null)
                {
                    Vector2 collisionDirection = (hit.transform.position - playerPosition).normalized;
                    Vector2 appliedForce = collisionDirection * 10f;
                    temp.AddForce(appliedForce);
                }
            }
        }
    }
}
