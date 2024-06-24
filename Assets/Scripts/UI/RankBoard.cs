using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankBoard : MonoBehaviour
{
    [SerializeField]
    private AnimatorController[] characterAnimators;
    [SerializeField]
    private Sprite[] standColors;
    [SerializeField]
    private Button lobbyBtn;
    [SerializeField]
    private RankSlot[] rankSlot;
    [SerializeField]
    private string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        RaceManager.Instance.OnShowRank += ShowRanking;
    }

    public void ShowRanking(Dictionary<int, PlayerLap> dicRank)
    {
        for(int i = 1; i < dicRank.Count; i++) 
        { 
            if(dicRank.ContainsKey(i)) 
            {
                SlotData data = new SlotData();
                int index = CheckedColorIndex(dicRank[i].color);
                data.rankAnime = characterAnimators[index];
                data.userName = dicRank[i].playerCode;
                data.userScore = dicRank[i].playerScore.ToString();
                if (dicRank[i].playerRank <= 3 && !dicRank[i].retire)
                {
                    data.victoryStand = standColors[index];
                    data.animeParam = i.ToString();
                    data.userRank = i.ToString();
                }
                else if(!dicRank[i].retire)
                {
                    data.userRank = "-";
                    data.animeParam = "NotRetire";
                }
                else
                {
                    data.userRank = "-";
                    data.animeParam = "Retire";
                }
                rankSlot[i].Init(data);
            }
        }

    }

    public int CheckedColorIndex(PlayerColor color)
    {
        switch(color) 
        {
            case PlayerColor.A:
                return 0;
            case PlayerColor.B:
                return 1;
            case PlayerColor.C:
                return 2;
            case PlayerColor.D:
                return 3;
            case PlayerColor.E:
                return 4;
            default:
                Debug.Log("ColorIndex Error");
                return -1;
        }
    }

    public void ReturnWaitingRoom()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
