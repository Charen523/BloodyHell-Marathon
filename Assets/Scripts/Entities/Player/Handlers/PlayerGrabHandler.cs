using UnityEngine;

public class PlayerGrabHandler
{
    private PlayerStateMachine _stateMachine;
    private IGrabbable _grabbable;
    public PlayerGrabHandler(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public bool DetectAndGrab()
    {
        RaycastHit2D hit = Physics2D.Raycast(_stateMachine.Player.transform.position, _stateMachine.Player.transform.right, 1f);

        if (hit.collider != null)
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
            _grabbable = _stateMachine.Player.TargetObject.GetComponent<IGrabbable>();
            _stateMachine.Player.TargetObject.transform.SetParent(_stateMachine.Player.HandPoint);
            _stateMachine.Player.TargetObject.GetComponent<Rigidbody2D>().isKinematic = true;
            _stateMachine.Player.TargetObject.GetComponent<Collider2D>().isTrigger = true;
            _grabbable.OnGrab();
        }
    }

    public void ReleaseObject()
    {
        if (_stateMachine.Player.TargetObject != null)
        {
            _grabbable = _stateMachine.Player.TargetObject.GetComponent<IGrabbable>();
            _stateMachine.Player.TargetObject.GetComponent<Rigidbody2D>().isKinematic = false;
            _stateMachine.Player.TargetObject.GetComponent<Collider2D>().isTrigger = false;
            _stateMachine.Player.TargetObject.transform.SetParent(null);
            _grabbable.OnRelease();
            _grabbable = null;
            _stateMachine.Player.TargetObject = null;
        }
    }
}
