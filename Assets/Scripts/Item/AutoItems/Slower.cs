using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : ItemPickUp
{
    public float MassIncrease = 10.0f; // 질량 증가량
    public float Duration = 3.0f; // 슬로우 효과 지속 시간

    public override void PickUp(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            StartCoroutine(ApplySlowEffect(rb));
        }
    }

    private IEnumerator ApplySlowEffect(Rigidbody2D rb)
    {
        float originalMass = rb.mass;
        rb.mass += MassIncrease;

        yield return new WaitForSeconds(Duration);

        rb.mass = originalMass;
    }
}
