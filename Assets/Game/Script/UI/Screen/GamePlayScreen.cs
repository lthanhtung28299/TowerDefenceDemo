using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayScreen : UIView<GamePlayScreen>
{
    [SerializeField] private Button btnPause, btnStartWave;
    [SerializeField] private TMP_Text txtLife, txtGold, txtWave;

    private void Start()
    {
        btnStartWave.onClick.AddListener(OnStartWave);
        btnPause.onClick.AddListener(OnPauseGame);
    }

    private void OnEnable()
    {
        GamePlaySignal.UpdateGold.AddListener(UpdateGold);
        GamePlaySignal.UpdateLife.AddListener(UpdateLife);
        GamePlaySignal.UpdateWave.AddListener(UpdateWave);
        GamePlaySignal.ShowButtonStartWave.AddListener(HandleShowButtonStartWave);
    }

    private void OnDisable()
    {
        GamePlaySignal.UpdateGold.RemoveListener(UpdateGold);
        GamePlaySignal.UpdateLife.RemoveListener(UpdateLife);
        GamePlaySignal.UpdateWave.RemoveListener(UpdateWave);
        GamePlaySignal.ShowButtonStartWave.RemoveListener(HandleShowButtonStartWave);
    }

    public void OnShow()
    {
        GameExecutor.Instance.WaitUtil(() => GameSceneManager.Instance.LoadingComplete, () =>
        {
            var gamePlayModel = GamePlayModel.Instance;
            txtGold.text = gamePlayModel.GoldRemain.ToString();
            txtLife.text = gamePlayModel.LifeRemain.ToString();
            txtWave.text = $"Wave: {gamePlayModel.CurrentWaveIndex + 1}/{gamePlayModel.TotalWaves}";
            btnStartWave.Show();
        });
    }

    private void UpdateGold(int gold)
    {
        txtGold.text = GamePlayModel.Instance.GoldRemain.ToString();
    }

    private void UpdateLife(int life)
    {
        txtLife.text = life.ToString();
    }

    private void UpdateWave(int wave)
    {
        txtWave.text = $"Wave: {wave + 1}/{GamePlayModel.Instance.TotalWaves}";
    }

    private void HandleShowButtonStartWave(bool isShow)
    {
        btnStartWave.gameObject.SetActive(isShow);
    }

    private void OnPauseGame()
    {
        PauseGamePopup.OpenPopup(s => s.OnShow());
    }

    private void OnStartWave()
    {
        GamePlayModel.Instance.StartWave();
        btnStartWave.Hide();
    }
}