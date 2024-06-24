using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseUIManager : Singleton<BaseUIManager>
{
    [SerializeField]private GameObject loadingPrefab;
    [HideInInspector]public GameObject loadingPanel;

    #region MonobehaviourCallbacks
    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();
        loadingPanel = Instantiate(loadingPrefab);
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
        loadingPanel.SetActive(true);

        // 비동기 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadingPanel.SetActive(false);
    }
    #endregion
}
