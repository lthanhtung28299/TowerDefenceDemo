using System;
using System.Collections;
using UnityEngine;

public class TowerProjectTile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Coroutine _launchCoroutine;
    private TowerDataInfo _towerDataInfo;

    public void Launch(BaseEnemy target, TowerDataInfo towerInfo)
    {
        _towerDataInfo = towerInfo;
        if (_launchCoroutine != null) StopCoroutine(_launchCoroutine);
        _launchCoroutine = StartCoroutine(IELaunch(target));
    }

    private IEnumerator IELaunch(BaseEnemy target)
    {
        var startPosition = transform.position;
        var endPosition = target.transform.position;
        float travelPercent = 0f;
        var direction = (endPosition - transform.position).normalized;
        transform.LookAt2DCoordinate(direction);
        while (travelPercent < 1f)
        {
            travelPercent += Time.deltaTime * speed;
            transform.position = Vector2.Lerp(startPosition, endPosition, travelPercent);
            yield return new WaitForEndOfFrame();
        }

        OnReachTarget(target);
    }

    private void OnReachTarget(BaseEnemy target)
    {
        switch (_towerDataInfo.projectileType)
        {
            case ProjectileType.Normal:
                target.Health.DealDamage(_towerDataInfo.damage);
                break;
            case ProjectileType.Explosion:
                DealDamageListEnemy();
                break;
        }

        PoolSpawner.Instance.Despawn(gameObject);
    }

    private void DealDamageListEnemy()
    {
        var listEnemyInRadius =
            EnemySpawner.Instance.GetEnemyFromPosition(transform.position, _towerDataInfo.radiusExplosion);
        foreach (var enemy in listEnemyInRadius)
        {
            enemy.Health.DealDamage(_towerDataInfo.damage);
        }
    }
}