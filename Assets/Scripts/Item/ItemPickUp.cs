using Unity.VisualScripting;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item Item { get; set; }

    private void Start()
    {
        Item = DataManager.Instance.GetData(name);
        Debug.Log("ItemPickUp Name : " + name);
    }

    private void OnTriggerEnter(Collider other)
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