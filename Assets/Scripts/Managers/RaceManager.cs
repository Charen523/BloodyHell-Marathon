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
    //1등 골인 후 대기시간
    [SerializeField]
    private int waitTime = 10;
    [SerializeField]
    private int checkPointScore;

    protected override void Awake()
    {
        canDestroyOnLoad = false;
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LastCheckPoint(PlayerLap player)
    {
        for(int i = 0; i < player.checkPoints.Length; i++) 
        {
            player.checkPoints[i] = false;
        }
        player.raceLap++;
    }

    public void PassedCheckPoint(int index, PlayerLap player)
    {
        if (!player.checkPoints[index])
        {
            player.checkPoints[index] = true;
            player.playerScore += checkPointScore;
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
        rankPopUp.SetActive(true);
    }
}
