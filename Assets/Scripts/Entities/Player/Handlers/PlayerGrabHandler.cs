using UnityEngine;

public class PlayerGrabHandler
{
    private PlayerStateMachine _stateMachine;
    public PlayerGrabHandler(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public bool DetectAndGrab()
    {
        Vector2 yOffset = Vector2.up * -0.3f;
        Vector2 startPosition = (Vector2)_stateMachine.Player.transform.position + yOffset;
        Vector2 direction = ((Vector2)_stateMachine.Player.HandPoint.position - startPosition).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direction, 0.6f);

        Debug.DrawRay(startPosition, direction, Color.red);

        float closestDistance = float.MaxValue;
        GameObject closestTarget = null;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != _stateMachine.Player.gameObject)
            {
                IGrabbable grabbable = hit.collider.GetComponent<IGrabbable>();
                if (grabbable != null)
                {
                    float distance = Vector2.Distance(_stateMachine.Player.transform.position, hit.collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = hit.collider.gameObject;
                    }
                }
            }
        }

        if (closestTarget != null)
        {
            _stateMachine.Player.TargetObject = closestTarget;
            return true;
        }

        return false;
    }

    public void GrabObject()
    {
        if (_stateMachine.Player.TargetObject != null)
        {
            IGrabbable grabbable = _stateMachine.Player.TargetObject.GetComponent<IGrabbable>();
            _stateMachine.Player.TargetObject.transform.SetParent(_stateMachine.Player.HandPoint);
            if (_stateMachine.Player.Collider != null)
            {
                Physics2D.IgnoreCollision(_stateMachine.Player.TargetObject.GetComponent<Collider2D>(), _stateMachine.Player.Collider, true);
            }
            grabbable.OnGrab();
        }
    }

    public void ReleaseObject()
    {
        if (_stateMachine.Player.TargetObject != null)
        {
            IGrabbable grabbable = _stateMachine.Player.TargetObject.GetComponent<IGrabbable>();
            _stateMachine.Player.TargetObject.transform.SetParent(null);
            if (_stateMachine.Player.Collider != null)
            {
                Physics2D.IgnoreCollision(_stateMachine.Player.TargetObject.GetComponent<Collider2D>(), _stateMachine.Player.Collider, false);
            }
            grabbable.OnRelease();
            _stateMachine.Player.TargetObject = null;
        }
    }
}
