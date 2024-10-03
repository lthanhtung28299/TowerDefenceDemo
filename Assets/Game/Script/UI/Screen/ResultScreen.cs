using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : UIView<ResultScreen>
{
    [SerializeField] private Button btnPlayAgain, btnHome, btnNext;
    [SerializeField] private GameObject winGo, loseGo;

    private void Start()
    {
        btnHome.onClick.AddListener(OnClickHome);
        btnNext.onClick.AddListener(OnClickNext);
        btnPlayAgain.onClick.AddListener(OnClickPlayAgain);
    }

    public void OnShowWinView()
    {
        SetDefaults();
        winGo.Show();
        btnPlayAgain.Show();
        btnHome.Show();
        btnNext.Show();
    }

    public void OnShowLoseView()
    {
        SetDefaults();
        loseGo.Show();
        btnHome.Show();
        btnPlayAgain.Show();
    }

    private void OnClickPlayAgain()
    {
        GameSceneManager.Instance.LoadSceneGamePlay(GamePlayModel.Instance.LevelInfo, () =>
        {
            GamePlayScreen.OpenScreen(s => s.OnShow());
        });
    }

    private void OnClickHome()
    {
        GameSceneManager.Instance.BackToMainMenu();
    }

    private void OnClickNext()
    {
        GameSceneManager.Instance.BackToMainMenu(() =>
        {
            GameExecutor.Instance.WaitUtil(HomeScreen.IsOnScreen, () =>
            {
                SelectLevelScreen.OpenScreen(s => s.OnShow());
            });
        });
    }

    private void SetDefaults()
    {
        winGo.Hide();
        loseGo.Hide();
        btnHome.Hide();
        btnNext.Hide();
        btnPlayAgain.Hide();
    }
}