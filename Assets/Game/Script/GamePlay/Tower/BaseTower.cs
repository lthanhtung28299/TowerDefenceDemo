using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [SerializeField] private Transform towerRenderTransform;
    [SerializeField] private TowerProjectTile projectTile;
    [field: SerializeField] public Transform FirePosition { get; private set; }
    protected List<BaseEnemy> enemies = new List<BaseEnemy>();
    protected TowerDataInfo info;
    protected BaseEnemy target;
    private Coroutine _fireCoroutine;
    private bool _isFiring = false;

    public void InitializeTower(TowerDataInfo towerInfo)
    {
        SetListEnemies();
        info = towerInfo;
        BuildTower();
    }

    protected virtual void Update()
    {
        GetTargetEnemy();
        FireProjectile();
        LookAtTarget();
    }

    private void SetListEnemies()
    {
        enemies = EnemySpawner.Instance.listEnemySpawned;
    }

    private void BuildTower()
    {
        StartCoroutine(BuildTowerCoroutine());
    }

    private IEnumerator BuildTowerCoroutine()
    {
        _isFiring = true;
        yield return new WaitForSeconds(info.buildDelay);
        _isFiring = false;
    }
    
    protected void GetTargetEnemy()
    {
        if(enemies is {Count: 0}) return;
        var listEnemySpawned = enemies.OrderByDescending(s => s.PathProgress).ToList();
        foreach (var enemy in listEnemySpawned)
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) <= info.range && !enemy.Health.IsDead)
            {
                target = enemy;
                break;
            }
            else
            {
                target = null;
            }
        }
    }

    protected void LookAtTarget()
    {
        if(target == null) return;
        var direction = (target.transform.position - towerRenderTransform.position).normalized;
        towerRenderTransform.LookAt2DCoordinate(direction);
    }

    protected void FireProjectile()
    {
        if (target == null || _isFiring || target.Health.IsDead) return;
        _fireCoroutine = StartCoroutine(IEFire());
    }
    
    private IEnumerator IEFire()
    {
        _isFiring = true;
        var projectTileGo = PoolSpawner.Instance.Spawn(projectTile.gameObject, FirePosition);
        projectTileGo.transform.SetParent(ObjectFinder.GetObject(ObjectID.WorldSpawnerHolder));
        var projectileSpawned = projectTileGo.GetComponent<TowerProjectTile>();
        projectileSpawned.Launch(target,info);
        yield return new WaitForSeconds(info.fireRate);
        _isFiring = false;
    }
}