using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public static class PlayerProperties
{//방 나가면 자동 삭제.
    public const string indexKey = "myIndex";
    public const string readyKey = "IsReady";
}

public static class RoomProperties
{//수동 삭제.
    public const string playerSlotKey = "PlayerSlots";
}

public class RoomManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private RoomUIManager roomUIManager;
    
    #region Private Field
    private bool isStarted = false;
    private bool isWaiting = false;
    private Coroutine gameStartCoroutine;
    private int myIndex = -2; //씬 바뀌는 순간 사라질 데이터.
    #endregion

    #region Monobehaviour Callbacks
    private IEnumerator Start()
    {
        if (PhotonNetwork.IsMasterClient && !isStarted)
        {
            isStarted = true;
            InitRoom();
            yield return new WaitUntil(() => !isWaiting);
            myIndex = ClosePlayerSlot();
            SetPlayerIndex(myIndex);
            AddPlayerToData(PhotonNetwork.LocalPlayer, myIndex); //Master의 PlayerData 추가.
            InitMaster(); //방장 권한.
            Debug.Log("정상시작");
        }
    }
    #endregion

    #region Init Data
    private void InitRoom()
    {
        /*방에 존재하는 플레이어 감지 프로퍼티 초기화.*/
        if (!PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperties.playerSlotKey, out object slotValue))
        {
            bool[] playerSlots = new bool[5];
            isWaiting = true;
            Debug.Log("방 생성 준비");
            ExitGames.Client.Photon.Hashtable newPlayerSlots
                = new ExitGames.Client.Photon.Hashtable { { RoomProperties.playerSlotKey, playerSlots } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(newPlayerSlots);
        }
    }

    private void InitMaster()
    {//처음 방에 입장할 때 + 방장 바뀔 때
        roomUIManager.MapDropdown.onValueChanged.AddListener(OnMapChanged);
        roomUIManager.MapDropdown.interactable = true;
        roomUIManager.skipBtn.SetActive(true);
        photonView.RPC("ChangeMasterUI", RpcTarget.AllBuffered, myIndex);
    }
    #endregion

    #region Player Data & Index
    private void AddPlayerToData(Photon.Realtime.Player newPlayer, int index)
    {//마스터 전용 메서드.
        Debug.Log($"New Player Added: index {index}");
        photonView.RPC("AddPlayerUI", RpcTarget.AllBuffered, newPlayer.UserId, index);
    }
    private int ClosePlayerSlot()
    {//AddPlayerToData 보조함수.
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

                    return i;
                }
            }
            Debug.LogError("PlayerSlot out of index.");
            return -1;
        }
        Debug.LogError("Cant find playerSlotKey in CurrentRoom CustomProperties.");
        return -1;
    }
    
    private void RemovePlayerFromData(Photon.Realtime.Player otherPlayer)
    {//마스터 전용 메서드.
        int playerIndex;
        if (otherPlayer.CustomProperties.TryGetValue(PlayerProperties.indexKey, out object value))
        {
            playerIndex = (int)value;
            photonView.RPC("UpdateReadyUI", RpcTarget.AllBuffered, playerIndex, false);
            OpenPlayerSlot(playerIndex);
            photonView.RPC("RemovePlayerUI", RpcTarget.AllBuffered, playerIndex);
        }
        else
        {
            Debug.Log("Cant found LocalPlayer PlayerProperties indexKey");
        }
    }
    private void OpenPlayerSlot(int index)
    {//RemovePlayerFromData 보조함수.
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperties.playerSlotKey, out object value))
        {
            bool[] playerSlots = (bool[])value;

            try
            {
                playerSlots[index] = false;

            }catch (IndexOutOfRangeException)
            {
                Debug.LogError("PlayerSlot out of index.");
            }
        }
        Debug.LogError("Cant find playerSlotKey in CurrentRoom CustomProperties.");
    }
    #endregion

    #region AllBuffered PunRPC 
    [PunRPC]
    private void ChangeMasterUI(int index)
    {
        roomUIManager.ChangeMasterPlayer(index);
    }

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
    private void UpdateReadyUI(int index, bool isReady)
    {
        roomUIManager.ChangeReadyUI(index, isReady);
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
            PhotonNetwork.CurrentRoom.IsVisible = false;    
            PhotonNetwork.CurrentRoom.IsOpen = false;
            roomUIManager.SkipBtnInteractable(true);
        }

        while (timer > 0)
        {
            roomUIManager.UpdateStartCounter(timer);
            yield return new WaitForSeconds(1f);
            timer--;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            LoadGameScene();
        }
    }

    [PunRPC]
    private void StopCountDown()
    {
        if (gameStartCoroutine != null)
        {
            StopCoroutine(gameStartCoroutine);
            roomUIManager.HideStartCounter();
        }

        if (!PhotonNetwork.IsMasterClient) return;

        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }


   
    #endregion

    #region Individual PunRPC
    [PunRPC]
    private void RequestChangeReadyUI(int index, bool isReady)
    {
        photonView.RPC("UpdateReadyUI", RpcTarget.AllBuffered, index, isReady);
    }

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

    [PunRPC]
    private void GivePlayerIndex(int index)
    {
        myIndex = index;
        Debug.Log($"My Index: {myIndex}");
        myIndex = index;
        SetPlayerIndex(myIndex);
    }

    private void SetPlayerIndex(int index)
    {//GivePlayerIndex 보조함수.
        ExitGames.Client.Photon.Hashtable newPlayerIndex
                                = new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.indexKey, index } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(newPlayerIndex);
    }
    #endregion

    #region UI Interacts
    public void OnSkipBtn()
    {
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        switch (roomUIManager.MapDropdown.value)
        {
            case 0:
                CustomSceneManager.Instance.PhotonLoadLevel("GameScene");
                break;
            case 1:
                CustomSceneManager.Instance.PhotonLoadLevel("GameScene2");
                break;
            default:
                Debug.LogError("Selected Map Option is out of index.");
                break;
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

        //내 버튼UI 바꾸라고 마스터에게 알리기.
        photonView.RPC("RequestChangeReadyUI", RpcTarget.MasterClient, myIndex, isReady);
        
        StartCoroutine(ReadyStateDelay(isReady));
    }

    private IEnumerator ReadyStateDelay(bool isReady)
    {
        yield return new WaitForSeconds(0.5f);
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
    {
        OpenPlayerSlot(myIndex);
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperties.playerSlotKey, out object slotValue))
        {
            isWaiting = false;
            Debug.Log("PlayerSlots 생김");
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    // 게스트 플레이어가 추가되면 해당 플레이어의 id를 표시하고 인원수가 다 차면 게임 10초 뒤에 시작
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int currentIndex = ClosePlayerSlot();
            AddPlayerToData(newPlayer, currentIndex);
            photonView.RPC("GivePlayerIndex", newPlayer, currentIndex);
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    // 게스트 플레이어가 도중에 나가면 게임 시작을 취소하고, 해당 플레이어는 화면과 데이터상 목록에서 제외
    {
        StopCountDown();
        RemovePlayerFromData(otherPlayer);
        
        if (!PhotonNetwork.IsMasterClient) { return; }

        roomUIManager.SkipBtnInteractable(false);
        if (PhotonNetwork.CurrentRoom.PlayerCount <= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
        RequestCheckReady();
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (!PhotonNetwork.IsMasterClient) { return; }

        InitMaster();

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue(PlayerProperties.indexKey, out object value))
            {
                int index = (int)value;
                photonView.RPC("AddPlayerUI", RpcTarget.AllBuffered, player.UserId, index);

                bool isReady;
                if (player.CustomProperties.TryGetValue(PlayerProperties.readyKey, out object value2))
                {
                    isReady = (bool)value2;
                    photonView.RPC("UpdateReadyUI", RpcTarget.AllBuffered, index, isReady);
                }
                else
                {
                    isReady = false;
                    photonView.RPC("UpdateReadyUI", RpcTarget.AllBuffered, index, isReady);
                }
            }
        }
    }
    #endregion
}
