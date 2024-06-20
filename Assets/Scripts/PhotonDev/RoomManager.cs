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
	// ���� ���� ī��Ʈ �ٿ�. �ٵǸ� �����Ͱ� ���� ����
	private IEnumerator StartGame()
	{
		float timer = 10f;
		while (timer > 0)
		{
			startCounterTxt.SetText($"���� ���� {timer.ToString("F0")}...");
			yield return new WaitForSeconds(1f);
			timer--;
		}
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel("MainScene");
		}
	}

	// �� �÷��̾�� �� ���̵� �Ҵ�
	private int GetNextPlayerId()
	{
		return playerIdNumber++;
	}

	// �÷��̾ ȭ�鿡 ǥ���ϴ� ������ ����
	[PunRPC]
	private void MakePlayerListContent(int id)
	{
		GameObject newListContent = Instantiate(playerListContentPrefab, playerListParent);
		TextMeshProUGUI tmp = newListContent.GetComponentInChildren<TextMeshProUGUI>();
		tmp.SetText($"���� Id : {id}");
		if (PhotonNetwork.IsMasterClient)
		{
			playerObjectList.Add(id, newListContent);
		}
	}

	// �÷��̾ ȭ�鿡 ǥ���ϴ� ������ ����
	[PunRPC]
	private void RemovePlayerListContent(int id)
	{
		if (playerObjectList.TryGetValue(id, out GameObject playerInfo))
		{
			Destroy(playerInfo);
			playerObjectList.Remove(id);
		}
	}

	// �����Ͱ� ó���ϴ� �÷��̾� �߰�
	private void AddPlayerToData(Photon.Realtime.Player newPlayer)
	{
		PhotonPlayerData.Instance.AddPlayer(newPlayer.UserId, GetNextPlayerId());

		int playerInGameId = PhotonPlayerData.Instance.PlayerIdDict[newPlayer.UserId];
		photonView.RPC("MakePlayerListContent", RpcTarget.AllBuffered, playerInGameId);
	}

	// �����Ͱ� ó���ϴ� �÷��̾� ����
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

	// �����Ͱ� �ο� Ȯ���ϰ� ���� ī��Ʈ �ٿ�
	[PunRPC]
	private void StartCountDown()
	{
		Debug.Log($"���� {PhotonPlayerData.Instance.MaxNumberOfPlayers}�� ����, 10�� �� ����");
		startCounterBG.SetActive(true);
		gameStartCoroutine = StartCoroutine(StartGame());
	}
	#endregion

	#region MonoBehaviourPunCallbacks Callbacks

	// �Խ�Ʈ �÷��̾ �߰��Ǹ� �ش� �÷��̾��� id�� ǥ���ϰ� �ο����� �� ���� ���� 10�� �ڿ� ����
	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			base.OnPlayerEnteredRoom(newPlayer);
			AddPlayerToData(newPlayer);
			// �ο��� �� ���� ���� ���� ī��Ʈ �ٿ�
			if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonPlayerData.Instance.MaxNumberOfPlayers)
			{
				photonView.RPC("StartCountDown", RpcTarget.AllBuffered);
			}
		}

	}

	// �Խ�Ʈ �÷��̾ ���߿� ������ ���� ������ ����ϰ�, �ش� �÷��̾�� ȭ��� �����ͻ� ��Ͽ��� ����
	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
		if (!PhotonNetwork.IsMasterClient) { return; }
		base.OnPlayerLeftRoom(otherPlayer);
		RemovePlayerFromData(otherPlayer);
	}

	#endregion
}
