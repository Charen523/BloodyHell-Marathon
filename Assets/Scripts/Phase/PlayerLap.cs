using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLap
{
    [Header("플레이어 정보")]
    public string playerCode;

    [Header("플레이어 레이스 정보")]
    public int playerRank;
    public int playerScore;
    public int raceLap;
    public int currentPoint = 0;
    public bool[] checkPoints;
    public bool retire = true;
}
