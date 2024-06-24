using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerData : Singleton<PhotonPlayerData>
{
    #region SerilizedField
    [SerializeField] private int maxNumberOfPlayers = 1;
	#endregion
	#region PrivateField
	private Dictionary<string, int> playerIdDict;
	#endregion
	#region Public Elements
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
    #region MonoBehaviour Callbacks
    protected override void Awake()
    {
        base.Awake();
        playerIdDict = new Dictionary<string, int>();
	}
	#endregion
	#region Public Methods

    // 플레이어 dictionary에 플레이어 UserId, PlayerId 할당
	public void AddPlayer(string userId, int playerNumber)
    {
        PlayerIdDict[userId] = playerNumber;
    }

    // 플레이어 dictionary에서 플레이어 제거
	public void RemovePlayer(string userId)
	{
		PlayerIdDict.Remove(userId);
	}

	#endregion
}
