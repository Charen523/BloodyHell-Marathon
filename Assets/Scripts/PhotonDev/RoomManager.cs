using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
	[Header("PlayerList")]
	[SerializeField] private GameObject playerListContent;
	[SerializeField] private Transform playerListParent;

	private int playerIdNumber = 0;

	#region MonoBehaviour Callbacks
	private void Awake()
	{
		// 마스터만 해당 로직 실행
		if (!PhotonNetwork.IsMasterClient) return;
		DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
		pool.ResourceCache.Add("PlayerListContent", playerListContent);
	}

	private void Start()
	{
		if (!PhotonNetwork.IsMasterClient) return;
		PhotonPlayerData.Instance.AddPlayer(PhotonNetwork.LocalPlayer.UserId, GetNextPlayerId());
		MakePlayerListContent(PhotonPlayerData.Instance.PlayerIdDict[PhotonNetwork.LocalPlayer.UserId]);
	}
	#endregion
	#region Private Methods
	private IEnumerator StartGame()
	{
		float timer = 10f;
		while(timer  > 0)
		{			
			yield return new WaitForSeconds(1f);
			timer--;
		}
		PhotonNetwork.LoadLevel(2);
	}
	private int GetNextPlayerId()
	{
		return playerIdNumber++;
	}

	[PunRPC]
	private void MakePlayerListContent(int id)
	{
		GameObject newListContent = PhotonNetwork.Instantiate("PlayerListContent", Vector3.zero, Quaternion.identity, 0);
		TextMeshProUGUI tmp = newListContent.GetComponentInChildren<TextMeshProUGUI>();
		tmp.SetText($"유저 Id : {id}");
	}
	#endregion

	#region Public Methods
	// 새로운 플레이어 입장시 UserId와 새 PlayerId 매칭, 리스트에 표시
	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			base.OnPlayerEnteredRoom(newPlayer);
			PhotonPlayerData.Instance.AddPlayer(newPlayer.UserId, GetNextPlayerId());

			MakePlayerListContent(PhotonPlayerData.Instance.PlayerIdDict[newPlayer.UserId]);

			//유저가 다 모이면 게임 시작
			if (PhotonNetwork.CountOfPlayersInRooms == 5)
			{
				Debug.Log("유저 5명 모임, 10초 뒤 시작");
				StartCoroutine(StartGame());
			}
		}
	}
	#endregion
}
