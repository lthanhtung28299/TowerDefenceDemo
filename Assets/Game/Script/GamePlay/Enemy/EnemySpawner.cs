using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    public List<BaseEnemy> listEnemySpawned { get; private set; } = new List<BaseEnemy>();
    [SerializeField] private Transform enemyHolder;
    
    public void SpawnEnemy(BaseEnemy e, WalkablePathInfo pathInfo)
    {
        var enemySpawned = PoolSpawner.Instance.Spawn(e.gameObject, enemyHolder);
        var enemy = enemySpawned.GetComponent<BaseEnemy>();
        enemy.SetUpEnemy(pathInfo);
        listEnemySpawned.Add(enemy);
    }
    
    [Button]
    public void SpawnEnemy(BaseEnemy e)
    {
        var enemySpawned = PoolSpawner.Instance.Spawn(e.gameObject, enemyHolder);
        var enemy = enemySpawned.GetComponent<BaseEnemy>();
        enemy.GetWalkablePathInfo();
        listEnemySpawned.Add(enemy);
    }
    

    public void DespawnEnemy(BaseEnemy e)
    {
        listEnemySpawned.Remove(e);
        PoolSpawner.Instance.Despawn(e.gameObject);
    }

    public List<BaseEnemy> GetEnemyFromPosition(Vector2 pos, float radius)
    {
        return listEnemySpawned.Where(enemy => Vector2.Distance(enemy.transform.position, pos) <= radius).ToList();
    }
}
