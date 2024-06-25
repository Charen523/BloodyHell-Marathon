using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : ItemPickUp
{
    public override void PickUp(Collider2D collision)
    {       
        Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            Vector3 collisionDirection = rigidbody.velocity.normalized;
            PhotonNetwork.Instantiate("Objects/BounceBall", transform.position - collisionDirection * 2, Quaternion.identity);
        }
    }
}
