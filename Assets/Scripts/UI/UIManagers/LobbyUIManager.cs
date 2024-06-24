using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    #region Serialize Field
    [Header("Room List")]
    [SerializeField] private ToggleGroup roomToggleGroup;
    [SerializeField] private Transform roomListParent;
    [SerializeField] private GameObject roomListPrefab;

    #endregion

    public void MakeCurrentRoom(string roomId, string masterId, int currentPlayerNum, int MaxPlayerNum)
    {
        GameObject roomList = Instantiate(roomListPrefab, roomListParent);
        
        TMP_Text roomName = roomList.transform.Find("RoomIdTxt").GetComponent<TMP_Text>();
        roomName.text = roomId;

        TMP_Text masterName = roomList.transform.Find("RoomIdTxt").GetComponent<TMP_Text>();
        masterName.text = masterId;

        TMP_Text currentPlayer = roomList.transform.Find("RoomIdTxt").GetComponent<TMP_Text>();
        currentPlayer.text = currentPlayerNum.ToString();

        TMP_Text maxPlayer = roomList.transform.Find("RoomIdTxt").GetComponent<TMP_Text>();
        maxPlayer.text = MaxPlayerNum.ToString();

        roomList.GetComponent<Toggle>().group = roomToggleGroup;
    }
}
