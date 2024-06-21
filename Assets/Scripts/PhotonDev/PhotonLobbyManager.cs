using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PhotonLobbyManager : MonoBehaviourPunCallbacks
{
	#region SerializeField
	[Header("RoomListObjects")]
	[SerializeField] private GameObject roomListPrefab;
	[SerializeField] private Transform roomListParent;
	#endregion
	#region Private Fields
	private Dictionary<string, GameObject> roomDict;
	#endregion
	#region MonoBehaviour
	private void Start()
	{
		roomDict = new Dictionary<string, GameObject>();
		if (!PhotonNetwork.IsConnected)
		{
			//혹시라도 연결 끊기면 실행
			PhotonNetwork.ConnectUsingSettings();
		}
	}
	#endregion
	#region MonoBehaviourPunCallbacks
	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}
	public override void OnJoinedLobby()
	{
		base.OnJoinedLobby();
		Debug.Log("Joined Lobby");
	}
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		base.OnRoomListUpdate(roomList);
		foreach (RoomInfo roomInfo in roomList)
		{
			if (roomDict.ContainsKey(roomInfo.Name))
			{
				Destroy(roomDict[roomInfo.Name]);
				roomDict.Remove(roomInfo.Name);
			}
			else
			{
				if (!roomDict.ContainsKey(roomInfo.Name))
				{
					GameObject roomEntry = Instantiate(roomListPrefab, roomListParent);
					roomEntry.GetComponentInChildren<TextMeshProUGUI>().text = $"방 이름 : {roomInfo.Name.Substring(0, 4)}\n현재 인원 : {roomInfo.PlayerCount}";
					Button button = roomEntry.GetComponent<Button>();
					button.onClick.AddListener(() =>
					{
						JoinRoom(roomInfo.Name);
					}
					);
					roomDict.Add(roomInfo.Name, roomEntry);
				}
			}
		}
	}
	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		SceneManager.LoadScene("RoomScene");
	}
	public override void OnCreatedRoom()
	{
		base.OnCreatedRoom();
		PhotonNetwork.LoadLevel("RoomScene");
	}
	#endregion
	#region Public Methods
	public void JoinRoom(string roomName)
	{
		PhotonNetwork.JoinRoom(roomName);
	}
	public void MakeRoom()
	{
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.PublishUserId = true;
		roomOptions.MaxPlayers = PhotonPlayerData.Instance.MaxNumberOfPlayers;
		PhotonNetwork.CreateRoom(null, roomOptions);
	}
	#endregion
}
