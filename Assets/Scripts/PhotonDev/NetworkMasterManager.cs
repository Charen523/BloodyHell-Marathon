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
	[SerializeField] private NetworkPlayerData networkPlayerData;
	#endregion
	#region Private Fields
	private PhotonView pv;
	#endregion
	#region Public Elements
	#endregion
	#region MonoBehaviour Callbacks
	private void Awake()
	{
		// 게임 시작시 각 플레이어에게 줄 캐릭터를 생성하고 소유권을 나눠줌
		if (PhotonNetwork.IsMasterClient)
		{
			int i = 0;
			Vector2 createPos = startPos;
			foreach (var player in PhotonNetwork.PlayerList)
			{
				// 새 플레이어 생성하고 소유권 부여, 마스터의 Dictionary에 추가
				GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab[i], createPos, Quaternion.identity);
				Debug.Log("플레이어 생성");
				pv = newPlayer.GetComponent<PhotonView>();
				pv.TransferOwnership(player.ActorNumber);
				newPlayer.GetComponent<Player>().playerlap.playerCode = player.UserId;
				Debug.Log("플레이어 초기화 끝");

				i++;
				createPos += positionInterval;
			}
		}
	}
	#endregion
}
