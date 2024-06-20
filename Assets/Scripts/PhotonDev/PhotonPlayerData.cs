using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerData : MonoBehaviourPunCallbacks
{
    #region SerilizedField
    [SerializeField] private int maxNumberOfPlayers = 1;
	#endregion
	#region PrivateField
	private static PhotonPlayerData instance;
	private Dictionary<string, int> playerIdDict;
	#endregion
	#region Public Elements
	public static PhotonPlayerData Instance
    {
        get
        {
            return instance;
        }
    }
    
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
	public int MaxNumberOfPlayers
    {
        get
        {
            return maxNumberOfPlayers;
		}
    }
	#endregion

	private void Awake()
    {
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
	public void RemovePlayer(string userId)
	{
		PlayerIdDict.Remove(userId);
	}
}
