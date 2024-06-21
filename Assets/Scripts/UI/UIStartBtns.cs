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
        //SceneManager.LoadScene(1); //로비씬으로. CustomSceneManager으로 이동필요.
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
        //1단계. 이미지 슬라이드로 보여주기.
        //2단계. gif 슬라이드로 보여주기.
        //3단계. 튜토리얼 씬으로 넘어가 user가 직접 테스트해볼 수 있게 하기.
    }
}
