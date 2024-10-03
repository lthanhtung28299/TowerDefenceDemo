using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoSingleton<GameSceneManager>
{
    private const string MainMenuScene = "MainMenu";
    private const string GamePlayScene = "GamePlay";
    public GameState gameState { get; private set; }
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingFill;
    [field: SerializeField] public bool IsTesting {get; private set;} 

    public bool LoadingComplete { get; private set; }
    public void Start()
    {
        GameLevelModel.Instance.Initialize();
        GridMapModel.Instance.Initialize();
        TowerModel.Instance.Initialize();
        HomeScreen.OpenScreen();
    }

    public void BackToMainMenu(Action onComplete = null)
    {
        StartCoroutine(LoadingScreen(MainMenuScene,() =>
        {
            gameState = GameState.MainMenu;
            HomeScreen.OpenScreen();
            ScreenManager.Instance.ShowHideBackGround(true);
            onComplete?.Invoke();
        }));
    }

    public void LoadSceneGamePlay(GameLevelInfo info, Action onComplete = null)
    {
        StartCoroutine(LoadingScreen(GamePlayScene, () =>
        {
            gameState = GameState.GamePlay;
            ScreenManager.Instance.ShowHideBackGround(false);
            GamePlayModel.Instance.Initialize(info);
            onComplete?.Invoke();
        }));
    }

    private IEnumerator LoadingScreen(string sceneName, Action onComplete)
    {
        LoadingComplete = false;
        loadingScreen.Show();
        PoolSpawner.Instance.ClearAllPool();
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        if (async == null) yield break;
        async.allowSceneActivation = false;

        while (async.isDone == false)
        {
            loadingFill.fillAmount = async.progress / 0.9f;
            if (Mathf.Approximately(async.progress, 0.9f))
            {
                loadingFill.fillAmount = 1f;
                async.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingScreen.Hide();
        onComplete?.Invoke();
        LoadingComplete = true;
    }
}

public enum GameState
{
    MainMenu,
    GamePlay
}