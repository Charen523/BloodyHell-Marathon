using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class RaceManager : Singleton<RaceManager>
{
    //레이스 플레이 여부
    public bool isRacePlaying = false;
    //랭킹 팝업 띄우기
    [SerializeField]
    private GameObject rankPopUp;

    //첫번째로 도착한 유저 여부
    public bool isFirst = true;
    //플레이어 랭킹
    private Dictionary<int, PlayerLap> dicRank = new Dictionary<int, PlayerLap>();
    //플레이어정보 
    public Dictionary<string, PlayerLap> dicPlayer = new Dictionary<string, PlayerLap>();
    [SerializeField]
    private List<PlayerLap> racers = new List<PlayerLap>();
    //랭킹 순위
    private int rankIndex = 0;

    //트랙 정보
    public Track track;

    //1등 골인 후 대기시간
    [SerializeField]
    private int waitTime = 10;
    [SerializeField]
    private Image countdownBar;
    [SerializeField]
    private TextMeshProUGUI countdownTxt;
    [SerializeField]
    private GameObject countdown;

    public UnityAction<PlayerLap> OnLastCheckPoint;
    public UnityAction<int, PlayerLap> OnCheckPoint;
    public UnityAction<Dictionary<int, PlayerLap>> OnShowRank;

    private PhotonShowBoard showBoard;
    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        OnLastCheckPoint += LastCheckPoint;
        OnCheckPoint += OnCheckPoint;
        InitData();
        var keys = dicPlayer.Keys;
        foreach (string key in keys)
        {
            dicPlayer[key].checkPoints = new bool[track.checkPoint.Length];
        }
    }

    public void InitData()
    {
        Dictionary<string, int>  dicPlayerId = PhotonPlayerData.Instance.PlayerIdDict;
        var keys = dicPlayerId.Keys;
        foreach (string key in keys)
        {
            string strKey = key.Substring(0, 7);
            dicPlayer.Add(strKey, new PlayerLap());
            dicPlayer[strKey].playerCode = strKey;
            dicPlayer[strKey].color = dicPlayerId[key];
            racers.Add(dicPlayer[strKey]);
        }
    }

    public void LastCheckPoint(PlayerLap player)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        for (int i = 1; i < player.checkPoints.Length; i++) 
        {
            player.checkPoints[i] = false;
        }
        player.raceLap++;
        player.currentPoint = 0;
        dicPlayer[player.playerCode].playerScore += track.checkPointScore[track.checkPointScore.Length - 1];
        if (dicPlayer[player.playerCode].playerScore >= track.finalScore)
        {
            GoalIn(player);
        }
    }

    public void PassedCheckPoint(int index, PlayerLap player)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!player.checkPoints[index])
        {
            player.checkPoints[index] = true;
            player.currentPoint = index;
            dicPlayer[player.playerCode].playerScore += track.checkPointScore[index];
        }
    }

    [PunRPC]
    public void GoalIn(PlayerLap player)
    {
        if (isFirst)
        {
            isFirst = false;
            showBoard.ShowBoard();
            //StartCoroutine(CountDown());
        }

        racers.Remove(player);
        dicRank.Add(++rankIndex, player);
        dicRank[rankIndex].playerRank = rankIndex;
        dicRank[rankIndex].retire = false;
    }



    public IEnumerator CountDown()
    {
        int currentTime = waitTime;
        countdown.SetActive(true);
        for (int i = currentTime; i > 0; i--)
        {
            
            countdownTxt.text = currentTime.ToString();
            countdownBar.fillAmount = (float)currentTime / waitTime;
            yield return new WaitForSeconds(1);
        }
        isRacePlaying = false;
        yield return null;
        for (int i = 0; i < racers.Count; i++)
        {
            dicRank.Add(++rankIndex, racers[i]);
            dicRank[rankIndex].playerRank = rankIndex;
            dicRank[rankIndex].retire = true;
            racers.Remove(racers[i]);
        }
        //전체 게임 정지 및 결과화면
        Time.timeScale = 0;
        rankPopUp.SetActive(true);
        OnShowRank?.Invoke(dicRank);
    }

    
}
