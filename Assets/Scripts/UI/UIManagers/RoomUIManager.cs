using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIManager : Singleton<RoomUIManager>
{
    #region SerializeField
    [Header("Room Gameobjects")]
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TMP_Dropdown mapDropdown;
    [SerializeField] private GameObject skipBtn;

    [Header("PlayerList")]
    [SerializeField] private GameObject[] joinSlots;
    [SerializeField] private Transform playerListParent;

    [Header("GameStart Counter")]
    [SerializeField] private GameObject startCounterBG;
    [SerializeField] private TextMeshProUGUI startCounterTxt;
    [SerializeField] private int startCount = 10;
    #endregion

    #region private Variables
    private bool[] playerSlots;
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

}
