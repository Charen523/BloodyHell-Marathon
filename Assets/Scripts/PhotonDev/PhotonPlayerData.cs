using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerData : MonoBehaviourPunCallbacks
{
    private static PhotonPlayerData instance;

    public static PhotonPlayerData Instance
    {
        get
        {
            return instance;
        }
    }

    private Dictionary<string, int> playerIdDict;
    public Dictionary<string, int> PlayerIdDict 
    {
        get
        {
            return playerIdDict;
		}
        private set
        {
            playerIdDict = value;
        } 
    }

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(gameObject);
        }
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        playerIdDict = new Dictionary<string, int>();
	}

    public void AddPlayer(string userId, int playerNumber)
    {
        PlayerIdDict[userId] = playerNumber;
    }
}
