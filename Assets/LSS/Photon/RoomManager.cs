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
		// �����͸� �ش� ���� ����
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
		tmp.SetText($"���� Id : {id}");
	}
	#endregion

	#region Public Methods
	// ���ο� �÷��̾� ����� UserId�� �� PlayerId ��Ī, ����Ʈ�� ǥ��
	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			base.OnPlayerEnteredRoom(newPlayer);
			PhotonPlayerData.Instance.AddPlayer(newPlayer.UserId, GetNextPlayerId());

			MakePlayerListContent(PhotonPlayerData.Instance.PlayerIdDict[newPlayer.UserId]);

			//������ �� ���̸� ���� ����
			if (PhotonNetwork.CountOfPlayersInRooms == 5)
			{
				Debug.Log("���� 5�� ����, 10�� �� ����");
				StartCoroutine(StartGame());
			}
		}
	}
	#endregion
}
