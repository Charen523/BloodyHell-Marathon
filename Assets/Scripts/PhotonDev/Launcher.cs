using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

//TODO: UI 관련 스크립트 분리 리팩토링 필요.
public class Launcher : MonoBehaviourPunCallbacks
{
	#region Private Serializable Fields
	/// <summary>
	/// 룸당 최대 인원 수. 이를 넘으면 참여 불가
	/// </summary>
	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	[SerializeField]
	private int maxPlayersPerRoom = 5; //랜덤 방 생성시 플레이어수
	#endregion

	#region Private Fields
	/// <summary>
	/// 게임 버전. 이게 같은 유저끼리 만나진다고 한다.
	/// </summary>
	private string gameVersion = "1";
	private bool isConnecting = false;
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
        CustomSceneManager.Instance.ShowLoadPanel();
        Debug.Log("ConnectUsingSettings show load.");
        PhotonNetwork.ConnectUsingSettings();

		PhotonNetwork.GameVersion = gameVersion;
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
	public void ConnectToRoom()
	{
        CustomSceneManager.Instance.ShowLoadPanel();
        Debug.Log("ConnectToRoom show load.");

        if (PhotonNetwork.IsConnected)
		{
			// 랜덤한 룸에 연결
			if (isConnecting)
			{
                
                PhotonNetwork.JoinRandomRoom();
			}
		}
		else
		{
			// 현재 포톤 연결 세팅에 맞춰서 연결
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
        }

        CustomSceneManager.Instance.HideLoadPanel();
        Debug.Log("ConnectToRoom hide load.");
    }

	public void ConnectToLobby()
	{
        CustomSceneManager.Instance.ShowLoadPanel();
        Debug.Log("ConnectToLobby show load.");

        if (PhotonNetwork.IsConnected)
		{
			if (isConnecting)
			{
                CustomSceneManager.Instance.HideLoadPanel();
                Debug.Log("OnJoinedLobby hide load.");
                CustomSceneManager.Instance.LoadScene("PhotonLobbyScene");
			}
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
        }
        CustomSceneManager.Instance.HideLoadPanel();
        Debug.Log("ConnectToLobby hide load.");
    }

	#endregion

	#region MonoBehaviourPunCallbacks Callbacks

	public override void OnConnectedToMaster()
	{
		Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
		isConnecting = true;
        CustomSceneManager.Instance.HideLoadPanel();
        Debug.Log("OnConnectedToMaster hide load.");
    }

	public override void OnDisconnected(DisconnectCause cause)
	{
        isConnecting = false;
        CustomSceneManager.Instance.HideLoadPanel();
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

	public override void OnCreatedRoom()
	{
		CustomSceneManager.Instance.HideLoadPanel();
		Debug.Log("OnCreatedRoom hide load.");
		CustomSceneManager.Instance.LoadScene("RoomScene");
	}
	#endregion
}
