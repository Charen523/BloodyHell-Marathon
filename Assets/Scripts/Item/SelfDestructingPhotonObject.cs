using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructingPhotonObject : MonoBehaviour
{
    public float Duration = 5.0f;

    private void Start()
    {
        StartCoroutine(DestructingPhotonObject());
    }

    private IEnumerator DestructingPhotonObject()
    {
        yield return new WaitForSeconds(Duration);

        PhotonNetwork.Destroy(gameObject);
    }
}
