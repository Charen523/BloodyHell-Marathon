using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
	#region Private Serializable Fields
	/// <summary>
	/// 룸당 최대 인원 수. 이를 넘으면 참여 불가
	/// </summary>
	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	#endregion
	#region Private Fields
	/// <summary>
	/// 게임 버전. 이게 같은 유저끼리 만나진다고 한다.
	/// </summary>
	private string gameVersion = "1";
	private int maxPlayersPerRoom;
	#endregion
	#region Public Fields
	//룸에 연결하는 버튼
	[Tooltip("The Ui Panel to let the user enter name, connect enter Room")]
	[SerializeField]
	private GameObject controlPanel;

	//로딩창
	[Tooltip("The UI Label to inform the user that the connection is in progress")]
	[SerializeField]
	private GameObject progressLabel;
	#endregion
	#region MonoBehaviour CallBacks

	private void Awake()
	{
		// #Critical
		// 현재 있는 모든 유저가 같은 씬을 열도록 하는 기능
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.AutomaticallySyncScene = true;
	}
	private void Start()
	{
		progressLabel.SetActive(false);
		controlPanel.SetActive(true);
		maxPlayersPerRoom = PhotonPlayerData.Instance.MaxNumberOfPlayers;
	}

	#endregion
	#region Public Methods
	/// <summary>
	/// Start the connection process.
	/// - If already connected, we attempt joining a random room
	/// - if not yet connected, Connect this application instance to Photon Cloud Network
	/// 연결 함수
	/// 연결 되어있으면 랜덤 룸에 연결
	/// 연결 안되어 있으면 연결하고 다시 룸에 연결
	/// </summary>
	public void Connect()
	{
		progressLabel.SetActive(true);
		controlPanel.SetActive(false);
		// we check if we are connected or not, we join if we are, else we initiate the connection to the server.
		// 연결 되어있나 확인
		if (PhotonNetwork.IsConnected)
		{
			// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
			// 랜덤한 룸에 연결
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			// #Critical, we must first and foremost connect to Photon Online Server.
			// 연결 안되어있으면 현재 포톤연결세팅에 맞춰서 연결
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
		}
	}
	
	#endregion
	#region MonoBehaviourPunCallbacks Callbacks
	public override void OnConnectedToMaster()
	{
		Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
		PhotonNetwork.JoinRandomRoom();
	}
	public override void OnDisconnected(DisconnectCause cause)
	{
		progressLabel.SetActive(false);
		controlPanel.SetActive(true);
		Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
	}
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
		// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
		// 룸에 연결 실패. 아마도 룸이 없을 것이라고 판단하고 룸을 새로 만들어서 연결
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.PublishUserId = true;
		roomOptions.MaxPlayers = maxPlayersPerRoom;
		PhotonNetwork.CreateRoom(null, roomOptions);
	}
	public override void OnJoinedRoom()
	{
		Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
		//룸에 게스트로 연결
	}

	/// <summary>
	/// 룸을 새로 만들었을 경우 Room 씬으로 이동하고, 이 유저가 룸의 호스트
	/// </summary>
	public override void OnCreatedRoom()
	{
		base.OnCreatedRoom();
		PhotonNetwork.LoadLevel("RoomScene");
	}
	#endregion
}
