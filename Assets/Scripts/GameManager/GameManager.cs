using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //레이스 플레이 여부
    public bool isRacePlaying = false;
    //랭킹 팝업 띄우기
    [SerializeField]
    private GameObject rankPopUp;
    //첫번째로 도착한 유저 여부
    public bool isFirst = true;
    //완주한 플레이어 랭킹
    public Dictionary<int, string> dicRank = new Dictionary<int, string>();
    //랭킹 순위
    private int rankIndex = 0;
    //1등 골인 후 대기시간
    [SerializeField]
    private int waitTime = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoalIn(Player player)
    {
        if (isFirst)
        {
            isFirst = false;
            StartCoroutine(CountDown());
            dicRank.Add(++rankIndex, player.name);
        }
        else if(isRacePlaying) 
        {
            dicRank.Add(++rankIndex, player.name);
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
