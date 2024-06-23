using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private RoomUIManager roomUIManager;

	#region Private Field
	private Coroutine gameStartCoroutine;
    #endregion

    #region MonoBehaviour Callbacks
    private void Start()
	{
        if(!PhotonNetwork.IsMasterClient) { return; }

        //마스터 전용.
        AddPlayerToData(PhotonNetwork.LocalPlayer);
        roomUIManager.MapDropdown.onValueChanged.AddListener(OnMapChanged);
    }
	#endregion

	#region Private Methods
	
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

	private void AddPlayerToData(Photon.Realtime.Player newPlayer)
    {// 마스터가 처리하는 플레이어 추가
        PhotonPlayerData.Instance.AddPlayer(newPlayer.UserId, newPlayer.ActorNumber);

		int playerInGameId = PhotonPlayerData.Instance.PlayerIdDict[newPlayer.UserId];
		photonView.RPC("MakePlayerListContent", RpcTarget.AllBuffered, playerInGameId);
	}

	
	private void RemovePlayerFromData(Photon.Realtime.Player otherPlayer)
    {// 마스터가 처리하는 플레이어 제거
        StopCoroutine(gameStartCoroutine);
		roomUIManager.HideStartCounter();
		PhotonPlayerData.Instance.PlayerIdDict.Remove(otherPlayer.UserId);

		int playerInGameId = PhotonPlayerData.Instance.PlayerIdDict[otherPlayer.UserId];
		if (PhotonNetwork.IsMasterClient)
		{
			photonView.RPC("RemovePlayerListContent", RpcTarget.AllBuffered, playerInGameId);
		}
	}

	[PunRPC]
	private void StartCountDown()
    {// 마스터가 인원 확인하고 시작 카운트 다운
        Debug.Log($"유저 {PhotonPlayerData.Instance.MaxNumberOfPlayers}명 모임, 10초 뒤 시작");
		roomUIManager.ShowStartCounter();
		gameStartCoroutine = StartCoroutine(StartGame());
	}

    private void OnMapChanged(int value)
    {
        if (!PhotonNetwork.IsMasterClient) { return; }

        photonView.RPC("UpdateMapDropdown", RpcTarget.OthersBuffered, value);
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
    public void LeaveRoom()
    {//exitBtn
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {// 게스트 플레이어가 추가되면 해당 플레이어의 id를 표시하고 인원수가 다 차면 게임 10초 뒤에 시작
        if (!PhotonNetwork.IsMasterClient) { return; }

        base.OnPlayerEnteredRoom(newPlayer);
        AddPlayerToData(newPlayer);
        // 인원이 다 차면 게임 시작 카운트 다운
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonPlayerData.Instance.MaxNumberOfPlayers)
        {
            photonView.RPC("StartCountDown", RpcTarget.AllBuffered);
        }
    }

	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    // 게스트 플레이어가 도중에 나가면 게임 시작을 취소하고, 해당 플레이어는 화면과 데이터상 목록에서 제외
    {
        if (gameStartCoroutine != null)
        {
            StopCoroutine(gameStartCoroutine);
            roomUIManager.GetSkipBtn().interactable = false;
        }

        if (!PhotonNetwork.IsMasterClient) { return; }

		base.OnPlayerLeftRoom(otherPlayer);
		RemovePlayerFromData(otherPlayer);
	}

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (!PhotonNetwork.IsMasterClient) { return; }

        base.OnMasterClientSwitched(newMasterClient);

        roomUIManager.ShowSkipBtn();
        roomUIManager.MapDropdown.interactable = true;
        roomUIManager.MapDropdown.onValueChanged.AddListener(OnMapChanged);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0); //StartScene으로 이동.
    }
    #endregion
}
