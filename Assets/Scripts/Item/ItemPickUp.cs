using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item Item { get; set; }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp(other);
        }
    }

    public virtual void PickUp(Collider other)
    {
        if(Item.Type == ItemType.Manual)
        {
            other.GetComponent<Player>().PickedUpItem.Add(Item);
        }
    }
}