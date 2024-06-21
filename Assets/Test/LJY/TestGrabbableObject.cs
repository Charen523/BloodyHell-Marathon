using UnityEngine;

public class TestGrabbableObject : MonoBehaviour, IGrabbable
{
    Rigidbody2D _rigid;
    Collider2D _collider;

    RigidbodyType2D memoryBodyType;
    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        memoryBodyType = _rigid.bodyType;
    }
    public void OnGrab()
    {
        if(_rigid != null)
        {
			_rigid.isKinematic = true;
			_rigid.velocity = Vector2.zero;
		}
        
        transform.localPosition = new Vector3(0f, transform.localPosition.y);
    }

    public void OnRelease()
    {
        if(_rigid != null)
        {
			_rigid.bodyType = memoryBodyType;
		}
    }
}
