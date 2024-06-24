using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankSlot : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI userRankTxt;
    [SerializeField]
    private TextMeshProUGUI userNameTxt;
    [SerializeField]
    private TextMeshProUGUI userScoreTxt;
    [SerializeField]
    private Animator rankAnime;
    [SerializeField]
    private Image victoryStandImg;

    public void Init(SlotData data)
    {
        userRankTxt.text = data.userRank;
        userNameTxt.text = data.userName;
        userScoreTxt.text = data.userScore;
        rankAnime.runtimeAnimatorController = data.rankAnime;
        rankAnime.SetTrigger(data.animeParam);
        if (victoryStandImg != null)
        {
            victoryStandImg.gameObject.SetActive(true);
            victoryStandImg.sprite = data.victoryStand;
        }
    }
}
