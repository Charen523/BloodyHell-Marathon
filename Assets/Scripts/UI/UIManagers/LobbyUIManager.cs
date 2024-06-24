using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : Singleton<LobbyUIManager>
{
    #region Serialize Field
    [Header("Room List")]
    [SerializeField] private ToggleGroup roomToggleGroup;
    [SerializeField] private Transform roomListParent;
    [SerializeField] private GameObject roomListPrefab;
    #endregion

    [HideInInspector] public int roomCount;
    private Dictionary <string, GameObject> roomList;

    #region MonobehaviourCallbacks
    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();
        roomList = new Dictionary<string, GameObject>();
    }
    #endregion

    public void MakeNewRoomList(string name, int playerCount, int maxPlayers)
    {//LobbyManager에서  RoomList 추가.
        GameObject roomPrefab = Instantiate(roomListPrefab, roomListParent);

        TMP_Text roomName = roomPrefab.transform.Find("RoomIdTxt").GetComponent<TMP_Text>();
        roomName.text = name.Substring(0, 4);

        TMP_Text currentPlayer = roomPrefab.transform.Find("CurrentPlayerNum").GetComponent<TMP_Text>();
        currentPlayer.text = playerCount.ToString();

        TMP_Text maxPlayer = roomPrefab.transform.Find("MaxPlayerNum").GetComponent<TMP_Text>();
        maxPlayer.text = $"참여인원:    / {maxPlayers}";

        roomToggleGroup.RegisterToggle(roomPrefab.GetComponent<Toggle>());
        roomList.Add(name, roomPrefab);
    }

    public void DeleteRoomList(string name)
    {
        if (roomList.ContainsKey(name))
        {
            Destroy(roomList[name]);
            roomList.Remove(name);
        }
    }

    public int GetSelectedToggle()
    {
        var selectedToggle = roomToggleGroup.GetFirstActiveToggle();

        if (int.TryParse(selectedToggle.name, out int index)) { return index; }

        Transform parent = selectedToggle.transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i) == selectedToggle.transform)
                return i + 4;
        }

        Debug.LogError($"SelectedToggle not in ToggleIndex: {selectedToggle.name}");
        return -1;
    }
    public void LoadStartScene()
    {
        CustomSceneManager.Instance.LoadScene("StartScene");
    }
}


