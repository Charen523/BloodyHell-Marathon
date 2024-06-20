using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
	#region SerializeField
	[Header("PlayerList")]
	[SerializeField] private GameObject playerListContentPrefab;
	[SerializeField] private Transform playerListParent;

	[Header("GameStartCounter")]
	[SerializeField] private GameObject startCounterBG;
	[SerializeField] private TextMeshProUGUI startCounterTxt;
	#endregion
	#region Private Field
	private int playerIdNumber = 0;
	private Coroutine gameStartCoroutine;
	private Dictionary<int, GameObject> playerObjectList = new Dictionary<int, GameObject>();
	#endregion

	#region MonoBehaviour Callbacks

	private void Start()
	{
		if (!PhotonNetwork.IsMasterClient) return;
		AddPlayerToData(PhotonNetwork.LocalPlayer);
		startCounterBG.SetActive(false);
	}
	#endregion

	#region Private Methods
	// 게임 시작 카운트 다운. 다되면 마스터가 게임 시작
	private IEnumerator StartGame()
	{
		float timer = 10f;
		while (timer > 0)
		{
			startCounterTxt.SetText($"게임 시작 {timer.ToString("F0")}...");
			yield return new WaitForSeconds(1f);
			timer--;
		}
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel("MainScene");
		}
	}

	// 새 플레이어에게 새 아이디 할당
	private int GetNextPlayerId()
	{
		return playerIdNumber++;
	}

	// 플레이어를 화면에 표시하는 프리팹 생성
	[PunRPC]
	private void MakePlayerListContent(int id)
	{
		GameObject newListContent = Instantiate(playerListContentPrefab, playerListParent);
		TextMeshProUGUI tmp = newListContent.GetComponentInChildren<TextMeshProUGUI>();
		tmp.SetText($"유저 Id : {id}");
		if (PhotonNetwork.IsMasterClient)
		{
			playerObjectList.Add(id, newListContent);
		}
	}

	// 플레이어를 화면에 표시하는 프리팹 제거
	[PunRPC]
	private void RemovePlayerListContent(int id)
	{
		if (playerObjectList.TryGetValue(id, out GameObject playerInfo))
		{
			Destroy(playerInfo);
			playerObjectList.Remove(id);
		}
	}

	// 마스터가 처리하는 플레이어 추가
	private void AddPlayerToData(Photon.Realtime.Player newPlayer)
	{
		PhotonPlayerData.Instance.AddPlayer(newPlayer.UserId, GetNextPlayerId());

		int playerInGameId = PhotonPlayerData.Instance.PlayerIdDict[newPlayer.UserId];
		photonView.RPC("MakePlayerListContent", RpcTarget.AllBuffered, playerInGameId);
	}

	// 마스터가 처리하는 플레이어 제거
	private void RemovePlayerFromData(Photon.Realtime.Player otherPlayer)
	{
		StopCoroutine(gameStartCoroutine);
		startCounterBG.SetActive(false);
		PhotonPlayerData.Instance.PlayerIdDict.Remove(otherPlayer.UserId);

		int playerInGameId = PhotonPlayerData.Instance.PlayerIdDict[otherPlayer.UserId];
		if (PhotonNetwork.IsMasterClient)
		{
			photonView.RPC("RemovePlayerListContent", RpcTarget.AllBuffered, playerInGameId);
		}
	}

	// 마스터가 인원 확인하고 시작 카운트 다운
	[PunRPC]
	private void StartCountDown()
	{
		Debug.Log($"유저 {PhotonPlayerData.Instance.MaxNumberOfPlayers}명 모임, 10초 뒤 시작");
		startCounterBG.SetActive(true);
		gameStartCoroutine = StartCoroutine(StartGame());
	}
	#endregion

	#region MonoBehaviourPunCallbacks Callbacks

	// 게스트 플레이어가 추가되면 해당 플레이어의 id를 표시하고 인원수가 다 차면 게임 10초 뒤에 시작
	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			base.OnPlayerEnteredRoom(newPlayer);
			AddPlayerToData(newPlayer);
			// 인원이 다 차면 게임 시작 카운트 다운
			if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonPlayerData.Instance.MaxNumberOfPlayers)
			{
				photonView.RPC("StartCountDown", RpcTarget.AllBuffered);
			}
		}

	}

	// 게스트 플레이어가 도중에 나가면 게임 시작을 취소하고, 해당 플레이어는 화면과 데이터상 목록에서 제외
	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
		if (!PhotonNetwork.IsMasterClient) { return; }
		base.OnPlayerLeftRoom(otherPlayer);
		RemovePlayerFromData(otherPlayer);
	}

	#endregion
}
