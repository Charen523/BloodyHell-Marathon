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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickUp?.Invoke();
            if (Item.Type == ItemType.Manual)
            {
                other.GetComponent<Player>().PickedUpItem.Add(Item);
            }
            PickUp(other);
        }
    }

    public abstract void PickUp(Collider other);
}