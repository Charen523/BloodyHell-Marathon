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

    #region Monobehaviour Callbacks
    private IEnumerator Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitRoom(); 

            //SetCustomProperties가 비동기이기 때문.
            yield return new WaitForSeconds(0.5f);
            AddPlayerToData(PhotonNetwork.LocalPlayer, AssignPlayerSlot()); //Master의 PlayerData 추가.

            yield return new WaitForSeconds(0.5f);
            InitMaster(); //방장 권한.
        }
    }
    #endregion

    #region Init Data
    private void InitRoom()
    {
        /*방에 존재하는 플레이어 감지 프로퍼티 초기화.*/
        bool[] playerSlots = new bool[5];
        ExitGames.Client.Photon.Hashtable newPlayerSlots 
            = new ExitGames.Client.Photon.Hashtable { { RoomProperties.playerSlotKey, playerSlots } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(newPlayerSlots);
    }

    private void InitMaster()
    {
        roomUIManager.MapDropdown.onValueChanged.AddListener(OnMapChanged);
        roomUIManager.MapDropdown.interactable = true;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerProperties.indexKey, out object index))
        {
            int playerIndex = (int)index;   
            roomUIManager.ChangeMasterPlayer(playerIndex);
        }
        else
        {
            Debug.LogError("LocalPlayer doesnt have indexKey CustomProperties.");
        }
    }
    #endregion

    #region Player Datas
    private void AddPlayerToData(Photon.Realtime.Player newPlayer, int index)
    {// 마스터가 처리하는 플레이어 추가
        PhotonPlayerData.Instance.AddPlayer(newPlayer.UserId, index);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("AddPlayerUI", RpcTarget.AllBuffered, newPlayer.UserId, index);
        }
    }

    private void RemovePlayerFromData(Photon.Realtime.Player otherPlayer)
    {// 마스터가 처리하는 플레이어 제거
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonPlayerData.Instance.PlayerIdDict.Remove(otherPlayer.UserId);
            int playerInGameId = PhotonPlayerData.Instance.PlayerIdDict[otherPlayer.UserId];
            photonView.RPC("RemovePlayerUI", RpcTarget.AllBuffered, playerInGameId);
        }
    }

    private int AssignPlayerSlot()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperties.playerSlotKey, out object value))
        {
            bool[] playerSlots = (bool[])value;

            for (int i = 0; i < playerSlots.Length; i++)
            {
                if (!playerSlots[i])
                {
                    playerSlots[i] = true;

                    ExitGames.Client.Photon.Hashtable newPlayerSlots
                        = new ExitGames.Client.Photon.Hashtable() { { RoomProperties.playerSlotKey, playerSlots } };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(newPlayerSlots);

                    SetPlayerIndex(i);
                    return i;
                }
            }
            Debug.LogError("PlayerSlot out of index.");
            return -1;
        }
        Debug.LogError("Cant find playerSlotKey in CurrentRoom CustomProperties.");
        return -1;
    }

    private void SetPlayerIndex(int index)
    {
        ExitGames.Client.Photon.Hashtable newPlayerIndex
                                = new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.indexKey, index } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(newPlayerIndex);
    }
    #endregion

    #region AllBuffered PunRPC 
    [PunRPC]
    private void AddPlayerUI(string userId, int index)
    {//들어온 사람 정보 표시.
        roomUIManager.ShowConnectedPlayer(userId, index);
    }

    [PunRPC]
    private void RemovePlayerUI(int index)
    {//나간 사람 UI 숨김.
        roomUIManager.HideDisconnectedPlayer(index);
    }

    [PunRPC]
    private void UpdateMapDropdown(int value)
    {//마스터가 맵을 바꿀 때.
        roomUIManager.MapDropdown.value = value;
    }

    [PunRPC]
    private void StartCountDown()
    {// 마스터가 인원 확인하고 시작 카운트 다운
        roomUIManager.ShowStartCounter();
        gameStartCoroutine = StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {// 게임 시작 코루틴.
        int timer = roomUIManager.StartCount;

        if (PhotonNetwork.IsMasterClient)
        {
            roomUIManager.SkipBtnInteractable(true);
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
    private void StopCountDown()
    {
        if (gameStartCoroutine != null)
        {
            StopCoroutine(gameStartCoroutine);
            roomUIManager.HideStartCounter();
        }
    }
    #endregion

    #region Master PunRPC
    [PunRPC]
    private void RequestCheckReady()
    {//모든 사람이 준비완료 되었는지 확인.
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue(PlayerProperties.readyKey, out object isReady))
            {
                if (!(bool)isReady) 
                {
                    Debug.Log("준비 취소");
                    return; //준비 아님.
                } 
            }
            else
            {//버튼 누른 적 없음.
                Debug.Log("준비 안 누른 사람 있음.");
                return;
            } 
        }
        // 모든 플레이어가 준비된 상태라면 게임 시작
        photonView.RPC("StartCountDown", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RequestStopGame()
    {//준비 카운트 정지.
        roomUIManager.SkipBtnInteractable(false);
        photonView.RPC("StopCountDown", RpcTarget.AllBuffered);
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
                    roomUIManager.PhotonLoadLevel("CharacterAnimeTestScene COPY");
                    break;
                case 1:
                    roomUIManager.PhotonLoadLevel("GameScene");
                    break;
                case 2:
                    roomUIManager.PhotonLoadLevel("ItemTest");
                    break;
                default:
                    Debug.LogError("Selected Map Option is out of index.");
                    break;
            }
        }
    }

    private void OnMapChanged(int value)
    {//MapDropdown
        if (!PhotonNetwork.IsMasterClient) //비상용. 팀원과 테스트해보고 삭제할것.
            Debug.LogError("Only Master could interact MapDropdown. Edit required.");

        photonView.RPC("UpdateMapDropdown", RpcTarget.OthersBuffered, value);
    }

    public void OnReadyButtonClicked()
    { //ReadyBtn
        bool isReady;

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerProperties.readyKey))
        {//등록된 적 있으면 그 상태 불러오기.
            isReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties[PlayerProperties.readyKey];
        }
        else
        {//없으면 레디한 적이 없다고 간주.
            isReady = false;
        }
        isReady = !isReady; //Ready토글.

        // 준비 상태 업데이트
        ExitGames.Client.Photon.Hashtable newReadyKey
            = new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.readyKey, isReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(newReadyKey);

        StartCoroutine(ReadyStateDelay(isReady));
    }

    private IEnumerator ReadyStateDelay(bool isReady)
    {
        yield return new WaitForSeconds(0.3f);

        // 마스터에게 상태 확인 요청
        if (isReady)
        {
            photonView.RPC("RequestCheckReady", RpcTarget.MasterClient);
        }
        else
        {
            photonView.RPC("RequestStopGame", RpcTarget.MasterClient);
        }
    }

    public void LeaveRoom()
    {//exitBtn
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    // 게스트 플레이어가 추가되면 해당 플레이어의 id를 표시하고 인원수가 다 차면 게임 10초 뒤에 시작
    {
        if (!PhotonNetwork.IsMasterClient) { return; }

        base.OnPlayerEnteredRoom(newPlayer);

        AddPlayerToData(newPlayer, AssignPlayerSlot());

    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    // 게스트 플레이어가 도중에 나가면 게임 시작을 취소하고, 해당 플레이어는 화면과 데이터상 목록에서 제외
    {
        StopCountDown();
        RemovePlayerFromData(otherPlayer);
        
        if (!PhotonNetwork.IsMasterClient) { return; }

        roomUIManager.SkipBtnInteractable(false);
	}

    public override void OnLeftRoom()
    {//나가는 사람용.
        roomUIManager.LoadScene("StartScene");
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (!PhotonNetwork.IsMasterClient) { return; }

        base.OnMasterClientSwitched(newMasterClient);
        InitMaster();
    }
    #endregion
}
