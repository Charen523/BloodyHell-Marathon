using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonShowBoard : MonoBehaviourPunCallbacks
{
    public void ShowBoard()
    {
        photonView.RPC("StartCounting", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void StartCounting()
    {
        StartCoroutine(RaceManager.Instance.CountDown());
    }
}
