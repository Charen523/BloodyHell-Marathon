using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : Singleton<CustomSceneManager>
{
    public enum SceneNames
    {
        Start,
        Lobby,
        Room,
        Game
    }

    protected override void Awake()
    {
        canDestroyOnLoad = false;
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

    private void Start()
    {
        LoadScene(SceneNames.Start);
    }

    #region Scene Load & Unload
    public void LoadScene(SceneNames scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    private IEnumerator LoadSceneAsync(SceneNames scene)
    {
        //TODO: 로딩 애니메이션 시작

        // 비동기 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        while (!asyncLoad.isDone)
        {
            //TODO: 로딩 진행도 표시
            yield return null;
        }

        //TODO: 로딩 애니메이션 종료
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
        switch (scene.name)
        {
            case "Start":
                InitializeStartScene();
                break;
            case "Lobby":
                InitializeLobbyScene();
                break;
            case "Room":
                InitializeRoomScene();
                break;
            case "Game":
                InitializeGameScene();
                break;
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