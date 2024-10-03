using System.Collections;
using UnityEngine;

public class GamePlayModel : Singleton<GamePlayModel>
{
    public int GoldRemain { get; private set; }
    public int LifeRemain { get; private set; }
    public GameLevelInfo LevelInfo { get; set; }

    public WaveInfo CurrentWave { get; private set; }
    public int CurrentWaveIndex { get; private set; }
    public float CurrentWaveTimeRemaining { get; private set; }
    public int TotalWaves => LevelInfo.waves.Count;
    private Timer _waveDurationTimer;
    private int _totalEnemySpawnCount;
    private int _enemyWaveSpawnComplete;

    public void Initialize(GameLevelInfo info)
    {
        // Khởi tạo lai dữ liệu khi bắt đầu vào gamePlay
        SetDefaults();
        LevelInfo = info;
        CurrentWaveIndex = 0;
        GoldRemain = info.startGold;
        LifeRemain = info.life;
        GameExecutor.Instance.WaitUtil(() => GameSceneManager.Instance.LoadingComplete, () =>
        {
            GridManager.Instance.LoadGridMap(info.levelMapId);
            PoolSpawner.Instance.Spawn(info.backGround, MapSpawnHolder.Instance.backGroundHolder);
        });
    }

    public void AddGoldKillEnemy(int amount)
    {
        GoldRemain += amount;
        GamePlaySignal.UpdateGold.Dispatch(GoldRemain);
    }

    public void DecreaseGold(int amount)
    {
        GoldRemain -= amount;
        GamePlaySignal.UpdateGold.Dispatch(GoldRemain);
    }

    public void DecreaseLife(int life)
    {
        LifeRemain -= life;
        GamePlaySignal.UpdateLife.Dispatch(LifeRemain);
        if (LifeRemain <= 0) GameOver();
    }

    public void StartWave()
    {
        if (CurrentWaveIndex > TotalWaves) return;
        GamePlaySignal.ShowButtonStartWave.Dispatch(false);
        CurrentWave = LevelInfo.waves[CurrentWaveIndex];
        _waveDurationTimer?.StopTimer(true);
        _waveDurationTimer =
            new Timer(CurrentWave.waveDuration, true)
                .AddActionEverySecond(timeRemaining =>
                {
                    CurrentWaveTimeRemaining = timeRemaining.time;
                    if (CurrentWaveTimeRemaining <= 10f && CurrentWaveIndex < TotalWaves)
                    {
                        GamePlaySignal.ShowButtonStartWave.Dispatch(true);
                    }
                })
                .OnComplete(s =>
                {
                    CurrentWaveTimeRemaining = 0;
                    ClearWave();
                });
        _waveDurationTimer.StartTimer();
        GamePlaySignal.UpdateWave.Dispatch(CurrentWaveIndex);
        SpawnEnemy(CurrentWave);
    }

    public void ClearWave()
    {
        _waveDurationTimer?.StopTimer(true);
        CurrentWaveTimeRemaining = 0;
        CurrentWaveIndex++;
        if (CurrentWaveIndex < TotalWaves)
        {
            StartWave();
        }
        else
        {
            GameExecutor.Instance.WaitUtil(() => EnemySpawner.Instance.listEnemySpawned.Count == 0, LevelComplete);
        }
    }

    private void SpawnEnemy(WaveInfo wave)
    {
        var listEnemySpawn = wave.enemies;
        _enemyWaveSpawnComplete = 0;
        _totalEnemySpawnCount = listEnemySpawn.Count;
        foreach (var enemySpawnWave in listEnemySpawn)
        {
            GameExecutor.Instance.StartCoroutine(IESpawnEnemy(enemySpawnWave));
        }
    }

    private IEnumerator IESpawnEnemy(EnemySpawnWave enemySpawnWave)
    {
        yield return new WaitForSeconds(enemySpawnWave.startDelayTime);
        var walkablePaths = GridManager.Instance.WalkablePath;
        var enemySpawner = EnemySpawner.Instance;
        var enemySpawnRemain = enemySpawnWave.spawnCount;
        while (enemySpawnRemain > 0)
        {
            var randomPath = walkablePaths[Random.Range(0, walkablePaths.Count)];
            enemySpawner.SpawnEnemy(enemySpawnWave.enemy, randomPath);
            enemySpawnRemain--;
            yield return new WaitForSeconds(Random.Range(0.3f, 2f));
        }

        _enemyWaveSpawnComplete++;
        if (_enemyWaveSpawnComplete < _totalEnemySpawnCount) yield break;
        yield return new WaitUntil(() => EnemySpawner.Instance.listEnemySpawned.Count == 0);
        ClearWave();
    }

    public void LevelComplete()
    {
        if (LifeRemain > 0)
        {
            ResultScreen.OpenScreen(s => s.OnShowWinView());
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        _waveDurationTimer?.StopTimer(true);
        CurrentWaveTimeRemaining = 0;
        ResultScreen.OpenScreen(s => s.OnShowLoseView());
    }

    private void SetDefaults()
    {
        GoldRemain = 0;
        LifeRemain = 0;
        LevelInfo = null;
        CurrentWave = null;
        CurrentWaveIndex = 0;
        CurrentWaveTimeRemaining = 0;
        _waveDurationTimer?.StopTimer(true);
    }
}