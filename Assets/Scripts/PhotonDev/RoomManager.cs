using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerProperties
{
    public const string indexKey = "myIndex";
    public const string readyKey = "IsReady";
}

public static class RoomProperties
{
    public const string playerSlotKey = "PlayerSlots";
}

public class RoomManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private RoomUIManager roomUIManager;

	#region Private Field
	private Coroutine gameStartCoroutine;
    #endregion

	#region Private Methods
    private void InitRoom()
    {
        AddPlayerToData(PhotonNetwork.LocalPlayer, 0); //Master의 PlayerData 추가.
        InitMaster(); //방장 권한.

        /*방에 존재하는 플레이어 감지*/
        bool[] playerSlots = new bool[5];
        playerSlots[0] = true;
        ExitGames.Client.Photon.Hashtable newPlayerSlots
            = new ExitGames.Client.Photon.Hashtable { { RoomProperties.playerSlotKey, playerSlots } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(newPlayerSlots);
    }

    private void InitMaster()
    {
        roomUIManager.GetSkipBtn().interactable = true;
        roomUIManager.MapDropdown.interactable = true;
        roomUIManager.MapDropdown.onValueChanged.AddListener(OnMapChanged);
        //roomUIManager.ChangeMasterPlayer(newMasterClient.CustomProperties);
    }

    private void AddPlayerToData(Photon.Realtime.Player newPlayer, int index)
    {// 마스터가 처리하는 플레이어 추가
        PhotonPlayerData.Instance.AddPlayer(newPlayer.UserId, index);
        SetPlayerIndex(index);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("AddPlayerUI", RpcTarget.AllBuffered, newPlayer.UserId, index);
        }
    }

    private void RemovePlayerFromData(Photon.Realtime.Player otherPlayer)
    {// 마스터가 처리하는 플레이어 제거
        StopCoroutine(gameStartCoroutine);
        roomUIManager.HideStartCounter();
        PhotonPlayerData.Instance.PlayerIdDict.Remove(otherPlayer.UserId);

        int playerInGameId = PhotonPlayerData.Instance.PlayerIdDict[otherPlayer.UserId];
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RemovePlayerUI", RpcTarget.AllBuffered, playerInGameId);
        }
    }

    private int AssignPlayerSlot()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperties.playerSlotKey, out var values))
        {
            bool[] playerSlots = (bool[])values;

            for (int i = 0; i < playerSlots.Length; i++)
            {
                if (!playerSlots[i])
                {
                    playerSlots[i] = true;
                    ExitGames.Client.Photon.Hashtable newPlayerSlots
                        = new ExitGames.Client.Photon.Hashtable() { { RoomProperties.playerSlotKey, playerSlots } };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(newPlayerSlots);

                    SetPlayerIndex(i);
                }
            }

            Debug.LogError("PlayerSlot out of index.");
            return -1;
        }

        Debug.LogError("Cant find playerSlotKey in CurrentRoom CustomProperties.");
        return -1;
    }

    private IEnumerator StartGame()
    {// 게임 시작 카운트 다운. 다되면 마스터가 게임 시작
        int timer = roomUIManager.StartCount;
        
        if (PhotonNetwork.IsMasterClient)
        {
            roomUIManager.GetSkipBtn().interactable = true;
        }

		while (timer > 0)
		{
            roomUIManager.UpdateStartCounter(timer);
            yield return new WaitForSeconds(1f);
			timer--;
		}

		LoadGameScene();
    }

	

	[PunRPC]
	private void StartCountDown()
    {// 마스터가 인원 확인하고 시작 카운트 다운
        Debug.Log($"유저 {PhotonPlayerData.Instance.MaxNumberOfPlayers}명 모임, 10초 뒤 시작");
		roomUIManager.ShowStartCounter();
		gameStartCoroutine = StartCoroutine(StartGame());
	}

    

    [PunRPC]
    private void AddPlayerUI(string userId, int index)
    {
        roomUIManager.ShowConnectedPlayer(userId, index);
    }

    [PunRPC]
    private void RemovePlayerUI(int index)
    {
        roomUIManager.HideDisconnectedPlayer(index);
    }

    private void SetPlayerIndex(int index)
    {
        ExitGames.Client.Photon.Hashtable newPlayerIndex
                                = new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.indexKey, index } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(newPlayerIndex);
    }

    private void TogglePlayerReady()
    {
        bool isReady;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerProperties.readyKey))
        {//등록된 적 있으면 그 상태 불러오기.
            isReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties[PlayerProperties.readyKey];
        }
        else
        {//없으면 레디한 적이 없다고 간주.
            isReady = false;
        }
        isReady = !isReady;

        // 준비 상태 업데이트
        ExitGames.Client.Photon.Hashtable newReadyKey
            = new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.readyKey, isReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(newReadyKey);

        // 마스터에게 상태 확인 요청
        if (isReady)
        {
            photonView.RPC("RequestCheckAllPlayersReady", RpcTarget.MasterClient);
        }
        else
        {
            
                photonView.RPC("StopGame", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RequestCheckAllPlayersReady()
    {
        CheckAllPlayersReady();
    }

    private void CheckAllPlayersReady()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            object isReady;
            if (player.CustomProperties.TryGetValue(PlayerProperties.readyKey, out isReady))
            {
                if (!(bool)isReady)
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        // 모든 플레이어가 준비된 상태라면 게임 시작
        photonView.RPC("StartCountDown", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void StopGameMaster()
    {
        roomUIManager.GetSkipBtn().interactable = false;
        photonView.RPC("StopGameAll", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void StopGameAll()
    {
        if (gameStartCoroutine != null)
        {
            StopCoroutine(gameStartCoroutine);
            roomUIManager.HideStartCounter();
        }
    }
    #endregion

    #region UI Interacts
    public void LoadGameScene()
    {//skipBtn or 자동시작.
        if (PhotonNetwork.IsMasterClient)
        {
            switch (roomUIManager.MapDropdown.value)
            {
                case 0:
                    PhotonNetwork.LoadLevel("CharacterAnimeTestScene COPY");
                    break;
                case 1:
                    PhotonNetwork.LoadLevel("GameScene");
                    break;
                default:
                    Debug.LogError("Selected Map Option is out of index.");
                    break;
            }
        }
    }

    private void OnMapChanged(int value)
    {//MapDropdown
        if (!PhotonNetwork.IsMasterClient) { return; }

        photonView.RPC("UpdateMapDropdown", RpcTarget.OthersBuffered, value);
    }

    [PunRPC]
    private void UpdateMapDropdown(int value)
    {
        roomUIManager.MapDropdown.value = value;
    }

    public void LeaveRoom()
    {//exitBtn
        PhotonNetwork.LeaveRoom();
    }

    public void OnReadyButtonClicked()
    { //ReadyBtn
        TogglePlayerReady();
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitRoom();
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {// 게스트 플레이어가 추가되면 해당 플레이어의 id를 표시하고 인원수가 다 차면 게임 10초 뒤에 시작
        if (!PhotonNetwork.IsMasterClient) { return; }

        base.OnPlayerEnteredRoom(newPlayer);

        //플레이어 번호 부여.
        int playerIndex = AssignPlayerSlot();
        AddPlayerToData(newPlayer, playerIndex);

    }

	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    // 게스트 플레이어가 도중에 나가면 게임 시작을 취소하고, 해당 플레이어는 화면과 데이터상 목록에서 제외
    {
        StopGameAll();
        //

        if (!PhotonNetwork.IsMasterClient) { return; }

		base.OnPlayerLeftRoom(otherPlayer);
        roomUIManager.GetSkipBtn().interactable = false;
        RemovePlayerFromData(otherPlayer);
	}

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (!PhotonNetwork.IsMasterClient) { return; }

        base.OnMasterClientSwitched(newMasterClient);
        InitMaster();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0); //StartScene으로 이동.
    }
    #endregion
}
