using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoSingleton<TowerSpawner>
{
    public Dictionary<Vector2Int,BaseTower> TowersSpawned { get; private set; } = new Dictionary<Vector2Int,BaseTower>();

    private void OnEnable()
    {
        TowersSpawned.Clear();
    }

    public void SpawnTower(Vector2Int spawnPos,TowerDataInfo towerDataInfo)
    {
        var gridNode = GridManager.Instance.GetTileNode(spawnPos);
        if(gridNode == null) return;
        if(!gridNode.Node.canBuild) return;
        var towerSpawnGo = PoolSpawner.Instance.Spawn(towerDataInfo.tower.gameObject,spawnPos);
        towerSpawnGo.transform.SetParent(transform);
        var tower = towerSpawnGo.GetComponent<BaseTower>();
        tower.InitializeTower(towerDataInfo);
        TowersSpawned.TryAdd(spawnPos,tower);
        gridNode.SetBuiltNode(true);
    }

    public void DespawnTower(BaseTower tower)
    {
        TowersSpawned.Remove(Vector2Int.CeilToInt(tower.gameObject.transform.position));
        PoolSpawner.Instance.Despawn(tower.gameObject);
    }
    
    public void DespawnTower(Vector2Int pos)
    {
        if (!TowersSpawned.TryGetValue(pos, out var tower)) return;
        PoolSpawner.Instance.Despawn(tower.gameObject);
        TowersSpawned.Remove(pos);
        GridManager.Instance.GetTileNode(pos).SetBuiltNode(false);

    }

    public void DespawnAllTowers()
    {
        foreach (var tower in TowersSpawned)
        {
            DespawnTower(tower.Value);
        }
        TowersSpawned.Clear();
    }
}
