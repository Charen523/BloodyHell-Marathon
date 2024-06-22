using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Dictionary<int, string> dicRank = new Dictionary<int, string>();
    //플레이어정보 
    public Dictionary<string, PlayerLap> dicPlayer = new Dictionary<string, PlayerLap>();
    //랭킹 순위
    private int rankIndex = 0;

    //트랙 정보
    public Track track;
    public Player playeri;

    //1등 골인 후 대기시간
    [SerializeField]
    private int waitTime = 10;

    protected override void Awake()
    {
        dontDestroyOnLoad = false;
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        //나중에 멀티 정보 받아와서 딕셔너리에 플레이어들 이름넣기
        dicPlayer.Add("testPlayer", playeri.playerlap);
        var keys = dicPlayer.Keys;
        foreach (string key in keys)
        {
            dicPlayer[key].checkPoints = new bool[track.checkPoint.Length];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LastCheckPoint(PlayerLap player)
    {
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
        if (!player.checkPoints[index])
        {
            player.checkPoints[index] = true;
            player.currentPoint = index;
            dicPlayer[player.playerCode].playerScore += track.checkPointScore[index];
        }
    }

    public void GoalIn(PlayerLap player)
    {
        if (isFirst)
        {
            isFirst = false;
            StartCoroutine(CountDown());
            dicRank.Add(++rankIndex, player.playerCode);
        }
        else if (isRacePlaying)
        {
            dicRank.Add(++rankIndex, player.playerCode);
        }
        else
        {
            //플레이어 리타이어
        }
    }

    private IEnumerator CountDown()
    {
        for (int i = waitTime; i > 0; i--)
        {
            //TODO : 남은시간 보여주는 텍스트이미지 및 애니메이션효과
            // = waitTime.ToString();
            yield return new WaitForSeconds(1);
        }
        isRacePlaying = false;
        //TODO : 전체 게임 정지 및 결과화면
        rankPopUp.SetActive(true);
    }
}
