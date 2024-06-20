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

        //TODO: ��ġ�̵�?
        /*â ���� ����.*/
        Screen.fullScreenMode = FullScreenMode.Windowed;
        screenModeTxt.text = "â ���";
        isFullScreen = false;

        transform.parent.gameObject.SetActive(false);
    }

    //TODO: GameManager �Ǵ� CustomSceneManager������ ��ġ�� �ű��.
    private void ToggleScreenMode() 
    {
        if (isFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            screenModeTxt.text = "Ǯ��ũ��";
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
            screenModeTxt.text = "â ���";
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
        //1�ܰ�: Key ���̵�.
        //2�ܰ�: Rebinding ���. => ������� �ϰ� �ȴٸ� ���ó� InputManager��� �ҷ����� �ʿ�.
    }

    private void ShowCredit()
    {
        //�� �Ұ�. 
        //����� ����? �Ұ�.
        //���ĸ�Ÿ �ΰ�...?
        //Thank you for playing?
    }
}
