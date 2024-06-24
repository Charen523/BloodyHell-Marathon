using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMasterManager : MonoBehaviourPunCallbacks
{
	#region SerializeField
	[SerializeField] private string[] playerPrefab;
	[SerializeField] private Vector2 startPos;
	[SerializeField] private Vector2 positionInterval;
	[SerializeField] private string boxPrefab;
	[SerializeField] private string ballPrefab;
	[SerializeField] private NetworkPlayerData networkPlayerData;
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
		// 게임 시작시 각 플레이어에게 줄 캐릭터를 생성하고 소유권을 나눠줌
		if (PhotonNetwork.IsMasterClient)
		{
			int i = 0;
			Vector2 createPos = startPos;
			foreach (var playerIdPair in PhotonPlayerData.Instance.PlayerIdDict)
			{
				// 새 플레이어 생성하고 소유권 부여, 마스터의 Dictionary에 추가
				GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab[i], createPos, Quaternion.identity);
				Debug.Log("플레이어 생성");
				pv = newPlayer.GetComponent<PhotonView>();
				pv.TransferOwnership(playerIdPair.Value);
				PlayerObjectDict.Add(playerIdPair.Key, newPlayer);
				Debug.Log("플레이어 초기화 끝");

				i++;
				createPos += positionInterval;
			}
		}
	}
	private void Start()
	{
		//물체 2개 박스, 공 생성
		if (PhotonNetwork.IsMasterClient)
		{
			GameObject newBox = PhotonNetwork.Instantiate(boxPrefab, new Vector2(-10, -6.6f), Quaternion.identity);
			GameObject newBall = PhotonNetwork.Instantiate(ballPrefab, new Vector2(-6, -7f), Quaternion.identity);
		}
	}
	#endregion
}
