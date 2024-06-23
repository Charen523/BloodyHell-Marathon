using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIManager : MonoBehaviourPun
{
    #region SerializeField
    [Header("Room Gameobjects")]
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TMP_Dropdown mapDropdown;
    [SerializeField] private GameObject ExitBtn;
    [SerializeField] private GameObject skipBtn;

    [Header("PlayerList")]
    [SerializeField] private GameObject playerListContentPrefab;
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

    private void Start()
    {
        roomName.text = "Room. " + PhotonNetwork.CurrentRoom.Name.Substring(0, 4);

        startCounterBG.SetActive(false);
        GetSkipBtn().interactable = false;

        if (!PhotonNetwork.IsMasterClient)
        {
            skipBtn.SetActive(false);
            mapDropdown.interactable = false;
        }
    }
    #endregion

    public Button GetSkipBtn()
    {
        return skipBtn.GetComponent<Button>();
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
    public void MakePlayerListContent(int id)
    {
        GameObject newListContent = Instantiate(playerListContentPrefab, playerListParent);
        TextMeshProUGUI tmp = newListContent.GetComponentInChildren<TextMeshProUGUI>();
        tmp.SetText($"유저 Id : {id}");
    }

    [PunRPC]
    public void RemovePlayerListContent(int id)
    {
        foreach (Transform child in playerListParent)
        {
            TextMeshProUGUI tmp = child.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null && tmp.text.Contains(id.ToString()))
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    [PunRPC]
    public void UpdateMapDropdown(int value)
    {
        mapDropdown.value = value;
    }

    private void OnMapDropdownChanged(int value)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateMapDropdown", RpcTarget.OthersBuffered, value);
        }
    }
}
