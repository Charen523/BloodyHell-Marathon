using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
  Rigidbody2D _rigid;
  float _drag = 0.3f;
  Vector2 _impact;
  Vector2 _dampingVelocity;

  public Vector2 Movement => _impact;

  private void Start()
  {
    _rigid = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    _impact = Vector2.SmoothDamp(_impact, Vector2.zero, ref _dampingVelocity, _drag);
  }

  public void Reset()
  {
    _impact = Vector2.zero;
  }

  public void AddForce(Vector2 force)
  {
    _impact += force;
  }
}
