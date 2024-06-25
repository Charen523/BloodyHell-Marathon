using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Xml;
using ExitGames.Client.Photon;
using Photon.Pun.Demo.PunBasics;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem;

public class RaceManager : Singleton<RaceManager>, IPunObservable
{
    //레이스 플레이 여부
    public bool isRacePlaying = false;
    //랭킹 팝업 띄우기
    [SerializeField]
    private GameObject rankPopUp;

    //첫번째로 도착한 유저 여부
    public bool isFirst = true;
    //플레이어 랭킹
    public Dictionary<int, PlayerLap> dicRank = new Dictionary<int, PlayerLap>();
    //플레이어정보 
    public Dictionary<string, PlayerLap> dicPlayer = new Dictionary<string, PlayerLap>();
    [SerializeField]
    private List<PlayerLap> racers = new List<PlayerLap>();
    //랭킹 순위
    private int rankIndex = 0;

    private PhotonView pv;

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

    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();
        pv = GetComponent<PhotonView>();
        if (pv.ViewID == 0)
        {
            //pv.ViewID = 999;
        }
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
            dicPlayer[key].checkPoints[0] = true;
        }
    }

    public void InitData()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue(PlayerProperties.indexKey, out object value))
            {
                dicPlayer.Add(player.UserId, new PlayerLap());
                dicPlayer[player.UserId].playerCode = player.UserId;
                dicPlayer[player.UserId].color = (int)value;
                racers.Add(dicPlayer[player.UserId]);
            }
        }
    }

    public void LastCheckPoint(PlayerLap player)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        for (int i = 1; i < dicPlayer[player.playerCode].checkPoints.Length; i++) 
        {
            dicPlayer[player.playerCode].checkPoints[i] = false;
        }
        dicPlayer[player.playerCode].raceLap++;
        dicPlayer[player.playerCode].currentPoint = 0;
        dicPlayer[player.playerCode].playerScore += track.checkPointScore[track.checkPointScore.Length - 1];
        if (dicPlayer[player.playerCode].playerScore >= track.finalScore)
        {
            GoalIn(player);
        }
    }

    public void PassedCheckPoint(int index, PlayerLap player)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!dicPlayer[player.playerCode].checkPoints[index])
        {
            dicPlayer[player.playerCode].checkPoints[index] = true;
            dicPlayer[player.playerCode].currentPoint = index;
            dicPlayer[player.playerCode].playerScore += track.checkPointScore[index];
        }
    }

    public void GoalIn(PlayerLap player)
    {
        if (isFirst)
        {
            isFirst = false;
            //showBoard.ShowBoard();
            Debug.Log("False");
            pv.RPC("StartCounting", RpcTarget.AllBuffered);
        }
        racers.Remove(player);
        dicRank.Add(++rankIndex, player);
        dicRank[rankIndex].playerRank = rankIndex;
        dicRank[rankIndex].retire = false;

        // GoalIn 메서드를 RPC로 호출
        //pv.RPC("RPC_GoalIn", RpcTarget.OthersBuffered, player.playerCode);
    }

    //[PunRPC]
    //private void RPC_GoalIn(string playerCode)
    //{
    //    if (dicPlayer.ContainsKey(playerCode))
    //    {
    //        PlayerLap player = dicPlayer[playerCode];
    //        racers.Remove(player);
    //        dicRank.Add(++rankIndex, player);
    //        dicRank[rankIndex].playerRank = rankIndex;
    //        dicRank[rankIndex].retire = false;
    //    }
    //}

    [PunRPC]
    private void StartCounting()
    {
        Debug.Log("Start");
        StartCoroutine(CountDown());
    }

    public IEnumerator CountDown()
    {
        Debug.Log("Count");
        int currentTime = waitTime;
        countdown.SetActive(true);
        for (int i = currentTime; i > 0; i--)
        {            
            countdownTxt.text = i.ToString();
            countdownBar.fillAmount = (float)i / waitTime;
            yield return new WaitForSeconds(1);
        }
        countdownTxt.text = 0.ToString();
        countdownBar.fillAmount = 0;        
        yield return null;
        SetRetireUser();
        yield return new WaitForSeconds(1.0f);
        rankPopUp.SetActive(true);
        OnShowRank?.Invoke(dicRank);
        Debug.Log(dicRank.Count);
    }    

    private void SetRetireUser()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        isRacePlaying = false;
        for (int i = 0; i < racers.Count; i++)
        {
            dicRank.Add(++rankIndex, racers[i]);
            dicRank[rankIndex].playerRank = rankIndex;
            dicRank[rankIndex].retire = true;
            racers.Remove(racers[i]);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(dicRank.Count);
            foreach (var kvp in dicRank)
            {
                stream.SendNext(kvp.Key);
                stream.SendNext(kvp.Value.playerCode);
                stream.SendNext(kvp.Value.color);
                stream.SendNext(kvp.Value.playerScore);
                stream.SendNext(kvp.Value.playerRank);
                stream.SendNext(kvp.Value.retire);
            }
        }
        else
        {
            int count = (int)stream.ReceiveNext();
            dicRank.Clear();
            for (int i = 1; i < count + 1; i++)
            {
                int key = (int)stream.ReceiveNext();
                string code = (string)stream.ReceiveNext();
                int color = (int)stream.ReceiveNext();
                int score = (int)stream.ReceiveNext();
                int rank = (int)stream.ReceiveNext();
                bool retire = (bool)stream.ReceiveNext();
                dicRank.Add(key, new PlayerLap());
                dicRank[key].playerCode = code;
                dicRank[key].color = color;
                dicRank[key].playerScore = score;
                dicRank[key].playerRank = rank;
                dicRank[key].retire = retire;
            }
        }
    }
}
