using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    private enum SettingBtnType
    {
        ScreenMode,
        KeySettings,
        Credit
    }

    private List<Button> SettingBtns;

    private TextMeshProUGUI screenModeTxt;
    private bool isFullScreen;

    private void Awake()
    {
        SettingBtns = new List<Button>();
        foreach (Transform child in transform.Find("Content").Find("SettingBtns"))
        {
            SettingBtns.Add(child.GetComponent<Button>());  
        }
        screenModeTxt = SettingBtns[(int)SettingBtnType.ScreenMode].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        SettingBtns[(int)SettingBtnType.ScreenMode].onClick.AddListener(ToggleScreenMode);
        SettingBtns[(int)SettingBtnType.KeySettings].onClick.AddListener(ShowKeyPanel);
        SettingBtns[(int)SettingBtnType.Credit].onClick.AddListener(ShowCredit);

        //TODO: 위치이동?
        /*창 모드로 시작.*/
        Screen.fullScreenMode = FullScreenMode.Windowed;
        screenModeTxt.text = "창 모드";
        isFullScreen = false;

        transform.parent.gameObject.SetActive(false);
    }

    //TODO: GameManager 또는 CustomSceneManager등으로 위치를 옮기기.
    private void ToggleScreenMode() 
    {
        if (isFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            screenModeTxt.text = "풀스크린";
            isFullScreen = false;

#if UNITY_EDITOR
            System.Type gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            EditorWindow gameView = EditorWindow.GetWindow(gameViewType);
            gameView.maximized = true;
#endif
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            screenModeTxt.text = "창 모드";
            isFullScreen = true;

#if UNITY_EDITOR
            System.Type gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            EditorWindow gameView = EditorWindow.GetWindow(gameViewType);
            gameView.maximized = false;
#endif
        }
    }

    private void ShowKeyPanel()
    {
        //1단계: Key 가이드.
        //2단계: Rebinding 기능. => 여기까지 하게 된다면 역시나 InputManager등에서 불러오기 필요.
    }

    private void ShowCredit()
    {
        //팀 소개. 
        //사용한 에셋? 소개.
        //스파르타 로고...?
        //Thank you for playing?
    }
}
