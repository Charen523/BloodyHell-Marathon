using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMasterManager : MonoBehaviour
{
	#region SerializeField
	[SerializeField] private string[] playerPrefab;
	[SerializeField] private Vector2 startPos;
	[SerializeField] private Vector2 positionInterval;
	#endregion
	#region Private Fields
	private Dictionary<string, GameObject> playerDict;
	private PhotonView pv;
	#endregion
	#region Public Elements
	public Dictionary<string, GameObject> PlayerObjectDict
	{
		get
		{
			return playerDict;
		}
		private set
		{
			playerDict = value;
		}
	}
	#endregion

	#region MonoBehaviour Callbacks
	private void Awake()
	{
		playerDict = new Dictionary<string, GameObject>();
	}
	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			int i = 0;
			Vector2 createPos = startPos;
			foreach (var playerIdPair in PhotonPlayerData.Instance.PlayerIdDict)
			{
				GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab[i], createPos, Quaternion.identity);
				pv = newPlayer.GetComponent<PhotonView>();
				pv.TransferOwnership(playerIdPair.Value);
				PlayerObjectDict.Add(playerIdPair.Key, newPlayer);
				i++;
				createPos += positionInterval;
			}
		}
	}
	#endregion
}
