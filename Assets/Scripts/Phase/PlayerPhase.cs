using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPhase
{
    [Header("플레이어 정보")]
    public string playerCode;

    [Header("플레이어 레이스 정보")]
    public int playerRank;
    public int racePhase;
    public bool[] checkPoints;
    public float[] raceTime;
}
