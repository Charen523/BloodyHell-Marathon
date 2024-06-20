using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
	#region Private Serializable Fields
	/// <summary>
	/// ��� �ִ� �ο� ��. �̸� ������ ���� �Ұ�
	/// </summary>
	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	[SerializeField]
	private int maxPlayersPerRoom = 4;
	#endregion

	#region Private Fields
	/// <summary>
	/// ���� ����. �̰� ���� �������� �������ٰ� �Ѵ�.
	/// </summary>
	private string gameVersion = "1";
	private bool isConnecting = false;
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
		PhotonNetwork.ConnectUsingSettings();
		PhotonNetwork.GameVersion = gameVersion;
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
	public void ConnectToRoom()
	{
		progressLabel.SetActive(true);
		controlPanel.SetActive(false);

		if (PhotonNetwork.IsConnected)
		{
			// ������ �뿡 ����
			if (isConnecting)
			{
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				progressLabel.SetActive(false);
				controlPanel.SetActive(true);
			}
		}
		else
		{
			// ���� ���� ���� ���ÿ� ���缭 ����
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
		}
	}

	public void ConnectToLobby()
	{
		progressLabel.SetActive(true);
		controlPanel.SetActive(false);

		if (PhotonNetwork.IsConnected)
		{
			if (isConnecting)
			{
				PhotonNetwork.JoinLobby();
			}
			else
			{
				progressLabel.SetActive(false);
				controlPanel.SetActive(true);
			}
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
		}
	}

	#endregion

	#region MonoBehaviourPunCallbacks Callbacks

	public override void OnConnectedToMaster()
	{
		Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
		isConnecting = true;
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		progressLabel.SetActive(false);
		controlPanel.SetActive(true);
		isConnecting = false;
		Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

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

	public override void OnCreatedRoom()
	{
		base.OnCreatedRoom();
		PhotonNetwork.LoadLevel("RoomScene");
	}

	public override void OnJoinedLobby()
	{
		base.OnJoinedLobby();
		PhotonNetwork.LoadLevel("PhotonLobbyScene");
	}

	#endregion
}
