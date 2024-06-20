using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    private List<Slider> volumeSliders;
    private List<GameObject> volumeIcons;
    public Sprite[] iconSprites;

    private List<Button> SettingBtns;
    private TextMeshProUGUI screenModeTxt;
    private bool isFullScreen;

    private void Awake()
    {
        volumeSliders = new List<Slider>();
        volumeIcons = new List<GameObject>();
        foreach (Transform child in transform.Find("Content").Find("Volumes"))
        {
            volumeSliders.Add(child.GetChild(0).GetComponent<Slider>());
            volumeIcons.Add(child.GetChild(1).gameObject);
        }

        SettingBtns = new List<Button>();
        foreach (Transform child in transform.Find("Content").Find("SettingBtns"))
        {
            SettingBtns.Add(child.GetComponent<Button>());  
        }
        screenModeTxt = SettingBtns[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        InitVolumeSlider();
        InitMusicIcons();
        InitSettingBtns();

        transform.parent.gameObject.SetActive(false);
    }

    private void InitVolumeSlider()
    {
        float masterVolume, bgmVolume, sfxVolume;

        /*슬라이더 이벤트 추가*/
        volumeSliders[0].onValueChanged.AddListener(SetMasterVolume);
        volumeSliders[1].onValueChanged.AddListener(SetBGMVolume);
        volumeSliders[2].onValueChanged.AddListener(SetSFXVolume);

        /*슬라이더 AudioManager와 동기화*/
        AudioManager.Instance.audioMixer.GetFloat("MasterVolume", out masterVolume);
        AudioManager.Instance.audioMixer.GetFloat("BGMVolume", out bgmVolume);
        AudioManager.Instance.audioMixer.GetFloat("SFXVolume", out sfxVolume);
        volumeSliders[0].value = Mathf.Pow(10, masterVolume / 20);
        volumeSliders[1].value = Mathf.Pow(10, bgmVolume / 20);
        volumeSliders[2].value = Mathf.Pow(10, sfxVolume / 20);

    }

    private void InitMusicIcons()
    {
        volumeIcons[0].GetComponent<Button>().onClick.AddListener(ToggleMasterMute);
        volumeIcons[1].GetComponent<Button>().onClick.AddListener(ToggleBGMMute);
        volumeIcons[2].GetComponent<Button>().onClick.AddListener(ToggleSFXMute);
    }

    private void InitSettingBtns()
    {
        SettingBtns[0].onClick.AddListener(ToggleScreenMode);
        SettingBtns[1].onClick.AddListener(ShowKeyPanel);
        SettingBtns[2].onClick.AddListener(ShowCredit);

        //TODO: 위치이동?
        /*창 모드로 시작.*/
        Screen.fullScreenMode = FullScreenMode.Windowed;
        screenModeTxt.text = "창 모드";
        isFullScreen = false;
    }

    private void SetMasterVolume(float volume)
    {
        float dB = Mathf.Log10(volume) * 20;
        AudioManager.Instance.SetMasterVolume(dB);
    }

    private void ToggleMasterMute()
    {
        AudioManager.Instance.ToggleMasterMute();
        ToggleIcon(0);
    }

    private void SetBGMVolume(float volume)
    {
        float dB = Mathf.Log10(volume) * 20;
        AudioManager.Instance.SetBGMVolume(dB);
    }

    private void ToggleBGMMute()
    {
        AudioManager.Instance.ToggleBGMMute();
        ToggleIcon(1);
    }

    private void SetSFXVolume(float volume)
    {
        float dB = Mathf.Log10(volume) * 20;
        AudioManager.Instance.SetSFXVolume(dB);
    }

    private void ToggleSFXMute()
    {
        AudioManager.Instance.ToggleSFXMute();
        ToggleIcon(2);
    }

    private void ToggleIcon(int index)
    {
        if(AudioManager.Instance.isMuted[index])
        {
            volumeIcons[index].GetComponent<Image>().sprite = iconSprites[0];
        }
        else
        {
            volumeIcons[index].GetComponent<Image>().sprite = iconSprites[1];
        }
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
        //Thank you for playing?DADAD
    }
}
