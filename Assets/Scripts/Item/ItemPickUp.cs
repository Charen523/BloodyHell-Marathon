using UnityEngine;

public abstract class ItemPickUp : MonoBehaviour
{
    public Item Item { get; set; }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp(other);
        }
    }

    public abstract void PickUp(Collider other);
}