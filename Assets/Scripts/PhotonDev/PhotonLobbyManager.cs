using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;


public class PhotonLobbyManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private LobbyUIManager lobbyUIManager;

    private List<string> enableRooms;

    #region MonoBehaviour Callbacks
    private void Start()
	{
		enableRooms = new List<string>();

		if (!PhotonNetwork.IsConnected)
		{
			//혹시라도 연결 끊기면 실행
			PhotonNetwork.ConnectUsingSettings();
		}
	}
    #endregion

    public void ParticipateRoom()
    {
        int toggleIndex = LobbyUIManager.Instance.GetSelectedToggle();
        if (toggleIndex < 4)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.PublishUserId = true;
            roomOptions.MaxPlayers = toggleIndex + 2;
            PhotonNetwork.CreateRoom(null, roomOptions);
        }
        else if (toggleIndex != -1)
        {
			PhotonNetwork.JoinRoom(enableRooms[toggleIndex - 4]);
        }
        else
        {
            Debug.LogError($"Wrong SelectedToggle. Cant Participate Game.");
        }
    }


    #region MonoBehaviourPunCallbacks
    public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("Joined Lobby");
		//가능하면 내 id 보여주기.
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		foreach (RoomInfo roomInfo in roomList)
		{
			if (roomInfo.RemovedFromList || !roomInfo.IsVisible)
			{
                enableRooms.Remove(roomInfo.Name);
			}
			else if (!enableRooms.Contains(roomInfo.Name))
			{
				lobbyUIManager.MakeNewRoomList(roomInfo.Name, roomInfo.PlayerCount, roomInfo.MaxPlayers);
                enableRooms.Add(roomInfo.Name);
            }
        }
	}

    public override void OnCreatedRoom()
    {
        CustomSceneManager.Instance.PhotonLoadLevel("RoomScene");
    }
    
	public override void OnJoinedRoom()
	{
		CustomSceneManager.Instance.LoadScene("RoomScene");
	}
	#endregion

	
}
