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
        Vector3 yOffset = Vector2.up * -0.2f;
        Vector2 direction = _stateMachine.Player.HandPoint.position - (_stateMachine.Player.transform.position + yOffset).normalized;
        RaycastHit2D hit = Physics2D.Raycast(_stateMachine.Player.transform.position + yOffset, direction, 1f);

        if (hit.collider != null && hit.collider.gameObject != _stateMachine.Player.gameObject)
        {
            IGrabbable grabbable = hit.collider.GetComponent<IGrabbable>();
            if (grabbable != null)
            {
                _stateMachine.Player.TargetObject = hit.collider.gameObject;
                return true;
            }
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
