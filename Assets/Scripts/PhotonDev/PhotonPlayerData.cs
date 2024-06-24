using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerData : Singleton<PhotonPlayerData>
{
    #region Serialized Field
    [SerializeField] private int maxNumberOfPlayers = 1;
	#endregion

	#region Private Field
	private Dictionary<string, int> playerIdDict; //string = UserId, int = Sprite.
	#endregion

	#region Properties
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

	protected override void Awake()
    {
        base.Awake();
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
