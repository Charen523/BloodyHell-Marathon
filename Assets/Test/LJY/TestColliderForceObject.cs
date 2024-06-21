using UnityEngine;

public class CollisionForceApplier : MonoBehaviour
{
    public float forceMagnitude = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ForceReceiver forceReceiver = collision.gameObject.GetComponent<ForceReceiver>();
        if (forceReceiver != null)
        {
            Vector2 collisionDirection = (collision.transform.position - transform.position).normalized;
            Vector2 appliedForce = collisionDirection * forceMagnitude;
            forceReceiver.AddForce(appliedForce);
        }
    }
}
