using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : Singleton<CustomSceneManager>
{
    public enum SceneNames
    {
        StartScene,
        PhotonLobbyScene,
        RoomScene,
        GameScene
    }

    public GameObject loadingCanvas;
    
    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();

    }
    
    private void OnEnable()
    {
        // 씬 로드 이벤트에 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 씬 로드 이벤트에서 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region Scene Load & Unload
    public void LoadScene(SceneNames scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    private IEnumerator LoadSceneAsync(SceneNames scene)
    {
        loadingCanvas.SetActive(true);
        
        // 비동기 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadingCanvas.SetActive(false);
    }

    public void UnloadScene(SceneNames scene)
    {
        StartCoroutine(UnloadSceneAsync(scene));
    }

    private IEnumerator UnloadSceneAsync(SceneNames scene)
    {
        // 씬 언로드 비동기 실행
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene.ToString());
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }
    #endregion

    #region Scene Init
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Enum.TryParse(scene.name, out SceneNames sceneName))
        {
            switch (sceneName)
            {
                case SceneNames.StartScene:
                    InitializeStartScene();
                    break;
                case SceneNames.PhotonLobbyScene:
                    InitializeLobbyScene();
                    break;
                case SceneNames.RoomScene:
                    InitializeRoomScene();
                    break;
                case SceneNames.GameScene:
                    InitializeGameScene();
                    break;
            }
        }
        
    }
    private void InitializeStartScene()
    {
        // Start 씬 초기화 작업
    }

    private void InitializeLobbyScene()
    {
        // Lobby 씬 초기화 작업
    }

    private void InitializeRoomScene()
    {
        // Room 씬 초기화 작업
    }

    private void InitializeGameScene()
    {
        // Game 씬 초기화 작업
    }
    #endregion
}