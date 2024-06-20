using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIStartBtns : MonoBehaviour
{
    private enum StartBtnType
    {
        Join,
        Lobby,
        Setting,
        Exit,
        Tutorial
    }

    private List<Button> StartBtns;
    private GameObject settingPanel;

    private void Awake()
    {
        StartBtns = new List<Button>();
        foreach (Transform child in transform)
        {
            StartBtns.Add(child.GetComponent<Button>());
        }

        settingPanel = FindSettingPanel();
    }

    private void Start()
    {
        StartBtns[(int)StartBtnType.Join].onClick.AddListener(JoinAnyRoom);
        StartBtns[(int)StartBtnType.Lobby].onClick.AddListener(CallLobbyScene);
        StartBtns[(int)StartBtnType.Setting].onClick.AddListener(ShowSetting);
        StartBtns[(int)StartBtnType.Exit].onClick.AddListener(ApplicationExit);
        StartBtns[(int)StartBtnType.Tutorial].onClick.AddListener(Tutorial);
    }

    private void JoinAnyRoom()
    {
        //EX) LobbyManager.instance.Connect(); 
        //EX) LobbyManager.instance.JoinRoom();
    }

    private void CallLobbyScene()
    {
        //EX) LobbyManager.instance.Connect();
        SceneManager.LoadScene(1); //LobbyScene.
    }

    private void ShowSetting()
    {
        settingPanel.SetActive(true);
    }

    private GameObject FindSettingPanel()
    {
        GameObject panel = transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject;
        if (panel != null)
        {
            return panel;
        }
        else
        {
            Debug.LogError("Couldn't Find Setting Panel...");
            return null;
        }
    }

    private void ApplicationExit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void Tutorial()
    {
        //1�ܰ�. �̹��� �����̵�� �����ֱ�.
        //2�ܰ�. gif �����̵�� �����ֱ�.
        //3�ܰ�. Ʃ�丮�� ������ �Ѿ user�� ���� �׽�Ʈ�غ� �� �ְ� �ϱ�.
    }
}
