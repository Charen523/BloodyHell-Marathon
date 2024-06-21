using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class ItemPickUp : MonoBehaviour
{
    public Item Item { get; set; }
    public event Action OnPickUp;

    private void Start()
    {
        Item = DataManager.Instance.GetData(name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPickUp?.Invoke();
            if (Item.Type == ItemType.Manual)
            {
                collision.GetComponent<Player>().PickedUpItems.Add(Item);
            }
            else // ItemType.Auto
            { 
                PickUp(collision);
            }
            ObjectPoolManager.Instance.ReleaseObject(Item.Rcode, this.gameObject);
        }
    }

    public abstract void PickUp(Collider2D other);
}