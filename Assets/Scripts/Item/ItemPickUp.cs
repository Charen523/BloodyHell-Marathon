using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
public abstract class ItemPickUp : MonoBehaviourPunCallbacks
{
    public Item Item { get; set; }
    public ItemSpawner Spawner { get; set; }

    private void Awake()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

        PhotonTransformView photonTransformView = gameObject.GetComponent<PhotonTransformView>();
        photonTransformView.m_SynchronizePosition = true;
        photonTransformView.m_SynchronizeRotation = false;
        photonTransformView.m_SynchronizeScale = false;
        photonTransformView.m_UseLocal = false;
    }

    private void Start()
    {
        Item = DataManager.Instance.GetData(name.Split('(')[0]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Item.Type == ItemType.Manual)
            {
                collision.GetComponent<Player>().PickedUpItems.Add(Item);
            }
            else // ItemType.Auto
            {
                PickUp(collision);
            }
            if(collision.GetComponent<PhotonView>().IsMine) ItemUIManager.Instance.OnItemUI(Item.Name, Item.Description);
            photonView.RPC("OnPickedUp", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    public void OnPickedUp()
    {
        ObjectPoolManager.Instance.ReleaseObject(Item.Rcode, gameObject);

        if (PhotonNetwork.IsMasterClient && Spawner != null)
        {
            Spawner.photonView.RPC("SpawnItem", RpcTarget.MasterClient);           
        }
    }

    public abstract void PickUp(Collider2D other);
}
