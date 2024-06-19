using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartBtns : MonoBehaviour
{
    private enum StartBtnType
    {
        Host,
        Join,
        Setting,
        Exit
    }

    private List<Button> StartBtns;

    private void Awake()
    {
        StartBtns = new List<Button>();
        foreach (Transform child in transform)
        {
            StartBtns.Add(child.GetComponent<Button>());
        }
    }

    private void Start()
    {
        StartBtns[(int)StartBtnType.Host].onClick.AddListener(HostRoom);
        StartBtns[(int)StartBtnType.Join].onClick.AddListener(FindRoom);
        StartBtns[(int)StartBtnType.Setting].onClick.AddListener(OpenSetting);
        StartBtns[(int)StartBtnType.Exit].onClick.AddListener(ApplicationExit);
    }

    private void HostRoom() { }

    private void FindRoom() { }

    private void OpenSetting() { }

    private void ApplicationExit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
