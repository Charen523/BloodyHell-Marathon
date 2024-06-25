using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIManager : Singleton<RoomUIManager>
{
    #region SerializeField
    [Header("Room Gameobjects")]
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TMP_Dropdown mapDropdown;
    [SerializeField] public GameObject skipBtn;
    private List<GameObject> readyBtns = new List<GameObject>();

    [Header("PlayerList")]
    [SerializeField] private GameObject[] joinSlots;
    [SerializeField] private Transform playerListParent;

    [Header("GameStart Counter")]
    [SerializeField] private GameObject startCounterBG;
    [SerializeField] private TextMeshProUGUI startCounterTxt;
    [SerializeField] private int startCount = 10;
    #endregion

    #region public Variables
    public int StartCount => startCount;
    public TMP_Dropdown MapDropdown => mapDropdown;
    #endregion

    #region Monobehaviour Callbacks

    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();
    }
    private void Start()
    {
        roomName.text = "Room. " + PhotonNetwork.CurrentRoom.Name.Substring(0, 4);

        startCounterBG.SetActive(false);
        SkipBtnInteractable(false);
        
        for (int i = 0; i < joinSlots.Length; i++)
        {
            joinSlots[i].SetActive(false);
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            skipBtn.SetActive(false);
            mapDropdown.interactable = false;
        }

        foreach (GameObject obj in joinSlots)
        {
            GameObject button = obj.transform.Find("ReadyBtn").gameObject;
            readyBtns.Add(button);
        }
    }
    #endregion

    public void SkipBtnInteractable(bool isInteractable)
    {
        skipBtn.GetComponent<Button>().interactable = isInteractable;
    }

    public void UpdateStartCounter(int timer)
    {
        startCounterTxt.SetText($"전원 준비완료!\n게임 시작 {timer}초 전...");
    }

    public void ShowStartCounter()
    {
        startCounterBG.SetActive(true);
    }

    public void HideStartCounter()
    {
        startCounterBG.SetActive(false);
    }

    [PunRPC]
    public void ShowConnectedPlayer(string UserId, int index)
    {
        joinSlots[index].SetActive(true);
        TextMeshProUGUI tmp = joinSlots[index].transform.Find("PlayerIdTxt").GetComponent<TextMeshProUGUI>();

        string shortUserId = UserId.Substring(0, 7);
        tmp.SetText(shortUserId);
    }

    [PunRPC]
    public void HideDisconnectedPlayer(int index)
    {
        joinSlots[index].SetActive(false);
    }

    [PunRPC]
    public void ChangeMasterPlayer(int index)
    {
        for (int i = 0; i < joinSlots.Length; i++)
        {
            TextMeshProUGUI tmp = joinSlots[i].transform.Find("PlayerNumTxt").GetComponent<TextMeshProUGUI>();

            if (i == index)
            {
                tmp.text = "방 장:";
            }
            else
            {
                tmp.text = "룸메이트:";
            }
        }
    }

    [PunRPC]
    public void ChangeReadyUI(int index, bool isReady)
    {
        GameObject changeButton = readyBtns[index];

        if (isReady)
        {
            changeButton.GetComponent<Image>().color = new Color(127 / 255f, 255 / 255f, 111 / 255f);
            changeButton.GetComponentInChildren<TMP_Text>().text = "준비완료!";
        }
        else
        {
            changeButton.GetComponent<Image>().color = new Color(255 / 255f, 107 / 255f, 32 / 255f);
            changeButton.GetComponentInChildren<TMP_Text>().text = "준비중";
        }
    }

}
