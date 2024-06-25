using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : Singleton<CustomSceneManager>
{
    [SerializeField]private GameObject loadingPrefab;
    [HideInInspector]public GameObject loadingPanel;

    #region MonobehaviourCallbacks
    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();
        loadingPanel = Instantiate(loadingPrefab, transform);
        loadingPanel.SetActive(false);
    }
    #endregion

    #region LoadPanel
    public void ShowLoadPanel()
    {
        loadingPanel.SetActive(true);
    }

    public void HideLoadPanel()
    {
        loadingPanel.SetActive(false);
    }
    #endregion

    #region Scene Load & Unload
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        ShowLoadPanel();
        Debug.Log("LoadSceneAsync show load.");

        // 비동기 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        HideLoadPanel();
    }

    public void PhotonLoadLevel(string sceneName)
    {
        StartCoroutine(PhotonLoadLevelAsync(sceneName));
    }

    private IEnumerator PhotonLoadLevelAsync(string sceneName)
    {
        ShowLoadPanel();
        Debug.Log("photonloadlevel show load.");

        // 비동기 씬 로드
        PhotonNetwork.LoadLevel(sceneName);
        while (PhotonNetwork.LevelLoadingProgress < 1.0f)
        {
            yield return null;
        }
        HideLoadPanel();
    }
    #endregion
}
