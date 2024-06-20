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
	/// ��� �ִ� �ο� ��. �̸� ������ ���� �Ұ�
	/// </summary>
	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	#endregion
	#region Private Fields
	/// <summary>
	/// ���� ����. �̰� ���� �������� �������ٰ� �Ѵ�.
	/// </summary>
	private string gameVersion = "1";
	private int maxPlayersPerRoom;
	#endregion
	#region Public Fields
	//�뿡 �����ϴ� ��ư
	[Tooltip("The Ui Panel to let the user enter name, connect enter Room")]
	[SerializeField]
	private GameObject controlPanel;

	//�ε�â
	[Tooltip("The UI Label to inform the user that the connection is in progress")]
	[SerializeField]
	private GameObject progressLabel;
	#endregion
	#region MonoBehaviour CallBacks

	private void Awake()
	{
		// #Critical
		// ���� �ִ� ��� ������ ���� ���� ������ �ϴ� ���
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
	/// ���� �Լ�
	/// ���� �Ǿ������� ���� �뿡 ����
	/// ���� �ȵǾ� ������ �����ϰ� �ٽ� �뿡 ����
	/// </summary>
	public void Connect()
	{
		progressLabel.SetActive(true);
		controlPanel.SetActive(false);
		// we check if we are connected or not, we join if we are, else we initiate the connection to the server.
		// ���� �Ǿ��ֳ� Ȯ��
		if (PhotonNetwork.IsConnected)
		{
			// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
			// ������ �뿡 ����
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			// #Critical, we must first and foremost connect to Photon Online Server.
			// ���� �ȵǾ������� ���� ���濬�Ἴ�ÿ� ���缭 ����
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
		// �뿡 ���� ����. �Ƹ��� ���� ���� ���̶�� �Ǵ��ϰ� ���� ���� ���� ����
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.PublishUserId = true;
		roomOptions.MaxPlayers = maxPlayersPerRoom;
		PhotonNetwork.CreateRoom(null, roomOptions);
	}
	public override void OnJoinedRoom()
	{
		Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
		//�뿡 �Խ�Ʈ�� ����
	}

	/// <summary>
	/// ���� ���� ������� ��� Room ������ �̵��ϰ�, �� ������ ���� ȣ��Ʈ
	/// </summary>
	public override void OnCreatedRoom()
	{
		base.OnCreatedRoom();
		PhotonNetwork.LoadLevel("RoomScene");
	}
	#endregion
}
