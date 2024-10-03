using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLevelDataSo", menuName = "Data/GameLevelDataSo")]
public class GameLevelDataSo : ScriptableObject
{
    public List<GameLevelInfo> LevelInfos = new List<GameLevelInfo>();

    public static GameLevelDataSo GetInstance()
    {
        return DataHolder.GetInstance().GetData<GameLevelDataSo>();
    }
}

[Serializable]
public class GameLevelInfo
{
    public string levelId;
    public string levelMapId;
    public GameObject backGround;
    public int startGold;
    public int life;
    public List<WaveInfo> waves = new List<WaveInfo>();
    public bool isClear;

    public GameLevelInfo(GameLevelInfo gameLevelInfo)
    {
        levelId = gameLevelInfo.levelId;
        levelMapId = gameLevelInfo.levelMapId;
        backGround = gameLevelInfo.backGround;
        startGold = gameLevelInfo.startGold;
        life = gameLevelInfo.life;
        waves.Clear();
        foreach (var info in gameLevelInfo.waves)
        {
            waves.Add(info);
        }
    }
}

[Serializable]
public class WaveInfo
{
    public List<EnemySpawnWave> enemies = new List<EnemySpawnWave>();
    public int pathIndexSpawnAvailable;
    public float waveDuration;

    public WaveInfo(WaveInfo waveInfo)
    {
        enemies.Clear();
        foreach (var enemy in waveInfo.enemies)
        {
            enemies.Add(new EnemySpawnWave(enemy));
        }

        pathIndexSpawnAvailable = waveInfo.pathIndexSpawnAvailable;
        waveDuration = waveInfo.waveDuration;
    }
}

[Serializable]
public class EnemySpawnWave
{
    public BaseEnemy enemy;
    public int spawnCount;
    public float startDelayTime;

    public EnemySpawnWave(EnemySpawnWave enemyWave)
    {
        enemy = enemyWave.enemy;
        spawnCount = enemyWave.spawnCount;
        startDelayTime = enemyWave.startDelayTime;
    }
}