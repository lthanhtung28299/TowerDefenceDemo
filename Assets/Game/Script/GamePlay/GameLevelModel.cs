using System.Collections.Generic;

public class GameLevelModel : Singleton<GameLevelModel>, ISaveData
{
    public List<GameLevelInfo> levels { get; private set; } = new List<GameLevelInfo>();

    public void Initialize()
    {
        var levelData = GameLevelDataSo.GetInstance().LevelInfos;
        levels.Clear();
        foreach (var levelInfo in levelData)
        {
            levels.Add(new GameLevelInfo(levelInfo));
        }
    }

    public void Save()
    {
    }

    public void GetSaveData()
    {
    }
}