using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : ItemPickUp
{
    public float AddSpeed = 3.0f;
    public float Duration = 3.0f;
    public float forceMagnitude = 30f;
    public override void PickUp(Collider2D collision)
    {
        ForceReceiver forceReceiver = collision.gameObject.GetComponent<ForceReceiver>();
        Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (forceReceiver != null && rigidbody != null)
        {
            Vector2 collisionDirection = rigidbody.velocity.normalized;
            Vector2 appliedForce = collisionDirection * forceMagnitude;
            forceReceiver.AddForce(appliedForce);
        }
    }
}
